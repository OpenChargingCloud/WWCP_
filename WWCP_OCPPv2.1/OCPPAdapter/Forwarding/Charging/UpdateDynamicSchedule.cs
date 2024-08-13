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
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Delegates

    /// <summary>
    /// A UpdateDynamicSchedule request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<ForwardingDecision<UpdateDynamicScheduleRequest, UpdateDynamicScheduleResponse>>

        OnUpdateDynamicScheduleRequestFilterDelegate(DateTime                       Timestamp,
                                                     IEventSender                   Sender,
                                                     IWebSocketConnection           Connection,
                                                     UpdateDynamicScheduleRequest   Request,
                                                     CancellationToken              CancellationToken);


    /// <summary>
    /// A filtered UpdateDynamicSchedule request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="ForwardingDecision">The forwarding decision.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task

        OnUpdateDynamicScheduleRequestFilteredDelegate(DateTime                                                                          Timestamp,
                                                       IEventSender                                                                      Sender,
                                                       IWebSocketConnection                                                              Connection,
                                                       UpdateDynamicScheduleRequest                                                      Request,
                                                       ForwardingDecision<UpdateDynamicScheduleRequest, UpdateDynamicScheduleResponse>   ForwardingDecision,
                                                       CancellationToken                                                                 CancellationToken);

    #endregion

    public partial class OCPPWebSocketAdapterFORWARD
    {

        #region Events

        public event OnUpdateDynamicScheduleRequestReceivedDelegate?    OnUpdateDynamicScheduleRequestReceived;
        public event OnUpdateDynamicScheduleRequestFilterDelegate?      OnUpdateDynamicScheduleRequestFilter;
        public event OnUpdateDynamicScheduleRequestFilteredDelegate?    OnUpdateDynamicScheduleRequestFiltered;
        public event OnUpdateDynamicScheduleRequestSentDelegate?        OnUpdateDynamicScheduleRequestSent;

        public event OnUpdateDynamicScheduleResponseReceivedDelegate?   OnUpdateDynamicScheduleResponseReceived;
        public event OnUpdateDynamicScheduleResponseSentDelegate?       OnUpdateDynamicScheduleResponseSent;

        #endregion

        public async Task<ForwardingDecision>

            Forward_UpdateDynamicSchedule(OCPP_JSONRequestMessage  JSONRequestMessage,
                                          IWebSocketConnection     WebSocketConnection,
                                          CancellationToken        CancellationToken   = default)

        {

            #region Parse the UpdateDynamicSchedule request

            if (!UpdateDynamicScheduleRequest.TryParse(JSONRequestMessage.Payload,
                                                       JSONRequestMessage.RequestId,
                                                       JSONRequestMessage.DestinationId,
                                                       JSONRequestMessage.NetworkPath,
                                                       out var request,
                                                       out var errorResponse,
                                                       JSONRequestMessage.RequestTimestamp,
                                                       JSONRequestMessage.RequestTimeout - Timestamp.Now,
                                                       JSONRequestMessage.EventTrackingId,
                                                       parentNetworkingNode.OCPP.CustomUpdateDynamicScheduleRequestParser))
            {
                return ForwardingDecision.REJECT(errorResponse);
            }

            #endregion

            #region Send OnUpdateDynamicScheduleRequestReceived event

            await LogEvent(
                      OnUpdateDynamicScheduleRequestReceived,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          WebSocketConnection,
                          request,
                          CancellationToken
                      )
                  );

            #endregion


            #region Send OnUpdateDynamicScheduleRequestFilter event

            var forwardingDecision = await CallFilter(
                                               OnUpdateDynamicScheduleRequestFilter,
                                               filter => filter.Invoke(
                                                             Timestamp.Now,
                                                             parentNetworkingNode,
                                                             WebSocketConnection,
                                                             request,
                                                             CancellationToken
                                                         )
                                           );

            #endregion

            #region Default result

            if (forwardingDecision is null && DefaultForwardingResult == ForwardingResults.FORWARD)
                forwardingDecision = new ForwardingDecision<UpdateDynamicScheduleRequest, UpdateDynamicScheduleResponse>(
                                         request,
                                         ForwardingResults.FORWARD
                                     );

            if (forwardingDecision is null ||
               (forwardingDecision.Result == ForwardingResults.REJECT && forwardingDecision.RejectResponse is null))
            {

                var response = forwardingDecision?.RejectResponse ??
                                   new UpdateDynamicScheduleResponse(
                                       request,
                                       ChargingProfileStatus.Rejected,
                                       Result: Result.Filtered(ForwardingDecision.DefaultLogMessage)
                                   );

                forwardingDecision = new ForwardingDecision<UpdateDynamicScheduleRequest, UpdateDynamicScheduleResponse>(
                                         request,
                                         ForwardingResults.REJECT,
                                         response,
                                         response.ToJSON(
                                             parentNetworkingNode.OCPP.CustomUpdateDynamicScheduleResponseSerializer,
                                             parentNetworkingNode.OCPP.CustomStatusInfoSerializer,
                                             parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                             parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                         )
                                     );

            }

            #endregion

            if (forwardingDecision.NewRequest is not null)
                forwardingDecision.NewJSONRequest = forwardingDecision.NewRequest.ToJSON(
                                                        parentNetworkingNode.OCPP.CustomUpdateDynamicScheduleRequestSerializer,
                                                        parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                        parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                    );

            #region Send OnUpdateDynamicScheduleRequestFiltered event

            await LogEvent(
                      OnUpdateDynamicScheduleRequestFiltered,
                      loggingDelegate => loggingDelegate.Invoke(
                          Timestamp.Now,
                          parentNetworkingNode,
                          WebSocketConnection,
                          request,
                          forwardingDecision,
                          CancellationToken
                      )
                  );

            #endregion


            #region Attach OnUpdateDynamicScheduleRequestSent event

            if (forwardingDecision.Result == ForwardingResults.FORWARD)
            {

                var sentLogging = OnUpdateDynamicScheduleRequestSent;
                if (sentLogging is not null)
                    forwardingDecision.SentMessageLogger = async (sentMessageResult) =>
                        await LogEvent(
                                  OnUpdateDynamicScheduleRequestSent,
                                  loggingDelegate => loggingDelegate.Invoke(
                                      Timestamp.Now,
                                      parentNetworkingNode,
                                      sentMessageResult.Connection,
                                      request,
                                      sentMessageResult.Result,
                                      CancellationToken
                                  )
                              );

            }

            #endregion

            return forwardingDecision;

        }

    }

}