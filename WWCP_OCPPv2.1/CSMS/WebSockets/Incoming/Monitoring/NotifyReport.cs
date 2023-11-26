﻿/*
 * Copyright (c) 2014-2023 GraphDefined GmbH
 * This file is part of WWCP OCPP <https://github.com/OpenChargingCloud/WWCP_OCPP>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#region Usings

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    #region OnNotifyReport

    /// <summary>
    /// A notify report request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    public delegate Task

        OnNotifyReportRequestDelegate(DateTime              Timestamp,
                                      IEventSender          Sender,
                                      NotifyReportRequest   Request);


    /// <summary>
    /// A notify report at the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<NotifyReportResponse>

        OnNotifyReportDelegate(DateTime              Timestamp,
                               IEventSender          Sender,
                               NotifyReportRequest   Request,
                               CancellationToken     CancellationToken);


    /// <summary>
    /// A notify report response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="Response">The stop transaction response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnNotifyReportResponseDelegate(DateTime               Timestamp,
                                       IEventSender           Sender,
                                       NotifyReportRequest    Request,
                                       NotifyReportResponse   Response,
                                       TimeSpan               Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<NotifyReportRequest>?       CustomNotifyReportRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<NotifyReportResponse>?  CustomNotifyReportResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a NotifyReport WebSocket request was received.
        /// </summary>
        public event WebSocketJSONRequestLogHandler?               OnNotifyReportWSRequest;

        /// <summary>
        /// An event sent whenever a NotifyReport request was received.
        /// </summary>
        public event OnNotifyReportRequestDelegate?                OnNotifyReportRequest;

        /// <summary>
        /// An event sent whenever a NotifyReport was received.
        /// </summary>
        public event OnNotifyReportDelegate?                       OnNotifyReport;

        /// <summary>
        /// An event sent whenever a response to a NotifyReport was sent.
        /// </summary>
        public event OnNotifyReportResponseDelegate?               OnNotifyReportResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a NotifyReport was sent.
        /// </summary>
        public event WebSocketJSONRequestJSONResponseLogHandler?   OnNotifyReportWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_NotifyReport(DateTime                   RequestTimestamp,
                                 WebSocketServerConnection  Connection,
                                 ChargingStation_Id         ChargingStationId,
                                 EventTracking_Id           EventTrackingId,
                                 Request_Id                 RequestId,
                                 JObject                    JSONRequest,
                                 CancellationToken          CancellationToken)

        {

            #region Send OnNotifyReportWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyReportWSRequest?.Invoke(startTime,
                                                this,
                                                Connection,
                                                ChargingStationId,
                                                EventTrackingId,
                                                RequestTimestamp,
                                                JSONRequest);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyReportWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?     OCPPResponse        = null;
            OCPP_JSONErrorMessage?  OCPPErrorResponse   = null;

            try
            {

                if (NotifyReportRequest.TryParse(JSONRequest,
                                                 RequestId,
                                                 ChargingStationId,
                                                 out var request,
                                                 out var errorResponse,
                                                 CustomNotifyReportRequestParser) && request is not null) {

                    #region Send OnNotifyReportRequest event

                    try
                    {

                        OnNotifyReportRequest?.Invoke(Timestamp.Now,
                                                      this,
                                                      request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyReportRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    NotifyReportResponse? response = null;

                    var responseTasks = OnNotifyReport?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnNotifyReportDelegate)?.Invoke(Timestamp.Now,
                                                                                                                    this,
                                                                                                                    request,
                                                                                                                    CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= NotifyReportResponse.Failed(request);

                    #endregion

                    #region Send OnNotifyReportResponse event

                    try
                    {

                        OnNotifyReportResponse?.Invoke(Timestamp.Now,
                                                       this,
                                                       request,
                                                       response,
                                                       response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyReportResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       RequestId,
                                       response.ToJSON(
                                           CustomNotifyReportResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_NotifyReport)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_NotifyReport)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnNotifyReportWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnNotifyReportWSResponse?.Invoke(endTime,
                                                 this,
                                                 Connection,
                                                 ChargingStationId,
                                                 EventTrackingId,
                                                 RequestTimestamp,
                                                 JSONRequest,
                                                 endTime, //ToDo: Refactor me!
                                                 OCPPResponse?.Payload,
                                                 OCPPErrorResponse?.ToJSON(),
                                                 endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyReportWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}
