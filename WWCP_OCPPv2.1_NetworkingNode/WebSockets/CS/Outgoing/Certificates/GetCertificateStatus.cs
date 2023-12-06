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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CS
{

    /// <summary>
    /// A charging station HTTP Web Socket client.
    /// </summary>
    public partial class NetworkingNodeWSClient : WebSocketClient,
                                                   INetworkingNodeWebSocketClient,
                                                   INetworkingNodeServer,
                                                   INetworkingNodeClientEvents
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<GetCertificateStatusRequest>?  CustomGetCertificateStatusSerializer        { get; set; }

        public CustomJObjectParserDelegate<GetCertificateStatusResponse>?     CustomGetCertificateStatusResponseParser    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a get certificate status request will be sent to the CSMS.
        /// </summary>
        public event CS.OnGetCertificateStatusRequestDelegate?     OnGetCertificateStatusRequest;

        /// <summary>
        /// An event fired whenever a get certificate status request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?                   OnGetCertificateStatusWSRequest;

        /// <summary>
        /// An event fired whenever a response to a get certificate status request was received.
        /// </summary>
        public event ClientResponseLogHandler?                  OnGetCertificateStatusWSResponse;

        /// <summary>
        /// An event fired whenever a response to a get certificate status request was received.
        /// </summary>
        public event CS.OnGetCertificateStatusResponseDelegate?    OnGetCertificateStatusResponse;

        #endregion


        #region GetCertificateStatus(Request)

        /// <summary>
        /// Send a get certificate status request.
        /// </summary>
        /// <param name="Request">A GetCertificateStatus request.</param>
        public async Task<GetCertificateStatusResponse>

            GetCertificateStatus(GetCertificateStatusRequest Request)

        {

            #region Send OnGetCertificateStatusRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetCertificateStatusRequest?.Invoke(startTime,
                                                      this,
                                                      Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnGetCertificateStatusRequest));
            }

            #endregion


            GetCertificateStatusResponse? response = null;

            try
            {

                var requestMessage = await SendRequest(
                                         Request.NetworkingNodeId,
                                         Request.NetworkPath,
                                         Request.Action,
                                         Request.RequestId,
                                         Request.ToJSON(
                                             CustomGetCertificateStatusSerializer,
                                             CustomOCSPRequestDataSerializer,
                                             CustomSignatureSerializer,
                                             CustomCustomDataSerializer
                                         )
                                     );

                if (requestMessage.NoErrors)
                {

                    var sendRequestState = await WaitForResponse(requestMessage);

                    if (sendRequestState.NoErrors &&
                        sendRequestState.JSONResponse is not null)
                    {

                        if (GetCertificateStatusResponse.TryParse(Request,
                                                                  sendRequestState.JSONResponse.Payload,
                                                                  out var getCertificateStatusResponse,
                                                                  out var errorResponse,
                                                                  CustomGetCertificateStatusResponseParser) &&
                            getCertificateStatusResponse is not null)
                        {
                            response = getCertificateStatusResponse;
                        }

                        response ??= new GetCertificateStatusResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                    }

                    response ??= new GetCertificateStatusResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

                }

                response ??= new GetCertificateStatusResponse(
                                 Request,
                                 Result.GenericError(requestMessage.ErrorMessage)
                             );

            }
            catch (Exception e)
            {

                response = new GetCertificateStatusResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGetCertificateStatusResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGetCertificateStatusResponse?.Invoke(endTime,
                                                       this,
                                                       Request,
                                                       response,
                                                       endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSClient) + "." + nameof(OnGetCertificateStatusResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}