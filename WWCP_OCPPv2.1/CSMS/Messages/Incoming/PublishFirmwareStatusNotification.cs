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

    #region OnPublishFirmwareStatusNotification

    /// <summary>
    /// A publish firmware status notification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The firmware status notification request.</param>

    public delegate Task

        OnPublishFirmwareStatusNotificationRequestDelegate(DateTime                                   Timestamp,
                                                           IEventSender                               Sender,
                                                           PublishFirmwareStatusNotificationRequest   Request);


    /// <summary>
    /// A publish firmware status notification from the given charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The firmware status notification request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<PublishFirmwareStatusNotificationResponse>

        OnPublishFirmwareStatusNotificationDelegate(DateTime                                   Timestamp,
                                                    IEventSender                               Sender,
                                                    PublishFirmwareStatusNotificationRequest   Request,
                                                    CancellationToken                          CancellationToken);


    /// <summary>
    /// A publish firmware status notification response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The firmware status notification request.</param>
    /// <param name="Response">The firmware status notification response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnPublishFirmwareStatusNotificationResponseDelegate(DateTime                                    Timestamp,
                                                            IEventSender                                Sender,
                                                            PublishFirmwareStatusNotificationRequest    Request,
                                                            PublishFirmwareStatusNotificationResponse   Response,
                                                            TimeSpan                                    Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<PublishFirmwareStatusNotificationRequest>?  CustomPublishFirmwareStatusNotificationRequestParser    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification WebSocket request was received.
        /// </summary>
        public event WebSocketRequestLogHandler?                             OnPublishFirmwareStatusNotificationWSRequest;

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification request was received.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationRequestDelegate?     OnPublishFirmwareStatusNotificationRequest;

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification request was received.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationDelegate?            OnPublishFirmwareStatusNotification;

        /// <summary>
        /// An event sent whenever a response to a PublishFirmwareStatusNotification request was sent.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationResponseDelegate?    OnPublishFirmwareStatusNotificationResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a PublishFirmwareStatusNotification request was sent.
        /// </summary>
        public event WebSocketResponseLogHandler?                            OnPublishFirmwareStatusNotificationWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_WebSocket_ResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_PublishFirmwareStatusNotification(JArray                     json,
                                                      JObject                    requestData,
                                                      Request_Id                 requestId,
                                                      ChargingStation_Id         chargingStationId,
                                                      WebSocketServerConnection  Connection,
                                                      String                     OCPPTextMessage,
                                                      CancellationToken          CancellationToken)

        {

            OCPP_WebSocket_ResponseMessage?  OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?     OCPPErrorResponse   = null;

            #region Send OnPublishFirmwareStatusNotificationWSRequest event

            try
            {

                OnPublishFirmwareStatusNotificationWSRequest?.Invoke(Timestamp.Now,
                                                                     this,
                                                                     json);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationWSRequest));
            }

            #endregion

            try
            {

                if (PublishFirmwareStatusNotificationRequest.TryParse(requestData,
                                                                      requestId,
                                                                      chargingStationId,
                                                                      out var request,
                                                                      out var errorResponse,
                                                                      CustomPublishFirmwareStatusNotificationRequestParser) && request is not null) {

                    #region Send OnPublishFirmwareStatusNotificationRequest event

                    try
                    {

                        OnPublishFirmwareStatusNotificationRequest?.Invoke(Timestamp.Now,
                                                                           this,
                                                                           request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    PublishFirmwareStatusNotificationResponse? response = null;

                    var responseTasks = OnPublishFirmwareStatusNotification?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnPublishFirmwareStatusNotificationDelegate)?.Invoke(Timestamp.Now,
                                                                                                                                         this,
                                                                                                                                         request,
                                                                                                                                         CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= PublishFirmwareStatusNotificationResponse.Failed(request);

                    #endregion

                    #region Send OnPublishFirmwareStatusNotificationResponse event

                    try
                    {

                        OnPublishFirmwareStatusNotificationResponse?.Invoke(Timestamp.Now,
                                                                            this,
                                                                            request,
                                                                            response,
                                                                            response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationResponse));
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
                                            "The given 'PublishFirmwareStatusNotification' request could not be parsed!",
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
                                        "Processing the given 'PublishFirmwareStatusNotification' request led to an exception!",
                                        JSONObject.Create(
                                            new JProperty("request",    OCPPTextMessage),
                                            new JProperty("exception",  e.Message),
                                            new JProperty("stacktrace", e.StackTrace)
                                        )
                                    );

            }

            #region Send OnPublishFirmwareStatusNotificationWSResponse event

            try
            {

                OnPublishFirmwareStatusNotificationWSResponse?.Invoke(Timestamp.Now,
                                                                      this,
                                                                      json,
                                                                      OCPPResponse?.ToJSON() ?? OCPPErrorResponse?.ToJSON() ?? new JArray());

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationWSResponse));
            }

            #endregion


            return new Tuple<OCPP_WebSocket_ResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}
