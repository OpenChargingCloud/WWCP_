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
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CS;
using cloud.charging.open.protocols.OCPPv2_1.CS;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// A charging station HTTP Web Socket client.
    /// </summary>
    public partial class OCPPWebSocketAdapterOUT : IOCPPWebSocketAdapterOUT
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<NotifyEVChargingScheduleRequest>?  CustomNotifyEVChargingScheduleRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<NotifyEVChargingScheduleResponse>?     CustomNotifyEVChargingScheduleResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a NotifyEVChargingSchedule request will be sent to the CSMS.
        /// </summary>
        public event OCPPv2_1.CS.OnNotifyEVChargingScheduleRequestSentDelegate?     OnNotifyEVChargingScheduleRequestSent;

        /// <summary>
        /// An event fired whenever a NotifyEVChargingSchedule request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?                                   OnNotifyEVChargingScheduleWSRequest;

        /// <summary>
        /// An event fired whenever a response to a NotifyEVChargingSchedule request was received.
        /// </summary>
        public event ClientResponseLogHandler?                                  OnNotifyEVChargingScheduleWSResponse;

        /// <summary>
        /// An event fired whenever a response to a NotifyEVChargingSchedule request was received.
        /// </summary>
        public event OCPPv2_1.CS.OnNotifyEVChargingScheduleResponseReceivedDelegate?    OnNotifyEVChargingScheduleResponseReceived;

        #endregion


        #region NotifyEVChargingSchedule(Request)

        /// <summary>
        /// Notify about an EV charging schedule.
        /// </summary>
        /// <param name="Request">A NotifyEVChargingSchedule request.</param>
        public async Task<NotifyEVChargingScheduleResponse>

            NotifyEVChargingSchedule(NotifyEVChargingScheduleRequest  Request)

        {

            #region Send OnNotifyEVChargingScheduleRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnNotifyEVChargingScheduleRequestSent?.Invoke(startTime,
                                                          parentNetworkingNode,
                                                          Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnNotifyEVChargingScheduleRequestSent));
            }

            #endregion


            NotifyEVChargingScheduleResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONRequestAndWait(
                                                 OCPP_JSONRequestMessage.FromRequest(
                                                     Request,
                                                     Request.ToJSON(
                                                         CustomNotifyEVChargingScheduleRequestSerializer,
                                                         parentNetworkingNode.OCPP.CustomChargingScheduleSerializer,
                                                         parentNetworkingNode.OCPP.CustomLimitBeyondSoCSerializer,
                                                         parentNetworkingNode.OCPP.CustomChargingSchedulePeriodSerializer,
                                                         parentNetworkingNode.OCPP.CustomV2XFreqWattEntrySerializer,
                                                         parentNetworkingNode.OCPP.CustomV2XSignalWattEntrySerializer,
                                                         parentNetworkingNode.OCPP.CustomSalesTariffSerializer,
                                                         parentNetworkingNode.OCPP.CustomSalesTariffEntrySerializer,
                                                         parentNetworkingNode.OCPP.CustomRelativeTimeIntervalSerializer,
                                                         parentNetworkingNode.OCPP.CustomConsumptionCostSerializer,
                                                         parentNetworkingNode.OCPP.CustomCostSerializer,

                                                         parentNetworkingNode.OCPP.CustomAbsolutePriceScheduleSerializer,
                                                         parentNetworkingNode.OCPP.CustomPriceRuleStackSerializer,
                                                         parentNetworkingNode.OCPP.CustomPriceRuleSerializer,
                                                         parentNetworkingNode.OCPP.CustomTaxRuleSerializer,
                                                         parentNetworkingNode.OCPP.CustomOverstayRuleListSerializer,
                                                         parentNetworkingNode.OCPP.CustomOverstayRuleSerializer,
                                                         parentNetworkingNode.OCPP.CustomAdditionalServiceSerializer,

                                                         parentNetworkingNode.OCPP.CustomPriceLevelScheduleSerializer,
                                                         parentNetworkingNode.OCPP.CustomPriceLevelScheduleEntrySerializer,

                                                         parentNetworkingNode.OCPP.CustomSignatureSerializer,
                                                         parentNetworkingNode.OCPP.CustomCustomDataSerializer
                                                     )
                                                 )
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (NotifyEVChargingScheduleResponse.TryParse(Request,
                                                                  sendRequestState.JSONResponse.Payload,
                                                                  out var reportChargingProfilesResponse,
                                                                  out var errorResponse,
                                                                  CustomNotifyEVChargingScheduleResponseParser) &&
                        reportChargingProfilesResponse is not null)
                    {
                        response = reportChargingProfilesResponse;
                    }

                    response ??= new NotifyEVChargingScheduleResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new NotifyEVChargingScheduleResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new NotifyEVChargingScheduleResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnNotifyEVChargingScheduleResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnNotifyEVChargingScheduleResponseReceived?.Invoke(endTime,
                                                           parentNetworkingNode,
                                                           Request,
                                                           response,
                                                           endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnNotifyEVChargingScheduleResponseReceived));
            }

            #endregion

            return response;

        }

        #endregion


    }

    public partial class OCPPWebSocketAdapterIN : IOCPPWebSocketAdapterIN
    {

        /// <summary>
        /// An event fired whenever a response to a NotifyEVChargingSchedule request was received.
        /// </summary>
        public event OCPPv2_1.CS.OnNotifyEVChargingScheduleResponseReceivedDelegate? OnNotifyEVChargingScheduleResponseReceived;

    }

}
