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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class OCPPWebSocketAdapterIN : IOCPPWebSocketAdapterIN
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<TransactionEventRequest>?       CustomTransactionEventRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<TransactionEventResponse>?  CustomTransactionEventResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a TransactionEvent WebSocket request was received.
        /// </summary>
        public event WebSocketJSONRequestLogHandler?                      OnTransactionEventWSRequest;

        /// <summary>
        /// An event sent whenever a TransactionEvent request was received.
        /// </summary>
        public event OCPPv2_1.CSMS.OnTransactionEventRequestReceivedDelegate?     OnTransactionEventRequestReceived;

        /// <summary>
        /// An event sent whenever a TransactionEvent request was received.
        /// </summary>
        public event OCPPv2_1.CSMS.OnTransactionEventDelegate?            OnTransactionEvent;

        /// <summary>
        /// An event sent whenever a TransactionEvent response was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnTransactionEventResponseSentDelegate?    OnTransactionEventResponseSent;

        /// <summary>
        /// An event sent whenever a TransactionEvent WebSocket response was sent.
        /// </summary>
        public event WebSocketJSONRequestJSONResponseLogHandler?          OnTransactionEventWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONRequestErrorMessage?>>

            Receive_TransactionEvent(DateTime                   RequestTimestamp,
                                     IWebSocketConnection  WebSocketConnection,
                                     NetworkingNode_Id          DestinationNodeId,
                                     NetworkPath                NetworkPath,
                                     EventTracking_Id           EventTrackingId,
                                     Request_Id                 RequestId,
                                     JObject                    JSONRequest,
                                     CancellationToken          CancellationToken)

        {

            #region Send OnTransactionEventWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnTransactionEventWSRequest?.Invoke(startTime,
                                                    parentNetworkingNode,
                                                    WebSocketConnection,
                                                    DestinationNodeId,
                                                    NetworkPath,
                                                    EventTrackingId,
                                                    RequestTimestamp,
                                                    JSONRequest);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterIN) + "." + nameof(OnTransactionEventWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONRequestErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (TransactionEventRequest.TryParse(JSONRequest,
                                                     RequestId,
                                                     DestinationNodeId,
                                                     NetworkPath,
                                                     out var request,
                                                     out var errorResponse,
                                                     CustomTransactionEventRequestParser)) {

                    #region Send OnTransactionEventRequest event

                    try
                    {

                        OnTransactionEventRequestReceived?.Invoke(Timestamp.Now,
                                                          parentNetworkingNode,
                                                          WebSocketConnection,
                                                          request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(OCPPWebSocketAdapterIN) + "." + nameof(OnTransactionEventRequestReceived));
                    }

                    #endregion

                    #region Call async subscribers

                    TransactionEventResponse? response = null;

                    var responseTasks = OnTransactionEvent?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnTransactionEventDelegate)?.Invoke(Timestamp.Now,
                                                                                                                        parentNetworkingNode,
                                                                                                                        WebSocketConnection,
                                                                                                                        request,
                                                                                                                        CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= TransactionEventResponse.Failed(request);

                    #endregion

                    #region Send OnTransactionEventResponse event

                    try
                    {

                        OnTransactionEventResponseSent?.Invoke(Timestamp.Now,
                                                           parentNetworkingNode,
                                                           WebSocketConnection,
                                                           request,
                                                           response,
                                                           response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(OCPPWebSocketAdapterIN) + "." + nameof(OnTransactionEventResponseSent));
                    }

                    #endregion

                    OCPPResponse = OCPP_JSONResponseMessage.From(
                                       NetworkPath.Source,
                                       NetworkPath,
                                       RequestId,
                                       response.ToJSON(
                                           CustomTransactionEventResponseSerializer,
                                           parentNetworkingNode.OCPP.CustomIdTokenInfoSerializer,
                                           parentNetworkingNode.OCPP.CustomIdTokenSerializer,
                                           parentNetworkingNode.OCPP.CustomAdditionalInfoSerializer,
                                           parentNetworkingNode.OCPP.CustomMessageContentSerializer,
                                           parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                           parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONRequestErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_TransactionEvent)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONRequestErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_TransactionEvent)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnTransactionEventWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnTransactionEventWSResponse?.Invoke(endTime,
                                                     parentNetworkingNode,
                                                     WebSocketConnection,
                                                     DestinationNodeId,
                                                     NetworkPath,
                                                     EventTrackingId,
                                                     RequestTimestamp,
                                                     JSONRequest,
                                                     OCPPResponse?.Payload,
                                                     OCPPErrorResponse?.ToJSON(),
                                                     endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterIN) + "." + nameof(OnTransactionEventWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONRequestErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        /// <summary>
        /// An event sent whenever a TransactionEvent response was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnTransactionEventResponseSentDelegate? OnTransactionEventResponseSent;

    }

}
