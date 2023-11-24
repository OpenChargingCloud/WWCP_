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

    #region OnNotifyEvent (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a notify event request will be sent to the CSMS.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The reserve now request.</param>
    public delegate Task OnNotifyEventRequestDelegate(DateTime             Timestamp,
                                                      IEventSender         Sender,
                                                      NotifyEventRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a notify event request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnNotifyEventResponseDelegate(DateTime              Timestamp,
                                                       IEventSender          Sender,
                                                       NotifyEventRequest    Request,
                                                       NotifyEventResponse   Response,
                                                       TimeSpan              Runtime);

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

        public CustomJObjectSerializerDelegate<NotifyEventRequest>?  CustomNotifyEventRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<NotifyEventResponse>?     CustomNotifyEventResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a notify event request will be sent to the CSMS.
        /// </summary>
        public event OnNotifyEventRequestDelegate?     OnNotifyEventRequest;

        /// <summary>
        /// An event fired whenever a notify event request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?          OnNotifyEventWSRequest;

        /// <summary>
        /// An event fired whenever a response to a notify event request was received.
        /// </summary>
        public event ClientResponseLogHandler?         OnNotifyEventWSResponse;

        /// <summary>
        /// An event fired whenever a response to a notify event request was received.
        /// </summary>
        public event OnNotifyEventResponseDelegate?    OnNotifyEventResponse;

        #endregion


        #region NotifyEvent(Request)

        /// <summary>
        /// Notify about an event.
        /// </summary>
        /// <param name="Request">A NotifyEvent request.</param>
        public async Task<NotifyEventResponse>

            NotifyEvent(NotifyEventRequest  Request)

        {

            #region Send OnNotifyEventRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyEventRequest?.Invoke(startTime,
                                             this,
                                             Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnNotifyEventRequest));
            }

            #endregion


            NotifyEventResponse? response = null;

            try
            {

                var requestMessage = await SendRequest(Request.Action,
                                                       Request.RequestId,
                                                       Request.ToJSON(
                                                           CustomNotifyEventRequestSerializer,
                                                           CustomEventDataSerializer,
                                                           CustomComponentSerializer,
                                                           CustomEVSESerializer,
                                                           CustomVariableSerializer,
                                                           CustomSignatureSerializer,
                                                           CustomCustomDataSerializer
                                                       ));

                if (requestMessage.NoErrors)
                {

                    var sendRequestState = await WaitForResponse(requestMessage);

                    if (sendRequestState.NoErrors &&
                        sendRequestState.Response is not null)
                    {

                        if (NotifyEventResponse.TryParse(Request,
                                                         sendRequestState.Response,
                                                         out var notifyEventResponse,
                                                         out var errorResponse,
                                                         CustomNotifyEventResponseParser) &&
                            notifyEventResponse is not null)
                        {
                            response = notifyEventResponse;
                        }

                        response ??= new NotifyEventResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                    }

                    response ??= new NotifyEventResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

                }

                response ??= new NotifyEventResponse(
                                 Request,
                                 Result.GenericError(requestMessage.ErrorMessage)
                             );

            }
            catch (Exception e)
            {

                response = new NotifyEventResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnNotifyEventResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnNotifyEventResponse?.Invoke(endTime,
                                              this,
                                              Request,
                                              response,
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnNotifyEventResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}