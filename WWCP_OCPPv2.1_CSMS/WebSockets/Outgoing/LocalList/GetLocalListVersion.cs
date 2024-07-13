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
using cloud.charging.open.protocols.OCPPv2_1.CS;

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

        public CustomJObjectSerializerDelegate<GetLocalListVersionRequest>?  CustomGetLocalListVersionRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<GetLocalListVersionResponse>?     CustomGetLocalListVersionResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetLocalListVersion request was sent.
        /// </summary>
        public event OnGetLocalListVersionRequestSentDelegate?     OnGetLocalListVersionRequestSent;

        /// <summary>
        /// An event sent whenever a response to a GetLocalListVersion request was sent.
        /// </summary>
        public event OnGetLocalListVersionResponseReceivedDelegate?    OnGetLocalListVersionResponseReceived;

        #endregion


        #region GetLocalListVersion(Request)

        public async Task<GetLocalListVersionResponse> GetLocalListVersion(GetLocalListVersionRequest Request)
        {

            #region Send OnGetLocalListVersionRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetLocalListVersionRequestSent?.Invoke(startTime,
                                                     this,
                                                     Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetLocalListVersionRequestSent));
            }

            #endregion


            GetLocalListVersionResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.DestinationId,
                                                 Request.NetworkPath.Append(NetworkingNodeId),
                                                 Request.RequestId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomGetLocalListVersionRequestSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (GetLocalListVersionResponse.TryParse(Request,
                                                             sendRequestState.JSONResponse.Payload,
                                                             out var getLocalListVersionResponse,
                                                             out var errorResponse,
                                                             CustomGetLocalListVersionResponseParser) &&
                        getLocalListVersionResponse is not null)
                    {
                        response = getLocalListVersionResponse;
                    }

                    response ??= new GetLocalListVersionResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new GetLocalListVersionResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new GetLocalListVersionResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGetLocalListVersionResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGetLocalListVersionResponseReceived?.Invoke(endTime,
                                                      this,
                                                      Request,
                                                      response,
                                                      endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetLocalListVersionResponseReceived));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
