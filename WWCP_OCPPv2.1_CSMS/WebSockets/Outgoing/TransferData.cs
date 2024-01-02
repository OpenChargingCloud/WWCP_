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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : AOCPPWebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<DataTransferRequest>?  CustomDataTransferRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<DataTransferResponse>?     CustomDataTransferResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a DataTransfer request was sent.
        /// </summary>
        public event OnDataTransferRequestSentDelegate?     OnDataTransferRequest;

        /// <summary>
        /// An event sent whenever a response to a DataTransfer request was sent.
        /// </summary>
        public event OnDataTransferResponseReceivedDelegate?    OnDataTransferResponse;

        #endregion


        #region TransferData(Request)

        public async Task<DataTransferResponse> TransferData(DataTransferRequest Request)
        {

            #region Send OnDataTransferRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnDataTransferRequest?.Invoke(startTime,
                                              this,
                                              Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnDataTransferRequest));
            }

            #endregion


            DataTransferResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.DestinationNodeId,
                                                 Request.NetworkPath.Append(NetworkingNodeId),
                                                 Request.RequestId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomDataTransferRequestSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (DataTransferResponse.TryParse(Request,
                                                      sendRequestState.JSONResponse.Payload,
                                                      out var dataTransferResponse,
                                                      out var errorResponse,
                                                      CustomDataTransferResponseParser) &&
                        dataTransferResponse is not null)
                    {
                        response = dataTransferResponse;
                    }

                    response ??= new DataTransferResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                }

                response ??= new DataTransferResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

            }
            catch (Exception e)
            {

                response = new DataTransferResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnDataTransferResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnDataTransferResponse?.Invoke(endTime,
                                               this,
                                               Request,
                                               response,
                                               endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnDataTransferResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
