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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region OnGetFile (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a GetFile request will be sent to a charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetFileRequestDelegate(DateTime         Timestamp,
                                                  IEventSender     Sender,
                                                  GetFileRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a GetFile request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetFileResponseDelegate(DateTime             Timestamp,
                                                   IEventSender         Sender,
                                                   GetFileRequest       Request,
                                                   CS.GetFileResponse   Response,
                                                   TimeSpan             Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class NetworkingNodeWSServer : WebSocketServer,
                                                  INetworkingNodeChannel
    {

        #region Custom binary serializer delegates

        public CustomJObjectSerializerDelegate<GetFileRequest>?  CustomGetFileRequestSerializer    { get; set; }

        public CustomBinaryParserDelegate<CS.GetFileResponse>?   CustomGetFileResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetFile request was sent.
        /// </summary>
        public event OnGetFileRequestDelegate?     OnGetFileRequest;

        /// <summary>
        /// An event sent whenever a response to a GetFile request was sent.
        /// </summary>
        public event OnGetFileResponseDelegate?    OnGetFileResponse;

        #endregion


        #region GetFile(Request)

        public async Task<CS.GetFileResponse> GetFile(GetFileRequest Request)
        {

            #region Send OnGetFileRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetFileRequest?.Invoke(startTime,
                                         this,
                                         Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnGetFileRequest));
            }

            #endregion


            CS.GetFileResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.RequestId,
                                                 Request.ChargingStationId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomGetFileRequestSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.BinaryResponse is not null)
                {

                    if (CS.GetFileResponse.TryParse(Request,
                                                    sendRequestState.BinaryResponse.Payload,
                                                    out var getFileResponse,
                                                    out var errorResponse,
                                                    CustomGetFileResponseParser) &&
                        getFileResponse is not null)
                    {
                        response = getFileResponse;
                    }

                    response ??= new CS.GetFileResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                }

                response ??= new CS.GetFileResponse(
                                 Request,
                                 Request.FileName,
                                 GetFileStatus.Rejected
                             );

            }
            catch (Exception e)
            {

                response = new CS.GetFileResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGetFileResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGetFileResponse?.Invoke(endTime,
                                          this,
                                          Request,
                                          response,
                                          endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnGetFileResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
