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

using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    #region OnUsePriorityCharging

    /// <summary>
    /// A UsePriorityCharging request.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task

        OnUsePriorityChargingRequestDelegate(DateTime                     Timestamp,
                                             IEventSender                 Sender,
                                             UsePriorityChargingRequest   Request);


    /// <summary>
    /// A UsePriorityCharging request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<UsePriorityChargingResponse>

        OnUsePriorityChargingDelegate(DateTime                     Timestamp,
                                      IEventSender                 Sender,
                                      WebSocketClientConnection    Connection,
                                      UsePriorityChargingRequest   Request,
                                      CancellationToken            CancellationToken);


    /// <summary>
    /// A UsePriorityCharging response.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the response.</param>
    /// <param name="Sender">The sender of the response.</param>
    /// <param name="Request">The reserve now request.</param>
    /// <param name="Response">The reserve now response.</param>
    /// <param name="Runtime">The runtime of this request.</param>
    public delegate Task

        OnUsePriorityChargingResponseDelegate(DateTime                      Timestamp,
                                              IEventSender                  Sender,
                                              UsePriorityChargingRequest    Request,
                                              UsePriorityChargingResponse   Response,
                                              TimeSpan                      Runtime);

    #endregion


    /// <summary>
    /// The charging station HTTP WebSocket client runs on a charging station
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<UsePriorityChargingRequest>?          CustomUsePriorityChargingRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<CS.UsePriorityChargingResponse>?  CustomUsePriorityChargingResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever an UsePriorityCharging websocket request was received.
        /// </summary>
        public event WSClientRequestLogHandler?                OnUsePriorityChargingWSRequest;

        /// <summary>
        /// An event sent whenever an UsePriorityCharging request was received.
        /// </summary>
        public event OnUsePriorityChargingRequestDelegate?     OnUsePriorityChargingRequest;

        /// <summary>
        /// An event sent whenever an UsePriorityCharging request was received.
        /// </summary>
        public event OnUsePriorityChargingDelegate?            OnUsePriorityCharging;

        /// <summary>
        /// An event sent whenever a response to an UsePriorityCharging request was sent.
        /// </summary>
        public event OnUsePriorityChargingResponseDelegate?    OnUsePriorityChargingResponse;

        /// <summary>
        /// An event sent whenever a websocket response to an UsePriorityCharging request was sent.
        /// </summary>
        public event WSClientResponseLogHandler?               OnUsePriorityChargingWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_UsePriorityCharging(DateTime                   RequestTimestamp,
                                        WebSocketClientConnection  WebSocketConnection,
                                        ChargingStation_Id         chargingStationId,
                                        EventTracking_Id           EventTrackingId,
                                        String                     requestText,
                                        Request_Id                 requestId,
                                        JObject                    requestJSON,
                                        CancellationToken          CancellationToken)

        {

            #region Send OnUsePriorityChargingWSRequest event

            try
            {

                OnUsePriorityChargingWSRequest?.Invoke(Timestamp.Now,
                                                       WebSocketConnection,
                                                       chargingStationId,
                                                       EventTrackingId,
                                                       requestJSON);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnUsePriorityChargingWSRequest));
            }

            #endregion

            OCPP_JSONResponseMessage? OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?    OCPPErrorResponse   = null;

            try
            {

                if (UsePriorityChargingRequest.TryParse(requestJSON,
                                                        requestId,
                                                        ChargingStationIdentity,
                                                        out var request,
                                                        out var errorResponse,
                                                        CustomUsePriorityChargingRequestParser) && request is not null) {

                    #region Send OnUsePriorityChargingRequest event

                    try
                    {

                        OnUsePriorityChargingRequest?.Invoke(Timestamp.Now,
                                                             this,
                                                             request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnUsePriorityChargingRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    UsePriorityChargingResponse? response = null;

                    var results = OnUsePriorityCharging?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnUsePriorityChargingDelegate)?.Invoke(Timestamp.Now,
                                                                                                                     this,
                                                                                                                     WebSocketConnection,
                                                                                                                     request,
                                                                                                                     CancellationToken)).
                                      ToArray();

                    if (results?.Length > 0)
                    {

                        await Task.WhenAll(results!);

                        response = results.FirstOrDefault()?.Result;

                    }

                    response ??= UsePriorityChargingResponse.Failed(request);

                    #endregion

                    #region Send OnUsePriorityChargingResponse event

                    try
                    {

                        OnUsePriorityChargingResponse?.Invoke(Timestamp.Now,
                                                              this,
                                                              request,
                                                              response,
                                                              response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnUsePriorityChargingResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       requestId,
                                       response.ToJSON(
                                           CustomUsePriorityChargingResponseSerializer,
                                           CustomStatusInfoSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.CouldNotParse(
                                            requestId,
                                            nameof(Receive_UsePriorityCharging)[8..],
                                            requestJSON,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {
                OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.FormationViolation(
                                        requestId,
                                        nameof(Receive_UsePriorityCharging)[8..],
                                        requestJSON,
                                        e
                                    );
            }

            #region Send OnUsePriorityChargingWSResponse event

            try
            {

                OnUsePriorityChargingWSResponse?.Invoke(Timestamp.Now,
                                                        WebSocketConnection,
                                                        requestJSON,
                                                        OCPPResponse?.Payload,
                                                        OCPPErrorResponse?.ToJSON());

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnUsePriorityChargingWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}