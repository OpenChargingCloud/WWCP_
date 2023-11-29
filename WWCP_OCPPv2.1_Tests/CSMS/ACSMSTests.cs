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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;

using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.tests.CSMS
{

    /// <summary>
    /// Central system test defaults.
    /// </summary>
    public abstract class ACSMSTests
    {

        #region Data

        protected TestCSMS?                     testCSMS01;
        protected TestCSMS?                     testCSMS02;
        protected TestCSMS?                     testCSMS03;

        protected CSMSWSServer?                 testBackendWebSockets01;
        protected CSMSWSServer?                 testBackendWebSockets02;
        protected CSMSWSServer?                 testBackendWebSockets03;


        protected List<LogJSONRequest>?         csms1WebSocketJSONMessagesReceived;
        protected List<LogDataJSONResponse>?    csms1WebSocketJSONMessageResponsesSent;
        protected List<LogJSONRequest>?         csms1WebSocketJSONMessagesSent;
        protected List<LogDataJSONResponse>?    csms1WebSocketJSONMessageResponsesReceived;

        protected List<LogBinaryRequest>?       csms1WebSocketBinaryMessagesReceived;
        protected List<LogDataBinaryResponse>?  csms1WebSocketBinaryMessageResponsesSent;
        protected List<LogBinaryRequest>?       csms1WebSocketBinaryMessagesSent;
        protected List<LogDataBinaryResponse>?  csms1WebSocketBinaryMessageResponsesReceived;


        protected List<LogJSONRequest>?         csms2WebSocketJSONMessagesReceived;
        protected List<LogDataJSONResponse>?    csms2WebSocketJSONMessageResponsesSent;
        protected List<LogJSONRequest>?         csms2WebSocketJSONMessagesSent;
        protected List<LogDataJSONResponse>?    csms2WebSocketJSONMessageResponsesReceived;

        protected List<LogBinaryRequest>?       csms2WebSocketBinaryMessagesReceived;
        protected List<LogDataBinaryResponse>?  csms2WebSocketBinaryMessageResponsesSent;
        protected List<LogBinaryRequest>?       csms2WebSocketBinaryMessagesSent;
        protected List<LogDataBinaryResponse>?  csms2WebSocketBinaryMessageResponsesReceived;


        protected List<LogJSONRequest>?         csms3WebSocketJSONMessagesReceived;
        protected List<LogDataJSONResponse>?    csms3WebSocketJSONMessageResponsesSent;
        protected List<LogJSONRequest>?         csms3WebSocketJSONMessagesSent;
        protected List<LogDataJSONResponse>?    csms3WebSocketJSONMessageResponsesReceived;

        protected List<LogBinaryRequest>?       csms3WebSocketBinaryMessagesReceived;
        protected List<LogDataBinaryResponse>?  csms3WebSocketBinaryMessageResponsesSent;
        protected List<LogBinaryRequest>?       csms3WebSocketBinaryMessagesSent;
        protected List<LogDataBinaryResponse>?  csms3WebSocketBinaryMessageResponsesReceived;

        #endregion


        #region SetupOnce()

        [OneTimeSetUp]
        public virtual void SetupOnce()
        {

        }

        #endregion

        #region SetupEachTest()

        [SetUp]
        public virtual void SetupEachTest()
        {

            Timestamp.Reset();

            testCSMS01      = new TestCSMS(
                                  Id:                      CSMS_Id.Parse("OCPPTest01"),
                                  RequireAuthentication:   true,
                                  HTTPUploadPort:          IPPort.Parse(9100),
                                  DNSClient:               new DNSClient(
                                                               SearchForIPv6DNSServers: false,
                                                               SearchForIPv4DNSServers: false
                                                           )
                              );

            Assert.IsNotNull(testCSMS01);

            testBackendWebSockets01  = testCSMS01.CreateWebSocketService(
                                           TCPPort:                 IPPort.Parse(9101),
                                           DisableWebSocketPings:   true,
                                           AutoStart:               true
                                       );

            Assert.IsNotNull(testBackendWebSockets01);


            csms1WebSocketJSONMessagesReceived          = [];
            csms1WebSocketJSONMessageResponsesSent      = [];
            csms1WebSocketJSONMessagesSent              = [];
            csms1WebSocketJSONMessageResponsesReceived  = [];

            testBackendWebSockets01.OnTextMessageReceived         += (timestamp, webSocketServer, webSocketConnection, eventTrackingId, requestMessage) => {
                csms1WebSocketJSONMessagesReceived.        Add(new LogJSONRequest(timestamp, JArray.Parse(requestMessage)));
                return Task.CompletedTask;
            };

            testBackendWebSockets01.OnJSONMessageResponseSent     += (timestamp, webSocketServer, webSocketConnection, eventTrackingId, requestTimestamp, jsonRequestMessage, binaryRequestMessage, responseTimestamp, responseMessage) => {
                csms1WebSocketJSONMessageResponsesSent.    Add(new LogDataJSONResponse(requestTimestamp, jsonRequestMessage, binaryRequestMessage, responseTimestamp, responseMessage ?? []));
                return Task.CompletedTask;
            };

            testBackendWebSockets01.OnTextMessageSent             += (timestamp, webSocketServer, webSocketConnection, eventTrackingId, requestMessage) => {
                csms1WebSocketJSONMessagesSent.            Add(new LogJSONRequest(timestamp, JArray.Parse(requestMessage)));
                return Task.CompletedTask;
            };

            testBackendWebSockets01.OnJSONMessageResponseReceived += (timestamp, webSocketServer, webSocketConnection, eventTrackingId, requestTimestamp, jsonRequestMessage, binaryRequestMessage, responseTimestamp, responseMessage) => {
                csms1WebSocketJSONMessageResponsesReceived.Add(new LogDataJSONResponse(requestTimestamp, jsonRequestMessage, binaryRequestMessage, responseTimestamp, responseMessage ?? []));
                return Task.CompletedTask;
            };

        }

        #endregion

        #region ShutdownEachTest()

        [TearDown]
        public virtual void ShutdownEachTest()
        {

            testBackendWebSockets01?.Shutdown();

            testCSMS01               = null;
            testBackendWebSockets01  = null;

        }

        #endregion

        #region ShutdownOnce()

        [OneTimeTearDown]
        public virtual void ShutdownOnce()
        {

        }

        #endregion

    }

}
