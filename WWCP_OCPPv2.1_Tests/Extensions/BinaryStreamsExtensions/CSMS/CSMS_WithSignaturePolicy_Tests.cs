﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using NUnit.Framework;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.tests.ChargingStation;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.extensions.BinaryStreamsExtensions
{

    /// <summary>
    /// Unit tests for a CSMS sending signed binary data messages to charging stations.
    /// </summary>
    [TestFixture]
    public class CSMS_WithSignaturePolicy_Tests : AChargingStationTests
    {

        #region SendBinaryData_Test1()

        /// <summary>
        /// A test for sending a signed binary data to a charging station.
        /// </summary>
        [Test]
        public async Task SendBinaryData_Test1()
        {

            Assert.IsNotNull(testCSMS01);
            Assert.IsNotNull(testBackendWebSockets01);
            Assert.IsNotNull(chargingStation1);
            Assert.IsNotNull(chargingStation2);
            Assert.IsNotNull(chargingStation3);

            if (testCSMS01              is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var timeReference      = Timestamp.Now - TimeSpan.FromHours(1);

                #region Set the CSMS             signature policy

                var now1            = Timestamp.Now;
                var requestKeyPair  = KeyPair.GenerateKeys()!;
                testCSMS01.      SignaturePolicy.AddSigningRule     (SetDefaultChargingTariffRequest. DefaultJSONLDContext,
                                                                     requestKeyPair!,
                                                                     UserIdGenerator:         (signableMessage) => "csms001",
                                                                     DescriptionGenerator:    (signableMessage) => I18NString.Create("Just a backend test request!"),
                                                                     TimestampGenerator:      (signableMessage) => now1);

                testCSMS01.      SignaturePolicy.AddVerificationRule(SetDefaultChargingTariffResponse.DefaultJSONLDContext);

                #endregion

                #region Set the charging station signature policy

                var now2            = Timestamp.Now;
                chargingStation1.SignaturePolicy.AddVerificationRule(SetDefaultChargingTariffRequest. DefaultJSONLDContext);

                chargingStation1.SignaturePolicy.AddSigningRule     (SetDefaultChargingTariffResponse.DefaultJSONLDContext,
                                                                     requestKeyPair!,
                                                                     UserIdGenerator:         (signableMessage) => "cs001",
                                                                     DescriptionGenerator:    (signableMessage) => I18NString.Create("Just a charging station test response!"),
                                                                     TimestampGenerator:      (signableMessage) => now2);

                #endregion

                #region Setup charging station incoming request monitoring

                var setDefaultChargingTariffRequests = new ConcurrentList<SetDefaultChargingTariffRequest>();

                chargingStation1.OnSetDefaultChargingTariffRequest += (timestamp, sender, setDefaultChargingTariffRequest) => {
                    setDefaultChargingTariffRequests.TryAdd(setDefaultChargingTariffRequest);
                    return Task.CompletedTask;
                };

                #endregion

                #region Define a signed charging tariff

                var providerKeyPair    = KeyPair.GenerateKeys()!;

                var chargingTariff     = new ChargingTariff(

                                             Id:               ChargingTariff_Id.Parse("DE-GDF-T12345678"),
                                             ProviderId:       Provider_Id.      Parse("DE-GDF"),
                                             ProviderName:     new DisplayTexts(
                                                                   Languages.en,
                                                                   "GraphDefined EMP"
                                                               ),
                                             Currency:         Currency.EUR,
                                             TariffElements:   new[] {
                                                                   new TariffElement(
                                                                       new[] {
                                                                           PriceComponent.Energy(
                                                                               Price:      0.51M,
                                                                               VAT:        0.02M,
                                                                               StepSize:   WattHour.Parse(1000)
                                                                           )
                                                                       }
                                                                   )
                                                               },

                                             Created:          timeReference,
                                             Replaces:         null,
                                             References:       null,
                                             TariffType:       TariffType.REGULAR,
                                             Description:      new DisplayTexts(
                                                                   Languages.en,
                                                                   "0.53 / kWh"
                                                               ),
                                             URL:              URL.Parse("https://open.charging.cloud/emp/tariffs/DE-GDF-T12345678"),
                                             EnergyMix:        null,

                                             MinPrice:         null,
                                             MaxPrice:         new Price(
                                                                   ExcludingVAT:  0.51M,
                                                                   IncludingVAT:  0.53M
                                                               ),
                                             NotBefore:        timeReference,
                                             NotAfter:         null,

                                             SignKeys:         null,
                                             SignInfos:        null,
                                             Signatures:       null,

                                             CustomData:       null

                                         );

                Assert.IsNotNull(chargingTariff);


                Assert.IsTrue   (chargingTariff.Sign(providerKeyPair,
                                                     out var eerr,
                                                     "emp1",
                                                     I18NString.Create("Just a signed charging tariff!"),
                                                     timeReference,
                                                     testCSMS01.CustomChargingTariffSerializer,
                                                     testCSMS01.CustomPriceSerializer,
                                                     testCSMS01.CustomTaxRateSerializer,
                                                     testCSMS01.CustomTariffElementSerializer,
                                                     testCSMS01.CustomPriceComponentSerializer,
                                                     testCSMS01.CustomTariffRestrictionsSerializer,
                                                     testCSMS01.CustomEnergyMixSerializer,
                                                     testCSMS01.CustomEnergySourceSerializer,
                                                     testCSMS01.CustomEnvironmentalImpactSerializer,
                                                     testCSMS01.CustomIdTokenSerializer,
                                                     testCSMS01.CustomAdditionalInfoSerializer,
                                                     testCSMS01.CustomSignatureSerializer,
                                                     testCSMS01.CustomCustomDataSerializer));

                Assert.IsTrue   (chargingTariff.Signatures.Any());

                #endregion


                var response        = await testCSMS01.SetDefaultChargingTariff(
                                          NetworkingNodeId:   chargingStation1.Id,
                                          ChargingTariff:     chargingTariff,
                                          CustomData:         null
                                      );

                #region Verify the response

                Assert.AreEqual(ResultCode.OK,                            response.Result.ResultCode);
                Assert.AreEqual(SetDefaultChargingTariffStatus.Accepted,   response.Status);

                #endregion

                #region Verify the request at the charging station

                Assert.AreEqual(1,                                         setDefaultChargingTariffRequests.Count);
                Assert.AreEqual(chargingStation1.Id,                       setDefaultChargingTariffRequests.First().NetworkingNodeId);

                Assert.AreEqual(chargingTariff.Id,                         setDefaultChargingTariffRequests.First().ChargingTariff.Id);
                Assert.AreEqual(1,                                         setDefaultChargingTariffRequests.First().ChargingTariff.Signatures.Count());
                Assert.IsTrue  (                                           setDefaultChargingTariffRequests.First().ChargingTariff.Verify(out var errr));
                Assert.AreEqual(VerificationStatus.ValidSignature,         setDefaultChargingTariffRequests.First().ChargingTariff.Signatures.First().Status);
                Assert.AreEqual("emp1",                                    setDefaultChargingTariffRequests.First().ChargingTariff.Signatures.First().Name);
                Assert.AreEqual("Just a signed charging tariff!",          setDefaultChargingTariffRequests.First().ChargingTariff.Signatures.First().Description?.FirstText());
                Assert.AreEqual(timeReference.ToIso8601(),                 setDefaultChargingTariffRequests.First().ChargingTariff.Signatures.First().Timestamp?.  ToIso8601());

                Assert.AreEqual(1,                                         setDefaultChargingTariffRequests.First().Signatures.Count());
                Assert.AreEqual(VerificationStatus.ValidSignature,         setDefaultChargingTariffRequests.First().Signatures.First().Status);
                Assert.AreEqual("csms001",                                 setDefaultChargingTariffRequests.First().Signatures.First().Name);
                Assert.AreEqual("Just a backend test request!",            setDefaultChargingTariffRequests.First().Signatures.First().Description?.FirstText());
                Assert.AreEqual(now1.ToIso8601(),                          setDefaultChargingTariffRequests.First().Signatures.First().Timestamp?.  ToIso8601());

                #endregion

            }

        }

        #endregion


    }

}
