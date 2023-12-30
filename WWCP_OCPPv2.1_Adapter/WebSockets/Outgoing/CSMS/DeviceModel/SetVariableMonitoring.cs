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

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<SetVariableMonitoringRequest>?  CustomSetVariableMonitoringRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<SetVariableMonitoringResponse>?     CustomSetVariableMonitoringResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a SetVariableMonitoring request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnSetVariableMonitoringRequestDelegate?     OnSetVariableMonitoringRequest;

        /// <summary>
        /// An event sent whenever a response to a SetVariableMonitoring request was sent.
        /// </summary>
        public event OCPPv2_1.CSMS.OnSetVariableMonitoringResponseDelegate?    OnSetVariableMonitoringResponse;

        #endregion


        #region SetVariableMonitoring(Request)

        public async Task<SetVariableMonitoringResponse> SetVariableMonitoring(SetVariableMonitoringRequest Request)
        {

            #region Send OnSetVariableMonitoringRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnSetVariableMonitoringRequest?.Invoke(startTime,
                                                       this,
                                                       Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnSetVariableMonitoringRequest));
            }

            #endregion


            SetVariableMonitoringResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.DestinationNodeId,
                                                 Request.NetworkPath,
                                                 Request.RequestId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomSetVariableMonitoringRequestSerializer,
                                                     CustomSetMonitoringDataSerializer,
                                                     CustomComponentSerializer,
                                                     CustomEVSESerializer,
                                                     CustomVariableSerializer,
                                                     CustomPeriodicEventStreamParametersSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (SetVariableMonitoringResponse.TryParse(Request,
                                                               sendRequestState.JSONResponse.Payload,
                                                               out var setVariableMonitoringResponse,
                                                               out var errorResponse,
                                                               CustomSetVariableMonitoringResponseParser) &&
                        setVariableMonitoringResponse is not null)
                    {
                        response = setVariableMonitoringResponse;
                    }

                    response ??= new SetVariableMonitoringResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new SetVariableMonitoringResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new SetVariableMonitoringResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnSetVariableMonitoringResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnSetVariableMonitoringResponse?.Invoke(endTime,
                                                        this,
                                                        Request,
                                                        response,
                                                        endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnSetVariableMonitoringResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
