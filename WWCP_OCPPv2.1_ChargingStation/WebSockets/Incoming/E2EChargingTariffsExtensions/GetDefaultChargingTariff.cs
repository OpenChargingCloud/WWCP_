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
using cloud.charging.open.protocols.OCPP.WebSockets;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPP.CS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    /// <summary>
    /// The charging station HTTP WebSocket client runs on a charging station
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class ChargingStationWSClient : AOCPPWebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   ICSIncomingMessages,
                                                   ICSOutgoingMessagesEvents
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<GetDefaultChargingTariffRequest>?       CustomGetDefaultChargingTariffRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<GetDefaultChargingTariffResponse>?  CustomGetDefaultChargingTariffResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetDefaultChargingTariff websocket request was received.
        /// </summary>
        public event WSClientJSONRequestLogHandler?                 OnGetDefaultChargingTariffWSRequest;

        /// <summary>
        /// An event sent whenever a GetDefaultChargingTariff request was received.
        /// </summary>
        public event OnGetDefaultChargingTariffRequestDelegate?     OnGetDefaultChargingTariffRequest;

        /// <summary>
        /// An event sent whenever a GetDefaultChargingTariff request was received.
        /// </summary>
        public event OnGetDefaultChargingTariffDelegate?            OnGetDefaultChargingTariff;

        /// <summary>
        /// An event sent whenever a response to a GetDefaultChargingTariff request was sent.
        /// </summary>
        public event OnGetDefaultChargingTariffResponseDelegate?    OnGetDefaultChargingTariffResponse;

        /// <summary>
        /// An event sent whenever a websocket response to a GetDefaultChargingTariff request was sent.
        /// </summary>
        public event WSClientJSONRequestJSONResponseLogHandler?     OnGetDefaultChargingTariffWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_GetDefaultChargingTariff(DateTime                   RequestTimestamp,
                                             WebSocketClientConnection  WebSocketConnection,
                                             NetworkingNode_Id          DestinationNodeId,
                                             NetworkPath                NetworkPath,
                                             EventTracking_Id           EventTrackingId,
                                             Request_Id                 RequestId,
                                             JObject                    RequestJSON,
                                             CancellationToken          CancellationToken)

        {

            #region Send OnGetDefaultChargingTariffWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetDefaultChargingTariffWSRequest?.Invoke(startTime,
                                                            WebSocketConnection,
                                                            DestinationNodeId,
                                                            NetworkPath,
                                                            EventTrackingId,
                                                            RequestJSON);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnGetDefaultChargingTariffWSRequest));
            }

            #endregion

            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (GetDefaultChargingTariffRequest.TryParse(RequestJSON,
                                                             RequestId,
                                                             DestinationNodeId,
                                                             NetworkPath,
                                                             out var request,
                                                             out var errorResponse,
                                                             CustomGetDefaultChargingTariffRequestParser) && request is not null) {

                    #region Send OnGetDefaultChargingTariffRequest event

                    try
                    {

                        OnGetDefaultChargingTariffRequest?.Invoke(Timestamp.Now,
                                                                  this,
                                                                  WebSocketConnection,
                                                                  request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnGetDefaultChargingTariffRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    GetDefaultChargingTariffResponse? response = null;

                    var results = OnGetDefaultChargingTariff?.
                                      GetInvocationList()?.
                                      SafeSelect(subscriber => (subscriber as OnGetDefaultChargingTariffDelegate)?.Invoke(Timestamp.Now,
                                                                                                                          this,
                                                                                                                          WebSocketConnection,
                                                                                                                          request,
                                                                                                                          CancellationToken)).
                                      ToArray();

                    if (results?.Length > 0)
                    {

                        await Task.WhenAll(results!);

                        response = results.FirstOrDefault()?.Result;

                    }

                    response ??= GetDefaultChargingTariffResponse.Failed(request);

                    #endregion

                    #region Send OnGetDefaultChargingTariffResponse event

                    try
                    {

                        OnGetDefaultChargingTariffResponse?.Invoke(Timestamp.Now,
                                                                   this,
                                                                   WebSocketConnection,
                                                                   request,
                                                                   response,
                                                                   response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnGetDefaultChargingTariffResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       NetworkPath.Source,
                                       RequestId,
                                       response.ToJSON(
                                           CustomGetDefaultChargingTariffResponseSerializer,
                                           CustomStatusInfoSerializer,
                                           null,//CustomChargingTariffSerializer,
                                           null,//CustomPriceSerializer,
                                           null,//CustomTariffElementSerializer,
                                           null,//CustomPriceComponentSerializer,
                                           null,//CustomTaxRateSerializer,
                                           null,//CustomTariffRestrictionsSerializer,
                                           null,//CustomEnergyMixSerializer,
                                           null,//CustomEnergySourceSerializer,
                                           null,//CustomEnvironmentalImpactSerializer,
                                           CustomIdTokenSerializer,
                                           CustomAdditionalInfoSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_GetDefaultChargingTariff)[8..],
                                            RequestJSON,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {
                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_GetDefaultChargingTariff)[8..],
                                        RequestJSON,
                                        e
                                    );
            }

            #region Send OnGetDefaultChargingTariffWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnGetDefaultChargingTariffWSResponse?.Invoke(endTime,
                                                             WebSocketConnection,
                                                             DestinationNodeId,
                                                             NetworkPath,
                                                             EventTrackingId,
                                                             RequestTimestamp,
                                                             RequestJSON,
                                                             OCPPResponse?.Payload,
                                                             OCPPErrorResponse?.ToJSON(),
                                                             endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnGetDefaultChargingTariffWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}
