﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Logging Delegates

    /// <summary>
    /// A delegate called whenever an UnpublishFirmware request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request logging.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The connection of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnUnpublishFirmwareRequestSentDelegate(DateTime                   Timestamp,
                                                                IEventSender               Sender,
                                                                IWebSocketConnection?     Connection,
                                                                UnpublishFirmwareRequest   Request,
                                                                SentMessageResults         SentMessageResult,
                                                                CancellationToken          CancellationToken);


    /// <summary>
    /// A UnpublishFirmware response.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the response.</param>
    /// <param name="Sender">The sender of the response.</param>
    /// <param name="Connection">The HTTP Web Socket client connection.</param>
    /// <param name="Request">The reserve now request.</param>
    /// <param name="Response">The reserve now response.</param>
    /// <param name="Runtime">The runtime of this request.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task

        OnUnpublishFirmwareResponseSentDelegate(DateTime                    Timestamp,
                                                IEventSender                Sender,
                                                IWebSocketConnection?       Connection,
                                                UnpublishFirmwareRequest    Request,
                                                UnpublishFirmwareResponse   Response,
                                                TimeSpan                    Runtime,
                                                SentMessageResults          SentMessageResult,
                                                CancellationToken           CancellationToken);


    /// <summary>
    /// A logging delegate called whenever an UnpublishFirmware request error was sent.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The sender of the request error.</param>
    /// <param name="Connection">The connection of the request error.</param>
    /// <param name="Request">The optional request (when parsable).</param>
    /// <param name="RequestErrorMessage">The request error message.</param>
    /// <param name="Runtime">The optional runtime of the request error message.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task

        OnUnpublishFirmwareRequestErrorSentDelegate(DateTime                       Timestamp,
                                                    IEventSender                   Sender,
                                                    IWebSocketConnection?          Connection,
                                                    UnpublishFirmwareRequest?      Request,
                                                    OCPP_JSONRequestErrorMessage   RequestErrorMessage,
                                                    TimeSpan?                      Runtime,
                                                    SentMessageResults             SentMessageResult,
                                                    CancellationToken              CancellationToken);


    /// <summary>
    /// A logging delegate called whenever an UnpublishFirmware response error was sent.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The sender of the response error.</param>
    /// <param name="Connection">The connection of the response error.</param>
    /// <param name="Request">The optional request.</param>
    /// <param name="Response">The optional response.</param>
    /// <param name="ResponseErrorMessage">The response error message.</param>
    /// <param name="Runtime">The optional runtime of the response error message.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task

        OnUnpublishFirmwareResponseErrorSentDelegate(DateTime                        Timestamp,
                                                     IEventSender                    Sender,
                                                     IWebSocketConnection?           Connection,
                                                     UnpublishFirmwareRequest?       Request,
                                                     UnpublishFirmwareResponse?      Response,
                                                     OCPP_JSONResponseErrorMessage   ResponseErrorMessage,
                                                     TimeSpan?                       Runtime,
                                                     SentMessageResults              SentMessageResult,
                                                     CancellationToken               CancellationToken);

    #endregion


    public partial class OCPPWebSocketAdapterOUT
    {

        #region Send UnpublishFirmware request

        /// <summary>
        /// An event fired whenever an UnpublishFirmware request was sent.
        /// </summary>
        public event OnUnpublishFirmwareRequestSentDelegate?  OnUnpublishFirmwareRequestSent;


        /// <summary>
        /// Send an UnpublishFirmware request.
        /// </summary>
        /// <param name="Request">A UnpublishFirmware request.</param>
        public async Task<UnpublishFirmwareResponse>

            UnpublishFirmware(UnpublishFirmwareRequest Request)

        {

            UnpublishFirmwareResponse? response = null;

            try
            {

                #region Sign request message

                if (!parentNetworkingNode.OCPP.SignaturePolicy.SignRequestMessage(
                        Request,
                        Request.ToJSON(
                            parentNetworkingNode.OCPP.CustomUnpublishFirmwareRequestSerializer,
                            parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        ),
                        out var signingErrors
                    ))
                {

                    response = UnpublishFirmwareResponse.SignatureError(
                                   Request,
                                   signingErrors
                               );

                }

                #endregion

                else
                {

                    #region Send request message

                    var sendRequestState = await SendJSONRequestAndWait(

                                                     OCPP_JSONRequestMessage.FromRequest(
                                                         Request,
                                                         Request.ToJSON(
                                                             parentNetworkingNode.OCPP.CustomUnpublishFirmwareRequestSerializer,
                                                             parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                             parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                         )
                                                     ),

                                                     sendMessageResult => LogEvent(
                                                         OnUnpublishFirmwareRequestSent,
                                                         loggingDelegate => loggingDelegate.Invoke(
                                                             Timestamp.Now,
                                                             parentNetworkingNode,
                                                             sendMessageResult.Connection,
                                                             Request,
                                                             sendMessageResult.Result,
                                                             Request.CancellationToken
                                                         )
                                                     )

                                                 );

                    #endregion

                    if (sendRequestState.IsValidJSONResponse(Request, out var jsonResponse))
                        response = await parentNetworkingNode.OCPP.IN.Receive_UnpublishFirmwareResponse(
                                             Request,
                                             jsonResponse,
                                             sendRequestState.WebSocketConnectionReceived,
                                             sendRequestState.DestinationIdReceived,
                                             sendRequestState.NetworkPathReceived,
                                             Request.         EventTrackingId,
                                             Request.         RequestId,
                                             sendRequestState.ResponseTimestamp,
                                             Request.         CancellationToken
                                         );

                    if (sendRequestState.IsValidJSONRequestError(Request, out var jsonRequestError))
                        response = await parentNetworkingNode.OCPP.IN.Receive_UnpublishFirmwareRequestError(
                                             Request,
                                             jsonRequestError,
                                             sendRequestState.WebSocketConnectionReceived,
                                             sendRequestState.DestinationIdReceived,
                                             sendRequestState.NetworkPathReceived,
                                             Request.EventTrackingId,
                                             Request.RequestId,
                                             sendRequestState.ResponseTimestamp,
                                             Request.CancellationToken
                                         );

                    response ??= new UnpublishFirmwareResponse(
                                     Request,
                                     UnpublishFirmwareStatus.Error,
                                     Result: Result.FromSendRequestState(sendRequestState)
                                 );

                }

            }
            catch (Exception e)
            {

                response = UnpublishFirmwareResponse.ExceptionOccured(
                               Request,
                               e
                           );

            }

            return response;

        }

        #endregion


        #region Send OnUnpublishFirmwareResponseSent event

        /// <summary>
        /// An event sent whenever an UnpublishFirmware response was sent.
        /// </summary>
        public event OnUnpublishFirmwareResponseSentDelegate?  OnUnpublishFirmwareResponseSent;

        public Task SendOnUnpublishFirmwareResponseSent(DateTime                    Timestamp,
                                                        IEventSender                Sender,
                                                        IWebSocketConnection?       Connection,
                                                        UnpublishFirmwareRequest    Request,
                                                        UnpublishFirmwareResponse   Response,
                                                        TimeSpan                    Runtime,
                                                        SentMessageResults          SentMessageResult,
                                                        CancellationToken           CancellationToken = default)

            => LogEvent(
                   OnUnpublishFirmwareResponseSent,
                   loggingDelegate => loggingDelegate.Invoke(
                       Timestamp,
                       Sender,
                       Connection,
                       Request,
                       Response,
                       Runtime,
                       SentMessageResult,
                       CancellationToken
                   )
               );

        #endregion

        #region Send OnUnpublishFirmwareRequestErrorSent event

        /// <summary>
        /// An event sent whenever an UnpublishFirmware request error was sent.
        /// </summary>
        public event OnUnpublishFirmwareRequestErrorSentDelegate? OnUnpublishFirmwareRequestErrorSent;


        public Task SendOnUnpublishFirmwareRequestErrorSent(DateTime                      Timestamp,
                                                            IEventSender                  Sender,
                                                            IWebSocketConnection?         Connection,
                                                            UnpublishFirmwareRequest?     Request,
                                                            OCPP_JSONRequestErrorMessage  RequestErrorMessage,
                                                            TimeSpan                      Runtime,
                                                            SentMessageResults            SentMessageResult,
                                                            CancellationToken             CancellationToken = default)

            => LogEvent(
                   OnUnpublishFirmwareRequestErrorSent,
                   loggingDelegate => loggingDelegate.Invoke(
                       Timestamp,
                       Sender,
                       Connection,
                       Request,
                       RequestErrorMessage,
                       Runtime,
                       SentMessageResult,
                       CancellationToken
                   )
               );

        #endregion

        #region Send OnUnpublishFirmwareResponseErrorSent event

        /// <summary>
        /// An event sent whenever an UnpublishFirmware response error was sent.
        /// </summary>
        public event OnUnpublishFirmwareResponseErrorSentDelegate? OnUnpublishFirmwareResponseErrorSent;


        public Task SendOnUnpublishFirmwareResponseErrorSent(DateTime                       Timestamp,
                                                             IEventSender                   Sender,
                                                             IWebSocketConnection?          Connection,
                                                             UnpublishFirmwareRequest?      Request,
                                                             UnpublishFirmwareResponse?     Response,
                                                             OCPP_JSONResponseErrorMessage  ResponseErrorMessage,
                                                             TimeSpan                       Runtime,
                                                             SentMessageResults             SentMessageResult,
                                                             CancellationToken              CancellationToken = default)

            => LogEvent(
                   OnUnpublishFirmwareResponseErrorSent,
                   loggingDelegate => loggingDelegate.Invoke(
                       Timestamp,
                       Sender,
                       Connection,
                       Request,
                       Response,
                       ResponseErrorMessage,
                       Runtime,
                       SentMessageResult,
                       CancellationToken
                   )
               );

        #endregion

    }

}