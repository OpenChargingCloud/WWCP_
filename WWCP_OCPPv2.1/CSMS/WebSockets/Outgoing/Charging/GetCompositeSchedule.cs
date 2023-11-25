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
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.CS;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    #region OnGetCompositeSchedule (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever a get composite schedule request will be sent to a charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetCompositeScheduleRequestDelegate(DateTime                      Timestamp,
                                                               IEventSender                  Sender,
                                                               GetCompositeScheduleRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a get composite schedule request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetCompositeScheduleResponseDelegate(DateTime                       Timestamp,
                                                                IEventSender                   Sender,
                                                                GetCompositeScheduleRequest    Request,
                                                                GetCompositeScheduleResponse   Response,
                                                                TimeSpan                       Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<GetCompositeScheduleRequest>?  CustomGetCompositeScheduleRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<GetCompositeScheduleResponse>?     CustomGetCompositeScheduleResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a GetCompositeSchedule request was sent.
        /// </summary>
        public event OnGetCompositeScheduleRequestDelegate?     OnGetCompositeScheduleRequest;

        /// <summary>
        /// An event sent whenever a response to a GetCompositeSchedule request was sent.
        /// </summary>
        public event OnGetCompositeScheduleResponseDelegate?    OnGetCompositeScheduleResponse;

        #endregion


        #region GetCompositeSchedule(Request)


        public async Task<GetCompositeScheduleResponse> GetCompositeSchedule(GetCompositeScheduleRequest Request)
        {

            #region Send OnGetCompositeScheduleRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnGetCompositeScheduleRequest?.Invoke(startTime,
                                                      this,
                                                      Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCompositeScheduleRequest));
            }

            #endregion


            GetCompositeScheduleResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.RequestId,
                                                 Request.ChargingStationId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomGetCompositeScheduleRequestSerializer,
                                                     CustomSignatureSerializer,
                                                     CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (GetCompositeScheduleResponse.TryParse(Request,
                                                              sendRequestState.JSONResponse.Payload,
                                                              out var getCompositeScheduleResponse,
                                                              out var errorResponse,
                                                              CustomGetCompositeScheduleResponseParser) &&
                        getCompositeScheduleResponse is not null)
                    {
                        response = getCompositeScheduleResponse;
                    }

                    response ??= new GetCompositeScheduleResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new GetCompositeScheduleResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new GetCompositeScheduleResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnGetCompositeScheduleResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnGetCompositeScheduleResponse?.Invoke(endTime,
                                                       this,
                                                       Request,
                                                       response,
                                                       endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnGetCompositeScheduleResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
