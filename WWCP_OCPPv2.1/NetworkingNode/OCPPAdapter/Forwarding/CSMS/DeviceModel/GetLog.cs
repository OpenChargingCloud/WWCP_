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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Delegates

    /// <summary>
    /// A GetLog request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<ForwardingDecision<GetLogRequest, GetLogResponse>>

        OnGetLogRequestFilterDelegate(DateTime               Timestamp,
                                      IEventSender           Sender,
                                      IWebSocketConnection   Connection,
                                      GetLogRequest          Request,
                                      CancellationToken      CancellationToken);


    /// <summary>
    /// A filtered GetLog request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="ForwardingDecision">The forwarding decision.</param>
    public delegate Task

        OnGetLogRequestFilteredDelegate(DateTime                                            Timestamp,
                                        IEventSender                                        Sender,
                                        IWebSocketConnection                                Connection,
                                        GetLogRequest                                       Request,
                                        ForwardingDecision<GetLogRequest, GetLogResponse>   ForwardingDecision);

    #endregion


    /// <summary>
    /// The OCPP adapter for forwarding messages.
    /// </summary>
    public partial class OCPPWebSocketAdapterFORWARD
    {

        #region Events

        public event OnGetLogRequestFilterDelegate?      OnGetLogRequest;

        public event OnGetLogRequestFilteredDelegate?    OnGetLogRequestLogging;

        #endregion

        public async Task<ForwardingDecision>

            Forward_GetLog(OCPP_JSONRequestMessage  JSONRequestMessage,
                           IWebSocketConnection     Connection,
                           CancellationToken        CancellationToken   = default)

        {

            if (!GetLogRequest.TryParse(JSONRequestMessage.Payload,
                                        JSONRequestMessage.RequestId,
                                        JSONRequestMessage.DestinationId,
                                        JSONRequestMessage.NetworkPath,
                                        out var Request,
                                        out var errorResponse,
                                        parentNetworkingNode.OCPP.CustomGetLogRequestParser))
            {
                return ForwardingDecision.REJECT(errorResponse);
            }

            ForwardingDecision<GetLogRequest, GetLogResponse>? forwardingDecision = null;

            #region Send OnGetLogRequest event

            var requestFilter = OnGetLogRequest;
            if (requestFilter is not null)
            {
                try
                {

                    var results = await Task.WhenAll(requestFilter.GetInvocationList().
                                                     OfType <OnGetLogRequestFilterDelegate>().
                                                     Select (filterDelegate => filterDelegate.Invoke(Timestamp.Now,
                                                                                                     parentNetworkingNode,
                                                                                                     Connection,
                                                                                                     Request,
                                                                                                     CancellationToken)).
                                                     ToArray());

                    //ToDo: Find a good result!
                    forwardingDecision = results.First();

                }
                catch (Exception e)
                {
                    await HandleErrors(
                              "NetworkingNode",
                              nameof(OnGetLogRequest),
                              e
                          );
                }

            }

            #endregion

            #region Default result

            if (forwardingDecision is null && DefaultResult == ForwardingResults.FORWARD)
                forwardingDecision = new ForwardingDecision<GetLogRequest, GetLogResponse>(
                                         Request,
                                         ForwardingResults.FORWARD
                                     );

            if (forwardingDecision is null ||
               (forwardingDecision.Result == ForwardingResults.REJECT && forwardingDecision.RejectResponse is null))
            {

                var response = forwardingDecision?.RejectResponse ??
                                   new GetLogResponse(
                                       Request,
                                       Result.Filtered(ForwardingDecision.DefaultLogMessage)
                                   );

                forwardingDecision = new ForwardingDecision<GetLogRequest, GetLogResponse>(
                                         Request,
                                         ForwardingResults.REJECT,
                                         response,
                                         response.ToJSON(
                                             parentNetworkingNode.OCPP.CustomGetLogResponseSerializer,
                                             parentNetworkingNode.OCPP.CustomStatusInfoSerializer,
                                             parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                             parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                         )
                                     );

            }

            #endregion


            #region Send OnGetLogRequestLogging event

            var logger = OnGetLogRequestLogging;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                       OfType <OnGetLogRequestFilteredDelegate>().
                                       Select (loggingDelegate => loggingDelegate.Invoke(Timestamp.Now,
                                                                                         parentNetworkingNode,
                                                                                         Connection,
                                                                                         Request,
                                                                                         forwardingDecision)).
                                       ToArray());

                }
                catch (Exception e)
                {
                    await HandleErrors(
                              "NetworkingNode",
                              nameof(OnGetLogRequestLogging),
                              e
                          );
                }

            }

            #endregion

            return forwardingDecision;

        }

    }

}
