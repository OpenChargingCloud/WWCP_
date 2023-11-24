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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    #region OnBootNotification (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a boot notification request will be sent to the CSMS.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The reserve now request.</param>
    public delegate Task OnBootNotificationRequestDelegate(DateTime                  Timestamp,
                                                           IEventSender              Sender,
                                                           BootNotificationRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a boot notification request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnBootNotificationResponseDelegate(DateTime                   Timestamp,
                                                            IEventSender               Sender,
                                                            BootNotificationRequest    Request,
                                                            BootNotificationResponse   Response,
                                                            TimeSpan                   Runtime);

    #endregion


    /// <summary>
    /// A charging station HTTP Web Socket client.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<BootNotificationRequest>?  CustomBootNotificationRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<BootNotificationResponse>?     CustomBootNotificationResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a boot notification request will be sent to the CSMS.
        /// </summary>
        public event OnBootNotificationRequestDelegate?     OnBootNotificationRequest;

        /// <summary>
        /// An event fired whenever a boot notification request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?               OnBootNotificationWSRequest;

        /// <summary>
        /// An event fired whenever a response to a boot notification request was received.
        /// </summary>
        public event ClientResponseLogHandler?              OnBootNotificationWSResponse;

        /// <summary>
        /// An event fired whenever a response to a boot notification request was received.
        /// </summary>
        public event OnBootNotificationResponseDelegate?    OnBootNotificationResponse;

        #endregion


        #region SendBootNotification(Request)

        /// <summary>
        /// Send a boot notification.
        /// </summary>
        /// <param name="Request">A BootNotification request.</param>
        public async Task<BootNotificationResponse>

            SendBootNotification(BootNotificationRequest Request)

        {

            #region Send OnBootNotificationRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnBootNotificationRequest?.Invoke(startTime,
                                                  this,
                                                  Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnBootNotificationRequest));
            }

            #endregion


            BootNotificationResponse? response = null;

            try
            {

                var requestMessage = await SendRequest(Request.Action,
                                                       Request.RequestId,
                                                       Request.ToJSON(
                                                           CustomBootNotificationRequestSerializer,
                                                           CustomChargingStationSerializer,
                                                           CustomSignatureSerializer,
                                                           CustomCustomDataSerializer
                                                       ));

                if (requestMessage.NoErrors)
                {

                    var sendRequestState = await WaitForResponse(requestMessage);

                    if (sendRequestState.NoErrors &&
                        sendRequestState.Response is not null)
                    {

                        if (BootNotificationResponse.TryParse(Request,
                                                              sendRequestState.Response,
                                                              out var bootNotificationResponse,
                                                              out var errorResponse,
                                                              CustomBootNotificationResponseParser) &&
                            bootNotificationResponse is not null)
                        {
                            response = bootNotificationResponse;
                        }

                        response ??= new BootNotificationResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                    }

                    response ??= new BootNotificationResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

                }

                response ??= new BootNotificationResponse(
                                 Request,
                                 Result.GenericError(requestMessage.ErrorMessage)
                             );

            }
            catch (Exception e)
            {

                response = new BootNotificationResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnBootNotificationResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnBootNotificationResponse?.Invoke(endTime,
                                                   this,
                                                   Request,
                                                   response,
                                                   endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnBootNotificationResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}