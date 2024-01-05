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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CS;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// A charging station HTTP Web Socket client.
    /// </summary>
    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<Get15118EVCertificateRequest>?  CustomGet15118EVCertificateSerializer        { get; set; }

        public CustomJObjectParserDelegate<Get15118EVCertificateResponse>?     CustomGet15118EVCertificateResponseParser    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a get 15118 EV certificate request will be sent to the CSMS.
        /// </summary>
        public event OCPPv2_1.CS.OnGet15118EVCertificateRequestSentDelegate?     OnGet15118EVCertificateRequestSent;

        /// <summary>
        /// An event fired whenever a get 15118 EV certificate request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?                                OnGet15118EVCertificateWSRequest;

        /// <summary>
        /// An event fired whenever a response to a get 15118 EV certificate request was received.
        /// </summary>
        public event ClientResponseLogHandler?                               OnGet15118EVCertificateWSResponse;

        /// <summary>
        /// An event fired whenever a response to a get 15118 EV certificate request was received.
        /// </summary>
        public event OCPPv2_1.CS.OnGet15118EVCertificateResponseReceivedDelegate?    OnGet15118EVCertificateResponseReceived;

        #endregion


        #region Get15118EVCertificate(Request)

        /// <summary>
        /// Request a 15118 EV certificate.
        /// </summary>
        /// <param name="Request">A Get15118EVCertificate request.</param>
        public async Task<Get15118EVCertificateResponse>

            Get15118EVCertificate(Get15118EVCertificateRequest  Request)

        {

            #region Send OnGet15118EVCertificateRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGet15118EVCertificateRequestSent?.Invoke(startTime,
                                                       parentNetworkingNode,
                                                       Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnGet15118EVCertificateRequestSent));
            }

            #endregion


            Get15118EVCertificateResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONRequestAndWait(
                                                 OCPP_JSONRequestMessage.FromRequest(
                                                     Request,
                                                     Request.ToJSON(
                                                         CustomGet15118EVCertificateSerializer,
                                                         parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                         parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                     )
                                                 )
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (Get15118EVCertificateResponse.TryParse(Request,
                                                               sendRequestState.JSONResponse.Payload,
                                                               out var get15118EVCertificateResponse,
                                                               out var errorResponse,
                                                               CustomGet15118EVCertificateResponseParser) &&
                        get15118EVCertificateResponse is not null)
                    {
                        response = get15118EVCertificateResponse;
                    }

                    response ??= new Get15118EVCertificateResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new Get15118EVCertificateResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new Get15118EVCertificateResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGet15118EVCertificateResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGet15118EVCertificateResponseReceived?.Invoke(endTime,
                                                        parentNetworkingNode,
                                                        Request,
                                                        response,
                                                        endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnGet15118EVCertificateResponseReceived));
            }

            #endregion

            return response;

        }

        #endregion


    }

    public partial class OCPPWebSocketAdapterIN : IOCPPWebSocketAdapterIN
    {

        /// <summary>
        /// An event fired whenever a response to a get 15118 EV certificate request was received.
        /// </summary>
        public event OCPPv2_1.CS.OnGet15118EVCertificateResponseReceivedDelegate? OnGet15118EVCertificateResponseReceived;

    }

}