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

using cloud.charging.open.protocols.WWCP.WebSockets;
using cloud.charging.open.protocols.OCPP.WebSockets;
using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv1_6.CP;
using cloud.charging.open.protocols.OCPPv1_6.CS;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.NetworkingNode
{

    #region Logging Delegates

    /// <summary>
    /// A delegate called whenever a SendLocalList request was sent.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request logging.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The connection of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnSendLocalListRequestSentDelegate(DateTime                  Timestamp,
                                                               IEventSender              Sender,
                                                               IWebSocketConnection?     Connection,
                                                               SendLocalListRequest   Request,
                                                               SentMessageResults        SentMessageResult,
                                                               CancellationToken         CancellationToken);


    /// <summary>
    /// A SendLocalList response.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the response.</param>
    /// <param name="Sender">The sender of the response.</param>
    /// <param name="Connection">The HTTP WebSocket client connection.</param>
    /// <param name="Request">The reserve now request.</param>
    /// <param name="Response">The reserve now response.</param>
    /// <param name="Runtime">The runtime of this request.</param>
    /// <param name="SentMessageResult">The result of the send message process.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task

        OnSendLocalListResponseSentDelegate(DateTime                   Timestamp,
                                               IEventSender               Sender,
                                               IWebSocketConnection?      Connection,
                                               SendLocalListRequest    Request,
                                               SendLocalListResponse   Response,
                                               TimeSpan                   Runtime,
                                               SentMessageResults         SentMessageResult,
                                               CancellationToken          CancellationToken);


    /// <summary>
    /// A logging delegate called whenever a SendLocalList request error was sent.
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

        OnSendLocalListRequestErrorSentDelegate(DateTime                       Timestamp,
                                                   IEventSender                   Sender,
                                                   IWebSocketConnection?          Connection,
                                                   SendLocalListRequest?       Request,
                                                   OCPP_JSONRequestErrorMessage   RequestErrorMessage,
                                                   TimeSpan?                      Runtime,
                                                   SentMessageResults             SentMessageResult,
                                                   CancellationToken              CancellationToken);


    /// <summary>
    /// A logging delegate called whenever a SendLocalList response error was sent.
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

        OnSendLocalListResponseErrorSentDelegate(DateTime                        Timestamp,
                                                    IEventSender                    Sender,
                                                    IWebSocketConnection?           Connection,
                                                    SendLocalListRequest?        Request,
                                                    SendLocalListResponse?       Response,
                                                    OCPP_JSONResponseErrorMessage   ResponseErrorMessage,
                                                    TimeSpan?                       Runtime,
                                                    SentMessageResults              SentMessageResult,
                                                    CancellationToken               CancellationToken);

    #endregion


    public partial class OCPPWebSocketAdapterOUT
    {

        #region Send SendLocalList request

        /// <summary>
        /// An event fired whenever a SendLocalList request was sent.
        /// </summary>
        public event OnSendLocalListRequestSentDelegate?  OnSendLocalListRequestSent;


        /// <summary>
        /// Send a SendLocalList request.
        /// </summary>
        /// <param name="Request">A SendLocalList request.</param>
        public async Task<SendLocalListResponse>

            SendLocalList(SendLocalListRequest Request)

        {

            SendLocalListResponse? response = null;

            try
            {

                switch (Request.SerializationFormat)
                {

                    case SerializationFormats.JSON: {

                            #region Sign request message

                            if (!parentNetworkingNode.OCPP.SignaturePolicy.SignRequestMessage(
                                    Request,
                                    Request.ToJSON(
                                        parentNetworkingNode.OCPP.CustomSendLocalListRequestSerializer,
                                        parentNetworkingNode.OCPP.CustomAuthorizationDataSerializer,
                                        parentNetworkingNode.OCPP.CustomIdTagInfoSerializer,
                                        parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                        parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                    ),
                                    out var signingErrors
                                ))
                            {

                                response = SendLocalListResponse.SignatureError(
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
                                                                         parentNetworkingNode.OCPP.CustomSendLocalListRequestSerializer,
                                                                         parentNetworkingNode.OCPP.CustomAuthorizationDataSerializer,
                                                                         parentNetworkingNode.OCPP.CustomIdTagInfoSerializer,
                                                                         parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                                         parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                                     )
                                                                 ),

                                                                 sentMessageResult => LogEvent(
                                                                     OnSendLocalListRequestSent,
                                                                     loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         parentNetworkingNode,
                                                                         sentMessageResult.Connection,
                                                                         Request,
                                                                         sentMessageResult.Result,
                                                                         Request.CancellationToken
                                                                     )
                                                                 )

                                                             );

                                #endregion

                                if (sendRequestState.IsValidJSONResponse(Request, out var jsonResponse))
                                    response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListResponse(
                                                         Request,
                                                         jsonResponse,
                                                         sendRequestState.WebSocketConnectionReceived,
                                                         sendRequestState.DestinationReceived,
                                                         sendRequestState.NetworkPathReceived,
                                                         Request.         EventTrackingId,
                                                         Request.         RequestId,
                                                         sendRequestState.ResponseTimestamp,
                                                         Request.         CancellationToken
                                                     );

                                if (sendRequestState.IsValidJSONRequestError(Request, out var jsonRequestError))
                                    response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListRequestError(
                                                         Request,
                                                         jsonRequestError,
                                                         sendRequestState.WebSocketConnectionReceived,
                                                         sendRequestState.DestinationReceived,
                                                         sendRequestState.NetworkPathReceived,
                                                         Request.EventTrackingId,
                                                         Request.RequestId,
                                                         sendRequestState.ResponseTimestamp,
                                                         Request.CancellationToken
                                                     );

                                response ??= new SendLocalListResponse(
                                                 Request,
                                                 UpdateStatus.Failed,
                                                 Result: Result.FromSendRequestState(sendRequestState)
                                             );

                            }

                        }
                        break;


                    case SerializationFormats.JSON_UTF8_Binary: {

                        //    #region Sign request message

                        //    if (!parentNetworkingNode.OCPP.SignaturePolicy.SignRequestMessage(
                        //            Request,
                        //            Request.ToJSON(
                        //                parentNetworkingNode.OCPP.CustomSendLocalListRequestSerializer,
                        //                parentNetworkingNode.OCPP.CustomChargingStationSerializer,
                        //                parentNetworkingNode.OCPP.CustomSignatureSerializer,
                        //                parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        //            ),
                        //            out var signingErrors
                        //        ))
                        //    {

                        //        response = SendLocalListResponse.SignatureError(
                        //                       Request,
                        //                       signingErrors
                        //                   );

                        //    }

                        //    #endregion

                        //    else
                        //    {

                        //        #region Send request message

                        //        var sendRequestState = await SendBinaryRequestAndWait(

                        //                                         OCPP_BinaryRequestMessage.FromRequest(
                        //                                             Request,
                        //                                             Request.ToJSON(
                        //                                                 parentNetworkingNode.OCPP.CustomSendLocalListRequestSerializer,
                        //                                                 parentNetworkingNode.OCPP.CustomChargingStationSerializer,
                        //                                                 parentNetworkingNode.OCPP.CustomSignatureSerializer,
                        //                                                 parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        //                                             ).ToUTF8Bytes()
                        //                                         ),

                        //                                         sentMessageResult => LogEvent(
                        //                                             OnSendLocalListRequestSent,
                        //                                             loggingDelegate => loggingDelegate.Invoke(
                        //                                                 Timestamp.Now,
                        //                                                 parentNetworkingNode,
                        //                                                 sentMessageResult.Connection,
                        //                                                 Request,
                        //                                                 sentMessageResult.Result,
                        //                                                 Request.CancellationToken
                        //                                             )
                        //                                         )

                        //                                     );

                        //        #endregion

                        //        if (sendRequestState.IsValidBinaryResponse(Request, out var binaryResponse))
                        //            response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListResponse(
                        //                                 Request,
                        //                                 binaryResponse,
                        //                                 sendRequestState.WebSocketConnectionReceived,
                        //                                 sendRequestState.DestinationReceived,
                        //                                 sendRequestState.NetworkPathReceived,
                        //                                 Request.         EventTrackingId,
                        //                                 Request.         RequestId,
                        //                                 sendRequestState.ResponseTimestamp,
                        //                                 Request.         CancellationToken
                        //                             );

                        //        if (sendRequestState.IsValidBinaryRequestError(Request, out var binaryRequestError))
                        //            response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListRequestError(
                        //                                 Request,
                        //                                 binaryRequestError,
                        //                                 sendRequestState.WebSocketConnectionReceived,
                        //                                 sendRequestState.DestinationReceived,
                        //                                 sendRequestState.NetworkPathReceived,
                        //                                 Request.EventTrackingId,
                        //                                 Request.RequestId,
                        //                                 sendRequestState.ResponseTimestamp,
                        //                                 Request.CancellationToken
                        //                             );

                        //        response ??= new SendLocalListResponse(
                        //                         Request,
                        //                         RegistrationStatus.Rejected,
                        //                         Timestamp.Now,
                        //                         SendLocalListResponse.DefaultInterval,
                        //                         Result: Result.FromSendRequestState(sendRequestState)
                        //                     );

                        //    }

                        }
                        break;


                    case SerializationFormats.BinaryCompact: {

                            //#region Sign request message

                            //if (!parentNetworkingNode.OCPP.SignaturePolicy.SignRequestMessage(
                            //        Request,
                            //        Request.ToBinary(
                            //            //parentNetworkingNode.OCPP.CustomBinarySendLocalListRequestSerializer,
                            //            //parentNetworkingNode.OCPP.CustomBinaryChargingStationSerializer,
                            //            //parentNetworkingNode.OCPP.CustomBinarySignatureSerializer,
                            //            IncludeSignatures: false
                            //        ),
                            //        out var signingErrors
                            //    ))
                            //{

                            //    response = SendLocalListResponse.SignatureError(
                            //                   Request,
                            //                   signingErrors
                            //               );

                            //}

                            //#endregion

                            //else
                            //{

                            //    #region Send request message

                            //    var sendRequestState = await SendBinaryRequestAndWait(

                            //                                     OCPP_BinaryRequestMessage.FromRequest(
                            //                                         Request,
                            //                                         Request.ToBinary(
                            //                                             //parentNetworkingNode.OCPP.CustomSendLocalListRequestSerializer,
                            //                                             //parentNetworkingNode.OCPP.CustomChargingStationSerializer,
                            //                                             //parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            //                                             //parentNetworkingNode.OCPP.CustomCustomDataSerializer
                            //                                             IncludeSignatures: true
                            //                                         )
                            //                                     ),

                            //                                     sentMessageResult => LogEvent(
                            //                                         OnSendLocalListRequestSent,
                            //                                         loggingDelegate => loggingDelegate.Invoke(
                            //                                             Timestamp.Now,
                            //                                             parentNetworkingNode,
                            //                                             sentMessageResult.Connection,
                            //                                             Request,
                            //                                             sentMessageResult.Result,
                            //                                             Request.CancellationToken
                            //                                         )
                            //                                     )

                            //                                 );

                            //    #endregion

                            //    if (sendRequestState.IsValidJSONResponse(Request, out var jsonResponse))
                            //        response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListResponse(
                            //                             Request,
                            //                             jsonResponse,
                            //                             sendRequestState.WebSocketConnectionReceived,
                            //                             sendRequestState.DestinationReceived,
                            //                             sendRequestState.NetworkPathReceived,
                            //                             Request.         EventTrackingId,
                            //                             Request.         RequestId,
                            //                             sendRequestState.ResponseTimestamp,
                            //                             Request.         CancellationToken
                            //                         );

                            //    if (sendRequestState.IsValidJSONRequestError(Request, out var jsonRequestError))
                            //        response = await parentNetworkingNode.OCPP.IN.Receive_SendLocalListRequestError(
                            //                             Request,
                            //                             jsonRequestError,
                            //                             sendRequestState.WebSocketConnectionReceived,
                            //                             sendRequestState.DestinationReceived,
                            //                             sendRequestState.NetworkPathReceived,
                            //                             Request.EventTrackingId,
                            //                             Request.RequestId,
                            //                             sendRequestState.ResponseTimestamp,
                            //                             Request.CancellationToken
                            //                         );

                            //    response ??= new SendLocalListResponse(
                            //                     Request,
                            //                     RegistrationStatus.Rejected,
                            //                     Timestamp.Now,
                            //                     SendLocalListResponse.DefaultInterval,
                            //                     Result: Result.FromSendRequestState(sendRequestState)
                            //                 );

                            //}

                        }
                        break;


                    default:
                        response ??= new SendLocalListResponse(
                                         Request,
                                         UpdateStatus.Failed
                                     );
                        break;

                }

            }
            catch (Exception e)
            {

                response = SendLocalListResponse.ExceptionOccured(
                               Request,
                               e
                           );

            }

            return response;

        }

        #endregion


        #region Send OnSendLocalListResponseSent event

        /// <summary>
        /// An event sent whenever a SendLocalList response was sent.
        /// </summary>
        public event OnSendLocalListResponseSentDelegate?  OnSendLocalListResponseSent;

        public Task SendOnSendLocalListResponseSent(DateTime                   Timestamp,
                                                       IEventSender               Sender,
                                                       IWebSocketConnection?      Connection,
                                                       SendLocalListRequest    Request,
                                                       SendLocalListResponse   Response,
                                                       TimeSpan                   Runtime,
                                                       SentMessageResults         SentMessageResult,
                                                       CancellationToken          CancellationToken = default)

            => LogEvent(
                   OnSendLocalListResponseSent,
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

        #region Send OnSendLocalListRequestErrorSent event

        /// <summary>
        /// An event sent whenever a SendLocalList request error was sent.
        /// </summary>
        public event OnSendLocalListRequestErrorSentDelegate? OnSendLocalListRequestErrorSent;


        public Task SendOnSendLocalListRequestErrorSent(DateTime                      Timestamp,
                                                           IEventSender                  Sender,
                                                           IWebSocketConnection?         Connection,
                                                           SendLocalListRequest?      Request,
                                                           OCPP_JSONRequestErrorMessage  RequestErrorMessage,
                                                           TimeSpan                      Runtime,
                                                           SentMessageResults            SentMessageResult,
                                                           CancellationToken             CancellationToken = default)

            => LogEvent(
                   OnSendLocalListRequestErrorSent,
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

        #region Send OnSendLocalListResponseErrorSent event

        /// <summary>
        /// An event sent whenever a SendLocalList response error was sent.
        /// </summary>
        public event OnSendLocalListResponseErrorSentDelegate? OnSendLocalListResponseErrorSent;


        public Task SendOnSendLocalListResponseErrorSent(DateTime                       Timestamp,
                                                            IEventSender                   Sender,
                                                            IWebSocketConnection?          Connection,
                                                            SendLocalListRequest?       Request,
                                                            SendLocalListResponse?      Response,
                                                            OCPP_JSONResponseErrorMessage  ResponseErrorMessage,
                                                            TimeSpan                       Runtime,
                                                            SentMessageResults             SentMessageResult,
                                                            CancellationToken              CancellationToken = default)

            => LogEvent(
                   OnSendLocalListResponseErrorSent,
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