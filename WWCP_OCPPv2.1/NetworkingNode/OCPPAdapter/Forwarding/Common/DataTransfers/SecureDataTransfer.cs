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
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Delegates

    /// <summary>
    /// A SecureDataTransfer request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<ForwardingDecision<SecureDataTransferRequest, SecureDataTransferResponse>>

        OnSecureDataTransferRequestFilterDelegate(DateTime                    Timestamp,
                                                  IEventSender                Sender,
                                                  IWebSocketConnection        Connection,
                                                  SecureDataTransferRequest   Request,
                                                  CancellationToken           CancellationToken);


    /// <summary>
    /// A filtered SecureDataTransfer request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP Web Socket connection.</param>
    /// <param name="Request">The request.</param>
    /// <param name="ForwardingDecision">The forwarding decision.</param>
    public delegate Task

        OnSecureDataTransferRequestFilteredDelegate(DateTime                                                                    Timestamp,
                                                    IEventSender                                                                Sender,
                                                    IWebSocketConnection                                                        Connection,
                                                    SecureDataTransferRequest                                                   Request,
                                                    ForwardingDecision<SecureDataTransferRequest, SecureDataTransferResponse>   ForwardingDecision);

    #endregion


    /// <summary>
    /// The OCPP adapter for forwarding messages.
    /// </summary>
    public partial class OCPPWebSocketAdapterFORWARD : IOCPPWebSocketAdapterFORWARD
    {

        #region Events

        public event OnSecureDataTransferRequestReceivedDelegate?    OnSecureDataTransferRequestReceived;
        public event OnSecureDataTransferRequestFilterDelegate?      OnSecureDataTransferRequestFilter;
        public event OnSecureDataTransferRequestFilteredDelegate?    OnSecureDataTransferRequestFiltered;
        public event OnSecureDataTransferRequestSentDelegate?        OnSecureDataTransferRequestSent;

        public event OnSecureDataTransferResponseReceivedDelegate?   OnSecureDataTransferResponseReceived;
        public event OnSecureDataTransferResponseSentDelegate?       OnSecureDataTransferResponseSent;

        #endregion

        public async Task<ForwardingDecision>

            Forward_SecureDataTransfer(OCPP_BinaryRequestMessage  BinaryRequestMessage,
                                       IWebSocketConnection       Connection,
                                       CancellationToken          CancellationToken   = default)

        {

            if (!SecureDataTransferRequest.TryParse(BinaryRequestMessage.Payload,
                                                    BinaryRequestMessage.RequestId,
                                                    BinaryRequestMessage.DestinationId,
                                                    BinaryRequestMessage.NetworkPath,
                                                    out var Request,
                                                    out var errorResponse,
                                                    BinaryRequestMessage.RequestTimestamp,
                                                    null, //BinaryRequestMessage.RequestTimeout,
                                                    BinaryRequestMessage.EventTrackingId,
                                                    parentNetworkingNode.OCPP.CustomSecureDataTransferRequestParser))
            {
                return ForwardingDecision.REJECT(errorResponse);
            }

            ForwardingDecision<SecureDataTransferRequest, SecureDataTransferResponse>? forwardingDecision = null;

            #region Send OnSecureDataTransferRequestFilter event

            var requestFilter = OnSecureDataTransferRequestFilter;
            if (requestFilter is not null)
            {
                try
                {

                    var results = await Task.WhenAll(requestFilter.GetInvocationList().
                                                     OfType <OnSecureDataTransferRequestFilterDelegate>().
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
                              nameof(OnSecureDataTransferRequestFilter),
                              e
                          );
                }

            }

            #endregion

            #region Default result

            if (forwardingDecision is null && DefaultResult == ForwardingResults.FORWARD)
                forwardingDecision = new ForwardingDecision<SecureDataTransferRequest, SecureDataTransferResponse>(
                                         Request,
                                         ForwardingResults.FORWARD
                                     );

            if (forwardingDecision is null ||
               (forwardingDecision.Result == ForwardingResults.REJECT && forwardingDecision.BinaryRejectResponse is null))
            {

                var response = forwardingDecision?.RejectResponse ??
                                   new SecureDataTransferResponse(
                                       Request,
                                       Result.Filtered(ForwardingDecision.DefaultLogMessage)
                                   );

                forwardingDecision = new ForwardingDecision<SecureDataTransferRequest, SecureDataTransferResponse>(
                                         Request,
                                         ForwardingResults.REJECT,
                                         response,
                                         response.ToBinary(
                                             parentNetworkingNode.OCPP.CustomSecureDataTransferResponseSerializer,
                                             parentNetworkingNode.OCPP.CustomBinarySignatureSerializer,
                                             IncludeSignatures: true
                                         )
                                     );

            }

            #endregion


            #region Send OnSecureDataTransferRequestFiltered event

            var logger = OnSecureDataTransferRequestFiltered;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                       OfType <OnSecureDataTransferRequestFilteredDelegate>().
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
                              nameof(OnSecureDataTransferRequestFiltered),
                              e
                          );
                }

            }

            #endregion

            return forwardingDecision;

        }

    }

}
