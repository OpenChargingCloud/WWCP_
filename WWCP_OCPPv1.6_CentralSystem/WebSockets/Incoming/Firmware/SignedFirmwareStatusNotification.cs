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
using cloud.charging.open.protocols.OCPPv1_6.CP;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CS
{

    /// <summary>
    /// The CSMS HTTP/WebSocket/JSON server.
    /// </summary>
    public partial class CentralSystemWSServer : ACSMSWSServer,
                                                 ICSMSChannel
    {

        #region Custom JSON parser delegates

        public CustomJObjectParserDelegate<SignedFirmwareStatusNotificationRequest>?       CustomSignedFirmwareStatusNotificationRequestParser         { get; set; }

        public CustomJObjectSerializerDelegate<SignedFirmwareStatusNotificationResponse>?  CustomSignedFirmwareStatusNotificationResponseSerializer    { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// An event sent whenever a SignedFirmwareStatusNotification WebSocket request was received.
        /// </summary>
        public event OnOCPPJSONRequestLogDelegate?                          OnSignedFirmwareStatusNotificationWSRequest;

        /// <summary>
        /// An event sent whenever a SignedFirmwareStatusNotification request was received.
        /// </summary>
        public event OnSignedFirmwareStatusNotificationRequestDelegate?     OnSignedFirmwareStatusNotificationRequest;

        /// <summary>
        /// An event sent whenever a SignedFirmwareStatusNotification request was received.
        /// </summary>
        public event OnSignedFirmwareStatusNotificationDelegate?            OnSignedFirmwareStatusNotification;

        /// <summary>
        /// An event sent whenever a response to a SignedFirmwareStatusNotification request was sent.
        /// </summary>
        public event OnSignedFirmwareStatusNotificationResponseDelegate?    OnSignedFirmwareStatusNotificationResponse;

        /// <summary>
        /// An event sent whenever a WebSocket response to a SignedFirmwareStatusNotification request was sent.
        /// </summary>
        public event OnOCPPJSONRequestJSONResponseLogDelegate?              OnSignedFirmwareStatusNotificationWSResponse;

        #endregion


        #region Receive message (wired via reflection!)

        public async Task<Tuple<OCPP_JSONResponseMessage?,
                                OCPP_JSONErrorMessage?>>

            Receive_SignedFirmwareStatusNotification(DateTime                   RequestTimestamp,
                                                     WebSocketServerConnection  Connection,
                                                     NetworkingNode_Id          NetworkingNodeId,
                                                     NetworkPath                NetworkPath,
                                                     EventTracking_Id           EventTrackingId,
                                                     Request_Id                 RequestId,
                                                     JObject                    JSONRequest,
                                                     CancellationToken          CancellationToken)

        {

            #region Send OnSignedFirmwareStatusNotificationWSRequest event

            var startTime = Timestamp.Now;

            try
            {

                OnSignedFirmwareStatusNotificationWSRequest?.Invoke(startTime,
                                                                    this,
                                                                    Connection,
                                                                    NetworkingNodeId,
                                                                    EventTrackingId,
                                                                    RequestTimestamp,
                                                                    JSONRequest,
                                                                    CancellationToken);

            }
            catch (Exception e)
            {
                DebugX.Log(e, nameof(CentralSystemWSServer) + "." + nameof(OnSignedFirmwareStatusNotificationWSRequest));
            }

            #endregion


            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                if (SignedFirmwareStatusNotificationRequest.TryParse(JSONRequest,
                                                                     RequestId,
                                                                     NetworkingNodeId,
                                                                     NetworkPath,
                                                                     out var request,
                                                                     out var errorResponse,
                                                                     CustomSignedFirmwareStatusNotificationRequestParser) &&
                    request is not null) {

                    #region Send OnSignedFirmwareStatusNotificationRequest event

                    try
                    {

                        OnSignedFirmwareStatusNotificationRequest?.Invoke(Timestamp.Now,
                                                                          this,
                                                                          Connection,
                                                                          request);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CentralSystemWSServer) + "." + nameof(OnSignedFirmwareStatusNotificationRequest));
                    }

                    #endregion

                    #region Call async subscribers

                    SignedFirmwareStatusNotificationResponse? response = null;

                    var responseTasks = OnSignedFirmwareStatusNotification?.
                                            GetInvocationList()?.
                                            SafeSelect(subscriber => (subscriber as OnSignedFirmwareStatusNotificationDelegate)?.Invoke(Timestamp.Now,
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

                    response ??= SignedFirmwareStatusNotificationResponse.Failed(request);

                    #endregion

                    #region Send OnSignedFirmwareStatusNotificationResponse event

                    try
                    {

                        OnSignedFirmwareStatusNotificationResponse?.Invoke(Timestamp.Now,
                                                                           this,
                                                                           Connection,
                                                                           request,
                                                                           response,
                                                                           response.Runtime);

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(e, nameof(CentralSystemWSServer) + "." + nameof(OnSignedFirmwareStatusNotificationResponse));
                    }

                    #endregion

                    OCPPResponse = new OCPP_JSONResponseMessage(
                                       RequestId,
                                       response.ToJSON(
                                           CustomSignedFirmwareStatusNotificationResponseSerializer,
                                           CustomSignatureSerializer,
                                           CustomCustomDataSerializer
                                       )
                                   );

                }

                else
                    OCPPErrorResponse = OCPP_JSONErrorMessage.CouldNotParse(
                                            RequestId,
                                            nameof(Receive_SignedFirmwareStatusNotification)[8..],
                                            JSONRequest,
                                            errorResponse
                                        );

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.FormationViolation(
                                        RequestId,
                                        nameof(Receive_SignedFirmwareStatusNotification)[8..],
                                        JSONRequest,
                                        e
                                    );

            }


            #region Send OnSignedFirmwareStatusNotificationWSResponse event

            try
            {

                var endTime = Timestamp.Now;

                OnSignedFirmwareStatusNotificationWSResponse?.Invoke(endTime,
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
                DebugX.Log(e, nameof(CentralSystemWSServer) + "." + nameof(OnSignedFirmwareStatusNotificationWSResponse));
            }

            #endregion

            return new Tuple<OCPP_JSONResponseMessage?,
                             OCPP_JSONErrorMessage?>(OCPPResponse,
                                                     OCPPErrorResponse);

        }

        #endregion


    }

}