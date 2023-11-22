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

    #region OnBootNotification

    /// <summary>
    /// A boot notification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the boot notification request.</param>
    /// <param name="Sender">The sender of the boot notification request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The boot notification request.</param>
    public delegate Task

        OnBootNotificationRequestDelegate(DateTime                    Timestamp,
                                          IEventSender                Sender,
                                          WebSocketServerConnection   Connection,
                                          BootNotificationRequest     Request);


    /// <summary>
    /// A boot notification.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the boot notification request.</param>
    /// <param name="Sender">The sender of the boot notification request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The boot notification request.</param>
    /// <param name="CancellationToken">A token to cancel this boot notification request.</param>
    public delegate Task<BootNotificationResponse>

        OnBootNotificationDelegate(DateTime                    Timestamp,
                                   IEventSender                Sender,
                                   WebSocketServerConnection   Connection,
                                   BootNotificationRequest     Request,
                                   CancellationToken           CancellationToken);


    /// <summary>
    /// A boot notification response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the boot notification response.</param>
    /// <param name="Sender">The sender of the boot notification response.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The boot notification request.</param>
    /// <param name="Response">The boot notification response.</param>
    /// <param name="Runtime">The runtime of the boot notification response.</param>
    public delegate Task

        OnBootNotificationResponseDelegate(DateTime                    Timestamp,
                                           IEventSender                Sender,
                                           WebSocketServerConnection   Connection,
                                           BootNotificationRequest     Request,
                                           BootNotificationResponse    Response,
                                           TimeSpan                    Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<BootNotificationRequest>?  CustomBootNotificationRequestParser    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a BootNotification WebSocket request was received.
        /// </summary>
        public event WebSocketRequestLogHandler?            OnBootNotificationWSRequest;

        /// <summary>
        /// An event sent whenever a BootNotification request was received.
        /// </summary>
        public event OnBootNotificationRequestDelegate?     OnBootNotificationRequest;

        /// <summary>
        /// An event sent whenever a BootNotification was received.
        /// </summary>
        public event OnBootNotificationDelegate?            OnBootNotification;

        /// <summary>
        /// An event sent whenever a response to a BootNotification was sent.
        /// </summary>
        public event OnBootNotificationResponseDelegate?    OnBootNotificationResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a BootNotification was sent.
        /// </summary>
        public event WebSocketResponseLogHandler?           OnBootNotificationWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_WebSocket_ResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_BootNotification(JArray                     json,
                                     JObject                    requestData,
                                     Request_Id                 requestId,
                                     ChargingStation_Id         chargingStationId,
                                     WebSocketServerConnection  Connection,
                                     String                     OCPPTextMessage,
                                     CancellationToken          CancellationToken)

        {

            OCPP_WebSocket_ResponseMessage?  OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?     OCPPErrorResponse   = null;

            #region Send OnBootNotificationWSRequest event

            try
            {

                OnBootNotificationWSRequest?.Invoke(Timestamp.Now,
                                                    this,
                                                    json);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnBootNotificationWSRequest));
            }

            #endregion

            try
            {

                if (BootNotificationRequest.TryParse(requestData,
                                                     requestId,
                                                     chargingStationId,
                                                     out var request,
                                                     out var errorResponse,
                                                     CustomBootNotificationRequestParser) &&
                    request is not null) {

                    #region Send OnBootNotificationRequest event

                    try
                    {

                        OnBootNotificationRequest?.Invoke(Timestamp.Now,
                                                          this,
                                                          Connection,
                                                          request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnBootNotificationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    BootNotificationResponse? response = null;

                    var responseTasks = OnBootNotification?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnBootNotificationDelegate)?.Invoke(Timestamp.Now,
                                                                                                                        this,
                                                                                                                        Connection,
                                                                                                                        request,
                                                                                                                        CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= BootNotificationResponse.Failed(request);

                    #endregion

                    #region Send OnBootNotificationResponse event

                    try
                    {

                        OnBootNotificationResponse?.Invoke(Timestamp.Now,
                                                           this,
                                                           Connection,
                                                           request,
                                                           response,
                                                           response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnBootNotificationResponse));
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
                                            "The given 'BootNotification' request could not be parsed!",
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
                                        "Processing the given 'BootNotification' request led to an exception!",
                                        JSONObject.Create(
                                            new JProperty("request",    OCPPTextMessage),
                                            new JProperty("exception",  e.Message),
                                            new JProperty("stacktrace", e.StackTrace)
                                        )
                                    );

            }

            #region Send OnBootNotificationWSResponse event

            try
            {

                OnBootNotificationWSResponse?.Invoke(Timestamp.Now,
                                                     this,
                                                     json,
                                                     OCPPResponse?.ToJSON() ?? OCPPErrorResponse?.ToJSON() ?? []);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnBootNotificationWSResponse));
            }

            #endregion


            return new Tuple<OCPP_WebSocket_ResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}
