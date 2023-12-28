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
using cloud.charging.open.protocols.OCPP.NN;

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

        public CustomJObjectSerializerDelegate<NotifyNetworkTopologyRequest>?  CustomNotifyNetworkTopologyRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<NotifyNetworkTopologyResponse>?     CustomNotifyNetworkTopologyResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a NotifyNetworkTopology request was sent.
        /// </summary>
        public event OnNotifyNetworkTopologyRequestDelegate?     OnNotifyNetworkTopologyRequest;

        /// <summary>
        /// An event sent whenever a response to a NotifyNetworkTopology request was sent.
        /// </summary>
        public event OnNotifyNetworkTopologyResponseDelegate?    OnNotifyNetworkTopologyResponse;

        #endregion


        #region NotifyNetworkTopology(Request)

        public async Task<NotifyNetworkTopologyResponse> NotifyNetworkTopology(NotifyNetworkTopologyRequest Request)
        {

            #region Send OnNotifyNetworkTopologyRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyNetworkTopologyRequest?.Invoke(startTime,
                                              this,
                                              Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnNotifyNetworkTopologyRequest));
            }

            #endregion


            NotifyNetworkTopologyResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.DestinationNodeId,
                                                 Request.NetworkPath,
                                                 Request.RequestId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomNotifyNetworkTopologyRequestSerializer,
                                                     null, //CustomNotifyNetworkTopologySerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (NotifyNetworkTopologyResponse.TryParse(Request,
                                                      sendRequestState.JSONResponse.Payload,
                                                      out var dataTransferResponse,
                                                      out var errorResponse,
                                                      CustomNotifyNetworkTopologyResponseParser) &&
                        dataTransferResponse is not null)
                    {
                        response = dataTransferResponse;
                    }

                    response ??= new NotifyNetworkTopologyResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                }

                response ??= new NotifyNetworkTopologyResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

            }
            catch (Exception e)
            {

                response = new NotifyNetworkTopologyResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnNotifyNetworkTopologyResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnNotifyNetworkTopologyResponse?.Invoke(endTime,
                                               this,
                                               Request,
                                               response,
                                               endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(NetworkingNodeWSServer) + "." + nameof(OnNotifyNetworkTopologyResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
