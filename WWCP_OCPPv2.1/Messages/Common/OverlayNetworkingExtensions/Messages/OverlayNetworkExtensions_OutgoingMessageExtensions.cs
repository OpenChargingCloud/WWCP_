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

using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1
{

    public static class OverlayNetworkExtensions_OutgoingMessageExtensions
    {

        #region NotifyNetworkTopology                 (NetworkingNode, ...)

        /// <summary>
        /// Transfer the given binary data to the CSMS.
        /// </summary>
        /// <param name="NetworkTopologyInformation">A network topology information.</param>
        /// 
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<SentMessageResult>

            NotifyNetworkTopology(this INetworkingNodeButNotCSMS  NetworkingNode,

                                  NetworkTopologyInformation      NetworkTopologyInformation,

                                  CustomData?                     CustomData            = null,

                                  SourceRouting?                  Destination           = null,
                                  NetworkPath?                    NetworkPath           = null,

                                  IEnumerable<KeyPair>?           SignKeys              = null,
                                  IEnumerable<SignInfo>?          SignInfos             = null,
                                  IEnumerable<Signature>?         Signatures            = null,

                                  Request_Id?                     RequestId             = null,
                                  DateTime?                       RequestTimestamp      = null,
                                  EventTracking_Id?               EventTrackingId       = null,
                                  SerializationFormats?           SerializationFormat   = null,
                                  CancellationToken               CancellationToken     = default)


                => NetworkingNode.OCPP.OUT.NotifyNetworkTopology(
                       new NotifyNetworkTopologyMessage(

                           Destination ?? SourceRouting.CSMS,

                           NetworkTopologyInformation,

                           SignKeys,
                           SignInfos,
                           Signatures,

                           CustomData,

                           RequestId        ?? NetworkingNode.OCPP.NextRequestId,
                           RequestTimestamp ?? Timestamp.Now,
                           EventTrackingId  ?? EventTracking_Id.New,
                           NetworkPath      ?? NetworkPath.From(NetworkingNode.Id),
                           SerializationFormat,
                           CancellationToken

                       )
                   );

        #endregion

        #region NotifyNetworkTopology                 (NetworkingNode, ...)

        /// <summary>
        /// Transfer the given binary data to the CSMS.
        /// </summary>
        /// <param name="NetworkTopologyInformation">A network topology information.</param>
        /// 
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<SentMessageResult>

            NotifyNetworkTopology(this INetworkingNode        NetworkingNode,

                                  NetworkTopologyInformation  NetworkTopologyInformation,

                                  CustomData?                 CustomData            = null,

                                  SourceRouting?              Destination           = null,
                                  NetworkPath?                NetworkPath           = null,

                                  IEnumerable<KeyPair>?       SignKeys              = null,
                                  IEnumerable<SignInfo>?      SignInfos             = null,
                                  IEnumerable<Signature>?     Signatures            = null,

                                  Request_Id?                 RequestId             = null,
                                  DateTime?                   RequestTimestamp      = null,
                                  EventTracking_Id?           EventTrackingId       = null,
                                  SerializationFormats?       SerializationFormat   = null,
                                  CancellationToken           CancellationToken     = default)


                => NetworkingNode.OCPP.OUT.NotifyNetworkTopology(
                       new NotifyNetworkTopologyMessage(

                           Destination ?? SourceRouting.CSMS,

                           NetworkTopologyInformation,

                           SignKeys,
                           SignInfos,
                           Signatures,

                           CustomData,

                           RequestId        ?? NetworkingNode.OCPP.NextRequestId,
                           RequestTimestamp ?? Timestamp.Now,
                           EventTrackingId  ?? EventTracking_Id.New,
                           NetworkPath      ?? NetworkPath.From(NetworkingNode.Id),
                           SerializationFormat,
                           CancellationToken

                       )
                   );

        #endregion

        #region NotifyNetworkTopology                 (NetworkingNode, ...)

        ///// <summary>
        ///// Transfer the given binary data to the CSMS.
        ///// </summary>
        ///// <param name="NetworkTopologyInformation">A network topology information.</param>
        ///// 
        ///// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        ///// 
        ///// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        ///// 
        ///// <param name="RequestId">An optional request identification.</param>
        ///// <param name="RequestTimestamp">An optional request timestamp.</param>
        ///// <param name="RequestTimeout">An optional timeout for this request.</param>
        ///// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        ///// <param name="CancellationToken">An optional token to cancel this request.</param>
        //public static Task<OCPP.NN.NotifyNetworkTopologyResponse>

        //    NotifyNetworkTopology(this INetworkingNode2         NetworkingNode,

        //                          SourceRouting                 Destination,
        //                          NetworkTopologyInformation    NetworkTopologyInformation,

        //                          CustomData?                   CustomData          = null,

        //                          IEnumerable<KeyPair>?         SignKeys            = null,
        //                          IEnumerable<SignInfo>?        SignInfos           = null,
        //                          IEnumerable<Signature>?  Signatures          = null,

        //                          Request_Id?                   RequestId           = null,
        //                          DateTime?                     RequestTimestamp    = null,
        //                          TimeSpan?                     RequestTimeout      = null,
        //                          EventTracking_Id?             EventTrackingId     = null,
        //                          CancellationToken             CancellationToken   = default)


        //        => NetworkingNode.ocppOUT.NotifyNetworkTopology(
        //               new OCPP.NN.NotifyNetworkTopologyMessage(

        //                   Destination,
        //                   NetworkTopologyInformation,

        //                   SignKeys,
        //                   SignInfos,
        //                   Signatures,

        //                   CustomData,

        //                   RequestId        ?? NetworkingNode.OCPPAdapter.NextRequestId,
        //                   RequestTimestamp ?? Timestamp.Now,
        //                   RequestTimeout   ?? NetworkingNode.OCPP.DefaultRequestTimeout,
        //                   EventTrackingId  ?? EventTracking_Id.New,
        //                   NetworkPath      ?? NetworkPath.From(NetworkingNode.Id),
        //                   CancellationToken

        //               )
        //           );

        #endregion


    }

}
