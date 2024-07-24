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
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Delegates

    /// <summary>
    /// A GetDefaultChargingTariff request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<ForwardingDecision<GetDefaultChargingTariffRequest, GetDefaultChargingTariffResponse>>

        OnGetDefaultChargingTariffRequestFilterDelegate(DateTime                          Timestamp,
                                                        IEventSender                      Sender,
                                                        IWebSocketConnection              Connection,
                                                        GetDefaultChargingTariffRequest   Request,
                                                        CancellationToken                 CancellationToken);


    /// <summary>
    /// A filtered GetDefaultChargingTariff request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="ForwardingDecision">The forwarding decision.</param>
    public delegate Task

        OnGetDefaultChargingTariffRequestFilteredDelegate(DateTime                                                                                Timestamp,
                                                          IEventSender                                                                            Sender,
                                                          IWebSocketConnection                                                                    Connection,
                                                          GetDefaultChargingTariffRequest                                                         Request,
                                                          ForwardingDecision<GetDefaultChargingTariffRequest, GetDefaultChargingTariffResponse>   ForwardingDecision);

    #endregion


    /// <summary>
    /// The OCPP adapter for forwarding messages.
    /// </summary>
    public partial class OCPPWebSocketAdapterFORWARD
    {

        #region Events

        public event OnGetDefaultChargingTariffRequestFilterDelegate?      OnGetDefaultChargingTariffRequest;

        public event OnGetDefaultChargingTariffRequestFilteredDelegate?    OnGetDefaultChargingTariffRequestLogging;

        #endregion

        public async Task<ForwardingDecision>

            Forward_GetDefaultChargingTariff(OCPP_JSONRequestMessage  JSONRequestMessage,
                                             IWebSocketConnection     Connection,
                                             CancellationToken        CancellationToken   = default)

        {

            if (!GetDefaultChargingTariffRequest.TryParse(JSONRequestMessage.Payload,
                                                          JSONRequestMessage.RequestId,
                                                          JSONRequestMessage.DestinationId,
                                                          JSONRequestMessage.NetworkPath,
                                                          out var Request,
                                                          out var errorResponse,
                                                          JSONRequestMessage.RequestTimestamp,
                                                          JSONRequestMessage.RequestTimeout - Timestamp.Now,
                                                          JSONRequestMessage.EventTrackingId,
                                                          parentNetworkingNode.OCPP.CustomGetDefaultChargingTariffRequestParser))
            {
                return ForwardingDecision.REJECT(errorResponse);
            }

            ForwardingDecision<GetDefaultChargingTariffRequest, GetDefaultChargingTariffResponse>? forwardingDecision = null;

            #region Send OnGetDefaultChargingTariffRequest event

            var requestFilter = OnGetDefaultChargingTariffRequest;
            if (requestFilter is not null)
            {
                try
                {

                    var results = await Task.WhenAll(requestFilter.GetInvocationList().
                                                     OfType <OnGetDefaultChargingTariffRequestFilterDelegate>().
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
                              nameof(OnGetDefaultChargingTariffRequest),
                              e
                          );
                }

            }

            #endregion

            #region Default result

            if (forwardingDecision is null && DefaultForwardingResult == ForwardingResults.FORWARD)
                forwardingDecision = new ForwardingDecision<GetDefaultChargingTariffRequest, GetDefaultChargingTariffResponse>(
                                         Request,
                                         ForwardingResults.FORWARD
                                     );

            if (forwardingDecision is null ||
               (forwardingDecision.Result == ForwardingResults.REJECT && forwardingDecision.RejectResponse is null))
            {

                var response = forwardingDecision?.RejectResponse ??
                                   new GetDefaultChargingTariffResponse(
                                       Request,
                                       Result.Filtered(ForwardingDecision.DefaultLogMessage)
                                   );

                forwardingDecision = new ForwardingDecision<GetDefaultChargingTariffRequest, GetDefaultChargingTariffResponse>(
                                         Request,
                                         ForwardingResults.REJECT,
                                         response,
                                         response.ToJSON(
                                             parentNetworkingNode.OCPP.CustomGetDefaultChargingTariffResponseSerializer,
                                             parentNetworkingNode.OCPP.CustomStatusInfoSerializer,
                                             parentNetworkingNode.OCPP.CustomChargingTariffSerializer,
                                             parentNetworkingNode.OCPP.CustomPriceSerializer,
                                             parentNetworkingNode.OCPP.CustomTariffElementSerializer,
                                             parentNetworkingNode.OCPP.CustomPriceComponentSerializer,
                                             parentNetworkingNode.OCPP.CustomTaxRateSerializer,
                                             parentNetworkingNode.OCPP.CustomTariffRestrictionsSerializer,
                                             parentNetworkingNode.OCPP.CustomEnergyMixSerializer,
                                             parentNetworkingNode.OCPP.CustomEnergySourceSerializer,
                                             parentNetworkingNode.OCPP.CustomEnvironmentalImpactSerializer,
                                             parentNetworkingNode.OCPP.CustomIdTokenSerializer,
                                             parentNetworkingNode.OCPP.CustomAdditionalInfoSerializer,
                                             parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                             parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                         )
                                     );

            }

            #endregion


            #region Send OnGetDefaultChargingTariffRequestLogging event

            var logger = OnGetDefaultChargingTariffRequestLogging;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                       OfType <OnGetDefaultChargingTariffRequestFilteredDelegate>().
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
                              nameof(OnGetDefaultChargingTariffRequestLogging),
                              e
                          );
                }

            }

            #endregion

            return forwardingDecision;

        }

    }

}
