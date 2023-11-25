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

    #region OnAddSignaturePolicy (-Request/-Response) Delegate

    /// <summary>
    /// A delegate called whenever an AddSignaturePolicy request will be sent to a charging station.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The reserve now request.</param>
    public delegate Task OnAddSignaturePolicyRequestDelegate(DateTime                    Timestamp,
                                                             IEventSender                Sender,
                                                             AddSignaturePolicyRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to an AddSignaturePolicy request was received.
    /// </summary>
    /// <param name="Timestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnAddSignaturePolicyResponseDelegate(DateTime                     Timestamp,
                                                              IEventSender                 Sender,
                                                              AddSignaturePolicyRequest    Request,
                                                              AddSignaturePolicyResponse   Response,
                                                              TimeSpan                     Runtime);

    #endregion


    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON serializer delegates

        public CustomJObjectSerializerDelegate<AddSignaturePolicyRequest>?  CustomAddSignaturePolicyRequestSerializer    { get; set; }

        public CustomJObjectParserDelegate<AddSignaturePolicyResponse>?     CustomAddSignaturePolicyResponseParser       { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a AddSignaturePolicy request was sent.
        /// </summary>
        public event OnAddSignaturePolicyRequestDelegate?     OnAddSignaturePolicyRequest;

        /// <summary>
        /// An event sent whenever a response to a AddSignaturePolicy request was sent.
        /// </summary>
        public event OnAddSignaturePolicyResponseDelegate?    OnAddSignaturePolicyResponse;

        #endregion


        #region AddSignaturePolicy(Request)

        public async Task<AddSignaturePolicyResponse> AddSignaturePolicy(AddSignaturePolicyRequest Request)
        {

            #region Send OnAddSignaturePolicyRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnAddSignaturePolicyRequest?.Invoke(startTime,
                                                   this,
                                                   Request);
            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnAddSignaturePolicyRequest));
            }

            #endregion


            AddSignaturePolicyResponse? response = null;

            try
            {

                var sendRequestState = await SendJSONAndWait(
                                                 Request.EventTrackingId,
                                                 Request.RequestId,
                                                 Request.ChargingStationId,
                                                 Request.Action,
                                                 Request.ToJSON(
                                                     CustomAddSignaturePolicyRequestSerializer
                                                     //CustomMessageInfoSerializer,
                                                     //CustomMessageContentSerializer,
                                                     //CustomComponentSerializer,
                                                     //CustomEVSESerializer,
                                                     //CustomSignatureSerializer,
                                                     //CustomCustomDataSerializer
                                                 ),
                                                 Request.RequestTimeout
                                             );

                if (sendRequestState.NoErrors &&
                    sendRequestState.JSONResponse is not null)
                {

                    if (AddSignaturePolicyResponse.TryParse(Request,
                                                            sendRequestState.JSONResponse.Payload,
                                                            out var setDisplayMessageResponse,
                                                            out var errorResponse,
                                                            CustomAddSignaturePolicyResponseParser) &&
                        setDisplayMessageResponse is not null)
                    {
                        response = setDisplayMessageResponse;
                    }

                    response ??= new AddSignaturePolicyResponse(
                                     Request,
                                     Result.Format(errorResponse)
                                 );

                }

                response ??= new AddSignaturePolicyResponse(
                                 Request,
                                 Result.FromSendRequestState(sendRequestState)
                             );

            }
            catch (Exception e)
            {

                response = new AddSignaturePolicyResponse(
                               Request,
                               Result.FromException(e)
                           );

            }


            #region Send OnAddSignaturePolicyResponse event

            var endTime = Timestamp.Now;

            try
            {

                OnAddSignaturePolicyResponse?.Invoke(endTime,
                                                    this,
                                                    Request,
                                                    response,
                                                    endTime - startTime);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnAddSignaturePolicyResponse));
            }

            #endregion

            return response;

        }

        #endregion


    }

}
