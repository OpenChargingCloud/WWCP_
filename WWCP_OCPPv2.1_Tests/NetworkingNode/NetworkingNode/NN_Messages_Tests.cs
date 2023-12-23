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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.NN;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.NN;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.NetworkingNode.NN
{

    /// <summary>
    /// Unit tests for networking nodes sending messages to the CSMS.
    /// </summary>
    [TestFixture]
    public class NN_Messages_Tests : AChargingStationWithNetworkingNodeTests
    {

        #region Init_Test()

        ///// <summary>
        ///// A test for creating charging stations.
        ///// </summary>
        //[Test]
        //public void ChargingStation_Init_Test()
        //{

        //    ClassicAssert.IsNotNull(testCSMS01);
        //    ClassicAssert.IsNotNull(testBackendWebSockets01);
        //    ClassicAssert.IsNotNull(chargingStation1);
        //    ClassicAssert.IsNotNull(chargingStation2);
        //    ClassicAssert.IsNotNull(chargingStation3);

        //    if (testCSMS01              is not null &&
        //        testBackendWebSockets01 is not null &&
        //        chargingStation1        is not null &&
        //        chargingStation2        is not null &&
        //        chargingStation3        is not null)
        //    {

        //        ClassicAssert.AreEqual("GraphDefined OEM #1",  chargingStation1.VendorName);
        //        ClassicAssert.AreEqual("GraphDefined OEM #2",  chargingStation2.VendorName);
        //        ClassicAssert.AreEqual("GraphDefined OEM #3",  chargingStation3.VendorName);

        //    }

        //}

        #endregion


        #region SendBootNotifications_Test()

        /// <summary>
        /// A test for sending boot notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task SendBootNotifications_Test()
        {

            InitNetworkingNode1 = true;

            Assert.Multiple(() => {
                Assert.That(testCSMS01,                       Is.Not.Null);
                Assert.That(testBackendWebSockets01,          Is.Not.Null);
                Assert.That(networkingNode1,                  Is.Not.Null);
                Assert.That(testNetworkingNodeWebSockets01,   Is.Not.Null);
                Assert.That(chargingStation1,                 Is.Not.Null);
                Assert.That(chargingStation2,                 Is.Not.Null);
                Assert.That(chargingStation3,                 Is.Not.Null);
            });

            if (testCSMS01                     is not null &&
                testBackendWebSockets01        is not null &&
                networkingNode1                is not null &&
                testNetworkingNodeWebSockets01 is not null &&
                chargingStation1               is not null &&
                chargingStation2               is not null &&
                chargingStation3               is not null)
            {

                var csmsBootNotificationRequests  = new ConcurrentList<BootNotificationRequest>();

                testCSMS01.OnBootNotificationRequest += (timestamp, sender, connection, bootNotificationRequest) => {
                    csmsBootNotificationRequests.TryAdd(bootNotificationRequest);
                    return Task.CompletedTask;
                };


                var reason    = BootReason.PowerUp;
                var response  = await networkingNode1.SendBootNotification(
                                          BootReason:         reason,
                                          DestinationNodeId:  NetworkingNode_Id.CSMS
                                      );


                Assert.Multiple(() => {

                    Assert.That(response.Result.ResultCode,                                Is.EqualTo(ResultCode.OK));
                    Assert.That(response.Status,                                           Is.EqualTo(RegistrationStatus.Accepted));

                    Assert.That(csmsBootNotificationRequests.Count,                        Is.EqualTo(1), "The BootNotification did not reach the CSMS!");
                    //Assert.That(csmsBootNotificationRequests.First().DestinationNodeId,    Is.EqualTo(NetworkingNode_Id.CSMS));
                    Assert.That(csmsBootNotificationRequests.First().NetworkPath.Length,   Is.EqualTo(1));
                    Assert.That(csmsBootNotificationRequests.First().NetworkPath.Source,   Is.EqualTo(networkingNode1.Id));
                    Assert.That(csmsBootNotificationRequests.First().NetworkPath.Last,     Is.EqualTo(networkingNode1.Id));
                    Assert.That(csmsBootNotificationRequests.First().Reason,               Is.EqualTo(reason));

                    Assert.That(csmsBootNotificationRequests.First().ChargingStation,      Is.Not.Null);

                });

                var chargingStation = csmsBootNotificationRequests.First().ChargingStation;
                if (chargingStation is not null)
                {

                    Assert.That(chargingStation.Model,             Is.EqualTo(networkingNode1.Model));
                    Assert.That(chargingStation.VendorName,        Is.EqualTo(networkingNode1.VendorName));
                    Assert.That(chargingStation.SerialNumber,      Is.EqualTo(networkingNode1.SerialNumber));
                    Assert.That(chargingStation.FirmwareVersion,   Is.EqualTo(networkingNode1.FirmwareVersion));
                    Assert.That(chargingStation.Modem,             Is.Not.Null);

                    if (chargingStation. Modem is not null &&
                        chargingStation1.Modem is not null)
                    {
                        Assert.That(chargingStation.Modem.ICCID,   Is.EqualTo(networkingNode1.Modem.ICCID));
                        Assert.That(chargingStation.Modem.IMSI,    Is.EqualTo(networkingNode1.Modem.IMSI));
                    }

                }

            }

        }

        #endregion


        #region NotifyNetworkTopology_Test1()

        /// <summary>
        /// A test for sending NotifyNetworkTopology requests to the CSMS.
        /// </summary>
        [Test]
        public async Task NotifyNetworkTopology_Test1()
        {

            InitNetworkingNode1 = true;

            Assert.Multiple(() => {
                Assert.That(testCSMS01,                       Is.Not.Null);
                Assert.That(testBackendWebSockets01,          Is.Not.Null);
                Assert.That(networkingNode1,                  Is.Not.Null);
                Assert.That(testNetworkingNodeWebSockets01,   Is.Not.Null);
                Assert.That(chargingStation1,                 Is.Not.Null);
                Assert.That(chargingStation2,                 Is.Not.Null);
                Assert.That(chargingStation3,                 Is.Not.Null);
            });

            if (testCSMS01                     is not null &&
                testBackendWebSockets01        is not null &&
                networkingNode1                is not null &&
                testNetworkingNodeWebSockets01 is not null &&
                chargingStation1               is not null &&
                chargingStation2               is not null &&
                chargingStation3               is not null)
            {

                var csmsNotifyNetworkTopologyRequests  = new ConcurrentList<NotifyNetworkTopologyRequest>();

                testCSMS01.OnIncomingNotifyNetworkTopologyRequest += (timestamp, sender, connection, notifyNetworkTopologyRequest) => {
                    csmsNotifyNetworkTopologyRequests.TryAdd(notifyNetworkTopologyRequest);
                    return Task.CompletedTask;
                };


                var reason    = BootReason.PowerUp;
                var response  = await networkingNode1.NotifyNetworkTopology(
                                    new NotifyNetworkTopologyRequest(
                                        NetworkingNode_Id.CSMS,
                                        new NetworkTopologyInformation(
                                            RoutingNode:   networkingNode1.Id,
                                            Routes:        new[] {
                                                               new NetworkRoutingInformation(
                                                                   NetworkingNodeId:   networkingNode1.Id,
                                                                   Priority:           23,
                                                                   Uplink:             new NetworkLinkInformation(
                                                                                           Capacity:     5000,
                                                                                           Latency:      TimeSpan.FromMilliseconds(10),
                                                                                           ErrorRate:    PercentageDouble.Parse(0.05)
                                                                                       ),
                                                                   Downlink:           new NetworkLinkInformation(
                                                                                           Capacity:     15000,
                                                                                           Latency:      TimeSpan.FromMilliseconds(23),
                                                                                           ErrorRate:    PercentageDouble.Parse(0.42)
                                                                                       )
                                                               )
                                                           },
                                            NotBefore:     Timestamp.Now - TimeSpan.FromMinutes(5),
                                            NotAfter:      Timestamp.Now + TimeSpan.FromHours  (6),
                                            Priority:      5,
                                            CustomData:    null
                                        )
                                    )
                                );


                Assert.Multiple(() => {

                    Assert.That(response.Result.ResultCode,                                    Is.EqualTo(ResultCode.OK));
                    Assert.That(response.Status,                                               Is.EqualTo(NetworkTopologyStatus.Accepted));

                    Assert.That(csmsNotifyNetworkTopologyRequests.Count,                       Is.EqualTo(1), "The Network Topology Notification did not reach the CSMS!");
                    var csmsNotifyNetworkTopologyRequest = csmsNotifyNetworkTopologyRequests.First();

                    Assert.That(csmsNotifyNetworkTopologyRequest.DestinationNodeId,            Is.EqualTo(NetworkingNode_Id.CSMS));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Length,           Is.EqualTo(1));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Source,           Is.EqualTo(networkingNode1.Id));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Last,             Is.EqualTo(networkingNode1.Id));

                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkTopologyInformation,   Is.Not.Null);

                });

                var networkTopologyInformation = csmsNotifyNetworkTopologyRequests.First().NetworkTopologyInformation;
                if (networkTopologyInformation is not null)
                {

                    Assert.That(networkTopologyInformation.RoutingNode,                 Is.EqualTo(networkingNode1.Id));
                    Assert.That(networkTopologyInformation.Priority,                    Is.EqualTo(5));
                    Assert.That(networkTopologyInformation.Routes,                      Is.Not.Null);

                    var routes = networkTopologyInformation.Routes;
                    if (routes is not null)
                    {

                        Assert.That(routes.Values.First().NetworkingNodeId,             Is.EqualTo(networkingNode1.Id));
                        Assert.That(routes.Values.First().Priority,                     Is.EqualTo(23));

                        Assert.That(routes.Values.First().Uplink,                       Is.Not.Null);
                        Assert.That(routes.Values.First().Uplink?.Capacity,             Is.EqualTo(5000));
                        Assert.That(routes.Values.First().Uplink?.Latency,              Is.EqualTo(TimeSpan.FromMilliseconds(10)));
                        Assert.That(routes.Values.First().Uplink?.ErrorRate?.Value,     Is.EqualTo(0.05));

                        Assert.That(routes.Values.First().Downlink,                     Is.Not.Null);
                        Assert.That(routes.Values.First().Downlink?.Capacity,           Is.EqualTo(15000));
                        Assert.That(routes.Values.First().Downlink?.Latency,            Is.EqualTo(TimeSpan.FromMilliseconds(23)));
                        Assert.That(routes.Values.First().Downlink?.ErrorRate?.Value,   Is.EqualTo(0.42));

                    }

                }

            }

        }

        #endregion

        #region NotifyNetworkTopology_Test2()

        /// <summary>
        /// A test for sending NotifyNetworkTopology requests to the CSMS.
        /// </summary>
        [Test]
        public async Task NotifyNetworkTopology_Test2()
        {

            InitNetworkingNode1 = true;

            Assert.Multiple(() => {
                Assert.That(testCSMS01,                       Is.Not.Null);
                Assert.That(testBackendWebSockets01,          Is.Not.Null);
                Assert.That(networkingNode1,                  Is.Not.Null);
                Assert.That(testNetworkingNodeWebSockets01,   Is.Not.Null);
                Assert.That(chargingStation1,                 Is.Not.Null);
                Assert.That(chargingStation2,                 Is.Not.Null);
                Assert.That(chargingStation3,                 Is.Not.Null);
            });

            if (testCSMS01                     is not null &&
                testBackendWebSockets01        is not null &&
                networkingNode1                is not null &&
                testNetworkingNodeWebSockets01 is not null &&
                chargingStation1               is not null &&
                chargingStation2               is not null &&
                chargingStation3               is not null)
            {

                var csmsNotifyNetworkTopologyRequests  = new ConcurrentList<NotifyNetworkTopologyRequest>();

                testCSMS01.OnIncomingNotifyNetworkTopologyRequest += (timestamp, sender, connection, notifyNetworkTopologyRequest) => {
                    csmsNotifyNetworkTopologyRequests.TryAdd(notifyNetworkTopologyRequest);
                    return Task.CompletedTask;
                };


                var reason    = BootReason.PowerUp;
                var response  = await networkingNode1.NotifyNetworkTopology(
                                    DestinationNodeId:            NetworkingNode_Id.CSMS,
                                    NetworkTopologyInformation:   new NetworkTopologyInformation(
                                                                      RoutingNode:   networkingNode1.Id,
                                                                      Routes:        new[] {
                                                                                         new NetworkRoutingInformation(
                                                                                             NetworkingNodeId:   networkingNode1.Id,
                                                                                             Priority:           23,
                                                                                             Uplink:             new NetworkLinkInformation(
                                                                                                                     Capacity:     5000,
                                                                                                                     Latency:      TimeSpan.FromMilliseconds(10),
                                                                                                                     ErrorRate:    PercentageDouble.Parse(0.05)
                                                                                                                 ),
                                                                                             Downlink:           new NetworkLinkInformation(
                                                                                                                     Capacity:     15000,
                                                                                                                     Latency:      TimeSpan.FromMilliseconds(23),
                                                                                                                     ErrorRate:    PercentageDouble.Parse(0.42)
                                                                                                                 )
                                                                                         )
                                                                                     },
                                                                      NotBefore:     Timestamp.Now - TimeSpan.FromMinutes(5),
                                                                      NotAfter:      Timestamp.Now + TimeSpan.FromHours  (6),
                                                                      Priority:      5,
                                                                      CustomData:    null
                                                                  )
                                );


                Assert.Multiple(() => {

                    Assert.That(response.Result.ResultCode,                                    Is.EqualTo(ResultCode.OK));
                    Assert.That(response.Status,                                               Is.EqualTo(NetworkTopologyStatus.Accepted));

                    Assert.That(csmsNotifyNetworkTopologyRequests.Count,                       Is.EqualTo(1), "The Network Topology Notification did not reach the CSMS!");
                    var csmsNotifyNetworkTopologyRequest = csmsNotifyNetworkTopologyRequests.First();

                    Assert.That(csmsNotifyNetworkTopologyRequest.DestinationNodeId,            Is.EqualTo(NetworkingNode_Id.CSMS));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Length,           Is.EqualTo(1));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Source,           Is.EqualTo(networkingNode1.Id));
                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkPath.Last,             Is.EqualTo(networkingNode1.Id));

                    Assert.That(csmsNotifyNetworkTopologyRequest.NetworkTopologyInformation,   Is.Not.Null);

                });

                var networkTopologyInformation = csmsNotifyNetworkTopologyRequests.First().NetworkTopologyInformation;
                if (networkTopologyInformation is not null)
                {

                    Assert.That(networkTopologyInformation.RoutingNode,                        Is.EqualTo(networkingNode1.Id));
                    Assert.That(networkTopologyInformation.Priority,                           Is.EqualTo(5));
                    Assert.That(networkTopologyInformation.Routes,                             Is.Not.Null);

                    var routes = networkTopologyInformation.Routes;
                    if (routes is not null)
                    {

                        Assert.That(routes.Values.First().NetworkingNodeId,                    Is.EqualTo(networkingNode1.Id));
                        Assert.That(routes.Values.First().Priority,                            Is.EqualTo(23));

                        Assert.That(routes.Values.First().Uplink,                              Is.Not.Null);
                        Assert.That(routes.Values.First().Uplink?.Capacity,                    Is.EqualTo(5000));
                        Assert.That(routes.Values.First().Uplink?.Latency,                     Is.EqualTo(TimeSpan.FromMilliseconds(10)));
                        Assert.That(routes.Values.First().Uplink?.ErrorRate?.Value,            Is.EqualTo(0.05));

                        Assert.That(routes.Values.First().Downlink,                            Is.Not.Null);
                        Assert.That(routes.Values.First().Downlink?.Capacity,                  Is.EqualTo(15000));
                        Assert.That(routes.Values.First().Downlink?.Latency,                   Is.EqualTo(TimeSpan.FromMilliseconds(23)));
                        Assert.That(routes.Values.First().Downlink?.ErrorRate?.Value,          Is.EqualTo(0.42));

                    }

                }

            }

        }

        #endregion


    }

}
