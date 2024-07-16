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

using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using org.GraphDefined.Vanaheimr.Hermod.Logging;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;

using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode.CSMS;
using cloud.charging.open.protocols.OCPPv2_1.WebSockets;
using cloud.charging.open.protocols.OCPPv2_1.ISO15118_20.CommonMessages;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    /// <summary>
    /// The networking node HTTP WebSocket client runs on a networking node
    /// and connects to a CSMS to invoke methods.
    /// </summary>
    public partial class OCPPWebSocketClient : WebSocketClient,
                                               IOCPPWebSocketClient
    {

        #region Data

        /// <summary>
        /// The default HTTP user agent string.
        /// </summary>
        public new const String  DefaultHTTPUserAgent   = $"GraphDefined OCPP {Version.String} NN WebSocket Client";

        private    const String  LogfileName            = "NetworkingNodeWSClient.log";

        #endregion

        #region Properties

        public IOCPPAdapter    OCPPAdapter      { get; }

        public NetworkingMode  NetworkingMode   { get; } = NetworkingMode.Standard;

        #endregion

        #region Events

        #region Common Connection Management

        #endregion

        #region Generic JSON Messages

        /// <summary>
        /// An event sent whenever a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?   OnJSONMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a text message was sent.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?  OnJSONMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a text message was sent.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?    OnJSONRequestErrorSent;


        /// <summary>
        /// An event sent whenever a text message request was sent.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?   OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?  OnJSONMessageResponseReceived;

        /// <summary>
        /// An event sent whenever an error response to a text message request was received.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?    OnJSONRequestErrorReceived;

        #endregion

        #region Generic Binary Messages

        /// <summary>
        /// An event sent whenever a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?   OnBinaryMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a binary message was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?  OnBinaryMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a binary message was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseSent;


        /// <summary>
        /// An event sent whenever a binary message request was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?   OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?  OnBinaryMessageResponseReceived;

        /// <summary>
        /// An event sent whenever the error response to a binary message request was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseReceived;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new networking node websocket client running on a networking node
        /// and connecting to a CSMS to invoke methods.
        /// </summary>
        /// <param name="RemoteURL">The remote URL of the HTTP endpoint to connect to.</param>
        /// <param name="VirtualHostname">An optional HTTP virtual hostname.</param>
        /// <param name="Description">An optional description of this HTTP/websocket client.</param>
        /// <param name="RemoteCertificateValidator">The remote SSL/TLS certificate validator.</param>
        /// <param name="LocalCertificateSelector">A delegate to select a TLS client certificate.</param>
        /// <param name="ClientCert">The SSL/TLS client certificate to use of HTTP authentication.</param>
        /// <param name="HTTPUserAgent">The HTTP user agent identification.</param>
        /// <param name="URLPathPrefix">An optional default URL path prefix.</param>
        /// <param name="HTTPAuthentication">The WebService-Security username/password.</param>
        /// <param name="RequestTimeout">An optional Request timeout.</param>
        /// <param name="TransmissionRetryDelay">The delay between transmission retries.</param>
        /// <param name="MaxNumberOfRetries">The maximum number of transmission retries for HTTP request.</param>
        /// <param name="LoggingPath">The logging path.</param>
        /// <param name="LoggingContext">An optional context for logging client methods.</param>
        /// <param name="LogfileCreator">A delegate to create a log file from the given context and log file name.</param>
        /// <param name="HTTPLogger">A HTTP logger.</param>
        /// <param name="DNSClient">The DNS client to use.</param>
        public OCPPWebSocketClient(IOCPPAdapter                                                    OCPPAdapter,

                                   URL                                                             RemoteURL,
                                   HTTPHostname?                                                   VirtualHostname              = null,
                                   String?                                                         Description                  = null,
                                   Boolean?                                                        PreferIPv4                   = null,
                                   RemoteTLSServerCertificateValidationHandler<IWebSocketClient>?  RemoteCertificateValidator   = null,
                                   LocalCertificateSelectionHandler?                               LocalCertificateSelector     = null,
                                   X509Certificate?                                                ClientCert                   = null,
                                   SslProtocols?                                                   TLSProtocol                  = null,
                                   String                                                          HTTPUserAgent                = DefaultHTTPUserAgent,
                                   IHTTPAuthentication?                                            HTTPAuthentication           = null,
                                   TimeSpan?                                                       RequestTimeout               = null,
                                   TransmissionRetryDelayDelegate?                                 TransmissionRetryDelay       = null,
                                   UInt16?                                                         MaxNumberOfRetries           = 3,
                                   UInt32?                                                         InternalBufferSize           = null,

                                   IEnumerable<String>?                                            SecWebSocketProtocols        = null,
                                   NetworkingMode?                                                 NetworkingMode               = null,

                                   Boolean                                                         DisableWebSocketPings        = false,
                                   TimeSpan?                                                       WebSocketPingEvery           = null,
                                   TimeSpan?                                                       SlowNetworkSimulationDelay   = null,

                                   Boolean                                                         DisableMaintenanceTasks      = false,
                                   TimeSpan?                                                       MaintenanceEvery             = null,

                                   String?                                                         LoggingPath                  = null,
                                   String                                                          LoggingContext               = null, //CPClientLogger.DefaultContext,
                                   LogfileCreatorDelegate?                                         LogfileCreator               = null,
                                   HTTPClientLogger?                                               HTTPLogger                   = null,
                                   DNSClient?                                                      DNSClient                    = null)

            : base(RemoteURL,
                   VirtualHostname,
                   Description,
                   PreferIPv4,
                   RemoteCertificateValidator,
                   LocalCertificateSelector,
                   ClientCert,
                   TLSProtocol,
                   HTTPUserAgent ?? DefaultHTTPUserAgent,
                   HTTPAuthentication,
                   RequestTimeout,
                   TransmissionRetryDelay,
                   MaxNumberOfRetries,
                   InternalBufferSize,

                   SecWebSocketProtocols,

                   DisableWebSocketPings,
                   WebSocketPingEvery,
                   SlowNetworkSimulationDelay,

                   DisableMaintenanceTasks,
                   MaintenanceEvery,

                   LoggingPath,
                   LoggingContext,
                   LogfileCreator,
                   HTTPLogger,
                   DNSClient)

        {

            this.OCPPAdapter     = OCPPAdapter;
            this.NetworkingMode  = NetworkingMode ?? NetworkingNode.NetworkingMode.Standard;

            //this.Logger          = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                LoggingPath,
            //                                                                LoggingContext,
            //                                                                LogfileCreator);

        }

        #endregion


        #region ProcessWebSocketTextFrame   (RequestTimestamp, ClientConnection, TextMessage,   EventTrackingId, CancellationToken)

        public override async Task ProcessWebSocketTextFrame(DateTime                   RequestTimestamp,
                                                             WebSocketClientConnection  ClientConnection,
                                                             EventTracking_Id           EventTrackingId,
                                                             String                     TextMessage,
                                                             CancellationToken          CancellationToken)
        {

            if (TextMessage == "[]" ||
                TextMessage.IsNullOrEmpty())
            {
                DebugX.Log($"Received an empty JSON message within {nameof(OCPPWebSocketClient)}!");
                return;
            }

            try
            {

                var jsonArray = JArray.Parse(TextMessage);

                var textMessageResponse = await OCPPAdapter.IN.ProcessJSONMessage(
                                                    RequestTimestamp,
                                                    ClientConnection,
                                                    jsonArray,
                                                    EventTrackingId,
                                                    CancellationToken
                                                );

                //return textMessageResponse;

            }
            catch (Exception e)
            {

                DebugX.LogException(e, nameof(OCPPWebSocketClient) + "." + nameof(ProcessWebSocketTextFrame));

                //OCPPErrorResponse = new OCPP_WebSocket_ErrorMessage(
                //                        Request_Id.Zero,
                //                        ResultCodes.InternalError,
                //                        $"The OCPP message '{OCPPTextMessage}' received in " + nameof(AChargingStationWSClient) + " led to an exception!",
                //                        new JObject(
                //                            new JProperty("request",      OCPPTextMessage),
                //                            new JProperty("exception",    e.Message),
                //                            new JProperty("stacktrace",   e.StackTrace)
                //                        )
                //                    );

            }

        }

        #endregion

        #region ProcessWebSocketBinaryFrame (RequestTimestamp, ClientConnection, BinaryMessage, EventTrackingId, CancellationToken)

        public override async Task ProcessWebSocketBinaryFrame(DateTime                   RequestTimestamp,
                                                               WebSocketClientConnection  ClientConnection,
                                                               EventTracking_Id           EventTrackingId,
                                                               Byte[]                     BinaryMessage,
                                                               CancellationToken          CancellationToken)
        {

            if (BinaryMessage.Length == 0)
            {
                DebugX.Log($"Received an empty binary message within {nameof(OCPPWebSocketClient)}!");
                return;
            }

            try
            {

                var binaryMessageResponse = await OCPPAdapter.IN.ProcessBinaryMessage(
                                                      RequestTimestamp,
                                                      ClientConnection,
                                                      BinaryMessage,
                                                      EventTrackingId,
                                                      CancellationToken
                                                  );

            }
            catch (Exception e)
            {

                DebugX.LogException(e, nameof(OCPPWebSocketClient) + "." + nameof(ProcessWebSocketBinaryFrame));

                //OCPPErrorResponse = new OCPP_WebSocket_ErrorMessage(
                //                        Request_Id.Zero,
                //                        ResultCodes.InternalError,
                //                        $"The OCPP message '{OCPPTextMessage}' received in " + nameof(AChargingStationWSClient) + " led to an exception!",
                //                        new JObject(
                //                            new JProperty("request",      OCPPTextMessage),
                //                            new JProperty("exception",    e.Message),
                //                            new JProperty("stacktrace",   e.StackTrace)
                //                        )
                //                    );

            }

        }

        #endregion


        #region SendJSONRequest       (JSONRequestMessage)

        /// <summary>
        /// Send (and forget) the given JSON OCPP request message.
        /// </summary>
        /// <param name="JSONRequestMessage">A JSON OCPP request message.</param>
        public async Task<SendMessageResult> SendJSONRequest(OCPP_JSONRequestMessage JSONRequestMessage)
        {

            try
            {

                JSONRequestMessage.NetworkingMode = NetworkingMode;
                //RequestMessage.RequestTimeout ??= RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout);

                var ocppTextMessage = JSONRequestMessage.ToJSON().ToString(Formatting.None);

                if (SendStatus.Success == await SendTextMessage(
                                                    ocppTextMessage,
                                                    JSONRequestMessage.EventTrackingId,
                                                    JSONRequestMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(RequestMessage.RequestId,
                    //                SendRequestState.FromJSONRequest(
                    //                    Timestamp.Now,
                    //                    RequestMessage.DestinationId,
                    //                    RequestMessage.RequestTimeout ?? (RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout)),
                    //                    RequestMessage
                    //                ));

                    #region OnJSONMessageRequestSent

                    //var onJSONMessageRequestSent = OnJSONMessageRequestSent;
                    //if (onJSONMessageRequestSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onJSONMessageRequestSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnJSONMessageRequestSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendJSONResponse      (JSONResponseMessage)

        /// <summary>
        /// Send (and forget) the given JSON OCPP request message.
        /// </summary>
        /// <param name="JSONResponseMessage">A JSON OCPP request message.</param>
        public async Task<SendMessageResult> SendJSONResponse(OCPP_JSONResponseMessage JSONResponseMessage)
        {

            try
            {

                JSONResponseMessage.NetworkingMode = NetworkingMode;
                //ResponseMessage.ResponseTimeout ??= ResponseMessage.ResponseTimestamp + (ResponseTimeout ?? DefaultResponseTimeout);

                var ocppTextMessage = JSONResponseMessage.ToJSON().ToString(Formatting.None);

                if (SendStatus.Success == await SendTextMessage(
                                                    ocppTextMessage,
                                                    JSONResponseMessage.EventTrackingId,
                                                    JSONResponseMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(ResponseMessage.ResponseId,
                    //                SendResponseState.FromJSONResponse(
                    //                    Timestamp.Now,
                    //                    ResponseMessage.DestinationId,
                    //                    ResponseMessage.ResponseTimeout ?? (ResponseMessage.ResponseTimestamp + (ResponseTimeout ?? DefaultResponseTimeout)),
                    //                    ResponseMessage
                    //                ));

                    #region OnJSONMessageResponseSent

                    //var onJSONMessageResponseSent = OnJSONMessageResponseSent;
                    //if (onJSONMessageResponseSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onJSONMessageResponseSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnJSONMessageResponseSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendJSONRequestError  (JSONRequestErrorMessage)

        /// <summary>
        /// Send (and forget) the given JSON OCPP request message.
        /// </summary>
        /// <param name="JSONRequestErrorMessage">A JSON OCPP request message.</param>
        public async Task<SendMessageResult> SendJSONRequestError(OCPP_JSONRequestErrorMessage JSONRequestErrorMessage)
        {

            try
            {

                JSONRequestErrorMessage.NetworkingMode = NetworkingMode;
                //ErrorMessage.ErrorTimeout ??= ErrorMessage.ErrorTimestamp + (ErrorTimeout ?? DefaultErrorTimeout);

                var ocppTextMessage = JSONRequestErrorMessage.ToJSON().ToString(Formatting.None);

                if (SendStatus.Success == await SendTextMessage(
                                                    ocppTextMessage,
                                                    JSONRequestErrorMessage.EventTrackingId,
                                                    JSONRequestErrorMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(ErrorMessage.ErrorId,
                    //                SendErrorState.FromJSONError(
                    //                    Timestamp.Now,
                    //                    ErrorMessage.DestinationId,
                    //                    ErrorMessage.ErrorTimeout ?? (ErrorMessage.ErrorTimestamp + (ErrorTimeout ?? DefaultErrorTimeout)),
                    //                    ErrorMessage
                    //                ));

                    #region OnJSONMessageErrorSent

                    //var onJSONMessageErrorSent = OnJSONMessageErrorSent;
                    //if (onJSONMessageErrorSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onJSONMessageErrorSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnJSONMessageErrorSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendJSONResponseError (JSONResponseErrorMessage)

        /// <summary>
        /// Send (and forget) the given JSON OCPP response message.
        /// </summary>
        /// <param name="JSONResponseErrorMessage">A JSON OCPP response message.</param>
        public async Task<SendMessageResult> SendJSONResponseError(OCPP_JSONResponseErrorMessage JSONResponseErrorMessage)
        {

            try
            {

                JSONResponseErrorMessage.NetworkingMode = NetworkingMode;
                //ErrorMessage.ErrorTimeout ??= ErrorMessage.ErrorTimestamp + (ErrorTimeout ?? DefaultErrorTimeout);

                var ocppTextMessage = JSONResponseErrorMessage.ToJSON().ToString(Formatting.None);

                if (SendStatus.Success == await SendTextMessage(
                                                    ocppTextMessage,
                                                    JSONResponseErrorMessage.EventTrackingId,
                                                    JSONResponseErrorMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(ErrorMessage.ErrorId,
                    //                SendErrorState.FromJSONError(
                    //                    Timestamp.Now,
                    //                    ErrorMessage.DestinationId,
                    //                    ErrorMessage.ErrorTimeout ?? (ErrorMessage.ErrorTimestamp + (ErrorTimeout ?? DefaultErrorTimeout)),
                    //                    ErrorMessage
                    //                ));

                    #region OnJSONMessageErrorSent

                    //var onJSONMessageErrorSent = OnJSONMessageErrorSent;
                    //if (onJSONMessageErrorSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onJSONMessageErrorSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnJSONMessageErrorSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion


        #region SendBinaryRequest     (BinaryRequestMessage)

        /// <summary>
        /// Send (and forget) the given binary OCPP request message.
        /// </summary>
        /// <param name="BinaryRequestMessage">A binary OCPP request message.</param>
        public async Task<SendMessageResult> SendBinaryRequest(OCPP_BinaryRequestMessage BinaryRequestMessage)
        {

            try
            {

                BinaryRequestMessage.NetworkingMode = NetworkingMode;
                //RequestMessage.RequestTimeout ??= RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout);

                var ocppBinaryMessage = BinaryRequestMessage.ToByteArray();

                if (SendStatus.Success == await SendBinaryMessage(
                                                    ocppBinaryMessage,
                                                    BinaryRequestMessage.EventTrackingId,
                                                    BinaryRequestMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(RequestMessage.RequestId,
                    //                SendRequestState.FromJSONRequest(
                    //                    Timestamp.Now,
                    //                    RequestMessage.DestinationId,
                    //                    RequestMessage.RequestTimeout ?? (RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout)),
                    //                    RequestMessage
                    //                ));

                    #region OnBinaryMessageRequestSent

                    //var onBinaryMessageRequestSent = OnBinaryMessageRequestSent;
                    //if (onBinaryMessageRequestSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onBinaryMessageRequestSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnBinaryMessageRequestSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendBinaryResponse    (BinaryResponseMessage)

        /// <summary>
        /// Send (and forget) the given binary OCPP request message.
        /// </summary>
        /// <param name="BinaryResponseMessage">A binary OCPP request message.</param>
        public async Task<SendMessageResult> SendBinaryResponse(OCPP_BinaryResponseMessage BinaryResponseMessage)
        {

            try
            {

                BinaryResponseMessage.NetworkingMode = NetworkingMode;
                //ResponseMessage.ResponseTimeout ??= ResponseMessage.ResponseTimestamp + (ResponseTimeout ?? DefaultResponseTimeout);

                var ocppBinaryMessage = BinaryResponseMessage.ToByteArray();

                if (SendStatus.Success == await SendBinaryMessage(
                                                    ocppBinaryMessage,
                                                    BinaryResponseMessage.EventTrackingId,
                                                    BinaryResponseMessage.CancellationToken
                                                ))
                {

                    //requests.TryAdd(ResponseMessage.ResponseId,
                    //                SendResponseState.FromJSONResponse(
                    //                    Timestamp.Now,
                    //                    ResponseMessage.DestinationId,
                    //                    ResponseMessage.ResponseTimeout ?? (ResponseMessage.ResponseTimestamp + (ResponseTimeout ?? DefaultResponseTimeout)),
                    //                    ResponseMessage
                    //                ));

                    #region OnBinaryMessageResponseSent

                    //var onBinaryMessageResponseSent = OnBinaryMessageResponseSent;
                    //if (onBinaryMessageResponseSent is not null)
                    //{
                    //    try
                    //    {

                    //        await Task.WhenAll(onBinaryMessageResponseSent.GetInvocationList().
                    //                               OfType<OnWebSocketTextMessageDelegate>().
                    //                               Select(loggingDelegate => loggingDelegate.Invoke(
                    //                                                              Timestamp.Now,
                    //                                                              this,
                    //                                                              webSocketConnection.Item1,
                    //                                                              EventTrackingId,
                    //                                                              ocppTextMessage,
                    //                                                              CancellationToken
                    //                                                          )).
                    //                               ToArray());

                    //    }
                    //    catch (Exception e)
                    //    {
                    //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnBinaryMessageResponseSent));
                    //    }
                    //}

                    #endregion

                }

                return SendMessageResult.Success;

            }
            catch (Exception)
            {
                return SendMessageResult.TransmissionFailed;
            }

        }

        #endregion


    }

}
