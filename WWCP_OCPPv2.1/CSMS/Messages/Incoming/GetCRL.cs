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

    #region OnGetCRL

    /// <summary>
    /// A get certificate revocation list request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    public delegate Task

        OnGetCRLRequestDelegate(DateTime        Timestamp,
                                IEventSender    Sender,
                                GetCRLRequest   Request);


    /// <summary>
    /// A get certificate revocation list at the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<GetCRLResponse>

        OnGetCRLDelegate(DateTime            Timestamp,
                         IEventSender        Sender,
                         GetCRLRequest       Request,
                         CancellationToken   CancellationToken);


    /// <summary>
    /// A get certificate revocation list response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="Response">The stop transaction response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnGetCRLResponseDelegate(DateTime         Timestamp,
                                 IEventSender     Sender,
                                 GetCRLRequest    Request,
                                 GetCRLResponse   Response,
                                 TimeSpan         Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<GetCRLRequest>?  CustomGetCRLRequestParser    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetCRL WebSocket request was received.
        /// </summary>
        public event WebSocketRequestLogHandler?     OnGetCRLWSRequest;

        /// <summary>
        /// An event sent whenever a GetCRL request was received.
        /// </summary>
        public event OnGetCRLRequestDelegate?        OnGetCRLRequest;

        /// <summary>
        /// An event sent whenever a GetCRL was received.
        /// </summary>
        public event OnGetCRLDelegate?               OnGetCRL;

        /// <summary>
        /// An event sent whenever a response to a GetCRL was sent.
        /// </summary>
        public event OnGetCRLResponseDelegate?       OnGetCRLResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a GetCRL was sent.
        /// </summary>
        public event WebSocketResponseLogHandler?    OnGetCRLWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_WebSocket_ResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_GetCRL(JArray                     json,
                           JObject                    requestData,
                           Request_Id                 requestId,
                           ChargingStation_Id         chargingStationId,
                           WebSocketServerConnection  Connection,
                           String                     OCPPTextMessage,
                           CancellationToken          CancellationToken)

        {

            OCPP_WebSocket_ResponseMessage?  OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?     OCPPErrorResponse   = null;

            #region Send OnGetCRLWSRequest event

            try
            {

                OnGetCRLWSRequest?.Invoke(Timestamp.Now,
                                          this,
                                          json);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCRLWSRequest));
            }

            #endregion

            try
            {

                if (GetCRLRequest.TryParse(requestData,
                                           requestId,
                                           chargingStationId,
                                           out var request,
                                           out var errorResponse,
                                           CustomGetCRLRequestParser) && request is not null) {

                    #region Send OnGetCRLRequest event

                    try
                    {

                        OnGetCRLRequest?.Invoke(Timestamp.Now,
                                                this,
                                                request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCRLRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    GetCRLResponse? response = null;

                    var responseTasks = OnGetCRL?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnGetCRLDelegate)?.Invoke(Timestamp.Now,
                                                                                                              this,
                                                                                                              request,
                                                                                                              CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= GetCRLResponse.Failed(request);

                    #endregion

                    #region Send OnGetCRLResponse event

                    try
                    {

                        OnGetCRLResponse?.Invoke(Timestamp.Now,
                                                 this,
                                                 request,
                                                 response,
                                                 response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCRLResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_WebSocket_ResponseMessage(
                                       requestId,
                                       response.ToJSON()
                                   );

                }

                else
                    OCPPErrorResponse = new OCPP_WebSocket_ErrorMessage(
                                            requestId,
                                            ResultCodes.FormationViolation,
                                            "The given 'GetCRL' request could not be parsed!",
                                            new JObject(
                                                new JProperty("request",       OCPPTextMessage),
                                                new JProperty("errorResponse", errorResponse)
                                            )
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = new OCPP_WebSocket_ErrorMessage(
                                        requestId,
                                        ResultCodes.FormationViolation,
                                        "Processing the given 'GetCRL' request led to an exception!",
                                        JSONObject.Create(
                                            new JProperty("request",    OCPPTextMessage),
                                            new JProperty("exception",  e.Message),
                                            new JProperty("stacktrace", e.StackTrace)
                                        )
                                    );

            }

            #region Send OnGetCRLWSResponse event

            try
            {

                OnGetCRLWSResponse?.Invoke(Timestamp.Now,
                                           this,
                                           json,
                                           OCPPResponse?.ToJSON() ?? OCPPErrorResponse?.ToJSON() ?? new JArray());

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCRLWSResponse));
            }

            #endregion


            return new Tuple<OCPP_WebSocket_ResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}
