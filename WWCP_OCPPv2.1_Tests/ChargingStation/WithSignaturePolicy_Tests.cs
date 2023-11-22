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

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.ChargingStation
{

    /// <summary>
    /// Unit tests for charging stations sending sending signed messages
    /// based on a message signature policy to the CSMS.
    /// </summary>
    [TestFixture]
    public class WithSignaturePolicy_Tests : AChargingStationTests
    {

        #region Init_Test()

        /// <summary>
        /// A test for creating charging stations.
        /// </summary>
        [Test]
        public void ChargingStation_Init_Test()
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

                Assert.AreEqual("GraphDefined OEM #1",  chargingStation1.VendorName);
                Assert.AreEqual("GraphDefined OEM #2",  chargingStation2.VendorName);
                Assert.AreEqual("GraphDefined OEM #3",  chargingStation3.VendorName);

            }

        }

        #endregion

        #region SendBootNotifications_Test()

        /// <summary>
        /// A test for sending boot notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task SendBootNotifications_Test()
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

                var bootNotificationRequests = new ConcurrentList<CS.BootNotificationRequest>();

                testCSMS01.OnBootNotificationRequest += (timestamp, sender, connection, bootNotificationRequest) => {
                    bootNotificationRequests.TryAdd(bootNotificationRequest);
                    return Task.CompletedTask;
                };

                var now1                           = Timestamp.Now;
                var keyPair                        = KeyPair.GenerateKeys()!;
                chargingStation1.SignaturePolicy.AddSigningRule     (BootNotificationRequest. DefaultJSONLDContext,
                                                                     KeyPair:                 keyPair,
                                                                     UserIdGenerator:         (signableMessage) => "cs001",
                                                                     DescriptionGenerator:    (signableMessage) => I18NString.Create("Just a charging station test!"),
                                                                     TimestampGenerator:      (signableMessage) => now1);
                chargingStation1.SignaturePolicy.AddVerificationRule(BootNotificationResponse.DefaultJSONLDContext,
                                                                     VerificationRuleActions.VerifyAll);

                var now2                           = Timestamp.Now;
                var keyPair2                       = KeyPair.GenerateKeys()!;
                testCSMS01.SignaturePolicy.      AddVerificationRule(BootNotificationRequest. DefaultJSONLDContext,
                                                                     VerificationRuleActions.VerifyAll);
                testCSMS01.SignaturePolicy.      AddSigningRule     (BootNotificationResponse.DefaultJSONLDContext,
                                                                     keyPair2,
                                                                     UserIdGenerator:         (signableMessage) => "csms001",
                                                                     DescriptionGenerator:    (signableMessage) => I18NString.Create("Just a backend test!"),
                                                                     TimestampGenerator:      (signableMessage) => now2);


                var reason                         = BootReason.PowerUp;
                var response                       = await chargingStation1.SendBootNotification(
                                                         BootReason:   reason,
                                                         CustomData:   null
                                                     );

                Assert.AreEqual(ResultCodes.OK,                          response.Result.ResultCode);
                Assert.AreEqual(RegistrationStatus.Accepted,             response.Status);
                Assert.AreEqual(1,                                       response.Signatures.Count());
                Assert.AreEqual(VerificationStatus.ValidSignature,       response.Signatures.First().Status);
                Assert.AreEqual("csms001",                               response.Signatures.First().Name);
                Assert.AreEqual("Just a backend test!",                  response.Signatures.First().Description?.FirstText());
                Assert.AreEqual(now2.ToIso8601(),                        response.Signatures.First().Timestamp?.  ToIso8601());

                var chargingStation = bootNotificationRequests.First().ChargingStation;

                Assert.IsNotNull(chargingStation);
                if (chargingStation is not null)
                {

                    Assert.AreEqual(chargingStation1.Model,              chargingStation.Model);
                    Assert.AreEqual(chargingStation1.VendorName,         chargingStation.VendorName);
                    Assert.AreEqual(chargingStation1.SerialNumber,       chargingStation.SerialNumber);
                    Assert.AreEqual(chargingStation1.FirmwareVersion,    chargingStation.FirmwareVersion);

                    var modem = chargingStation.Modem;

                    Assert.IsNotNull(modem);
                    if (modem is not null)
                    {
                        Assert.AreEqual(chargingStation1.Modem!.ICCID,   modem.ICCID);
                        Assert.AreEqual(chargingStation1.Modem!.IMSI,    modem.IMSI);
                    }

                }


                Assert.AreEqual(1,                                       bootNotificationRequests.Count);
                Assert.AreEqual(chargingStation1.Id,                     bootNotificationRequests.First().ChargingStationId);
                Assert.AreEqual(reason,                                  bootNotificationRequests.First().Reason);
                Assert.AreEqual(1,                                       bootNotificationRequests.First().Signatures.Count());
                Assert.AreEqual(VerificationStatus.ValidSignature,             bootNotificationRequests.First().Signatures.First().Status);
                Assert.AreEqual("cs001",                                 bootNotificationRequests.First().Signatures.First().Name);
                Assert.AreEqual("Just a charging station test!",         bootNotificationRequests.First().Signatures.First().Description?.FirstText());
                Assert.AreEqual(now1.ToIso8601(),                        bootNotificationRequests.First().Signatures.First().Timestamp?.  ToIso8601());

            }

        }

        #endregion


    }

}