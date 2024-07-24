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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    public partial class OCPPWebSocketAdapterIN : IOCPPWebSocketAdapterIN
    {

        #region Events

        /// <summary>
        /// An event sent whenever a SetVariableMonitoring request was received.
        /// </summary>
        public event OnSetVariableMonitoringRequestReceivedDelegate?  OnSetVariableMonitoringRequestReceived;

        /// <summary>
        /// An event sent whenever a SetVariableMonitoring request was received for processing.
        /// </summary>
        public event OnSetVariableMonitoringDelegate?                 OnSetVariableMonitoring;

        #endregion

        #region Receive message (wired via reflection!)

        public async Task<OCPP_Response>

            Receive_SetVariableMonitoring(DateTime              RequestTimestamp,
                                          IWebSocketConnection  WebSocketConnection,
                                          NetworkingNode_Id     DestinationId,
                                          NetworkPath           NetworkPath,
                                          EventTracking_Id      EventTrackingId,
                                          Request_Id            RequestId,
                                          JObject               JSONRequest,
                                          CancellationToken     CancellationToken)

        {

            OCPP_Response? ocppResponse = null;

            try
            {

                if (SetVariableMonitoringRequest.TryParse(JSONRequest,
                                                          RequestId,
                                                          DestinationId,
                                                          NetworkPath,
                                                          out var request,
                                                          out var errorResponse,
                                                          RequestTimestamp,
                                                          parentNetworkingNode.OCPP.DefaultRequestTimeout,
                                                          EventTrackingId,
                                                          parentNetworkingNode.OCPP.CustomSetVariableMonitoringRequestParser)) {

                    SetVariableMonitoringResponse? response = null;

                    #region Verify request signature(s)

                    if (!parentNetworkingNode.OCPP.SignaturePolicy.VerifyRequestMessage(
                        request,
                        request.ToJSON(
                            parentNetworkingNode.OCPP.CustomSetVariableMonitoringRequestSerializer,
                            parentNetworkingNode.OCPP.CustomSetMonitoringDataSerializer,
                            parentNetworkingNode.OCPP.CustomComponentSerializer,
                            parentNetworkingNode.OCPP.CustomEVSESerializer,
                            parentNetworkingNode.OCPP.CustomVariableSerializer,
                            parentNetworkingNode.OCPP.CustomPeriodicEventStreamParametersSerializer,
                            parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        ),
                        out errorResponse))
                    {

                        response = SetVariableMonitoringResponse.SignatureError(
                                       request,
                                       errorResponse
                                   );

                    }

                    #endregion

                    #region Send OnSetVariableMonitoringRequestReceived event

                    var logger = OnSetVariableMonitoringRequestReceived;
                    if (logger is not null)
                    {
                        try
                        {

                            await Task.WhenAll(logger.GetInvocationList().
                                                   OfType<OnSetVariableMonitoringRequestReceivedDelegate>().
                                                   Select(loggingDelegate => loggingDelegate.Invoke(
                                                                                  Timestamp.Now,
                                                                                  parentNetworkingNode,
                                                                                  WebSocketConnection,
                                                                                  request
                                                                             )).
                                                   ToArray());

                        }
                        catch (Exception e)
                        {
                            await HandleErrors(
                                      nameof(OCPPWebSocketAdapterIN),
                                      nameof(OnSetVariableMonitoringRequestReceived),
                                      e
                                  );
                        }
                    }

                    #endregion


                    #region Call async subscribers

                    if (response is null)
                    {
                        try
                        {

                            var responseTasks = OnSetVariableMonitoring?.
                                                    GetInvocationList()?.
                                                    SafeSelect(subscriber => (subscriber as OnSetVariableMonitoringDelegate)?.Invoke(
                                                                                  Timestamp.Now,
                                                                                  parentNetworkingNode,
                                                                                  WebSocketConnection,
                                                                                  request,
                                                                                  CancellationToken
                                                                              )).
                                                    ToArray();

                            response = responseTasks?.Length > 0
                                           ? (await Task.WhenAll(responseTasks!)).FirstOrDefault()
                                           : SetVariableMonitoringResponse.Failed(request, $"Undefined {nameof(OnSetVariableMonitoring)}!");

                        }
                        catch (Exception e)
                        {

                            response = SetVariableMonitoringResponse.ExceptionOccured(request, e);

                            await HandleErrors(
                                      nameof(OCPPWebSocketAdapterIN),
                                      nameof(OnSetVariableMonitoring),
                                      e
                                  );

                        }
                    }

                    response ??= SetVariableMonitoringResponse.Failed(request);

                    #endregion

                    #region Sign response message

                    parentNetworkingNode.OCPP.SignaturePolicy.SignResponseMessage(
                        response,
                        response.ToJSON(
                            parentNetworkingNode.OCPP.CustomSetVariableMonitoringResponseSerializer,
                            parentNetworkingNode.OCPP.CustomSetMonitoringResultSerializer,
                            parentNetworkingNode.OCPP.CustomComponentSerializer,
                            parentNetworkingNode.OCPP.CustomEVSESerializer,
                            parentNetworkingNode.OCPP.CustomVariableSerializer,
                            parentNetworkingNode.OCPP.CustomStatusInfoSerializer,
                            parentNetworkingNode.OCPP.CustomSignatureSerializer,
                            parentNetworkingNode.OCPP.CustomCustomDataSerializer
                        ),
                        out var errorResponse2);

                    #endregion


                    #region Send OnSetVariableMonitoringResponse event

                    await (parentNetworkingNode.OCPP.OUT as OCPPWebSocketAdapterOUT).SendOnSetVariableMonitoringResponseSent(
                              Timestamp.Now,
                              parentNetworkingNode,
                              WebSocketConnection,
                              request,
                              response,
                              response.Runtime
                          );

                    #endregion

                    ocppResponse = OCPP_Response.JSONResponse(
                                       EventTrackingId,
                                       NetworkPath.Source,
                                       NetworkPath.From(parentNetworkingNode.Id),
                                       RequestId,
                                       response.ToJSON(
                                           parentNetworkingNode.OCPP.CustomSetVariableMonitoringResponseSerializer,
                                           parentNetworkingNode.OCPP.CustomSetMonitoringResultSerializer,
                                           parentNetworkingNode.OCPP.CustomComponentSerializer,
                                           parentNetworkingNode.OCPP.CustomEVSESerializer,
                                           parentNetworkingNode.OCPP.CustomVariableSerializer,
                                           parentNetworkingNode.OCPP.CustomStatusInfoSerializer,
                                           parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                           parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                       ),
                                       CancellationToken
                                   );

                }

                else
                    ocppResponse = OCPP_Response.CouldNotParse(
                                       EventTrackingId,
                                       RequestId,
                                       nameof(Receive_SetVariableMonitoring)[8..],
                                       JSONRequest,
                                       errorResponse
                                   );

            }
            catch (Exception e)
            {

                ocppResponse = OCPP_Response.FormationViolation(
                                   EventTrackingId,
                                   RequestId,
                                   nameof(Receive_SetVariableMonitoring)[8..],
                                   JSONRequest,
                                   e
                               );

            }

            return ocppResponse;

        }

        #endregion

    }

    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        #region Events

        /// <summary>
        /// An event sent whenever a response to a SetVariableMonitoring was sent.
        /// </summary>
        public event OnSetVariableMonitoringResponseSentDelegate?  OnSetVariableMonitoringResponseSent;

        #endregion

        #region Send OnSetVariableMonitoringResponse event

        public async Task SendOnSetVariableMonitoringResponseSent(DateTime                       Timestamp,
                                                                  IEventSender                   Sender,
                                                                  IWebSocketConnection           Connection,
                                                                  SetVariableMonitoringRequest   Request,
                                                                  SetVariableMonitoringResponse  Response,
                                                                  TimeSpan                       Runtime)
        {

            var logger = OnSetVariableMonitoringResponseSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                              OfType<OnSetVariableMonitoringResponseSentDelegate>().
                                              Select(filterDelegate => filterDelegate.Invoke(Timestamp,
                                                                                             Sender,
                                                                                             Connection,
                                                                                             Request,
                                                                                             Response,
                                                                                             Runtime)).
                                              ToArray());

                }
                catch (Exception e)
                {
                    await HandleErrors(
                              nameof(OCPPWebSocketAdapterOUT),
                              nameof(OnSetVariableMonitoringResponseSent),
                              e
                          );
                }

            }

        }

        #endregion

    }

}
