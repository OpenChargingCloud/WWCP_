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

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class OCPPWebSocketAdapterOUT
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<NotifyCRLRequest>?  CustomNotifyCRLRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<NotifyCRLResponse>?     CustomNotifyCRLResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a NotifyCRL request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnNotifyCRLRequestSentDelegate?     OnNotifyCRLRequestSent;

        /// <summary>
        /// An event sent whenever a response to a NotifyCRL request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnNotifyCRLResponseReceivedDelegate?    OnNotifyCRLResponseReceived;

        #endregion


        #region NotifyCRL(Request)

        /// <summary>
        /// Notify the charging station about the status of a certificate revocation list.
        /// </summary>
        /// <param name="Request">A delete certificate request.</param>
        public async Task<NotifyCRLResponse> NotifyCRL(NotifyCRLRequest Request)
        {

            #region Send OnNotifyCRLRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyCRLRequestSent?.Invoke(startTime,
                                           parentNetworkingNode,
                                           null,
                                           Request,
                                                SentMessageResults.Success);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnNotifyCRLRequestSent));
            }

            #endregion


            NotifyCRLResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONRequestAndWait(
                                                 OCPP_JSONRequestMessage.FromRequest(
                                                     Request,
                                                     Request.ToJSON(
                                                         CustomNotifyCRLRequestSerializer,
                                                         parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                         parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                     )
                                                 )
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (NotifyCRLResponse.TryParse(Request,
                                                   sendRequestState.JSONResponse.Payload,
                                                   out var deleteCertificateResponse,
                                                   out var errorResponse,
                                                   CustomNotifyCRLResponseParser) &&
                        deleteCertificateResponse is not null)
                    {
                        response = deleteCertificateResponse;
                    }

                    response ??= new NotifyCRLResponse(
                                     Request,
                                     Result.FormationViolation(errorResponse)
                                 );

                }

                response ??= new NotifyCRLResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new NotifyCRLResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnNotifyCRLResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnNotifyCRLResponseReceived?.Invoke(endTime,
                                            parentNetworkingNode,
                                            Request,
                                            response,
                                            endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnNotifyCRLResponseReceived));
            }

            #endregion

            return response;

        }

        #endregion


    }

    public partial class OCPPWebSocketAdapterIN
    {

        /// <summary>
        /// An event sent whenever a response to a NotifyCRL request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnNotifyCRLResponseReceivedDelegate? OnNotifyCRLResponseReceived;

    }

}
