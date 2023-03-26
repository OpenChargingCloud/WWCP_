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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Styx;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_0.tests
{

    /// <summary>
    /// Unit tests for charging stations sending messages to the CSMS.
    /// </summary>
    [TestFixture]
    public class ChargingStationMessagesTests : AChargingStationTests
    {

        #region ChargingStation_Init_Test()

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

        #region ChargingStation_SendBootNotifications_Test()

        /// <summary>
        /// A test for sending boot notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendBootNotifications_Test()
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

                var bootNotificationRequests = new List<CS.BootNotificationRequest>();

                testCSMS01.OnBootNotificationRequest += async (timestamp, sender, bootNotificationRequest) => {
                    bootNotificationRequests.Add(bootNotificationRequest);
                };

                var reason     = BootReasons.PowerUp;
                var response1  = await chargingStation1.SendBootNotification(reason);

                Assert.AreEqual (ResultCodes.OK,                         response1.Result.ResultCode);
                Assert.AreEqual (RegistrationStatus.Accepted,            response1.Status);

                Assert.AreEqual (1,                                      bootNotificationRequests.Count);
                Assert.AreEqual (chargingStation1.ChargeBoxId,           bootNotificationRequests.First().ChargeBoxId);
                Assert.AreEqual (reason,                                 bootNotificationRequests.First().Reason);

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

            }

        }

        #endregion

        #region ChargingStation_SendFirmwareStatusNotification_Test()

        /// <summary>
        /// A test for sending firmware status notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendFirmwareStatusNotification_Test()
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

                var firmwareStatusNotifications = new List<CS.FirmwareStatusNotificationRequest>();

                testCSMS01.OnFirmwareStatusNotificationRequest += async (timestamp, sender, firmwareStatusNotification) => {
                    firmwareStatusNotifications.Add(firmwareStatusNotification);
                };

                var status     = FirmwareStatus.Installed;

                var response1  = await chargingStation1.SendFirmwareStatusNotification(
                                           Status:       status,
                                           CustomData:   null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              firmwareStatusNotifications.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   firmwareStatusNotifications.First().ChargeBoxId);
                Assert.AreEqual(status,                         firmwareStatusNotifications.First().Status);

            }

        }

        #endregion

        #region ChargingStation_SendPublishFirmwareStatusNotification_Test()

        /// <summary>
        /// A test for sending piblish firmware status notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendPublishFirmwareStatusNotification_Test()
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

                var firmwareStatusNotifications = new List<CS.PublishFirmwareStatusNotificationRequest>();

                testCSMS01.OnPublishFirmwareStatusNotificationRequest += async (timestamp, sender, firmwareStatusNotification) => {
                    firmwareStatusNotifications.Add(firmwareStatusNotification);
                };

                var status     = PublishFirmwareStatus.Published;
                var url1       = URL.Parse("https://example.org/firmware.bin");

                var response1  = await chargingStation1.SendPublishFirmwareStatusNotification(
                                           Status:                                       status,
                                           PublishFirmwareStatusNotificationRequestId:   0,
                                           DownloadLocations:                            new[] { url1 },
                                           CustomData:                                   null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              firmwareStatusNotifications.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   firmwareStatusNotifications.First().ChargeBoxId);
                Assert.AreEqual(status,                         firmwareStatusNotifications.First().Status);

            }

        }

        #endregion

        #region ChargingStation_SendHeartbeats_Test()

        /// <summary>
        /// A test for sending heartbeats to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendHeartbeats_Test()
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

                var heartbeatRequests = new List<CS.HeartbeatRequest>();

                testCSMS01.OnHeartbeatRequest += async (timestamp, sender, heartbeatRequest) => {
                    heartbeatRequests.Add(heartbeatRequest);
                };


                var response1 = await chargingStation1.SendHeartbeat();


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                Assert.IsTrue  (Timestamp.Now - response1.CurrentTime < TimeSpan.FromSeconds(10));

                Assert.AreEqual(1,                              heartbeatRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   heartbeatRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_NotifyEvent_Test()

        /// <summary>
        /// A test for notifying the CSMS about events.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyEvent_Test()
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

                var notifyEventRequests = new List<CS.NotifyEventRequest>();

                testCSMS01.OnNotifyEventRequest += async (timestamp, sender, notifyEventRequest) => {
                    notifyEventRequests.Add(notifyEventRequest);
                };

                var response1  = await chargingStation1.NotifyEvent(
                                           GeneratedAt:      Timestamp.Now,
                                           SequenceNumber:   1,
                                           EventData:        new[] {
                                                                 new EventData(
                                                                     EventId:                 Event_Id.NewRandom,
                                                                     Timestamp:               Timestamp.Now,
                                                                     Trigger:                 EventTriggers.Alerting,
                                                                     ActualValue:             "ALERTA!",
                                                                     EventNotificationType:   EventNotificationTypes.HardWiredMonitor,
                                                                     Component:               new Component(
                                                                                                  Name:         "Alert System!",
                                                                                                  Instance:     "Alert System #1",
                                                                                                  EVSE:         new EVSE(
                                                                                                                    Id:            EVSE_Id.Parse(1),
                                                                                                                    ConnectorId:   Connector_Id.Parse(1),
                                                                                                                    CustomData:    null
                                                                                                                ),
                                                                                                  CustomData:   null
                                                                                              ),
                                                                     Variable:                new Variable(
                                                                                                  Name:         "Temperature Sensors",
                                                                                                  Instance:     "Temperature Sensor #1",
                                                                                                  CustomData:   null
                                                                                              ),
                                                                     Cause:                   Event_Id.NewRandom,
                                                                     TechCode:                "Tech Code #1",
                                                                     TechInfo:                "Tech Info #1",
                                                                     Cleared:                 false,
                                                                     TransactionId:           Transaction_Id.       NewRandom,
                                                                     VariableMonitoringId:    VariableMonitoring_Id.NewRandom,
                                                                     CustomData:              null
                                                                 )
                                                             },
                                           ToBeContinued:    false,
                                           CustomData:       null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyEventRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyEventRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_SendSecurityEventNotification_Test()

        /// <summary>
        /// A test for sending security event notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendSecurityEventNotification_Test()
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

                var securityEventNotificationRequests = new List<CS.SecurityEventNotificationRequest>();

                testCSMS01.OnSecurityEventNotificationRequest += async (timestamp, sender, securityEventNotificationRequest) => {
                    securityEventNotificationRequests.Add(securityEventNotificationRequest);
                };

                var response1  = await chargingStation1.SendSecurityEventNotification(
                                           Type:         SecurityEvent.MemoryExhaustion,
                                           Timestamp:    Timestamp.Now,
                                           TechInfo:     "Too many open TCP sockets!",
                                           CustomData:   null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              securityEventNotificationRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   securityEventNotificationRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_NotifyReport_Test()

        /// <summary>
        /// A test for notifying the CSMS about reports.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyReport_Test()
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

                var notifyReportRequests = new List<CS.NotifyReportRequest>();

                testCSMS01.OnNotifyReportRequest += async (timestamp, sender, notifyReportRequest) => {
                    notifyReportRequests.Add(notifyReportRequest);
                };

                var response1  = await chargingStation1.NotifyReport(
                                           NotifyReportRequestId:   1,
                                           SequenceNumber:          1,
                                           GeneratedAt:             Timestamp.Now,
                                           ReportData:              new[] {
                                                                        new ReportData(
                                                                            Component:                new Component(
                                                                                                           Name:                 "Alert System!",
                                                                                                           Instance:             "Alert System #1",
                                                                                                           EVSE:                 new EVSE(
                                                                                                                                     Id:            EVSE_Id.Parse(1),
                                                                                                                                     ConnectorId:   Connector_Id.Parse(1),
                                                                                                                                     CustomData:    null
                                                                                                                                 ),
                                                                                                           CustomData:           null
                                                                                                       ),
                                                                             Variable:                 new Variable(
                                                                                                           Name:                 "Temperature Sensors",
                                                                                                           Instance:             "Temperature Sensor #1",
                                                                                                           CustomData:           null
                                                                                                       ),
                                                                            VariableAttributes:        new[] {
                                                                                                           new VariableAttribute(
                                                                                                               Type:             AttributeTypes.Actual,
                                                                                                               Value:            "123",
                                                                                                               Mutability:       MutabilityTypes.ReadWrite,
                                                                                                               Persistent:       true,
                                                                                                               Constant:         false,
                                                                                                               CustomData:       null
                                                                                                           )
                                                                                                       },
                                                                            VariableCharacteristics:   new VariableCharacteristics(
                                                                                                           DataType:             DataTypes.Decimal,
                                                                                                           SupportsMonitoring:   true,
                                                                                                           Unit:                 UnitsOfMeasure.Celsius(
                                                                                                                                     Multiplier:   1,
                                                                                                                                     CustomData:   null
                                                                                                                                 ),
                                                                                                           MinLimit:             0.1M,
                                                                                                           MaxLimit:             9.9M,
                                                                                                           ValuesList:           new[] { "" },
                                                                                                           CustomData:           null
                                                                                                       ),
                                                                            CustomData:                null
                                                                        )
                                                                    },
                                           CustomData:              null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyReportRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyReportRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_NotifyMonitoringReport_Test()

        /// <summary>
        /// A test for notifying the CSMS about monitoring reports.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyMonitoringReport_Test()
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

                var notifyMonitoringReportRequests = new List<CS.NotifyMonitoringReportRequest>();

                testCSMS01.OnNotifyMonitoringReportRequest += async (timestamp, sender, notifyMonitoringReportRequest) => {
                    notifyMonitoringReportRequests.Add(notifyMonitoringReportRequest);
                };

                var response1  = await chargingStation1.NotifyMonitoringReport(
                                           NotifyMonitoringReportRequestId:   1,
                                           SequenceNumber:                    1,
                                           GeneratedAt:                       Timestamp.Now,
                                           MonitoringData:                    new[] {
                                                                                  new MonitoringData(
                                                                                      Component:              new Component(
                                                                                                                  Name:             "Alert System!",
                                                                                                                  Instance:         "Alert System #1",
                                                                                                                  EVSE:             new EVSE(
                                                                                                                                        Id:            EVSE_Id.Parse(1),
                                                                                                                                        ConnectorId:   Connector_Id.Parse(1),
                                                                                                                                        CustomData:    null
                                                                                                                                    ),
                                                                                                                  CustomData:       null
                                                                                                              ),
                                                                                      Variable:               new Variable(
                                                                                                                  Name:             "Temperature Sensors",
                                                                                                                  Instance:         "Temperature Sensor #1",
                                                                                                                  CustomData:       null
                                                                                                              ),
                                                                                      VariableMonitorings:   new[] {
                                                                                                                 new VariableMonitoring(
                                                                                                                     Id:            VariableMonitoring_Id.NewRandom,
                                                                                                                     Transaction:   true,
                                                                                                                     Value:         1.01M,
                                                                                                                     Type:          MonitorTypes.Periodic,
                                                                                                                     Severity:      Severities.Warning,
                                                                                                                     CustomData:    null
                                                                                                                 )
                                                                                                             },
                                                                                      CustomData:            null
                                                                                  )
                                                                              },
                                           ToBeContinued:                     false,
                                           CustomData:                        null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyMonitoringReportRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyMonitoringReportRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_SendLogStatusNotification_Test()

        /// <summary>
        /// A test for sending log status notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendLogStatusNotification_Test()
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

                var securityEventNotificationRequests = new List<CS.LogStatusNotificationRequest>();

                testCSMS01.OnLogStatusNotificationRequest += async (timestamp, sender, securityEventNotificationRequest) => {
                    securityEventNotificationRequests.Add(securityEventNotificationRequest);
                };

                var response1  = await chargingStation1.SendLogStatusNotification(
                                            Status:         UploadLogStatus.Uploaded,
                                            LogRequestId:   1,
                                            CustomData:     null
                                        );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              securityEventNotificationRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   securityEventNotificationRequests.First().ChargeBoxId);

            }

        }

        #endregion


        #region ChargingStation_TransferTextData_Test()

        /// <summary>
        /// A test for transfering text data to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_TransferTextData_Test()
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

                var dataTransferRequests = new List<CS.DataTransferRequest>();

                testCSMS01.OnIncomingDataTransferRequest += async (timestamp, sender, dataTransferRequest) => {
                    dataTransferRequests.Add(dataTransferRequest);
                };

                var vendorId   = "GraphDefined OEM";
                var messageId  = RandomExtensions.RandomString(10);
                var data       = RandomExtensions.RandomString(40);

                var response1  = await chargingStation1.TransferData(
                                     VendorId:    vendorId,
                                     MessageId:   messageId,
                                     Data:        data,
                                     CustomData:  null
                                 );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                Assert.AreEqual(data.Reverse(),                 response1.Data?.ToString());

                Assert.AreEqual(1,                              dataTransferRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   dataTransferRequests.First().ChargeBoxId);
                Assert.AreEqual(vendorId,                       dataTransferRequests.First().VendorId);
                Assert.AreEqual(messageId,                      dataTransferRequests.First().MessageId);
                Assert.AreEqual(data,                           dataTransferRequests.First().Data?.ToString());

            }

        }

        #endregion

        #region ChargingStation_TransferJObjectData_Test()

        /// <summary>
        /// A test for transfering JObject data to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_TransferJObjectData_Test()
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

                var dataTransferRequests = new List<CS.DataTransferRequest>();

                testCSMS01.OnIncomingDataTransferRequest += async (timestamp, sender, dataTransferRequest) => {
                    dataTransferRequests.Add(dataTransferRequest);
                };

                var vendorId   = "GraphDefined OEM";
                var messageId  = RandomExtensions.RandomString(10);
                var data       = new JObject(
                                     new JProperty(
                                         "key",
                                         RandomExtensions.RandomString(40)
                                     )
                                 );

                var response1  = await chargingStation1.TransferData(
                                     VendorId:    vendorId,
                                     MessageId:   messageId,
                                     Data:        data,
                                     CustomData:  null
                                 );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                Assert.AreEqual(JTokenType.Object,              response1.Data?.Type);
                Assert.AreEqual(data["key"]?.Value<String>(),   response1.Data?["key"]?.Value<String>()?.Reverse());

                Assert.AreEqual(1,                              dataTransferRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   dataTransferRequests.First().ChargeBoxId);
                Assert.AreEqual(vendorId,                       dataTransferRequests.First().VendorId);
                Assert.AreEqual(messageId,                      dataTransferRequests.First().MessageId);
                Assert.AreEqual(JTokenType.Object,              dataTransferRequests.First().Data?.Type);
                Assert.AreEqual(data["key"]?.Value<String>(),   dataTransferRequests.First().Data?["key"]?.Value<String>());

            }

        }

        #endregion

        #region ChargingStation_TransferJArrayData_Test()

        /// <summary>
        /// A test for transfering JArray data to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_TransferJArrayData_Test()
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

                var dataTransferRequests = new List<CS.DataTransferRequest>();

                testCSMS01.OnIncomingDataTransferRequest += async (timestamp, sender, dataTransferRequest) => {
                    dataTransferRequests.Add(dataTransferRequest);
                };

                var vendorId   = "GraphDefined OEM";
                var messageId  = RandomExtensions.RandomString(10);
                var data       = new JArray(
                                     RandomExtensions.RandomString(40)
                                 );

                var response1  = await chargingStation1.TransferData(
                                     VendorId:    vendorId,
                                     MessageId:   messageId,
                                     Data:        data,
                                     CustomData:  null
                                 );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                Assert.AreEqual(JTokenType.Array,               response1.Data?.Type);
                Assert.AreEqual(data[0]?.Value<String>(),       response1.Data?[0]?.Value<String>()?.Reverse());

                Assert.AreEqual(1,                              dataTransferRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   dataTransferRequests.First().ChargeBoxId);
                Assert.AreEqual(vendorId,                       dataTransferRequests.First().VendorId);
                Assert.AreEqual(messageId,                      dataTransferRequests.First().MessageId);
                Assert.AreEqual(JTokenType.Array,               dataTransferRequests.First().Data?.Type);
                Assert.AreEqual(data[0]?.Value<String>(),       dataTransferRequests.First().Data?[0]?.Value<String>());

            }

        }

        #endregion


        #region ChargingStation_SendCertificateSigningRequest_Test()

        /// <summary>
        /// A test for sending a certificate signing request to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendCertificateSigningRequest_Test()
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

                var notifyReportRequests = new List<CS.SignCertificateRequest>();

                testCSMS01.OnSignCertificateRequest += async (timestamp, sender, notifyReportRequest) => {
                    notifyReportRequests.Add(notifyReportRequest);
                };

                var response1  = await chargingStation1.SendCertificateSigningRequest(
                                           CSR:               "0x1234",
                                           CertificateType:   CertificateSigningUse.ChargingStationCertificate,
                                           CustomData:        null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyReportRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyReportRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_Get15118EVCertificate_Test()

        /// <summary>
        /// A test for receiving a 15118 EV contract certificate from the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_Get15118EVCertificate_Test()
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

                var notifyReportRequests = new List<CS.Get15118EVCertificateRequest>();

                testCSMS01.OnGet15118EVCertificateRequest += async (timestamp, sender, notifyReportRequest) => {
                    notifyReportRequests.Add(notifyReportRequest);
                };

                var response1  = await chargingStation1.Get15118EVCertificate(
                                           ISO15118SchemaVersion:   ISO15118SchemaVersion.Parse("15118-20:BastelBrothers"),
                                           CertificateAction:       CertificateAction.Install,
                                           EXIRequest:              EXIData.Parse("0x1234"),
                                           CustomData:              null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyReportRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyReportRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_GetCertificateStatus_Test()

        /// <summary>
        /// A test for notifying the CSMS about reports.
        /// </summary>
        [Test]
        public async Task ChargingStation_GetCertificateStatus_Test()
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

                var notifyReportRequests = new List<CS.GetCertificateStatusRequest>();

                testCSMS01.OnGetCertificateStatusRequest += async (timestamp, sender, notifyReportRequest) => {
                    notifyReportRequests.Add(notifyReportRequest);
                };

                var response1  = await chargingStation1.GetCertificateStatus(
                                           OCSPRequestData:   new OCSPRequestData(
                                                                  HashAlgorithm:    HashAlgorithms.SHA256,
                                                                  IssuerNameHash:   "0x1234",
                                                                  IssuerKeyHash:    "0x5678",
                                                                  SerialNumber:     "12345678",
                                                                  ResponderURL:     URL.Parse("https://example.org/12345678"),
                                                                  CustomData:       null
                                                              ),
                                           CustomData:        null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyReportRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyReportRequests.First().ChargeBoxId);

            }

        }

        #endregion


        #region ChargingStation_SendReservationStatusUpdate_Test()

        /// <summary>
        /// A test for sending reservation status updates to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendReservationStatusUpdate_Test()
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

                var securityEventNotificationRequests = new List<CS.ReservationStatusUpdateRequest>();

                testCSMS01.OnReservationStatusUpdateRequest += async (timestamp, sender, securityEventNotificationRequest) => {
                    securityEventNotificationRequests.Add(securityEventNotificationRequest);
                };

                var response1  = await chargingStation1.SendReservationStatusUpdate(
                                           ReservationId:             Reservation_Id.NewRandom,
                                           ReservationUpdateStatus:   ReservationUpdateStatus.Expired,
                                           CustomData:                null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              securityEventNotificationRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   securityEventNotificationRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_Authorize_Test()

        /// <summary>
        /// A test for authorizing id tokens against the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_Authorize_Test()
        {

            Assert.IsNotNull(testCSMS01);
            Assert.IsNotNull(testBackendWebSockets01);
            Assert.IsNotNull(chargingStation1);
            Assert.IsNotNull(chargingStation2);
            Assert.IsNotNull(chargingStation3);

            if (testCSMS01     is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var authorizeRequests = new List<CS.AuthorizeRequest>();

                testCSMS01.OnAuthorizeRequest += async (timestamp, sender, authorizeRequest) => {
                    authorizeRequests.Add(authorizeRequest);
                };

                var idToken   = IdToken.NewRandomRFID();
                var response1 = await chargingStation1.Authorize(idToken);


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                Assert.AreEqual(AuthorizationStatus.Accepted,   response1.IdTokenInfo.Status);

                Assert.AreEqual(1,                              authorizeRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   authorizeRequests.First().ChargeBoxId);
                Assert.AreEqual(idToken,                        authorizeRequests.First().IdToken);

            }

        }

        #endregion

        #region ChargingStation_NotifyEVChargingNeeds_Test()

        /// <summary>
        /// A test for notifying the CSMS about EV charging needs.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyEVChargingNeeds_Test()
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

                var notifyEVChargingNeedsRequests = new List<CS.NotifyEVChargingNeedsRequest>();

                testCSMS01.OnNotifyEVChargingNeedsRequest += async (timestamp, sender, notifyEVChargingNeedsRequest) => {
                    notifyEVChargingNeedsRequests.Add(notifyEVChargingNeedsRequest);
                };

                var response1  = await chargingStation1.NotifyEVChargingNeeds(
                                           EVSEId:              EVSE_Id.Parse(1),
                                           ChargingNeeds:       new ChargingNeeds(
                                                                    RequestedEnergyTransfer:   EnergyTransferModes.AC_ThreePhases,
                                                                    DepartureTime:             Timestamp.Now + TimeSpan.FromHours(3),
                                                                    ACChargingParameters:      new ACChargingParameters(
                                                                                                   EnergyAmount:         20,
                                                                                                   EVMinCurrent:          6,
                                                                                                   EVMaxCurrent:         32,
                                                                                                   EVMaxVoltage:        230,
                                                                                                   CustomData:         null
                                                                                               ),
                                                                    DCChargingParameters:      new DCChargingParameters(
                                                                                                   EVMaxCurrent:         20,
                                                                                                   EVMaxVoltage:        900,
                                                                                                   EnergyAmount:        300,
                                                                                                   EVMaxPower:           60,
                                                                                                   StateOfCharge:        23,
                                                                                                   EVEnergyCapacity:    250,
                                                                                                   FullSoC:              95,
                                                                                                   BulkSoC:              80,
                                                                                                   CustomData:         null
                                                                                               ),
                                                                    CustomData:                null
                                                                ),
                                           MaxScheduleTuples:   16,
                                           CustomData:          null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyEVChargingNeedsRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyEVChargingNeedsRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_SendTransactionEvent_Test()

        /// <summary>
        /// A test for sending a transaction event to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendTransactionEvent_Test()
        {

            Assert.IsNotNull(testCSMS01);
            Assert.IsNotNull(testBackendWebSockets01);
            Assert.IsNotNull(chargingStation1);
            Assert.IsNotNull(chargingStation2);
            Assert.IsNotNull(chargingStation3);

            if (testCSMS01     is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var transactionEventRequests = new List<CS.TransactionEventRequest>();

                testCSMS01.OnTransactionEventRequest += async (timestamp, sender, transactionEventRequest) => {
                    transactionEventRequests.Add(transactionEventRequest);
                };

                var evseId          = EVSE_Id.     Parse(1);
                var connectorId     = Connector_Id.Parse(1);
                var idToken         = IdToken.     NewRandomRFID();
                var startTimestamp  = Timestamp.Now;
                var meterStart      = 1234UL;
                var reservationId   = Reservation_Id.NewRandom;

                var response1       = await chargingStation1.SendTransactionEvent(

                                          EventType:            TransactionEvents.Started,
                                          Timestamp:            startTimestamp,
                                          TriggerReason:        TriggerReasons.Authorized,
                                          SequenceNumber:       0,
                                          TransactionInfo:      new Transaction(
                                                                    TransactionId:       Transaction_Id.NewRandom,
                                                                    ChargingState:       ChargingStates.Charging,
                                                                    TimeSpentCharging:   TimeSpan.FromSeconds(3),
                                                                    StoppedReason:       null,
                                                                    RemoteStartId:       null,
                                                                    CustomData:          null
                                                                ),

                                          Offline:              null,
                                          NumberOfPhasesUsed:   null,
                                          CableMaxCurrent:      null,
                                          ReservationId:        reservationId,
                                          IdToken:              idToken,
                                          EVSE:                 new EVSE(
                                                                    Id:                  evseId,
                                                                    ConnectorId:         connectorId,
                                                                    CustomData:          null
                                                                ),
                                          MeterValues:          new MeterValue[] {
                                                                    new MeterValue(
                                                                        SampledValues:   new SampledValue[] {

                                                                                             new SampledValue(
                                                                                                 Value:              meterStart,
                                                                                                 Context:            ReadingContexts.TransactionBegin,
                                                                                                 Measurand:          Measurands.Energy_Active_Export_Interval,
                                                                                                 Phase:              Phases.L1,
                                                                                                 Location:           MeasurementLocations.Outlet,
                                                                                                 SignedMeterValue:   new SignedMeterValue(
                                                                                                                         SignedMeterData:   meterStart.ToString(),
                                                                                                                         SigningMethod:     "secp256r1",
                                                                                                                         EncodingMethod:    "base64",
                                                                                                                         PublicKey:         "0x1234",
                                                                                                                         CustomData:        null
                                                                                                                     ),
                                                                                                 UnitOfMeasure:      UnitsOfMeasure.kW(
                                                                                                                         Multiplier:   0,
                                                                                                                         CustomData:   null
                                                                                                                     ),
                                                                                                 CustomData:         null
                                                                                             )

                                                                                         },
                                                                        Timestamp:       startTimestamp,
                                                                        CustomData:      null
                                                                    )
                                                                },
                                          CustomData:           null

                                      );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);
                //Assert.AreEqual(AuthorizationStatus.Accepted,   response1.IdTokenInfo.Status);
                //Assert.IsTrue  (response1.TransactionId.IsNotNullOrEmpty);

                Assert.AreEqual(1,                              transactionEventRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   transactionEventRequests.First().ChargeBoxId);
                //Assert.AreEqual(connectorId,                    transactionEventRequests.First().ConnectorId);
                //Assert.AreEqual(idToken,                        transactionEventRequests.First().IdTag);
                //Assert.AreEqual(startTimestamp.ToIso8601(),     transactionEventRequests.First().StartTimestamp.ToIso8601());
                //Assert.AreEqual(meterStart,                     transactionEventRequests.First().MeterStart);
                //Assert.AreEqual(reservationId,                  transactionEventRequests.First().ReservationId);

            }

        }

        #endregion

        #region ChargingStation_SendStatusNotification_Test()

        /// <summary>
        /// A test for sending status notifications to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendStatusNotification_Test()
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

                var statusNotificationRequests = new List<CS.StatusNotificationRequest>();

                testCSMS01.OnStatusNotificationRequest += async (timestamp, sender, statusNotificationRequest) => {
                    statusNotificationRequests.Add(statusNotificationRequest);
                };

                var evseId           = EVSE_Id.     Parse(1);
                var connectorId      = Connector_Id.Parse(1);
                var connectorStatus  = ConnectorStatus.Available;
                var statusTimestamp  = Timestamp.Now;

                var response1        = await chargingStation1.SendStatusNotification(
                                           EVSEId:        evseId,
                                           ConnectorId:   connectorId,
                                           Timestamp:     statusTimestamp,
                                           Status:        connectorStatus,
                                           CustomData:    null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              statusNotificationRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   statusNotificationRequests.First().ChargeBoxId);
                Assert.AreEqual(evseId,                         statusNotificationRequests.First().EVSEId);
                Assert.AreEqual(connectorId,                    statusNotificationRequests.First().ConnectorId);
                Assert.AreEqual(connectorStatus,                statusNotificationRequests.First().ConnectorStatus);
                Assert.AreEqual(statusTimestamp.ToIso8601(),    statusNotificationRequests.First().Timestamp.ToIso8601());

            }

        }

        #endregion

        #region ChargingStation_SendMeterValues_Test()

        /// <summary>
        /// A test for sending meter values to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendMeterValues_Test()
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

                var meterValuesRequests = new List<CS.MeterValuesRequest>();

                testCSMS01.OnMeterValuesRequest += async (timestamp, sender, meterValuesRequest) => {
                    meterValuesRequests.Add(meterValuesRequest);
                };

                var evseId       = EVSE_Id.Parse(1);
                var meterValues  = new MeterValue[] {
                                       new MeterValue(
                                           new SampledValue[] {
                                               new SampledValue(
                                                   Value:              1.01M,
                                                   Context:            ReadingContexts.TransactionBegin,
                                                   Measurand:          Measurands.Current_Import,
                                                   Phase:              Phases.L1,
                                                   Location:           MeasurementLocations.Outlet,
                                                   SignedMeterValue:   new SignedMeterValue(
                                                                           SignedMeterData:   "1.01",
                                                                           SigningMethod:     "secp256r1_1.01",
                                                                           EncodingMethod:    "base64_1.01",
                                                                           PublicKey:         "pubkey_1.01",
                                                                           CustomData:        null
                                                                       ),
                                                   UnitOfMeasure:      UnitsOfMeasure.kW(
                                                                           Multiplier:   1,
                                                                           CustomData:   null
                                                                       ),
                                                   CustomData:         null
                                               ),
                                               new SampledValue(
                                                   Value:              1.02M,
                                                   Context:            ReadingContexts.TransactionBegin,
                                                   Measurand:          Measurands.Voltage,
                                                   Phase:              Phases.L2,
                                                   Location:           MeasurementLocations.Inlet,
                                                   SignedMeterValue:   new SignedMeterValue(
                                                                           SignedMeterData:   "1.02",
                                                                           SigningMethod:     "secp256r1_1.02",
                                                                           EncodingMethod:    "base64_1.02",
                                                                           PublicKey:         "pubkey_1.02",
                                                                           CustomData:        null
                                                                       ),
                                                   UnitOfMeasure:      UnitsOfMeasure.kW(
                                                                           Multiplier:   2,
                                                                           CustomData:   null
                                                                       ),
                                                   CustomData:         null
                                               )
                                           },
                                           Timestamp.Now - TimeSpan.FromMinutes(5)
                                       ),
                                       new MeterValue(
                                           new SampledValue[] {
                                               new SampledValue(
                                                   Value:              2.01M,
                                                   Context:            ReadingContexts.TransactionEnd,
                                                   Measurand:          Measurands.Current_Offered,
                                                   Phase:              Phases.L3,
                                                   Location:           MeasurementLocations.Cable,
                                                   SignedMeterValue:   new SignedMeterValue(
                                                                           SignedMeterData:   "2.01",
                                                                           SigningMethod:     "secp256r1_2.01",
                                                                           EncodingMethod:    "base64_2.01",
                                                                           PublicKey:         "pubkey_2.01",
                                                                           CustomData:        null
                                                                       ),
                                                   UnitOfMeasure:      UnitsOfMeasure.kW(
                                                                           Multiplier:   3,
                                                                           CustomData:   null
                                                                       ),
                                                   CustomData:         null
                                               ),
                                               new SampledValue(
                                                   Value:              2.02M,
                                                   Context:            ReadingContexts.TransactionEnd,
                                                   Measurand:          Measurands.Frequency,
                                                   Phase:              Phases.N,
                                                   Location:           MeasurementLocations.EV,
                                                   SignedMeterValue:   new SignedMeterValue(
                                                                           SignedMeterData:   "2.02",
                                                                           SigningMethod:     "secp256r1_2.02",
                                                                           EncodingMethod:    "base64_2.02",
                                                                           PublicKey:         "pubkey_2.02",
                                                                           CustomData:        null
                                                                       ),
                                                   UnitOfMeasure:      UnitsOfMeasure.kW(
                                                                           Multiplier:   4,
                                                                           CustomData:   null
                                                                       ),
                                                   CustomData:         null
                                               )
                                           },
                                           Timestamp.Now
                                       )
                                   };

                var response1    = await chargingStation1.SendMeterValues(
                                       EVSEId:        evseId,
                                       MeterValues:   meterValues,
                                       CustomData:    null
                                   );


                Assert.AreEqual (ResultCodes.OK,                                                  response1.Result.ResultCode);

                Assert.AreEqual (1,                                                               meterValuesRequests.Count);
                Assert.AreEqual (chargingStation1.ChargeBoxId,                                    meterValuesRequests.First().ChargeBoxId);
                Assert.AreEqual (evseId,                                                          meterValuesRequests.First().EVSEId);

                Assert.AreEqual (meterValues.Length,                                              meterValuesRequests.First().MeterValues.Count());
                Assert.IsTrue   (meterValues.ElementAt(0).Timestamp - meterValuesRequests.First().MeterValues.ElementAt(0).Timestamp < TimeSpan.FromSeconds(2));
                Assert.IsTrue   (meterValues.ElementAt(1).Timestamp - meterValuesRequests.First().MeterValues.ElementAt(1).Timestamp < TimeSpan.FromSeconds(2));

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.Count(),                  meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.Count());
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.Count(),                  meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.Count());

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Value,       meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Value);
                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Value,       meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Value);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Value,       meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Value);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Value,       meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Value);

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Context,     meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Context);
                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Context,     meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Context);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Context,     meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Context);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Context,     meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Context);

                //Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Format,      meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Format);
                //Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Format,      meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Format);
                //Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Format,      meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Format);
                //Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Format,      meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Format);

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Measurand,   meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Measurand);
                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Measurand,   meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Measurand);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Measurand,   meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Measurand);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Measurand,   meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Measurand);

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Phase,       meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Phase);
                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Phase,       meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Phase);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Phase,       meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Phase);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Phase,       meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Phase);

                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Location,    meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Location);
                Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Location,    meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Location);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Location,    meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Location);
                Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Location,    meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Location);

                //Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(0).Unit,        meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(0).Unit);
                //Assert.AreEqual (meterValues.ElementAt(0).SampledValues.ElementAt(1).Unit,        meterValuesRequests.First().MeterValues.ElementAt(0).SampledValues.ElementAt(1).Unit);
                //Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(0).Unit,        meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(0).Unit);
                //Assert.AreEqual (meterValues.ElementAt(1).SampledValues.ElementAt(1).Unit,        meterValuesRequests.First().MeterValues.ElementAt(1).SampledValues.ElementAt(1).Unit);

            }

        }

        #endregion

        #region ChargingStation_NotifyChargingLimit_Test()

        /// <summary>
        /// A test for notifying the CSMS about charging limits.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyChargingLimit_Test()
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

                var notifyChargingLimitRequests = new List<CS.NotifyChargingLimitRequest>();

                testCSMS01.OnNotifyChargingLimitRequest += async (timestamp, sender, notifyChargingLimitRequest) => {
                    notifyChargingLimitRequests.Add(notifyChargingLimitRequest);
                };

                var response1  = await chargingStation1.NotifyChargingLimit(
                                           ChargingLimit:       new ChargingLimit(
                                                                    ChargingLimitSource:   ChargingLimitSources.SO,
                                                                    IsGridCritical:        true,
                                                                    CustomData:            null
                                                                ),
                                           ChargingSchedules:   new[] {
                                                                    new ChargingSchedule(
                                                                        Id:                        ChargingSchedule_Id.NewRandom(),
                                                                        ChargingRateUnit:          ChargingRateUnits.Watts,
                                                                        ChargingSchedulePeriods:   new[] {
                                                                                                       new ChargingSchedulePeriod(
                                                                                                           StartPeriod:      TimeSpan.Zero,
                                                                                                           Limit:            20,
                                                                                                           NumberOfPhases:   3,
                                                                                                           PhaseToUse:       PhasesToUse.Three,
                                                                                                           CustomData:       null
                                                                                                       )
                                                                                                   },
                                                                        StartSchedule:             Timestamp.Now,
                                                                        Duration:                  TimeSpan.FromMinutes(30),
                                                                        MinChargingRate:           6,
                                                                        SalesTariff:               new SalesTariff(
                                                                                                       Id:                   SalesTariff_Id.NewRandom,
                                                                                                       SalesTariffEntries:   new[] {
                                                                                                                                 new SalesTariffEntry(
                                                                                                                                     RelativeTimeInterval:   new RelativeTimeInterval(
                                                                                                                                                                 Start:        TimeSpan.Zero,
                                                                                                                                                                 Duration:     TimeSpan.FromMinutes(30),
                                                                                                                                                                 CustomData:   null
                                                                                                                                                             ),
                                                                                                                                     EPriceLevel:            1,
                                                                                                                                     ConsumptionCosts:       new[] {
                                                                                                                                                                 new ConsumptionCost(
                                                                                                                                                                     StartValue:   1,
                                                                                                                                                                     Costs:        new[] {
                                                                                                                                                                                       new Cost(
                                                                                                                                                                                           CostKind:           CostKinds.CarbonDioxideEmission,
                                                                                                                                                                                           Amount:             200,
                                                                                                                                                                                           AmountMultiplier:   23,
                                                                                                                                                                                           CustomData:         null
                                                                                                                                                                                       )
                                                                                                                                                                                   },
                                                                                                                                                                     CustomData:   null
                                                                                                                                                                 )
                                                                                                                                                             },
                                                                                                                                     CustomData:             null
                                                                                                                                 )
                                                                                                                             },
                                                                                                       Description:          "Green Charging ++",
                                                                                                       NumEPriceLevels:      1,
                                                                                                       CustomData:           null
                                                                                                   ),
                                                                        CustomData:                null
                                                                    )
                                                                },
                                           EVSEId:              EVSE_Id.Parse("1"),
                                           CustomData:          null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyChargingLimitRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyChargingLimitRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_SendClearedChargingLimit_Test()

        /// <summary>
        /// A test for indicating a cleared charging limit to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_SendClearedChargingLimit_Test()
        {

            Assert.IsNotNull(testCSMS01);
            Assert.IsNotNull(testBackendWebSockets01);
            Assert.IsNotNull(chargingStation1);
            Assert.IsNotNull(chargingStation2);
            Assert.IsNotNull(chargingStation3);

            if (testCSMS01     is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var transactionEventRequests = new List<CS.ClearedChargingLimitRequest>();

                testCSMS01.OnClearedChargingLimitRequest += async (timestamp, sender, transactionEventRequest) => {
                    transactionEventRequests.Add(transactionEventRequest);
                };

                var response  = await chargingStation1.SendClearedChargingLimit(
                                    ChargingLimitSource:   ChargingLimitSources.SO,
                                    EVSEId:                EVSE_Id.Parse("1"),
                                    CustomData:            null
                                );


                Assert.AreEqual(ResultCodes.OK,                 response.Result.ResultCode);

                Assert.AreEqual(1,                              transactionEventRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   transactionEventRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_ReportChargingProfiles_Test()

        /// <summary>
        /// A test for reporting charging profiles to the CSMS.
        /// </summary>
        [Test]
        public async Task ChargingStation_ReportChargingProfiles_Test()
        {

            Assert.IsNotNull(testCSMS01);
            Assert.IsNotNull(testBackendWebSockets01);
            Assert.IsNotNull(chargingStation1);
            Assert.IsNotNull(chargingStation2);
            Assert.IsNotNull(chargingStation3);

            if (testCSMS01     is not null &&
                testBackendWebSockets01 is not null &&
                chargingStation1        is not null &&
                chargingStation2        is not null &&
                chargingStation3        is not null)
            {

                var transactionEventRequests = new List<CS.ReportChargingProfilesRequest>();

                testCSMS01.OnReportChargingProfilesRequest += async (timestamp, sender, transactionEventRequest) => {
                    transactionEventRequests.Add(transactionEventRequest);
                };

                var response  = await chargingStation1.ReportChargingProfiles(
                                    ReportChargingProfilesRequestId:   1,
                                    ChargingLimitSource:               ChargingLimitSources.SO,
                                    EVSEId:                            EVSE_Id.Parse("1"),
                                    ChargingProfiles:                  new[] {
                                                                           new ChargingProfile(
                                                                               ChargingProfileId:        ChargingProfile_Id.NewRandom,
                                                                               StackLevel:               1,
                                                                               ChargingProfilePurpose:   ChargingProfilePurposes.TxDefaultProfile,
                                                                               ChargingProfileKind:      ChargingProfileKinds.   Absolute,
                                                                               ChargingSchedules:        new[] {
                                                                                                             new ChargingSchedule(
                                                                                                                 Id:                        ChargingSchedule_Id.NewRandom(),
                                                                                                                 ChargingRateUnit:          ChargingRateUnits.Watts,
                                                                                                                 ChargingSchedulePeriods:   new[] {
                                                                                                                                                new ChargingSchedulePeriod(
                                                                                                                                                    StartPeriod:      TimeSpan.Zero,
                                                                                                                                                    Limit:            20,
                                                                                                                                                    NumberOfPhases:   3,
                                                                                                                                                    PhaseToUse:       PhasesToUse.Three,
                                                                                                                                                    CustomData:       null
                                                                                                                                                )
                                                                                                                                            },
                                                                                                                 StartSchedule:             Timestamp.Now,
                                                                                                                 Duration:                  TimeSpan.FromMinutes(30),
                                                                                                                 MinChargingRate:           6,
                                                                                                                 SalesTariff:               new SalesTariff(
                                                                                                                                                Id:                   SalesTariff_Id.NewRandom,
                                                                                                                                                SalesTariffEntries:   new[] {
                                                                                                                                                                          new SalesTariffEntry(
                                                                                                                                                                              RelativeTimeInterval:   new RelativeTimeInterval(
                                                                                                                                                                                                          Start:        TimeSpan.Zero,
                                                                                                                                                                                                          Duration:     TimeSpan.FromMinutes(30),
                                                                                                                                                                                                          CustomData:   null
                                                                                                                                                                                                      ),
                                                                                                                                                                              EPriceLevel:            1,
                                                                                                                                                                              ConsumptionCosts:       new[] {
                                                                                                                                                                                                          new ConsumptionCost(
                                                                                                                                                                                                              StartValue:   1,
                                                                                                                                                                                                              Costs:        new[] {
                                                                                                                                                                                                                                new Cost(
                                                                                                                                                                                                                                    CostKind:           CostKinds.CarbonDioxideEmission,
                                                                                                                                                                                                                                    Amount:             200,
                                                                                                                                                                                                                                    AmountMultiplier:   23,
                                                                                                                                                                                                                                    CustomData:         null
                                                                                                                                                                                                                                )
                                                                                                                                                                                                                            },
                                                                                                                                                                                                              CustomData:   null
                                                                                                                                                                                                          )
                                                                                                                                                                                                      },
                                                                                                                                                                              CustomData:             null
                                                                                                                                                                          )
                                                                                                                                                                      },
                                                                                                                                                Description:          "Green Charging ++",
                                                                                                                                                NumEPriceLevels:      1,
                                                                                                                                                CustomData:           null
                                                                                                                                            ),
                                                                                                                 CustomData:                null
                                                                                                             )
                                                                                                         },
                                                                               CustomData:               null
                                                                           )
                                                                       },
                                    ToBeContinued:                     false,
                                    CustomData:                        null
                                );


                Assert.AreEqual(ResultCodes.OK,                 response.Result.ResultCode);

                Assert.AreEqual(1,                              transactionEventRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   transactionEventRequests.First().ChargeBoxId);

            }

        }

        #endregion


        #region ChargingStation_NotifyDisplayMessages_Test()

        /// <summary>
        /// A test for notifying the CSMS about display messages.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyDisplayMessages_Test()
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

                var notifyDisplayMessagesRequests = new List<CS.NotifyDisplayMessagesRequest>();

                testCSMS01.OnNotifyDisplayMessagesRequest += async (timestamp, sender, notifyDisplayMessagesRequest) => {
                    notifyDisplayMessagesRequests.Add(notifyDisplayMessagesRequest);
                };

                var response1  = await chargingStation1.NotifyDisplayMessages(
                                           NotifyDisplayMessagesRequestId:   1,
                                           MessageInfos:                     new[] {
                                                                                 new MessageInfo(
                                                                                     Id:               DisplayMessage_Id.NewRandom,
                                                                                     Priority:         MessagePriorities.InFront,
                                                                                     Message:          new MessageContent(
                                                                                                           Content:      "Hello World!",
                                                                                                           Format:       MessageFormats.UTF8,
                                                                                                           Language:     Language_Id.Parse("EN"),
                                                                                                           CustomData:   null
                                                                                                       ),
                                                                                     State:            MessageStates.Charging,
                                                                                     StartTimestamp:   Timestamp.Now,
                                                                                     EndTimestamp:     Timestamp.Now + TimeSpan.FromHours(3),
                                                                                     TransactionId:    Transaction_Id.NewRandom,
                                                                                     Display:          new Component(
                                                                                                           Name:         "Big Displays",
                                                                                                           Instance:     "Big Display #1",
                                                                                                           EVSE:         new EVSE(
                                                                                                                             Id:            EVSE_Id.     Parse(1),
                                                                                                                             ConnectorId:   Connector_Id.Parse(1),
                                                                                                                             CustomData:    null
                                                                                                                         ),
                                                                                                           CustomData:   null
                                                                                                       ),
                                                                                     CustomData:       null
                                                                                 )
                                                                             },
                                           CustomData:                       null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyDisplayMessagesRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyDisplayMessagesRequests.First().ChargeBoxId);

            }

        }

        #endregion

        #region ChargingStation_NotifyCustomerInformation_Test()

        /// <summary>
        /// A test for notifying the CSMS about customer information.
        /// </summary>
        [Test]
        public async Task ChargingStation_NotifyCustomerInformation_Test()
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

                var notifyCustomerInformationRequests = new List<CS.NotifyCustomerInformationRequest>();

                testCSMS01.OnNotifyCustomerInformationRequest += async (timestamp, sender, notifyCustomerInformationRequest) => {
                    notifyCustomerInformationRequests.Add(notifyCustomerInformationRequest);
                };

                var response1  = await chargingStation1.NotifyCustomerInformation(
                                           NotifyCustomerInformationRequestId:   1,
                                           Data:                                 "Hello World!",
                                           SequenceNumber:                       1,
                                           GeneratedAt:                          Timestamp.Now,
                                           ToBeContinued:                        false,
                                           CustomData:                           null
                                       );


                Assert.AreEqual(ResultCodes.OK,                 response1.Result.ResultCode);

                Assert.AreEqual(1,                              notifyCustomerInformationRequests.Count);
                Assert.AreEqual(chargingStation1.ChargeBoxId,   notifyCustomerInformationRequests.First().ChargeBoxId);

            }

        }

        #endregion


    }

}