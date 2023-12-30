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

using Newtonsoft.Json.Linq;

using NUnit.Framework;
using NUnit.Framework.Legacy;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv2_1.tests.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode;
using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CS;
using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.NetworkingNode.New
{

    /// <summary>
    /// Networking node test defaults.
    /// </summary>
    public class NewNetworkingNodeTests
    {

        #region SendBootNotifications_Test()

        /// <summary>
        /// A test for sending boot notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task TestAAA()
        {

            var tn01      = new TestNetworkingNode(
                                NetworkingNode_Id.Parse("NN01"),
                                "GraphDefined GmbH",
                                "NN-0001"
                            );

            var server1   = tn01.AttachWebSocketServer(
                                TCPPort:                 IPPort.Parse(4599),
                                RequireAuthentication:   true,
                                DisableWebSocketPings:   true,
                                AutoStart:               true
                            );


            tn01.ocppIN.OnBootNotification += async (timestamp, sender, connection, request, cancellationToken) => {

                return new BootNotificationResponse(
                           request,
                           RegistrationStatus.Accepted,
                           Timestamp.Now,
                           TimeSpan.FromMinutes(5)
                       );

            };




            //var tn02     = new TestNetworkingNode(
            //                   NetworkingNode_Id.Parse("NN02"),
            //                   "GraphDefined GmbH",
            //                   "NN-0002"
            //               );

            //var ocppIN2  = new OCPPWebSocketAdapterIN(tn02);

            //var server2  = new OCPPWebSocketServer(tn02.Id, ocppIN2);



            var chargingStation1 = new TestChargingStation(
                                       Id:                       NetworkingNode_Id.Parse("cs01"),
                                       VendorName:               "GraphDefined OEM #1",
                                       Model:                    "VCP.1",
                                       Description:              I18NString.Create(Languages.en, "Our first virtual charging station!"),
                                       SerialNumber:             "SN-CS0001",
                                       FirmwareVersion:          "v0.1",
                                       Modem:                    new Modem(
                                                                     ICCID:   "0000",
                                                                     IMSI:    "1111"
                                                                 ),
                                       EVSEs:                    new[] {
                                                                     new ChargingStationEVSE(
                                                                         Id:                  EVSE_Id.Parse(1),
                                                                         AdminStatus:         OperationalStatus.Operative,
                                                                         MeterType:           "MT1",
                                                                         MeterSerialNumber:   "MSN1",
                                                                         MeterPublicKey:      "MPK1",
                                                                         Connectors:          new[] {
                                                                                                  new ChargingStationConnector(
                                                                                                      Id:              Connector_Id.Parse(1),
                                                                                                      ConnectorType:   ConnectorType.sType2
                                                                                                  )
                                                                                              }
                                                                     )
                                                                 },
                                       MeterType:                "Virtual Energy Meter",
                                       MeterSerialNumber:        "SN-EN0001",
                                       MeterPublicKey:           "0xcafebabe",

                                       DisableSendHeartbeats:    true
                                   );


            server1.AddOrUpdateHTTPBasicAuth(chargingStation1.Id, "1234abcd");

            var response = await chargingStation1.ConnectWebSocket(
                                     RemoteURL:              URL.Parse($"http://127.0.0.1:{server1.IPPort}/{chargingStation1.Id}"),
                                     HTTPAuthentication:     HTTPBasicAuthentication.Create(chargingStation1.Id.ToString(), "1234abcd"),
                                     DisableWebSocketPings:  true
                                 );


            var response1 = await chargingStation1.SendBootNotification(BootReason.PowerUp);


            Assert.That(response1.Result.ResultCode,   Is.EqualTo(ResultCode.OK));
            Assert.That(response1.Status,              Is.EqualTo(RegistrationStatus.Accepted));


            await Task.Delay(500);


            var response2 = await tn01.ocppOUT.DataTransfer(
                                      new DataTransferRequest(
                                          chargingStation1.Id,
                                          Vendor_Id.GraphDefined,
                                          Message_Id.GraphDefined_TestMessage,
                                          "Hello World!"
                                      )
                                  );

            Assert.That(response2.Result.ResultCode,   Is.EqualTo(ResultCode.OK));
            Assert.That(response2.Status,              Is.EqualTo(DataTransferStatus.Accepted));

            Assert.That(response2.Data?.ToString(),    Is.EqualTo("Hello World!".Reverse()));

            //var r2 = await tn01.OUT.Reset(
            //                            new ResetRequest(
            //                                chargingStation1.Id,
            //                                ResetType.Immediate
            //                            )
            //                        );

        }

        #endregion



    }

}