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

using Newtonsoft.Json.Linq;

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
    /// A logging delegate called whenever a CostUpdated request was received.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket client connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnCostUpdatedRequestReceivedDelegate(DateTime               Timestamp,
                                                              IEventSender           Sender,
                                                              IWebSocketConnection   Connection,
                                                              CostUpdatedRequest     Request,
                                                              CancellationToken      CancellationToken);


    /// <summary>
    /// A logging delegate called whenever a CostUpdated response was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the response logging.</param>
    /// <param name="Sender">The sender of the request/response.</param>
    /// <param name="Connection">The connection of the request.</param>
    /// <param name="Request">The request, when available.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The optional runtime of the request/response pair.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnCostUpdatedResponseReceivedDelegate(DateTime               Timestamp,
                                                               IEventSender           Sender,
                                                               IWebSocketConnection   Connection,
                                                               CostUpdatedRequest?    Request,
                                                               CostUpdatedResponse    Response,
                                                               TimeSpan?              Runtime,
                                                               CancellationToken      CancellationToken);


    /// <summary>
    /// A logging delegate called whenever a CostUpdated request error was received.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The connection of the request.</param>
    /// <param name="Request">The request, when available.</param>
    /// <param name="RequestErrorMessage">The request error message.</param>
    /// <param name="Runtime">The runtime of the request/request error pair.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnCostUpdatedRequestErrorReceivedDelegate(DateTime                       Timestamp,
                                                                   IEventSender                   Sender,
                                                                   IWebSocketConnection           Connection,
                                                                   CostUpdatedRequest?            Request,
                                                                   OCPP_JSONRequestErrorMessage   RequestErrorMessage,
                                                                   TimeSpan?                      Runtime,
                                                                   CancellationToken              CancellationToken);


    /// <summary>
    /// A logging delegate called whenever a CostUpdated response error was received.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="Sender">The sender of the response error.</param>
    /// <param name="Connection">The connection of the response error.</param>
    /// <param name="Request">The request, when available.</param>
    /// <param name="Response">The response, when available.</param>
    /// <param name="ResponseErrorMessage">The response error message.</param>
    /// <param name="Runtime">The optional runtime of the response/response error message pair.</param>
    /// <param name="CancellationToken">An optional cancellation token.</param>
    public delegate Task OnCostUpdatedResponseErrorReceivedDelegate(DateTime                        Timestamp,
                                                                    IEventSender                    Sender,
                                                                    IWebSocketConnection            Connection,
                                                                    CostUpdatedRequest?             Request,
                                                                    CostUpdatedResponse?            Response,
                                                                    OCPP_JSONResponseErrorMessage   ResponseErrorMessage,
                                                                    TimeSpan?                       Runtime,
                                                                    CancellationToken               CancellationToken);

    #endregion


    /// <summary>
    /// A delegate called whenever a CostUpdated response is expected
    /// for a received CostUpdated request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket client connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<CostUpdatedResponse>

        OnCostUpdatedDelegate(DateTime               Timestamp,
                              IEventSender           Sender,
                              IWebSocketConnection   Connection,
                              CostUpdatedRequest     Request,
                              CancellationToken      CancellationToken);


    public partial class OCPPWebSocketAdapterIN
    {

        // Wired via reflection!

        #region Receive CostUpdated request

        /// <summary>
        /// An event sent whenever a CostUpdated request was received.
        /// </summary>
        public event OnCostUpdatedRequestReceivedDelegate?  OnCostUpdatedRequestReceived;

        /// <summary>
        /// An event sent whenever a CostUpdated request was received for processing.
        /// </summary>
        public event OnCostUpdatedDelegate?                 OnCostUpdated;


        public async Task<OCPP_Response>

            Receive_CostUpdated(DateTime              RequestTimestamp,
                                IWebSocketConnection  WebSocketConnection,
                                SourceRouting         SourceRouting,
                                NetworkPath           NetworkPath,
                                EventTracking_Id      EventTrackingId,
                                Request_Id            RequestId,
                                JObject               JSONRequest,
                                CancellationToken     CancellationToken)

        {

            OCPP_Response? ocppResponse = null;

            try
            {

                if (CostUpdatedRequest.TryParse(JSONRequest,
                                          RequestId,
                                          SourceRouting,
                                          NetworkPath,
                                          out var request,
                                          out var errorResponse,
                                          RequestTimestamp,
                                          parentNetworkingNode.OCPP.DefaultRequestTimeout,
                                          EventTrackingId,
                                          parentNetworkingNode.OCPP.CustomCostUpdatedRequestParser)) {

                    CostUpdatedResponse? response = null;

                    #region Verify request signature(s)

                    if (!parentNetworkingNode.OCPP.SignaturePolicy.VerifyRequestMessage(
                        request,
                        request.ToJSON(
                            parentNetworkingNode.OCPP.CustomCostUpdatedRequestSerializer,
                            parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        ),
                        out errorResponse))
                    {

                        response = CostUpdatedResponse.SignatureError(
                                       request,
                                       errorResponse
                                   );

                    }

                    #endregion

                    #region Send OnCostUpdatedRequestReceived event

                    await LogEvent(
                              OnCostUpdatedRequestReceived,
                              loggingDelegate => loggingDelegate.Invoke(
                                  Timestamp.Now,
                                  parentNetworkingNode,
                                  WebSocketConnection,
                                  request,
                                  CancellationToken
                              )
                          );

                    #endregion


                    response ??= await CallProcessor(
                                           OnCostUpdated,
                                           filter => filter.Invoke(
                                                         Timestamp.Now,
                                                         parentNetworkingNode,
                                                         WebSocketConnection,
                                                         request,
                                                         CancellationToken
                                                     )
                                       );

                    response ??= CostUpdatedResponse.Failed(request);


                    #region Sign response message

                    parentNetworkingNode.OCPP.SignaturePolicy.SignResponseMessage(
                        response,
                        response.ToJSON(
                            parentNetworkingNode.OCPP.CustomCostUpdatedResponseSerializer,
                            parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        ),
                        out var errorResponse2
                    );

                    #endregion

                    ocppResponse = OCPP_Response.JSONResponse(
                                       EventTrackingId,
                                       SourceRouting.To(NetworkPath.Source),
                                       NetworkPath.From(parentNetworkingNode.Id),
                                       RequestId,
                                       response.ToJSON(
                                           parentNetworkingNode.OCPP.CustomCostUpdatedResponseSerializer,
                                           parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                           parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                       ),
                                       async sentMessageResult => await parentNetworkingNode.OCPP.OUT.SendOnCostUpdatedResponseSent(
                                                                            Timestamp.Now,
                                                                            parentNetworkingNode,
                                                                            sentMessageResult.Connection,
                                                                            request,
                                                                            response,
                                                                            response.Runtime,
                                                                            sentMessageResult.Result,
                                                                            CancellationToken
                                                                        ),
                                       CancellationToken
                                   );

                }

                else
                    ocppResponse = OCPP_Response.CouldNotParse(
                                       EventTrackingId,
                                       RequestId,
                                       nameof(Receive_CostUpdated)[8..],
                                       JSONRequest,
                                       errorResponse
                                   );

            }
            catch (Exception e)
            {

                ocppResponse = OCPP_Response.ExceptionOccurred(
                                   EventTrackingId,
                                   RequestId,
                                   nameof(Receive_CostUpdated)[8..],
                                   JSONRequest,
                                   e
                               );

            }

            return ocppResponse;

        }

        #endregion

        #region Receive CostUpdated response

        /// <summary>
        /// An event fired whenever a CostUpdated response was received.
        /// </summary>
        public event OnCostUpdatedResponseReceivedDelegate? OnCostUpdatedResponseReceived;


        public async Task<CostUpdatedResponse>

            Receive_CostUpdatedResponse(CostUpdatedRequest    Request,
                                        JObject               ResponseJSON,
                                        IWebSocketConnection  WebSocketConnection,
                                        SourceRouting         SourceRouting,
                                        NetworkPath           NetworkPath,
                                        EventTracking_Id      EventTrackingId,
                                        Request_Id            RequestId,
                                        DateTime?             ResponseTimestamp   = null,
                                        CancellationToken     CancellationToken   = default)

        {

            CostUpdatedResponse? response = null;

            try
            {

                if (CostUpdatedResponse.TryParse(Request,
                                                 ResponseJSON,
                                                 SourceRouting,
                                                 NetworkPath,
                                                 out response,
                                                 out var errorResponse,
                                                 ResponseTimestamp,
                                                 parentNetworkingNode.OCPP.CustomCostUpdatedResponseParser,
                                                 parentNetworkingNode.OCPP.CustomSignatureParser,
                                                 parentNetworkingNode.OCPP.CustomCustomDataParser)) {

                    #region Verify response signature(s)

                    if (!parentNetworkingNode.OCPP.SignaturePolicy.VerifyResponseMessage(
                            response,
                            response.ToJSON(
                                parentNetworkingNode.OCPP.CustomCostUpdatedResponseSerializer,
                                parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                parentNetworkingNode.OCPP.CustomCustomDataSerializer
                            ),
                            out errorResponse
                        ))
                    {

                        response = CostUpdatedResponse.SignatureError(
                                       Request,
                                       errorResponse
                                   );

                    }

                    #endregion

                }

                else
                    response = CostUpdatedResponse.FormationViolation(
                                   Request,
                                   errorResponse
                               );

            }
            catch (Exception e)
            {

                response = CostUpdatedResponse.ExceptionOccured(
                               Request,
                               e
                           );

            }


            #region Send OnCostUpdatedResponseReceived event

            await LogEvent(
                      OnCostUpdatedResponseReceived,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          WebSocketConnection,
                          Request,
                          response,
                          response.Runtime,
                          CancellationToken
                      )
                  );

            #endregion

            return response;

        }

        #endregion

        #region Receive CostUpdated request error

        /// <summary>
        /// An event fired whenever a CostUpdated request error was received.
        /// </summary>
        public event OnCostUpdatedRequestErrorReceivedDelegate? CostUpdatedRequestErrorReceived;


        public async Task<CostUpdatedResponse>

            Receive_CostUpdatedRequestError(CostUpdatedRequest            Request,
                                            OCPP_JSONRequestErrorMessage  RequestErrorMessage,
                                            IWebSocketConnection          Connection,
                                            SourceRouting                 SourceRouting,
                                            NetworkPath                   NetworkPath,
                                            EventTracking_Id              EventTrackingId,
                                            Request_Id                    RequestId,
                                            DateTime?                     ResponseTimestamp   = null,
                                            CancellationToken             CancellationToken   = default)
        {

            //parentNetworkingNode.OCPP.SignaturePolicy.VerifyResponseMessage(
            //    response,
            //    response.ToJSON(
            //        parentNetworkingNode.OCPP.CustomCostUpdatedResponseSerializer,
            //        parentNetworkingNode.OCPP.CustomIdTokenInfoSerializer,
            //        parentNetworkingNode.OCPP.CustomIdTokenSerializer,
            //        parentNetworkingNode.OCPP.CustomAdditionalInfoSerializer,
            //        parentNetworkingNode.OCPP.CustomMessageContentSerializer,
            //        parentNetworkingNode.OCPP.CustomTransactionLimitsSerializer,
            //        parentNetworkingNode.OCPP.CustomSignatureSerializer,
            //        parentNetworkingNode.OCPP.CustomCustomDataSerializer
            //    ),
            //    out errorResponse
            //);

            #region Send CostUpdatedRequestErrorReceived event

            await LogEvent(
                      CostUpdatedRequestErrorReceived,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          Connection,
                          Request,
                          RequestErrorMessage,
                          RequestErrorMessage.ResponseTimestamp - Request.RequestTimestamp,
                          CancellationToken
                      )
                  );

            #endregion


            var response = CostUpdatedResponse.RequestError(
                               Request,
                               RequestErrorMessage.EventTrackingId,
                               RequestErrorMessage.ErrorCode,
                               RequestErrorMessage.ErrorDescription,
                               RequestErrorMessage.ErrorDetails,
                               RequestErrorMessage.ResponseTimestamp,
                               RequestErrorMessage.Destination,
                               RequestErrorMessage.NetworkPath
                           );

            #region Send OnCostUpdatedResponseReceived event

            await LogEvent(
                      OnCostUpdatedResponseReceived,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          Connection,
                          Request,
                          response,
                          response.Runtime,
                          CancellationToken
                      )
                  );

            #endregion

            return response;

        }

        #endregion

        #region Receive CostUpdated response error

        /// <summary>
        /// An event fired whenever a CostUpdated response error was received.
        /// </summary>
        public event OnCostUpdatedResponseErrorReceivedDelegate? CostUpdatedResponseErrorReceived;


        public async Task

            Receive_CostUpdatedResponseError(CostUpdatedRequest?            Request,
                                             CostUpdatedResponse?           Response,
                                             OCPP_JSONResponseErrorMessage  ResponseErrorMessage,
                                             IWebSocketConnection           Connection,
                                             SourceRouting                  SourceRouting,
                                             NetworkPath                    NetworkPath,
                                             EventTracking_Id               EventTrackingId,
                                             Request_Id                     RequestId,
                                             DateTime?                      ResponseTimestamp   = null,
                                             CancellationToken              CancellationToken   = default)

        {

            //parentNetworkingNode.OCPP.SignaturePolicy.VerifyResponseMessage(
            //    response,
            //    response.ToJSON(
            //        parentNetworkingNode.OCPP.CustomCostUpdatedResponseSerializer,
            //        parentNetworkingNode.OCPP.CustomIdTokenInfoSerializer,
            //        parentNetworkingNode.OCPP.CustomIdTokenSerializer,
            //        parentNetworkingNode.OCPP.CustomAdditionalInfoSerializer,
            //        parentNetworkingNode.OCPP.CustomMessageContentSerializer,
            //        parentNetworkingNode.OCPP.CustomTransactionLimitsSerializer,
            //        parentNetworkingNode.OCPP.CustomSignatureSerializer,
            //        parentNetworkingNode.OCPP.CustomCustomDataSerializer
            //    ),
            //    out errorResponse
            //);

            #region Send CostUpdatedResponseErrorReceived event

            await LogEvent(
                      CostUpdatedResponseErrorReceived,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          Connection,
                          Request,
                          Response,
                          ResponseErrorMessage,
                          Response is not null
                              ? ResponseErrorMessage.ResponseTimestamp - Response.ResponseTimestamp
                              : null,
                          CancellationToken
                      )
                  );

            #endregion


        }

        #endregion

    }

}
