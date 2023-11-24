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

    #region OnSetVariables

    /// <summary>
    /// A set variables request.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task

        OnSetVariablesRequestDelegate(DateTime              Timestamp,
                                      IEventSender          Sender,
                                      SetVariablesRequest   Request);


    /// <summary>
    /// A set variables request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<SetVariablesResponse>

        OnSetVariablesDelegate(DateTime                    Timestamp,
                               IEventSender                Sender,
                               WebSocketClientConnection   Connection,
                               SetVariablesRequest         Request,
                               CancellationToken           CancellationToken);


    /// <summary>
    /// A set variables response.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the response.</param>
    /// <param name="Sender">The sender of the response.</param>
    /// <param name="Request">The reserve now request.</param>
    /// <param name="Response">The reserve now response.</param>
    /// <param name="Runtime">The runtime of this request.</param>
    public delegate Task

        OnSetVariablesResponseDelegate(DateTime               Timestamp,
                                       IEventSender           Sender,
                                       SetVariablesRequest    Request,
                                       SetVariablesResponse   Response,
                                       TimeSpan               Runtime);

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

        public CustomJObjectParserDelegate<SetVariablesRequest>?       CustomSetVariablesRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<SetVariablesResponse>?  CustomSetVariablesResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a set variables websocket request was received.
        /// </summary>
        public event WSClientRequestLogHandler?         OnSetVariablesWSRequest;

        /// <summary>
        /// An event sent whenever a set variables request was received.
        /// </summary>
        public event OnSetVariablesRequestDelegate?     OnSetVariablesRequest;

        /// <summary>
        /// An event sent whenever a set variables request was received.
        /// </summary>
        public event OnSetVariablesDelegate?            OnSetVariables;

        /// <summary>
        /// An event sent whenever a response to a set variables request was sent.
        /// </summary>
        public event OnSetVariablesResponseDelegate?    OnSetVariablesResponse;

        /// <summary>
        /// An event sent whenever a websocket response to a set variables request was sent.
        /// </summary>
        public event WSClientResponseLogHandler?        OnSetVariablesWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_SetVariables(DateTime                   RequestTimestamp,
                                 WebSocketClientConnection  WebSocketConnection,
                                 ChargingStation_Id         chargingStationId,
                                 EventTracking_Id           EventTrackingId,
                                 String                     requestText,
                                 Request_Id                 requestId,
                                 JObject                    requestJSON,
                                 CancellationToken          CancellationToken)

        {

            #region Send OnSetVariablesWSRequest event

            try
            {

                OnSetVariablesWSRequest?.Invoke(Timestamp.Now,
                                                WebSocketConnection,
                                                chargingStationId,
                                                EventTrackingId,
                                                requestJSON);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSetVariablesWSRequest));
            }

            #endregion

            OCPP_JSONResponseMessage? OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?    OCPPErrorResponse   = null;

            try
            {

                if (SetVariablesRequest.TryParse(requestJSON,
                                                 requestId,
                                                 ChargingStationIdentity,
                                                 out var request,
                                                 out var errorResponse,
                                                 CustomSetVariablesRequestParser) && request is not null) {

                    #region Send OnSetVariablesRequest event

                    try
                    {

                        OnSetVariablesRequest?.Invoke(Timestamp.Now,
                                                      this,
                                                      request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSetVariablesRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    SetVariablesResponse? response = null;

                    var results = OnSetVariables?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnSetVariablesDelegate)?.Invoke(Timestamp.Now,
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

                    response ??= SetVariablesResponse.Failed(request);

                    #endregion

                    #region Send OnSetVariablesResponse event

                    try
                    {

                        OnSetVariablesResponse?.Invoke(Timestamp.Now,
                                                       this,
                                                       request,
                                                       response,
                                                       response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSetVariablesResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       requestId,
                                       response.ToJSON(
                                           CustomSetVariablesResponseSerializer,
                                           CustomSetVariableResultSerializer,
                                           CustomComponentSerializer,
                                           CustomEVSESerializer,
                                           CustomVariableSerializer,
                                           CustomStatusInfoSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.CouldNotParse(
                                            requestId,
                                            nameof(Receive_SetVariables)[8..],
                                            requestJSON,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {
                OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.FormationViolation(
                                        requestId,
                                        nameof(Receive_SetVariables)[8..],
                                        requestJSON,
                                        e
                                    );
            }

            #region Send OnSetVariablesWSResponse event

            try
            {

                OnSetVariablesWSResponse?.Invoke(Timestamp.Now,
                                                 WebSocketConnection,
                                                 requestJSON,
                                                 OCPPResponse?.Payload,
                                                 OCPPErrorResponse?.ToJSON());

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSetVariablesWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}