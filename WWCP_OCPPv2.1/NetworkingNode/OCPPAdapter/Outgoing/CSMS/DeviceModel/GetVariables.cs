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
    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<GetVariablesRequest>?  CustomGetVariablesRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<GetVariablesResponse>?     CustomGetVariablesResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetVariables request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnGetVariablesRequestSentDelegate?     OnGetVariablesRequestSent;

        /// <summary>
        /// An event sent whenever a response to a GetVariables request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnGetVariablesResponseReceivedDelegate?    OnGetVariablesResponseReceived;

        #endregion


        #region GetVariables(Request)

        public async Task<GetVariablesResponse> GetVariables(GetVariablesRequest Request)
        {

            #region Send OnGetVariablesRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetVariablesRequestSent?.Invoke(startTime,
                                              parentNetworkingNode,
                                              Request, SendMessageResult.Success);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnGetVariablesRequestSent));
            }

            #endregion


            GetVariablesResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONRequestAndWait(
                                                 OCPP_JSONRequestMessage.FromRequest(
                                                     Request,
                                                     Request.ToJSON(
                                                         CustomGetVariablesRequestSerializer,
                                                         parentNetworkingNode.OCPP.CustomGetVariableDataSerializer,
                                                         parentNetworkingNode.OCPP.CustomComponentSerializer,
                                                         parentNetworkingNode.OCPP.CustomEVSESerializer,
                                                         parentNetworkingNode.OCPP.CustomVariableSerializer,
                                                         parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                         parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                     )
                                                 )
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (GetVariablesResponse.TryParse(Request,
                                                      sendRequestState.JSONResponse.Payload,
                                                      out var getVariablesResponse,
                                                      out var errorResponse,
                                                      sendRequestState.ResponseTimestamp,
                                                      CustomGetVariablesResponseParser) &&
                        getVariablesResponse is not null)
                    {
                        response = getVariablesResponse;
                    }

                    response ??= new GetVariablesResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new GetVariablesResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new GetVariablesResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGetVariablesResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGetVariablesResponseReceived?.Invoke(endTime,
                                               parentNetworkingNode,
                                               Request,
                                               response,
                                               endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnGetVariablesResponseReceived));
            }

            #endregion

            return response;

        }

        #endregion


    }

    public partial class OCPPWebSocketAdapterIN : IOCPPWebSocketAdapterIN
    {

        /// <summary>
        /// An event sent whenever a response to a GetVariables request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnGetVariablesResponseReceivedDelegate? OnGetVariablesResponseReceived;

    }

}
