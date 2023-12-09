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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPPv2_1.CSMS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    /// <summary>
    /// A charging station HTTP Web Socket client.
    /// </summary>
    public partial class ChargingStationWSClient : WebSocketClient,
                                                   IChargingStationWebSocketClient,
                                                   IChargingStationServer,
                                                   IChargingStationClientEvents
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<MeterValuesRequest>?  CustomMeterValuesRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<MeterValuesResponse>?     CustomMeterValuesResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event fired whenever a meter values request will be sent to the CSMS.
        /// </summary>
        public event OnMeterValuesRequestDelegate?     OnMeterValuesRequest;

        /// <summary>
        /// An event fired whenever a meter values request will be sent to the CSMS.
        /// </summary>
        public event ClientRequestLogHandler?          OnMeterValuesWSRequest;

        /// <summary>
        /// An event fired whenever a response to a meter values request was received.
        /// </summary>
        public event ClientResponseLogHandler?         OnMeterValuesWSResponse;

        /// <summary>
        /// An event fired whenever a response to a meter values request was received.
        /// </summary>
        public event OnMeterValuesResponseDelegate?    OnMeterValuesResponse;

        #endregion


        #region SendMeterValues(Request)

        /// <summary>
        /// Send meter values.
        /// </summary>
        /// <param name="Request">A MeterValues request.</param>
        public async Task<MeterValuesResponse>

            SendMeterValues(MeterValuesRequest  Request)

        {

            #region Send OnMeterValuesRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnMeterValuesRequest?.Invoke(startTime,
                                             this,
                                             Request);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnMeterValuesRequest));
            }

            #endregion


            MeterValuesResponse? response = null;

            try
            {

                var requestMessage = await SendRequest(
                                         Request.NetworkingNodeId,
                                         Request.NetworkPath,
                                         Request.Action,
                                         Request.RequestId,
                                         Request.ToJSON(
                                             CustomMeterValuesRequestSerializer,
                                             CustomMeterValueSerializer,
                                             CustomSampledValueSerializer,
                                             CustomSignatureSerializer,
                                             CustomCustomDataSerializer
                                         )
                                     );

                if (requestMessage.NoErrors)
                {

                    var sendRequestState = await WaitForResponse(requestMessage);

                    if (sendRequestState.NoErrors &&
                        sendRequestState.JSONResponse is not null)
                    {

                        if (MeterValuesResponse.TryParse(Request,
                                                         sendRequestState.JSONResponse.Payload,
                                                         out var meterValuesResponse,
                                                         out var errorResponse,
                                                         CustomMeterValuesResponseParser) &&
                            meterValuesResponse is not null)
                        {
                            response = meterValuesResponse;
                        }

                        response ??= new MeterValuesResponse(
                                         Request,
                                         Result.Format(errorResponse)
                                     );

                    }

                    response ??= new MeterValuesResponse(
                                     Request,
                                     Result.FromSendRequestState(sendRequestState)
                                 );

                }

                response ??= new MeterValuesResponse(
                                 Request,
                                 Result.GenericError(requestMessage.ErrorMessage)
                             );

            }
            catch (Exception e)
            {

                response = new MeterValuesResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnMeterValuesResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnMeterValuesResponse?.Invoke(endTime,
                                              this,
                                              Request,
                                              response,
                                              endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(ChargingStationWSClient) + "." + nameof(OnMeterValuesResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
