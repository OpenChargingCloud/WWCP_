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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CSMS
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class NetworkingNodeWSServer : AOCPPWebSocketServer,
                                                  INetworkingNodeChannel
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<CustomerInformationRequest>?  CustomCustomerInformationRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<CustomerInformationResponse>?     CustomCustomerInformationResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a CustomerInformation request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnCustomerInformationRequestDelegate?     OnCustomerInformationRequest;

        /// <summary>
        /// An event sent whenever a response to a CustomerInformation request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnCustomerInformationResponseDelegate?    OnCustomerInformationResponse;

        #endregion


        #region RequestCustomerInformation(Request)

        public async Task<CustomerInformationResponse> CustomerInformation(CustomerInformationRequest Request)
        {

            #region Send OnCustomerInformationRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnCustomerInformationRequest?.Invoke(startTime,
                                                     this,
                                                     Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnCustomerInformationRequest));
            }

            #endregion


            CustomerInformationResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.DestinationNodeId,
                                                 Request.NetworkPath,
                                                 Request.RequestId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomCustomerInformationRequestSerializer,
                                                     CustomIdTokenSerializer,
                                                     CustomAdditionalInfoSerializer,
                                                     CustomCertificateHashDataSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (CustomerInformationResponse.TryParse(Request,
                                                             sendRequestState.JSONResponse.Payload,
                                                             out var customerInformationResponse,
                                                             out var errorResponse,
                                                             CustomCustomerInformationResponseParser) &&
                        customerInformationResponse is not null)
                    {
                        response = customerInformationResponse;
                    }

                    response ??= new CustomerInformationResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new CustomerInformationResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new CustomerInformationResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnCustomerInformationResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnCustomerInformationResponse?.Invoke(endTime,
                                                      this,
                                                      Request,
                                                      response,
                                                      endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnCustomerInformationResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}