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

    #region OnSecurityEventNotification (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a security event notification request will be sent to the CSMS.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The reserve now request.</param>
    public delegate Task OnSecurityEventNotificationRequestDelegate(DateTime                           Timestamp,
                                                                    IEventSender                       Sender,
                                                                    SecurityEventNotificationRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a security event notification request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnSecurityEventNotificationResponseDelegate(DateTime                            Timestamp,
                                                                     IEventSender                        Sender,
                                                                     SecurityEventNotificationRequest    Request,
                                                                     SecurityEventNotificationResponse   Response,
                                                                     TimeSpan                            Runtime);

    #endregion


    /// <summary>
    /// A CP client.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<SecurityEventNotificationRequest>?  CustomSecurityEventNotificationSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a security event notification request will be sent to the CSMS.
        /// </summary>
        public event OnSecurityEventNotificationRequestDelegate?     OnSecurityEventNotificationRequest;

        /// <summary>
        /// An event fired whenever a security event notification request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?                        OnSecurityEventNotificationWSRequest;

        /// <summary>
        /// An event fired whenever a response to a security event notification request was received.
        /// </summary>
        public event ClientResponseLogHandler?                       OnSecurityEventNotificationWSResponse;

        /// <summary>
        /// An event fired whenever a response to a security event notification request was received.
        /// </summary>
        public event OnSecurityEventNotificationResponseDelegate?    OnSecurityEventNotificationResponse;

        #endregion


        #region SendSecurityEventNotification        (Request)

        /// <summary>
        /// Send a security event notification.
        /// </summary>
        /// <param name="Request">A SecurityEventNotification request.</param>
        public async Task<SecurityEventNotificationResponse>

            SendSecurityEventNotification(SecurityEventNotificationRequest  Request)

        {

            #region Send OnSecurityEventNotificationRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnSecurityEventNotificationRequest?.Invoke(startTime,
                                                           this,
                                                           Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSecurityEventNotificationRequest));
            }

            #endregion


            SecurityEventNotificationResponse? response = null;

            var requestMessage = await SendRequest(Request.Action,
                                                   Request.RequestId,
                                                   Request.ToJSON(
                                                       CustomSecurityEventNotificationSerializer,
                                                       CustomSignatureSerializer,
                                                       CustomCustomDataSerializer
                                                   ));

            if (requestMessage.NoErrors)
            {

                var sendRequestState = await WaitForResponse(requestMessage);

                if (sendRequestState.NoErrors &&
                    sendRequestState.Response is not null)
                {

                    if (SecurityEventNotificationResponse.TryParse(Request,
                                                                   sendRequestState.Response,
                                                                   out var securityEventNotificationResponse,
                                                                   out var errorResponse) &&
                        securityEventNotificationResponse is not null)
                    {
                        response = securityEventNotificationResponse;
                    }

                    response ??= new SecurityEventNotificationResponse(Request,
                                                                       Result.Format(errorResponse));

                }

                response ??= new SecurityEventNotificationResponse(Request,
                                                                   Result.FromSendRequestState(sendRequestState));

            }

            response ??= new SecurityEventNotificationResponse(Request,
                                                               Result.GenericError(requestMessage.ErrorMessage));


            #region Send OnSecurityEventNotificationResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnSecurityEventNotificationResponse?.Invoke(endTime,
                                                            this,
                                                            Request,
                                                            response,
                                                            endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnSecurityEventNotificationResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}