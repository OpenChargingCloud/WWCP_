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

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    #region OnDeleteFile (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a DeleteFile request will be sent to a charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnDeleteFileRequestDelegate(DateTime            Timestamp,
                                                     IEventSender        Sender,
                                                     DeleteFileRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a DeleteFile request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnDeleteFileResponseDelegate(DateTime                Timestamp,
                                                      IEventSender            Sender,
                                                      DeleteFileRequest       Request,
                                                      CS.DeleteFileResponse   Response,
                                                      TimeSpan                Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom binary serializer delegates

        public CustomJObjectSerializerDelegate<DeleteFileRequest>?  CustomDeleteFileRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<CS.DeleteFileResponse>?  CustomDeleteFileResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a DeleteFile request was sent.
        /// </summary>
        public event OnDeleteFileRequestDelegate?     OnDeleteFileRequest;

        /// <summary>
        /// An event sent whenever a response to a DeleteFile request was sent.
        /// </summary>
        public event OnDeleteFileResponseDelegate?    OnDeleteFileResponse;

        #endregion


        #region DeleteFile(Request)

        public async Task<CS.DeleteFileResponse> DeleteFile(DeleteFileRequest Request)
        {

            #region Send OnDeleteFileRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnDeleteFileRequest?.Invoke(startTime,
                                            this,
                                            Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnDeleteFileRequest));
            }

            #endregion


            CS.DeleteFileResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.RequestId,
                                                 Request.ChargingStationId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomDeleteFileRequestSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (CS.DeleteFileResponse.TryParse(Request,
                                                       sendRequestState.JSONResponse.Payload,
                                                       out var deleteFileResponse,
                                                       out var errorResponse,
                                                       CustomDeleteFileResponseParser) &&
                        deleteFileResponse is not null)
                    {
                        response = deleteFileResponse;
                    }

                    response ??= new CS.DeleteFileResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                }

                response ??= new CS.DeleteFileResponse(
                                 Request,
                                 Request.FileName,
                                 DeleteFileStatus.Rejected
                             );

            }
            catch (Exception e)
            {

                response = new CS.DeleteFileResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnDeleteFileResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnDeleteFileResponse?.Invoke(endTime,
                                             this,
                                             Request,
                                             response,
                                             endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnDeleteFileResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
