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

using cloud.charging.open.protocols.OCPPv2_1.CS;


#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    #region OnTransactionEvent

    /// <summary>
    /// A transaction event request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the transaction event request.</param>
    /// <param name="Sender">The sender of the transaction event request.</param>
    /// <param name="Request">The transaction event request.</param>
    public delegate Task

        OnTransactionEventRequestDelegate(DateTime                 Timestamp,
                                          IEventSender             Sender,
                                          TransactionEventRequest  Request);


    /// <summary>
    /// A transaction event.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the transaction event request.</param>
    /// <param name="Sender">The sender of the transaction event request.</param>
    /// <param name="Request">The transaction event request.</param>
    /// <param name="CancellationToken">A token to cancel this transaction event request.</param>
    public delegate Task<TransactionEventResponse>

        OnTransactionEventDelegate(DateTime                  Timestamp,
                                   IEventSender              Sender,
                                   TransactionEventRequest   Request,
                                   CancellationToken         CancellationToken);


    /// <summary>
    /// A transaction event response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the transaction event response.</param>
    /// <param name="Sender">The sender of the transaction event response.</param>
    /// <param name="Request">The transaction event request.</param>
    /// <param name="Response">The transaction event response.</param>
    /// <param name="Runtime">The runtime of the transaction event response.</param>
    public delegate Task

        OnTransactionEventResponseDelegate(DateTime                   Timestamp,
                                           IEventSender               Sender,
                                           TransactionEventRequest    Request,
                                           TransactionEventResponse   Response,
                                           TimeSpan                   Runtime);

    #endregion



}
