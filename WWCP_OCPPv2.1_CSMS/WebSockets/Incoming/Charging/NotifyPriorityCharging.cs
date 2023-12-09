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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : ACSMSWSServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<NotifyPriorityChargingRequest>?       CustomNotifyPriorityChargingRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<NotifyPriorityChargingResponse>?  CustomNotifyPriorityChargingResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a NotifyPriorityCharging WebSocket request was received.
        /// </summary>
        public event OnOCPPJSONRequestLogDelegate?                   OnNotifyPriorityChargingWSRequest;

        /// <summary>
        /// An event sent whenever a NotifyPriorityCharging request was received.
        /// </summary>
        public event OnNotifyPriorityChargingRequestDelegate?      OnNotifyPriorityChargingRequest;

        /// <summary>
        /// An event sent whenever a NotifyPriorityCharging was received.
        /// </summary>
        public event OnNotifyPriorityChargingDelegate?             OnNotifyPriorityCharging;

        /// <summary>
        /// An event sent whenever a response to a NotifyPriorityCharging was sent.
        /// </summary>
        public event OnNotifyPriorityChargingResponseDelegate?     OnNotifyPriorityChargingResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a NotifyPriorityCharging was sent.
        /// </summary>
        public event OnOCPPJSONRequestJSONResponseLogDelegate?   OnNotifyPriorityChargingWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_NotifyPriorityCharging(DateTime                   RequestTimestamp,
                                           WebSocketServerConnection  Connection,
                                           NetworkingNode_Id          NetworkingNodeId,
                                           NetworkPath                NetworkPath,
                                           EventTracking_Id           EventTrackingId,
                                           Request_Id                 RequestId,
                                           JObject                    JSONRequest,
                                           CancellationToken          CancellationToken)

        {

            #region Send OnNotifyPriorityChargingWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyPriorityChargingWSRequest?.Invoke(startTime,
                                                          this,
                                                          Connection,
                                                          NetworkingNodeId,
                                                          EventTrackingId,
                                                          RequestTimestamp,
                                                          JSONRequest,
                                                          CancellationToken);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyPriorityChargingWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (NotifyPriorityChargingRequest.TryParse(JSONRequest,
                                                           RequestId,
                                                           NetworkingNodeId,
                                                           NetworkPath,
                                                           out var request,
                                                           out var errorResponse,
                                                           CustomNotifyPriorityChargingRequestParser) && request is not null) {

                    #region Send OnNotifyPriorityChargingRequest event

                    try
                    {

                        OnNotifyPriorityChargingRequest?.Invoke(Timestamp.Now,
                                                                this,
                                                                request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyPriorityChargingRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    NotifyPriorityChargingResponse? response = null;

                    var responseTasks = OnNotifyPriorityCharging?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnNotifyPriorityChargingDelegate)?.Invoke(Timestamp.Now,
                                                                                                                              this,
                                                                                                                              request,
                                                                                                                              CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= NotifyPriorityChargingResponse.Failed(request);

                    #endregion

                    #region Send OnNotifyPriorityChargingResponse event

                    try
                    {

                        OnNotifyPriorityChargingResponse?.Invoke(Timestamp.Now,
                                                                 this,
                                                                 request,
                                                                 response,
                                                                 response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyPriorityChargingResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       RequestId,
                                       response.ToJSON(
                                           CustomNotifyPriorityChargingResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_NotifyPriorityCharging)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_NotifyPriorityCharging)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnNotifyPriorityChargingWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnNotifyPriorityChargingWSResponse?.Invoke(endTime,
                                                           this,
                                                           Connection,
                                                           NetworkingNodeId,
                                                           EventTrackingId,
                                                           RequestTimestamp,
                                                           JSONRequest,
                                                           endTime, //ToDo: Refactor me!
                                                           OCPPResponse?.Payload,
                                                           OCPPErrorResponse?.ToJSON(),
                                                           endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyPriorityChargingWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}
