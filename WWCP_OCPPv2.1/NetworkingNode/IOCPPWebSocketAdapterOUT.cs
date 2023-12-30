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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// The common interface of all central systems channels.
    /// CSMS might have multiple channels, e.g. a SOAP and a WebSockets channel.
    /// </summary>
    public interface IOCPPWebSocketAdapterOUT : OCPP.NN.INetworkingNodeOutgoingMessages,
                                                OCPP.NN.INetworkingNodeOutgoingMessageEvents,

                                             //   OCPP.NN.CSMS.INetworkingNodeOutgoingMessages,
                                             //   OCPP.NN.CSMS.INetworkingNodeOutgoingMessageEvents,

                                                CS.  INetworkingNodeOutgoingMessages,
                                                //CS.  INetworkingNodeOutgoingMessageEvents,

                                                CSMS.INetworkingNodeOutgoingMessages,
                                                CSMS.INetworkingNodeOutgoingMessageEvents

    {

        Task<DataTransferResponse>           DataTransfer         (          DataTransferRequest           Request);

        Task NotifyJSONMessageResponseSent  (OCPP_JSONResponseMessage   JSONResponseMessage);
        Task NotifyJSONErrorResponseSent    (OCPP_JSONErrorMessage      JSONErrorMessage);
        Task NotifyBinaryMessageResponseSent(OCPP_BinaryResponseMessage BinaryResponseMessage);


    }

}
