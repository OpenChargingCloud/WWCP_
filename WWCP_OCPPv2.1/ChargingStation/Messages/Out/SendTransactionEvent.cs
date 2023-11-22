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

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    #region OnTransactionEvent (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a transaction event request will be sent to the CSMS.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the transaction event request.</param>
    /// <param name="Sender">The sender of the transaction event request.</param>
    /// <param name="Request">The transaction event request.</param>
    public delegate Task OnTransactionEventRequestDelegate(DateTime                  Timestamp,
                                                           IEventSender              Sender,
                                                           TransactionEventRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a transaction event request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the transaction event request.</param>
    /// <param name="Sender">The sender of the transaction event request.</param>
    /// <param name="Request">The transaction event request.</param>
    /// <param name="Response">The transaction event response.</param>
    /// <param name="Runtime">The runtime of the transaction event request.</param>
    public delegate Task OnTransactionEventResponseDelegate(DateTime                   Timestamp,
                                                            IEventSender               Sender,
                                                            TransactionEventRequest    Request,
                                                            TransactionEventResponse   Response,
                                                            TimeSpan                   Runtime);

    #endregion


    /// <summary>
    /// A CP client.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<TransactionEventRequest>?  CustomTransactionEventRequestSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever an authorize request will be sent to the CSMS.
        /// </summary>
        public event OnTransactionEventRequestDelegate?     OnTransactionEventRequest;

        /// <summary>
        /// An event fired whenever an authorize request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?               OnTransactionEventWSRequest;

        /// <summary>
        /// An event fired whenever a response to an authorize request was received.
        /// </summary>
        public event ClientResponseLogHandler?              OnTransactionEventWSResponse;

        /// <summary>
        /// An event fired whenever a response to an authorize request was received.
        /// </summary>
        public event OnTransactionEventResponseDelegate?    OnTransactionEventResponse;

        #endregion


        #region SendTransactionEvent                 (Request)

        /// <summary>
        /// Send a transaction event.
        /// </summary>
        /// <param name="Request">A TransactionEvent request.</param>
        public async Task<TransactionEventResponse>

            SendTransactionEvent(TransactionEventRequest  Request)

        {

            #region Send OnTransactionEventRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnTransactionEventRequest?.Invoke(startTime,
                                                  this,
                                                  Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnTransactionEventRequest));
            }

            #endregion


            TransactionEventResponse? response = null;

            var requestMessage = await SendRequest(Request.Action,
                                                   Request.RequestId,
                                                   Request.ToJSON(
                                                       CustomTransactionEventRequestSerializer,
                                                       CustomTransactionSerializer,
                                                       CustomIdTokenSerializer,
                                                       CustomAdditionalInfoSerializer,
                                                       CustomEVSESerializer,
                                                       CustomMeterValueSerializer,
                                                       CustomSampledValueSerializer,
                                                       CustomSignedMeterValueSerializer,
                                                       CustomUnitsOfMeasureSerializer,
                                                       CustomSignatureSerializer,
                                                       CustomCustomDataSerializer
                                                   ));

            if (requestMessage.NoErrors)
            {

                var sendRequestState = await WaitForResponse(requestMessage);

                if (sendRequestState.NoErrors &&
                    sendRequestState.Response is not null)
                {

                    if (TransactionEventResponse.TryParse(Request,
                                                          sendRequestState.Response,
                                                          out var transactionEventResponse,
                                                          out var errorResponse) &&
                        transactionEventResponse is not null)
                    {
                        response = transactionEventResponse;
                    }

                    response ??= new TransactionEventResponse(Request,
                                                              Result.Format(errorResponse));

                }

                response ??= new TransactionEventResponse(Request,
                                                          Result.FromSendRequestState(sendRequestState));

            }

            response ??= new TransactionEventResponse(Request,
                                                      Result.GenericError(requestMessage.ErrorMessage));


            #region Send OnTransactionEventResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnTransactionEventResponse?.Invoke(endTime,
                                                   this,
                                                   Request,
                                                   response,
                                                   endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnTransactionEventResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}