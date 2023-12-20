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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;
using cloud.charging.open.protocols.OCPPv2_1.CS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : AOCPPWebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<NotifyCustomerInformationRequest>?       CustomNotifyCustomerInformationRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<NotifyCustomerInformationResponse>?  CustomNotifyCustomerInformationResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a NotifyCustomerInformation WebSocket request was received.
        /// </summary>
        public event OnOCPPJSONRequestLogDelegate?                   OnNotifyCustomerInformationWSRequest;

        /// <summary>
        /// An event sent whenever a NotifyCustomerInformation request was received.
        /// </summary>
        public event OnNotifyCustomerInformationRequestDelegate?     OnNotifyCustomerInformationRequest;

        /// <summary>
        /// An event sent whenever a NotifyCustomerInformation was received.
        /// </summary>
        public event OnNotifyCustomerInformationDelegate?            OnNotifyCustomerInformation;

        /// <summary>
        /// An event sent whenever a response to a NotifyCustomerInformation was sent.
        /// </summary>
        public event OnNotifyCustomerInformationResponseDelegate?    OnNotifyCustomerInformationResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a NotifyCustomerInformation was sent.
        /// </summary>
        public event OnOCPPJSONRequestJSONResponseLogDelegate?       OnNotifyCustomerInformationWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_NotifyCustomerInformation(DateTime                   RequestTimestamp,
                                              WebSocketServerConnection  Connection,
                                              NetworkingNode_Id          DestinationNodeId,
                                              NetworkPath                NetworkPath,
                                              EventTracking_Id           EventTrackingId,
                                              Request_Id                 RequestId,
                                              JObject                    JSONRequest,
                                              CancellationToken          CancellationToken)

        {

            #region Send OnNotifyCustomerInformationWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyCustomerInformationWSRequest?.Invoke(startTime,
                                                             this,
                                                             Connection,
                                                             DestinationNodeId,
                                                             EventTrackingId,
                                                             RequestTimestamp,
                                                             JSONRequest,
                                                             CancellationToken);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyCustomerInformationWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (NotifyCustomerInformationRequest.TryParse(JSONRequest,
                                                              RequestId,
                                                              DestinationNodeId,
                                                              NetworkPath,
                                                              out var request,
                                                              out var errorResponse,
                                                              CustomNotifyCustomerInformationRequestParser) && request is not null) {

                    #region Send OnNotifyCustomerInformationRequest event

                    try
                    {

                        OnNotifyCustomerInformationRequest?.Invoke(Timestamp.Now,
                                                                   this,
                                                                   Connection,
                                                                   request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyCustomerInformationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    NotifyCustomerInformationResponse? response = null;

                    var responseTasks = OnNotifyCustomerInformation?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnNotifyCustomerInformationDelegate)?.Invoke(Timestamp.Now,
                                                                                                                                 this,
                                                                                                                                 Connection,
                                                                                                                                 request,
                                                                                                                                 CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= NotifyCustomerInformationResponse.Failed(request);

                    #endregion

                    #region Send OnNotifyCustomerInformationResponse event

                    try
                    {

                        OnNotifyCustomerInformationResponse?.Invoke(Timestamp.Now,
                                                                    this,
                                                                    Connection,
                                                                    request,
                                                                    response,
                                                                    response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyCustomerInformationResponse));
                    }

                    #endregion

                    OCPPResponse = OCPP_JSONResponseMessage.From(
                                       NetworkPath.Source,
                                       NetworkPath,
                                       RequestId,
                                       response.ToJSON(
                                           CustomNotifyCustomerInformationResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_NotifyCustomerInformation)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_NotifyCustomerInformation)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnNotifyCustomerInformationWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnNotifyCustomerInformationWSResponse?.Invoke(endTime,
                                                              this,
                                                              Connection,
                                                              DestinationNodeId,
                                                              EventTrackingId,
                                                              RequestTimestamp,
                                                              JSONRequest,
                                                              endTime, //ToDo: Refactor me!
                                                              OCPPResponse?.Payload,
                                                              OCPPErrorResponse?.ToJSON(),
                                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnNotifyCustomerInformationWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}
