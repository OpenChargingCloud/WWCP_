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

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CSMSWSServer : WebSocketServer,
                                        ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<PublishFirmwareStatusNotificationRequest>?       CustomPublishFirmwareStatusNotificationRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<PublishFirmwareStatusNotificationResponse>?  CustomPublishFirmwareStatusNotificationResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification WebSocket request was received.
        /// </summary>
        public event OnOCPPJSONRequestLogDelegate?                            OnPublishFirmwareStatusNotificationWSRequest;

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification request was received.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationRequestDelegate?    OnPublishFirmwareStatusNotificationRequest;

        /// <summary>
        /// An event sent whenever a PublishFirmwareStatusNotification request was received.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationDelegate?           OnPublishFirmwareStatusNotification;

        /// <summary>
        /// An event sent whenever a response to a PublishFirmwareStatusNotification request was sent.
        /// </summary>
        public event OnPublishFirmwareStatusNotificationResponseDelegate?   OnPublishFirmwareStatusNotificationResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a PublishFirmwareStatusNotification request was sent.
        /// </summary>
        public event OnOCPPJSONRequestJSONResponseLogDelegate?            OnPublishFirmwareStatusNotificationWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_PublishFirmwareStatusNotification(DateTime                   RequestTimestamp,
                                                      WebSocketServerConnection  Connection,
                                                      NetworkingNode_Id          NetworkingNodeId,
                                                      NetworkPath                NetworkPath,
                                                      EventTracking_Id           EventTrackingId,
                                                      Request_Id                 RequestId,
                                                      JObject                    JSONRequest,
                                                      CancellationToken          CancellationToken)

        {

            #region Send OnPublishFirmwareStatusNotificationWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnPublishFirmwareStatusNotificationWSRequest?.Invoke(startTime,
                                                                     this,
                                                                     Connection,
                                                                     NetworkingNodeId,
                                                                     EventTrackingId,
                                                                     RequestTimestamp,
                                                                     JSONRequest);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (PublishFirmwareStatusNotificationRequest.TryParse(JSONRequest,
                                                                      RequestId,
                                                                      NetworkingNodeId,
                                                                      NetworkPath,
                                                                      out var request,
                                                                      out var errorResponse,
                                                                      CustomPublishFirmwareStatusNotificationRequestParser) && request is not null) {

                    #region Send OnPublishFirmwareStatusNotificationRequest event

                    try
                    {

                        OnPublishFirmwareStatusNotificationRequest?.Invoke(Timestamp.Now,
                                                                           this,
                                                                           request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    PublishFirmwareStatusNotificationResponse? response = null;

                    var responseTasks = OnPublishFirmwareStatusNotification?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnPublishFirmwareStatusNotificationDelegate)?.Invoke(Timestamp.Now,
                                                                                                                                         this,
                                                                                                                                         request,
                                                                                                                                         CancellationToken)).
                                            ToArray();

                    if (responseTasks?.Length > 0)
                    {
                        await Task.WhenAll(responseTasks!);
                        response = responseTasks.FirstOrDefault()?.Result;
                    }

                    response ??= PublishFirmwareStatusNotificationResponse.Failed(request);

                    #endregion

                    #region Send OnPublishFirmwareStatusNotificationResponse event

                    try
                    {

                        OnPublishFirmwareStatusNotificationResponse?.Invoke(Timestamp.Now,
                                                                            this,
                                                                            request,
                                                                            response,
                                                                            response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       RequestId,
                                       response.ToJSON(
                                           CustomPublishFirmwareStatusNotificationResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_PublishFirmwareStatusNotification)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_PublishFirmwareStatusNotification)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnPublishFirmwareStatusNotificationWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnPublishFirmwareStatusNotificationWSResponse?.Invoke(endTime,
                                                                      this,
                                                                      Connection,
                                                                      NetworkingNodeId,
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
                DebugX.Log(e, nameof(CSMSWSServer) + "." + nameof(OnPublishFirmwareStatusNotificationWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}
