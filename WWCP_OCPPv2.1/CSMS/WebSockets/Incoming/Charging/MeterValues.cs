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
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    #region OnMeterValues

    /// <summary>
    /// A meter values request.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The meter values request.</param>
    public delegate Task

        OnMeterValuesRequestDelegate(DateTime             Timestamp,
                                     IEventSender         Sender,
                                     MeterValuesRequest   Request);


    /// <summary>
    /// Send meter values.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The meter values request.</param>
    /// <param name="CancellationToken">A token to cancel this request.</param>
    public delegate Task<MeterValuesResponse>

        OnMeterValuesDelegate(DateTime             Timestamp,
                              IEventSender         Sender,
                              MeterValuesRequest   Request,
                              CancellationToken    CancellationToken);


    /// <summary>
    /// A meter values response.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The meter values request.</param>
    /// <param name="Response">The meter values response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task

        OnMeterValuesResponseDelegate(DateTime              Timestamp,
                                      IEventSender          Sender,
                                      MeterValuesRequest    Request,
                                      MeterValuesResponse   Response,
                                      TimeSpan              Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<MeterValuesRequest>?       CustomMeterValuesRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<MeterValuesResponse>?  CustomMeterValuesResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a MeterValues WebSocket request was received.
        /// </summary>
        public event WebSocketRequestLogHandler?       OnMeterValuesWSRequest;

        /// <summary>
        /// An event sent whenever a MeterValues request was received.
        /// </summary>
        public event OnMeterValuesRequestDelegate?     OnMeterValuesRequest;

        /// <summary>
        /// An event sent whenever a MeterValues request was received.
        /// </summary>
        public event OnMeterValuesDelegate?            OnMeterValues;

        /// <summary>
        /// An event sent whenever a response to a MeterValues request was sent.
        /// </summary>
        public event OnMeterValuesResponseDelegate?    OnMeterValuesResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a MeterValues request was sent.
        /// </summary>
        public event WebSocketResponseLogHandler?      OnMeterValuesWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_WebSocket_ErrorMessage?>>

            Receive_MeterValues(JArray                     json,
                                JObject                    requestData,
                                Request_Id                 requestId,
                                ChargingStation_Id         chargingStationId,
                                WebSocketServerConnection  Connection,
                                String                     OCPPTextMessage,
                                CancellationToken          CancellationToken)

        {

            #region Send OnMeterValuesWSRequest event

            try
            {

                OnMeterValuesWSRequest?.Invoke(Timestamp.Now,
                                               this,
                                               json);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnMeterValuesWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_WebSocket_ErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (MeterValuesRequest.TryParse(requestData,
                                                requestId,
                                                chargingStationId,
                                                out var request,
                                                out var errorResponse,
                                                CustomMeterValuesRequestParser) && request is not null) {

                    #region Send OnMeterValuesRequest event

                    try
                    {

                        OnMeterValuesRequest?.Invoke(Timestamp.Now,
                                                     this,
                                                     request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnMeterValuesRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    MeterValuesResponse? response = null;

                    var responseTasks = OnMeterValues?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnMeterValuesDelegate)?.Invoke(Timestamp.Now,
                                                                                                                   this,
                                                                                                                   request,
                                                                                                                   CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= MeterValuesResponse.Failed(request);

                    #endregion

                    #region Send OnMeterValuesResponse event

                    try
                    {

                        OnMeterValuesResponse?.Invoke(Timestamp.Now,
                                                      this,
                                                      request,
                                                      response,
                                                      response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnMeterValuesResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       requestId,
                                       response.ToJSON(
                                           CustomMeterValuesResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.CouldNotParse(
                                            requestId,
                                            nameof(Receive_MeterValues)[8..],
                                            requestData,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_WebSocket_ErrorMessage.FormationViolation(
                                        requestId,
                                        nameof(Receive_MeterValues)[8..],
                                        requestData,
                                        e
                                    );

            }


            #region Send OnMeterValuesWSResponse event

            try
            {

                OnMeterValuesWSResponse?.Invoke(Timestamp.Now,
                                                this,
                                                json,
                                                OCPPResponse?.ToJSON() ?? OCPPErrorResponse?.ToJSON() ?? new JArray());

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnMeterValuesWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_WebSocket_ErrorMessage?>(OCPPResponse,
                                                           OCPPErrorResponse);

        }

        #endregion


    }

}