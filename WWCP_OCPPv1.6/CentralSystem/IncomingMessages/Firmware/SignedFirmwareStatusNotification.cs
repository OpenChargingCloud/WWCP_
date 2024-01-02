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

using cloud.charging.open.protocols.OCPPv1_6.CP;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CS
{

    /// <summary>
    /// A SignedFirmwareStatusNotification request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The stop transaction request.</param>
    public delegate Task

        OnSignedFirmwareStatusNotificationRequestDelegate(DateTime                                  Timestamp,
                                                          IEventSender                              Sender,
                                                          WebSocketServerConnection                 Connection,
                                                          SignedFirmwareStatusNotificationRequest   Request);


    /// <summary>
    /// A SignedFirmwareStatusNotification at the given charge point.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<SignedFirmwareStatusNotificationResponse>

        OnSignedFirmwareStatusNotificationDelegate(DateTime                                  Timestamp,
                                                   IEventSender                              Sender,
                                                   WebSocketServerConnection                 Connection,
                                                   SignedFirmwareStatusNotificationRequest   Request,
                                                   CancellationToken                         CancellationToken);


    /// <summary>
    /// A SignedFirmwareStatusNotification response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Connection">The HTTP WebSocket server connection.</param>
    /// <param name="Request">The stop transaction request.</param>
    /// <param name="Response">The stop transaction response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnSignedFirmwareStatusNotificationResponseDelegate(DateTime                                   Timestamp,
                                                           IEventSender                               Sender,
                                                           WebSocketServerConnection                  Connection,
                                                           SignedFirmwareStatusNotificationRequest    Request,
                                                           SignedFirmwareStatusNotificationResponse   Response,
                                                           TimeSpan                                   Runtime);

}
