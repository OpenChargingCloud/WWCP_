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

using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    #region OnDeleteFile

    /// <summary>
    /// A DeleteFile request.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The DeleteFile request.</param>
    public delegate Task

        OnDeleteFileRequestDelegate(DateTime                 Timestamp,
                                    IEventSender             Sender,
                                    CSMS.DeleteFileRequest   Request);


    /// <summary>
    /// A DeleteFile request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The DeleteFile request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<DeleteFileResponse>

        OnDeleteFileDelegate(DateTime                    Timestamp,
                             IEventSender                Sender,
                             WebSocketClientConnection   Connection,
                             CSMS.DeleteFileRequest      Request,
                             CancellationToken           CancellationToken);


    /// <summary>
    /// A DeleteFile response.
    /// </summary>
    /// <param name="Timestamp">The log timestamp of the response.</param>
    /// <param name="Sender">The sender of the response.</param>
    /// <param name="Request">The DeleteFile request.</param>
    /// <param name="Response">The DeleteFile response.</param>
    /// <param name="Runtime">The runtime of this request.</param>
    public delegate Task

        OnDeleteFileResponseDelegate(DateTime                 Timestamp,
                                     IEventSender             Sender,
                                     CSMS.DeleteFileRequest   Request,
                                     DeleteFileResponse       Response,
                                     TimeSpan                 Runtime);

    #endregion


    /// <summary>
    /// The charging station HTTP WebSocket client runs on a charging station
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<CSMS.DeleteFileRequest>?  CustomDeleteFileRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<DeleteFileResponse>?  CustomDeleteFileResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a DeleteFile websocket request was received.
        /// </summary>
        public event WSClientJSONRequestLogHandler?               OnDeleteFileWSRequest;

        /// <summary>
        /// An event sent whenever a DeleteFile request was received.
        /// </summary>
        public event OnDeleteFileRequestDelegate?                 OnDeleteFileRequest;

        /// <summary>
        /// An event sent whenever a DeleteFile request was received.
        /// </summary>
        public event OnDeleteFileDelegate?                        OnDeleteFile;

        /// <summary>
        /// An event sent whenever a response to a DeleteFile request was sent.
        /// </summary>
        public event OnDeleteFileResponseDelegate?                OnDeleteFileResponse;

        /// <summary>
        /// An event sent whenever a websocket response to a DeleteFile request was sent.
        /// </summary>
        public event WSClientJSONRequestJSONResponseLogHandler?   OnDeleteFileWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_DeleteFile(DateTime                   RequestTimestamp,
                               WebSocketClientConnection  WebSocketConnection,
                               ChargingStation_Id         ChargingStationId,
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
                                              ChargingStationId,
                                              EventTrackingId,
                                              RequestJSON);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnDeleteFileWSRequest));
            }

            #endregion

            OCPP_JSONResponseMessage?     OCPPResponse        = null;
            OCPP_JSONErrorMessage?  OCPPErrorResponse   = null;

            try
            {

                if (CSMS.DeleteFileRequest.TryParse(RequestJSON,
                                                    RequestId,
                                                    ChargingStationIdentity,
                                                    out var request,
                                                    out var errorResponse,
                                                    CustomDeleteFileRequestParser) &&
                    request is not null) {

                    #region Send OnDeleteFileRequest event

                    try
                    {

                        OnDeleteFileRequest?.Invoke(Timestamp.Now,
                                                    this,
                                                    request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnDeleteFileRequest));
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
                                                  request,
                                                  response,
                                                  response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnDeleteFileResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
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
                                               EventTrackingId,
                                               RequestTimestamp,
                                               RequestJSON,
                                               OCPPResponse?.Payload,
                                               OCPPErrorResponse?.ToJSON(),
                                               endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnDeleteFileWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}
