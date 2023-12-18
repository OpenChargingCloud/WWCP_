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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CS;
using cloud.charging.open.protocols.OCPP.CSMS;

using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CS
{

    /// <summary>
    /// The charging station HTTP WebSocket client runs on a charging station
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class NetworkingNodeWSClient : AOCPPWebSocketClient,
                                                  INetworkingNodeWebSocketClient,
                                                  INetworkingNodeServer,
                                                  INetworkingNodeClientEvents
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<DeleteFileRequest>?       CustomDeleteFileRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<DeleteFileResponse>?  CustomDeleteFileResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a DeleteFile websocket request was received.
        /// </summary>
        public event WSClientJSONRequestLogHandler?                OnDeleteFileWSRequest;

        /// <summary>
        /// An event sent whenever a DeleteFile request was received.
        /// </summary>
        public event OCPP.CS.OnDeleteFileRequestDelegate?          OnDeleteFileRequest;

        /// <summary>
        /// An event sent whenever a DeleteFile request was received.
        /// </summary>
        public event OCPP.CS.OnDeleteFileDelegate?                 OnDeleteFile;

        /// <summary>
        /// An event sent whenever a response to a DeleteFile request was sent.
        /// </summary>
        public event OCPP.CS.OnDeleteFileResponseDelegate?         OnDeleteFileResponse;

        /// <summary>
        /// An event sent whenever a websocket response to a DeleteFile request was sent.
        /// </summary>
        public event WSClientJSONRequestJSONResponseLogHandler?    OnDeleteFileWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_DeleteFile(DateTime                   RequestTimestamp,
                               WebSocketClientConnection  WebSocketConnection,
                               NetworkingNode_Id          DestinationNodeId,
                               NetworkPath                NetworkPath,
                               EventTracking_Id           EventTrackingId,
                               Request_Id                 RequestId,
                               JObject                    RequestJSON,
                               CancellationToken          CancellationToken)

        {

            #region Send OnDeleteFileWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnDeleteFileWSRequest?.Invoke(startTime,
                                              WebSocketConnection,
                                              DestinationNodeId,
                                              NetworkPath,
                                              EventTrackingId,
                                              RequestJSON);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnDeleteFileWSRequest));
            }

            #endregion

            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (DeleteFileRequest.TryParse(RequestJSON,
                                               RequestId,
                                               DestinationNodeId,
                                               NetworkPath,
                                               out var request,
                                               out var errorResponse,
                                               CustomDeleteFileRequestParser) && request is not null) {

                    #region Send OnDeleteFileRequest event

                    try
                    {

                        OnDeleteFileRequest?.Invoke(Timestamp.Now,
                                                    this,
                                                    WebSocketConnection,
                                                    request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnDeleteFileRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    DeleteFileResponse? response = null;

                    var results = OnDeleteFile?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnDeleteFileDelegate)?.Invoke(Timestamp.Now,
                                                                                                            this,
                                                                                                            WebSocketConnection,
                                                                                                            request,
                                                                                                            CancellationToken)).
                                      ToArray();

                    if (results?.Length > 0)
                    {

                        await Task.WhenAll(results!);

                        response = results.FirstOrDefault()?.Result;

                    }

                    response ??= DeleteFileResponse.Failed(request);

                    #endregion

                    #region Send OnDeleteFileResponse event

                    try
                    {

                        OnDeleteFileResponse?.Invoke(Timestamp.Now,
                                                     this,
                                                     WebSocketConnection,
                                                     request,
                                                     response,
                                                     response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnDeleteFileResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       NetworkPath.Source,
                                       RequestId,
                                       response.ToJSON(
                                           CustomDeleteFileResponseSerializer,
                                           CustomStatusInfoSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_DeleteFile)[8..],
                                            RequestJSON,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {
                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_DeleteFile)[8..],
                                        RequestJSON,
                                        e
                                    );
            }

            #region Send OnDeleteFileWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnDeleteFileWSResponse?.Invoke(endTime,
                                               WebSocketConnection,
                                               DestinationNodeId,
                                               NetworkPath,
                                               EventTrackingId,
                                               RequestTimestamp,
                                               RequestJSON,
                                               OCPPResponse?.Payload,
                                               OCPPErrorResponse?.ToJSON(),
                                               endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnDeleteFileWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}
