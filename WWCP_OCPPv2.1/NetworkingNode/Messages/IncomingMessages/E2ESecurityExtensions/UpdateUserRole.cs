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

using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// A received UpdateUserRole request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the UpdateUserRole request.</param>
    /// <param name="Sender">The sender of the UpdateUserRole request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The UpdateUserRole request.</param>
    public delegate Task

        OnUpdateUserRoleRequestReceivedDelegate(DateTime                Timestamp,
                                                IEventSender            Sender,
                                                IWebSocketConnection    Connection,
                                                UpdateUserRoleRequest   Request);


    /// <summary>
    /// Process a received UpdateUserRole request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the UpdateUserRole request.</param>
    /// <param name="Sender">The sender of the UpdateUserRole request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The UpdateUserRole request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<UpdateUserRoleResponse>

        OnUpdateUserRoleDelegate(DateTime                Timestamp,
                                 IEventSender            Sender,
                                 IWebSocketConnection    Connection,
                                 UpdateUserRoleRequest   Request,
                                 CancellationToken       CancellationToken);


    /// <summary>
    /// A sent UpdateUserRole response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the UpdateUserRole request.</param>
    /// <param name="Sender">The sender of the UpdateUserRole request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The UpdateUserRole request.</param>
    /// <param name="Response">The UpdateUserRole response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnUpdateUserRoleResponseSentDelegate(DateTime                 Timestamp,
                                             IEventSender             Sender,
                                             IWebSocketConnection     Connection,
                                             UpdateUserRoleRequest    Request,
                                             UpdateUserRoleResponse   Response,
                                             TimeSpan                 Runtime);

}
