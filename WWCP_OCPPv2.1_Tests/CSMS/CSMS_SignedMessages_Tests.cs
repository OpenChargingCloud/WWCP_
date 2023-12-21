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
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.tests.ChargingStation;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.CSMS
{

    /// <summary>
    /// Unit tests for a CSMS sending signed messages to charging stations.
    /// </summary>
    [TestFixture]
    public class CSMS_SignedMessages_Tests : AChargingStationTests
    {

        #region Reset_Test()

        /// <summary>
        /// A test for sending a reset message to a charging station.
        /// </summary>
        [Test]
        public async Task Reset_Test()
        {

            ClassicAssert.IsNotNull(testCSMS01);
            ClassicAssert.IsNotNull(testBackendWebSockets01);
            ClassicAssert.IsNotNull(chargingStation1);
            ClassicAssert.IsNotNull(chargingStation2);
            ClassicAssert.IsNotNull(chargingStation3);

            if (testCSMS01              is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var resetRequests = new ConcurrentList<ResetRequest>();

                chargingStation1.OnResetRequest += (timestamp, sender, connection, resetRequest) => {
                    resetRequests.TryAdd(resetRequest);
                    return Task.CompletedTask;
                };

                chargingStation1.SignaturePolicy.AddVerificationRule(ResetRequest.DefaultJSONLDContext,
                                                                     VerificationRuleActions.VerifyAll);


                var keyPair    = KeyPair.GenerateKeys()!;

                var resetType  = ResetType.Immediate;
                var now        = Timestamp.Now;
                var response   = await testCSMS01.Reset(
                                     DestinationNodeId:   chargingStation1.Id,
                                     ResetType:           resetType,
                                     SignInfos:           new[] {
                                                              keyPair.ToSignInfo1(
                                                                          Name:         "ahzf",
                                                                          Description:   I18NString.Create("Just a test!"),
                                                                          Timestamp:     now
                                                                      )
                                                          },
                                     CustomData:          null
                                 );

                ClassicAssert.AreEqual(ResultCode.OK,                response.Result.ResultCode);
                ClassicAssert.AreEqual(ResetStatus.Accepted,          response.Status);

                ClassicAssert.IsTrue  (testCSMS01.SignaturePolicy.VerifyResponseMessage(
                                    response,
                                    response.ToJSON(
                                        testCSMS01.CustomResetResponseSerializer,
                                        testCSMS01.CustomStatusInfoSerializer,
                                        testCSMS01.CustomSignatureSerializer,
                                        testCSMS01.CustomCustomDataSerializer
                                    ),
                                    out var errorResponse
                                ));


                ClassicAssert.AreEqual(1,                                   resetRequests.Count);
                ClassicAssert.AreEqual(chargingStation1.Id,                 resetRequests.First().DestinationNodeId);
                ClassicAssert.AreEqual(resetType,                           resetRequests.First().ResetType);
                ClassicAssert.AreEqual(1,                                   resetRequests.First().Signatures.Count());
                ClassicAssert.AreEqual(VerificationStatus.ValidSignature,   resetRequests.First().Signatures.First().Status);
                ClassicAssert.AreEqual("ahzf",                              resetRequests.First().Signatures.First().Name);
                ClassicAssert.AreEqual("Just a test!",                      resetRequests.First().Signatures.First().Description?.FirstText());
                ClassicAssert.AreEqual(now.ToIso8601(),                     resetRequests.First().Signatures.First().Timestamp?.  ToIso8601());

            }

        }

        #endregion


    }

}
