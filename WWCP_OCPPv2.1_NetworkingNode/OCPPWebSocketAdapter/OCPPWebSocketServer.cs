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

using System.Security.Authentication;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod;
using org.GraphDefined.Vanaheimr.Hermod.DNS;
using org.GraphDefined.Vanaheimr.Hermod.WebSocket;
using org.GraphDefined.Vanaheimr.Hermod.Sockets;
using org.GraphDefined.Vanaheimr.Hermod.Sockets.TCP;

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.CSMS;
using cloud.charging.open.protocols.OCPP.WebSockets;
using System.Collections.Concurrent;
using System.Reflection;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;
using Newtonsoft.Json;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    #region Common Connection Management

    /// <summary>
    /// A delegate for logging new HTTP Web Socket connections.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="NetworkingNodeChannel">The HTTP Web Socket channel.</param>
    /// <param name="NewConnection">The new HTTP Web Socket connection.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="EventTrackingId">The event tracking identification for correlating this request with other events.</param>
    /// <param name="SharedSubprotocols">An enumeration of shared HTTP Web Sockets subprotocols.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeNewWebSocketConnectionDelegate        (DateTime                           Timestamp,
                                                                                OCPPWebSocketServer                NetworkingNodeChannel,
                                                                                WebSocketServerConnection          NewConnection,
                                                                                NetworkingNode_Id                  NetworkingNodeId,
                                                                                EventTracking_Id                   EventTrackingId,
                                                                                IEnumerable<String>                SharedSubprotocols,
                                                                                CancellationToken                  CancellationToken);

    /// <summary>
    /// A delegate for logging a HTTP Web Socket CLOSE message.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="NetworkingNodeChannel">The HTTP Web Socket channel.</param>
    /// <param name="Connection">The HTTP Web Socket connection to be closed.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="EventTrackingId">The event tracking identification for correlating this request with other events.</param>
    /// <param name="StatusCode">The HTTP Web Socket Closing Status Code.</param>
    /// <param name="Reason">An optional HTTP Web Socket closing reason.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeCloseMessageReceivedDelegate          (DateTime                           Timestamp,
                                                                                OCPPWebSocketServer                NetworkingNodeChannel,
                                                                                WebSocketServerConnection          Connection,
                                                                                NetworkingNode_Id                  NetworkingNodeId,
                                                                                EventTracking_Id                   EventTrackingId,
                                                                                WebSocketFrame.ClosingStatusCode   StatusCode,
                                                                                String?                            Reason,
                                                                                CancellationToken                  CancellationToken);

    /// <summary>
    /// A delegate for logging a closed TCP connection.
    /// </summary>
    /// <param name="Timestamp">The logging timestamp.</param>
    /// <param name="NetworkingNodeChannel">The HTTP Web Socket channel.</param>
    /// <param name="Connection">The HTTP Web Socket connection to be closed.</param>
    /// <param name="NetworkingNodeId">The sending OCPP networking node/charging station identification.</param>
    /// <param name="EventTrackingId">The event tracking identification for correlating this request with other events.</param>
    /// <param name="Reason">An optional closing reason.</param>
    /// <param name="CancellationToken">A token to cancel the processing.</param>
    public delegate Task OnNetworkingNodeTCPConnectionClosedDelegate           (DateTime                           Timestamp,
                                                                                OCPPWebSocketServer                NetworkingNodeChannel,
                                                                                WebSocketServerConnection          Connection,
                                                                                NetworkingNode_Id                  NetworkingNodeId,
                                                                                EventTracking_Id                   EventTrackingId,
                                                                                String?                            Reason,
                                                                                CancellationToken                  CancellationToken);

    #endregion


    /// <summary>
    /// The OCPP HTTP Web Socket server.
    /// </summary>
    public partial class OCPPWebSocketServer : WebSocketServer
    {

        #region Data

        /// <summary>
        /// The default HTTP server name.
        /// </summary>
        public const            String                                                                                DefaultHTTPServiceName            = $"GraphDefined OCPP {Version.String} HTTP/WebSocket/JSON CSMS API";

        /// <summary>
        /// The default HTTP server TCP port.
        /// </summary>
        public static readonly  IPPort                                                                                DefaultHTTPServerPort             = IPPort.Parse(2010);

        /// <summary>
        /// The default HTTP server URI prefix.
        /// </summary>
        public static readonly  HTTPPath                                                                              DefaultURLPrefix                  = HTTPPath.Parse("/" + Version.String);

        /// <summary>
        /// The default request timeout.
        /// </summary>
        public static readonly  TimeSpan                                                                              DefaultRequestTimeout             = TimeSpan.FromSeconds(30);

        protected readonly      Dictionary<String, MethodInfo>                                                        incomingMessageProcessorsLookup   = [];
        protected readonly      ConcurrentDictionary<NetworkingNode_Id, Tuple<WebSocketServerConnection, DateTime>>   connectedNetworkingNodes          = [];
        protected readonly      ConcurrentDictionary<NetworkingNode_Id, NetworkingNode_Id>                            reachableViaNetworkingHubs        = [];
        protected readonly      ConcurrentDictionary<Request_Id, SendRequestState>                                    requests                          = [];

        public    const         String                                                                                networkingNodeId_WebSocketKey     = "networkingNodeId";
        public    const         String                                                                                networkingMode_WebSocketKey       = "networkingMode";
        public    const         String                                                                                LogfileName                       = "CSMSWSServer.log";

        #endregion

        #region Properties

        public IOCPPWebSocketAdapterIN   OCPPWebSocketAdapterIN     { get; }
        public IOCPPWebSocketAdapterOUT  OCPPWebSocketAdapterOUT    { get; }

        /// <summary>
        /// The enumeration of all connected networking nodes.
        /// </summary>
        public IEnumerable<NetworkingNode_Id> NetworkingNodeIds
            => connectedNetworkingNodes.Keys;

        /// <summary>
        /// Require a HTTP Basic Authentication of all networking nodes.
        /// </summary>
        public Boolean                                            RequireAuthentication    { get; }

        /// <summary>
        /// Logins and passwords for HTTP Basic Authentication.
        /// </summary>
        public ConcurrentDictionary<NetworkingNode_Id, String?>   NetworkingNodeLogins     { get; }
            = new();

        /// <summary>
        /// The JSON formatting to use.
        /// </summary>
        public Formatting                                         JSONFormatting           { get; set; }
            = Formatting.None;

        /// <summary>
        /// The request timeout for messages sent by this HTTP WebSocket server.
        /// </summary>
        public TimeSpan?                                          RequestTimeout           { get; set; }

        #endregion

        #region Events

        #region Common Connection Management

        /// <summary>
        /// An event sent whenever the HTTP connection switched successfully to web socket.
        /// </summary>
        public event OnNetworkingNodeNewWebSocketConnectionDelegate?    OnNetworkingNodeNewWebSocketConnection;

        /// <summary>
        /// An event sent whenever a web socket close frame was received.
        /// </summary>
        public event OnNetworkingNodeCloseMessageReceivedDelegate?      OnNetworkingNodeCloseMessageReceived;

        /// <summary>
        /// An event sent whenever a TCP connection was closed.
        /// </summary>
        public event OnNetworkingNodeTCPConnectionClosedDelegate?       OnNetworkingNodeTCPConnectionClosed;

        #endregion

        #region Generic JSON Messages

        /// <summary>
        /// An event sent whenever a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a text message was sent.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a text message was sent.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?      OnJSONErrorResponseSent;


        /// <summary>
        /// An event sent whenever a text message request was sent.
        /// </summary>
        public event OnWebSocketJSONMessageRequestDelegate?     OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a text message request was received.
        /// </summary>
        public event OnWebSocketJSONMessageResponseDelegate?    OnJSONMessageResponseReceived;

        /// <summary>
        /// An event sent whenever an error response to a text message request was received.
        /// </summary>
        public event OnWebSocketTextErrorResponseDelegate?      OnJSONErrorResponseReceived;

        #endregion

        #region Generic Binary Messages

        /// <summary>
        /// An event sent whenever a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?     OnBinaryMessageRequestReceived;

        /// <summary>
        /// An event sent whenever the response to a binary message was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?    OnBinaryMessageResponseSent;

        /// <summary>
        /// An event sent whenever the error response to a binary message was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseSent;


        /// <summary>
        /// An event sent whenever a binary message request was sent.
        /// </summary>
        public event OnWebSocketBinaryMessageRequestDelegate?     OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever the response to a binary message request was received.
        /// </summary>
        public event OnWebSocketBinaryMessageResponseDelegate?    OnBinaryMessageResponseReceived;

        /// <summary>
        /// An event sent whenever the error response to a binary message request was sent.
        /// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?      OnBinaryErrorResponseReceived;

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCPP HTTP Web Socket server.
        /// </summary>
        /// <param name="NetworkingNodeIdentity">The unique identification of this networking node.</param>
        /// 
        /// <param name="HTTPServiceName">An optional identification string for the HTTP service.</param>
        /// <param name="IPAddress">An IP address to listen on.</param>
        /// <param name="TCPPort">An optional TCP port for the HTTP server.</param>
        /// <param name="RequireAuthentication">Require a HTTP Basic Authentication of all charging boxes.</param>
        /// <param name="DNSClient">An optional DNS client to use.</param>
        /// <param name="AutoStart">Start the server immediately.</param>
        public OCPPWebSocketServer(IOCPPWebSocketAdapterIN              OCPPWebSocketAdapterIN,
                                   IOCPPWebSocketAdapterOUT             OCPPWebSocketAdapterOUT,

                                   String                               HTTPServiceName              = DefaultHTTPServiceName,
                                   IIPAddress?                          IPAddress                    = null,
                                   IPPort?                              TCPPort                      = null,

                                   Boolean                              RequireAuthentication        = true,
                                   Boolean                              DisableWebSocketPings        = false,
                                   TimeSpan?                            WebSocketPingEvery           = null,
                                   TimeSpan?                            SlowNetworkSimulationDelay   = null,

                                   ServerCertificateSelectorDelegate?   ServerCertificateSelector    = null,
                                   RemoteCertificateValidationHandler?  ClientCertificateValidator   = null,
                                   LocalCertificateSelectionHandler?    ClientCertificateSelector    = null,
                                   SslProtocols?                        AllowedTLSProtocols          = null,
                                   Boolean?                             ClientCertificateRequired    = null,
                                   Boolean?                             CheckCertificateRevocation   = null,

                                   ServerThreadNameCreatorDelegate?     ServerThreadNameCreator      = null,
                                   ServerThreadPriorityDelegate?        ServerThreadPrioritySetter   = null,
                                   Boolean?                             ServerThreadIsBackground     = null,
                                   ConnectionIdBuilder?                 ConnectionIdBuilder          = null,
                                   TimeSpan?                            ConnectionTimeout            = null,
                                   UInt32?                              MaxClientConnections         = null,

                                   DNSClient?                           DNSClient                    = null,
                                   Boolean                              AutoStart                    = false)

            : base(IPAddress,
                   TCPPort ?? IPPort.Parse(8000),
                   HTTPServiceName,

                   new[] {
                      "ocpp2.0.1",
                       Version.WebSocketSubProtocolId
                   },
                   DisableWebSocketPings,
                   WebSocketPingEvery,
                   SlowNetworkSimulationDelay,

                   ServerCertificateSelector,
                   ClientCertificateValidator,
                   ClientCertificateSelector,
                   AllowedTLSProtocols,
                   ClientCertificateRequired,
                   CheckCertificateRevocation,

                   ServerThreadNameCreator,
                   ServerThreadPrioritySetter,
                   ServerThreadIsBackground,
                   ConnectionIdBuilder,
                   ConnectionTimeout,
                   MaxClientConnections,

                   DNSClient,
                   false)

        {

            this.OCPPWebSocketAdapterIN   = OCPPWebSocketAdapterIN;
            this.OCPPWebSocketAdapterOUT  = OCPPWebSocketAdapterOUT;

            this.RequireAuthentication    = RequireAuthentication;

            //this.Logger          = new ChargePointwebsocketClient.CPClientLogger(this,
            //                                                                LoggingPath,
            //                                                                LoggingContext,
            //                                                                LogfileCreator);

            base.OnValidateTCPConnection        += ValidateTCPConnection;
            base.OnValidateWebSocketConnection  += ValidateWebSocketConnection;
            base.OnNewWebSocketConnection       += ProcessNewWebSocketConnection;
            base.OnCloseMessageReceived         += ProcessCloseMessage;

            if (AutoStart)
                Start();

        }

        #endregion


        #region AddOrUpdateHTTPBasicAuth(NetworkingNodeId, Password)

        /// <summary>
        /// Add the given HTTP Basic Authentication password for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        /// <param name="Password">The password of the charging station.</param>
        public void AddOrUpdateHTTPBasicAuth(NetworkingNode_Id  NetworkingNodeId,
                                             String             Password)
        {

            NetworkingNodeLogins.AddOrUpdate(NetworkingNodeId,
                                             Password,
                                             (chargingStationId, password) => Password);

        }

        #endregion

        #region RemoveHTTPBasicAuth     (NetworkingNodeId)

        /// <summary>
        /// Remove the given HTTP Basic Authentication for the given networking node.
        /// </summary>
        /// <param name="NetworkingNodeId">The unique identification of the networking node.</param>
        public Boolean RemoveHTTPBasicAuth(NetworkingNode_Id NetworkingNodeId)
        {

            if (NetworkingNodeLogins.ContainsKey(NetworkingNodeId))
                return NetworkingNodeLogins.TryRemove(NetworkingNodeId, out _);

            return true;

        }

        #endregion


        // Connection management...

        #region (protected) ValidateTCPConnection        (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<ConnectionFilterResponse> ValidateTCPConnection(DateTime                      LogTimestamp,
                                                                     IWebSocketServer              Server,
                                                                     System.Net.Sockets.TcpClient  Connection,
                                                                     EventTracking_Id              EventTrackingId,
                                                                     CancellationToken             CancellationToken)
        {

            return Task.FromResult(ConnectionFilterResponse.Accepted());

        }

        #endregion

        #region (protected) ValidateWebSocketConnection  (LogTimestamp, Server, Connection, EventTrackingId, CancellationToken)

        private Task<HTTPResponse?> ValidateWebSocketConnection(DateTime                   LogTimestamp,
                                                                IWebSocketServer           Server,
                                                                WebSocketServerConnection  Connection,
                                                                EventTracking_Id           EventTrackingId,
                                                                CancellationToken          CancellationToken)
        {

            #region Verify 'Sec-WebSocket-Protocol'...

            if (Connection.HTTPRequest?.SecWebSocketProtocol is null ||
                Connection.HTTPRequest?.SecWebSocketProtocol.Any() == false)
            {

                DebugX.Log("Missing 'Sec-WebSocket-Protocol' HTTP header!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                     JSONObject.Create(
                                                         new JProperty("en", "Missing 'Sec-WebSocket-Protocol' HTTP header!")
                                                     ))).ToUTF8Bytes(),
                               Connection      = "close"
                           }.AsImmutable);

            }
            else if (!SecWebSocketProtocols.Overlaps(Connection.HTTPRequest?.SecWebSocketProtocol ?? Array.Empty<String>()))
            {

                var error = $"This WebSocket service only supports {(SecWebSocketProtocols.Select(id => $"'{id}'").AggregateWith(", "))}!";

                DebugX.Log(error);

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.BadRequest,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               ContentType     = HTTPContentType.Application.JSON_UTF8,
                               Content         = JSONObject.Create(
                                                     new JProperty("description",
                                                         JSONObject.Create(
                                                             new JProperty("en", error)
                                                     ))).ToUTF8Bytes(),
                               Connection      = "close"
                           }.AsImmutable);

            }

            #endregion

            #region Verify HTTP Authentication

            if (RequireAuthentication)
            {

                if (Connection.HTTPRequest?.Authorization is HTTPBasicAuthentication basicAuthentication)
                {

                    if (NetworkingNodeLogins.TryGetValue(NetworkingNode_Id.Parse(basicAuthentication.Username), out var password) &&
                        basicAuthentication.Password == password)
                    {
                        DebugX.Log(nameof(OCPPWebSocketServer), " connection from " + Connection.RemoteSocket + " using authorization: " + basicAuthentication.Username + "/" + basicAuthentication.Password);
                        return Task.FromResult<HTTPResponse?>(null);
                    }
                    else
                        DebugX.Log(nameof(OCPPWebSocketServer), " connection from " + Connection.RemoteSocket + " invalid authorization: " + basicAuthentication.Username + "/" + basicAuthentication.Password);

                }
                else
                    DebugX.Log(nameof(OCPPWebSocketServer), " connection from " + Connection.RemoteSocket + " missing authorization!");

                return Task.FromResult<HTTPResponse?>(
                           new HTTPResponse.Builder() {
                               HTTPStatusCode  = HTTPStatusCode.Unauthorized,
                               Server          = HTTPServiceName,
                               Date            = Timestamp.Now,
                               Connection      = "close"
                           }.AsImmutable);

            }

            #endregion

            return Task.FromResult<HTTPResponse?>(null);

        }

        #endregion

        #region (protected) ProcessNewWebSocketConnection(LogTimestamp, Server, Connection, EventTrackingId, SharedSubprotocols, CancellationToken)

        protected async Task ProcessNewWebSocketConnection(DateTime                   LogTimestamp,
                                                           IWebSocketServer           Server,
                                                           WebSocketServerConnection  Connection,
                                                           EventTracking_Id           EventTrackingId,
                                                           IEnumerable<String>        SharedSubprotocols,
                                                           CancellationToken          CancellationToken)
        {

            #region Store the networking node/charging station identification within the Web Socket connection

            if (!Connection.TryGetCustomDataAs(networkingNodeId_WebSocketKey, out NetworkingNode_Id networkingNodeId) &&
                 Connection.HTTPRequest is not null)
            {

                //ToDo: TLS certificates

                #region HTTP Basic Authentication is used

                if (Connection.HTTPRequest.Authorization is HTTPBasicAuthentication httpBasicAuthentication)
                {

                    if (NetworkingNode_Id.TryParse(httpBasicAuthentication.Username, out networkingNodeId))
                    {

                        // Add the networking node/charging station identification to the Web Socket connection
                        Connection.TryAddCustomData(networkingNodeId_WebSocketKey, networkingNodeId);

                        if (!connectedNetworkingNodes.TryGetValue(networkingNodeId, out var value))
                            connectedNetworkingNodes.TryAdd(networkingNodeId, new Tuple<WebSocketServerConnection, DateTime>(Connection, Timestamp.Now));

                        else
                        {

                            DebugX.Log($"{nameof(OCPPWebSocketServer)} Duplicate networking node '{networkingNodeId}' detected!");

                            var oldNetworkingNode_WebSocketConnection = value.Item1;

                            connectedNetworkingNodes.TryRemove(networkingNodeId, out _);
                            connectedNetworkingNodes.TryAdd   (networkingNodeId, new Tuple<WebSocketServerConnection, DateTime>(Connection, Timestamp.Now));

                            try
                            {
                                oldNetworkingNode_WebSocketConnection.Close();
                            }
                            catch (Exception e)
                            {
                                DebugX.Log($"{nameof(OCPPWebSocketServer)} Closing old HTTP WebSocket connection failed: {e.Message}");
                            }

                        }

                    }

                }

                #endregion

                #region No authentication at all...

                else if (NetworkingNode_Id.TryParse(Connection.HTTPRequest.Path.ToString()[(Connection.HTTPRequest.Path.ToString().LastIndexOf("/") + 1)..], out networkingNodeId))
                {

                    // Add the charging station identification to the WebSocket connection
                    Connection.TryAddCustomData(networkingNodeId_WebSocketKey, networkingNodeId);

                    if (!connectedNetworkingNodes.TryGetValue(networkingNodeId, out Tuple<WebSocketServerConnection, DateTime>? value))
                         connectedNetworkingNodes.TryAdd(networkingNodeId, new Tuple<WebSocketServerConnection, DateTime>(Connection, Timestamp.Now));

                    else
                    {

                        DebugX.Log($"{nameof(OCPPWebSocketServer)} Duplicate charging station '{networkingNodeId}' detected!");

                        var oldChargingStation_WebSocketConnection = value.Item1;

                        connectedNetworkingNodes.TryRemove(networkingNodeId, out _);
                        connectedNetworkingNodes.TryAdd   (networkingNodeId, new Tuple<WebSocketServerConnection, DateTime>(Connection, Timestamp.Now));

                        try
                        {
                            oldChargingStation_WebSocketConnection.Close();
                        }
                        catch (Exception e)
                        {
                            DebugX.Log($"{nameof(OCPPWebSocketServer)} Closing old HTTP WebSocket connection failed: {e.Message}");
                        }

                    }

                }

                #endregion

            }

            #endregion

            #region Send OnNewNetworkingNodeWSConnection event

            var logger = OnNetworkingNodeNewWebSocketConnection;
            if (logger is not null)
            {

                var loggerTasks = logger.GetInvocationList().
                                         OfType <OnNetworkingNodeNewWebSocketConnectionDelegate>().
                                         Select (loggingDelegate => loggingDelegate.Invoke(LogTimestamp,
                                                                                           this,
                                                                                           Connection,
                                                                                           networkingNodeId,
                                                                                           EventTrackingId,
                                                                                           SharedSubprotocols,
                                                                                           CancellationToken)).
                                         ToArray();

                try
                {
                    await Task.WhenAll(loggerTasks);
                }
                catch (Exception e)
                {
                    await HandleErrors(
                              nameof(OCPPWebSocketServer),
                              nameof(OnNetworkingNodeNewWebSocketConnection),
                              e
                          );
                }

            }

            #endregion

        }

        #endregion

        #region (protected) ProcessCloseMessage          (LogTimestamp, Server, Connection, EventTrackingId, StatusCode, Reason, CancellationToken)

        protected async Task ProcessCloseMessage(DateTime                          LogTimestamp,
                                                 IWebSocketServer                  Server,
                                                 WebSocketServerConnection         Connection,
                                                 EventTracking_Id                  EventTrackingId,
                                                 WebSocketFrame.ClosingStatusCode  StatusCode,
                                                 String?                           Reason,
                                                 CancellationToken                 CancellationToken)
        {

            if (Connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey, out var networkingNodeId))
            {

                connectedNetworkingNodes.TryRemove(networkingNodeId, out _);

                #region Send OnNetworkingNodeCloseMessageReceived event

                var logger = OnNetworkingNodeCloseMessageReceived;
                if (logger is not null)
                {

                    var loggerTasks = logger.GetInvocationList().
                                             OfType <OnNetworkingNodeCloseMessageReceivedDelegate>().
                                             Select (loggingDelegate => loggingDelegate.Invoke(LogTimestamp,
                                                                                               this,
                                                                                               Connection,
                                                                                               networkingNodeId,
                                                                                               EventTrackingId,
                                                                                               StatusCode,
                                                                                               Reason,
                                                                                               CancellationToken)).
                                             ToArray();

                    try
                    {
                        await Task.WhenAll(loggerTasks);
                    }
                    catch (Exception e)
                    {
                        await HandleErrors(
                                  nameof(OCPPWebSocketServer),
                                  nameof(OnNetworkingNodeCloseMessageReceived),
                                  e
                              );
                    }

                }

                #endregion

            }

        }

        #endregion


        // Receive data...

        #region (protected) ProcessTextMessage  (RequestTimestamp, ServerConnection, TextMessage,   EventTrackingId, CancellationToken)

        /// <summary>
        /// Process all text messages of this WebSocket API.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="ServerConnection">The WebSocket connection.</param>
        /// <param name="TextMessage">The received text message.</param>
        /// <param name="EventTrackingId">The event tracking identification.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public override async Task<WebSocketTextMessageResponse> ProcessTextMessage(DateTime                   RequestTimestamp,
                                                                                    WebSocketServerConnection  ServerConnection,
                                                                                    String                     TextMessage,
                                                                                    EventTracking_Id           EventTrackingId,
                                                                                    CancellationToken          CancellationToken)
        {

            OCPP_JSONResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?     OCPPErrorResponse   = null;

            try
            {

                var jsonArray            = JArray.Parse(TextMessage);
                var sourceNodeId         = ServerConnection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey);

                var textMessageResponse  = await OCPPWebSocketAdapterIN.ProcessJSONMessage(
                                                     RequestTimestamp,
                                                     ServerConnection,
                                                     jsonArray,
                                                     EventTrackingId,
                                                     CancellationToken
                                                 );

                return textMessageResponse;

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.InternalError(
                                        nameof(OCPPWebSocketServer),
                                        EventTrackingId,
                                        TextMessage,
                                        e
                                    );

            }

            return null;

        }

        #endregion

        #region (protected) ProcessBinaryMessage(RequestTimestamp, ServerConnection, BinaryMessage, EventTrackingId, CancellationToken)

        /// <summary>
        /// Process all text messages of this WebSocket API.
        /// </summary>
        /// <param name="RequestTimestamp">The timestamp of the request.</param>
        /// <param name="Connection">The WebSocket connection.</param>
        /// <param name="BinaryMessage">The received binary message.</param>
        /// <param name="EventTrackingId">The event tracking identification.</param>
        /// <param name="CancellationToken">The cancellation token.</param>
        public override async Task<WebSocketBinaryMessageResponse> ProcessBinaryMessage(DateTime                   RequestTimestamp,
                                                                                        WebSocketServerConnection  ServerConnection,
                                                                                        Byte[]                     BinaryMessage,
                                                                                        EventTracking_Id           EventTrackingId,
                                                                                        CancellationToken          CancellationToken)
        {

            OCPP_BinaryResponseMessage?  OCPPResponse        = null;
            OCPP_JSONErrorMessage?       OCPPErrorResponse   = null;

            try
            {

                var sourceNodeId  = ServerConnection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey);

                var textMessageResponse = await OCPPWebSocketAdapterIN.ProcessBinaryMessage(RequestTimestamp,
                                                                                            ServerConnection,
                                                                                            BinaryMessage,
                                                                                            EventTrackingId,
                                                                                            CancellationToken);

                return textMessageResponse;

            }
            catch (Exception e)
            {

                OCPPErrorResponse = OCPP_JSONErrorMessage.InternalError(
                                        nameof(OCPPWebSocketServer),
                                        EventTrackingId,
                                        BinaryMessage,
                                        e
                                    );

            }

            return null;

        }

        #endregion




        private IEnumerable<Tuple<WebSocketServerConnection, NetworkingMode>> LookupNetworkingNode(NetworkingNode_Id NetworkingNodeId)
        {

            if (NetworkingNodeId == NetworkingNode_Id.Zero)
                return Array.Empty<Tuple<WebSocketServerConnection, NetworkingMode>>();

            var lookUpNetworkingNodeId = NetworkingNodeId;

            if (reachableViaNetworkingHubs.TryGetValue(lookUpNetworkingNodeId, out var networkingHubId))
            {
                lookUpNetworkingNodeId = networkingHubId;
                return WebSocketConnections.Where(connection => connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) == lookUpNetworkingNodeId).
                    Select(x => new Tuple<WebSocketServerConnection, NetworkingMode>(x, NetworkingMode.NetworkingExtensions));
            }

            return WebSocketConnections.Where (connection => connection.TryGetCustomDataAs<NetworkingNode_Id>(networkingNodeId_WebSocketKey) == lookUpNetworkingNodeId).
                                        Select(x => new Tuple<WebSocketServerConnection, NetworkingMode>(x, NetworkingMode.Standard));

        }

        public void AddStaticRouting(NetworkingNode_Id DestinationNodeId,
                                     NetworkingNode_Id NetworkingHubId)
        {

            reachableViaNetworkingHubs.TryAdd(DestinationNodeId,
                                              NetworkingHubId);

        }

        public void RemoveStaticRouting(NetworkingNode_Id DestinationNodeId,
                                        NetworkingNode_Id NetworkingHubId)
        {

            reachableViaNetworkingHubs.TryRemove(new KeyValuePair<NetworkingNode_Id, NetworkingNode_Id>(DestinationNodeId, NetworkingHubId));

        }


        // Send data...

        #region SendJSONRequest  (RequestMessage)

        /// <summary>
        /// Send (and forget) the given JSON OCPP request message.
        /// </summary>
        /// <param name="RequestMessage">A JSON OCPP request message.</param>
        public async Task<SendOCPPMessageResult> SendJSONRequest(OCPP_JSONRequestMessage RequestMessage)
        {

            try
            {

                var webSocketConnections  = LookupNetworkingNode(RequestMessage.DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode              = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    RequestMessage.NetworkingMode   = webSocketConnections.First().Item2;
                    //RequestMessage.RequestTimeout ??= RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout);

                    var ocppTextMessage             = RequestMessage.ToJSON().ToString(Formatting.None);


                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SendStatus.Success == await SendTextMessage(
                                                            webSocketConnection.Item1,
                                                            ocppTextMessage,
                                                            RequestMessage.EventTrackingId,
                                                            RequestMessage.CancellationToken
                                                        ))
                        {

                            //requests.TryAdd(RequestMessage.RequestId,
                            //                SendRequestState.FromJSONRequest(
                            //                    Timestamp.Now,
                            //                    RequestMessage.DestinationNodeId,
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

                            break;

                        }

                        RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendOCPPMessageResult.Success;

                }
                else
                    return SendOCPPMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendOCPPMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendBinaryRequest(RequestMessage)

        /// <summary>
        /// Send (and forget) the given binary OCPP request message.
        /// </summary>
        /// <param name="RequestMessage">A binary OCPP request message.</param>
        public async Task<SendOCPPMessageResult> SendBinaryRequest(OCPP_BinaryRequestMessage RequestMessage)
        {

            try
            {

                var webSocketConnections  = LookupNetworkingNode(RequestMessage.DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode              = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    RequestMessage.NetworkingMode   = webSocketConnections.First().Item2;
                    //RequestMessage.RequestTimeout ??= RequestMessage.RequestTimestamp + (RequestTimeout ?? DefaultRequestTimeout);

                    var ocppBinaryMessage           = RequestMessage.ToByteArray();


                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SendStatus.Success == await SendBinaryMessage(
                                                            webSocketConnection.Item1,
                                                            ocppBinaryMessage,
                                                            RequestMessage.EventTrackingId,
                                                            RequestMessage.CancellationToken
                                                        ))
                        {

                            //requests.TryAdd(RequestMessage.RequestId,
                            //                SendRequestState.FromJSONRequest(
                            //                    Timestamp.Now,
                            //                    RequestMessage.DestinationNodeId,
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

                            break;

                        }

                        RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendOCPPMessageResult.Success;

                }
                else
                    return SendOCPPMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendOCPPMessageResult.TransmissionFailed;
            }

        }

        #endregion


        #region SendJSONData     (EventTrackingId, DestinationNodeId, NetworkPath, RequestId, Action, JSONData,   RequestTimeout, ...)

        /// <summary>
        /// Send (and forget) the given JSON.
        /// </summary>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="DestinationNodeId">The networking node identification of the message destination.</param>
        /// <param name="NetworkPath">The network path.</param>
        /// <param name="RequestId">A unique request identification.</param>
        /// <param name="Action">An OCPP action.</param>
        /// <param name="JSONData">The JSON payload.</param>
        /// <param name="RequestTimeout">A request timeout.</param>
        public async Task<SendOCPPMessageResult> SendJSONData(EventTracking_Id   EventTrackingId,
                                                              NetworkingNode_Id  DestinationNodeId,
                                                              NetworkPath        NetworkPath,
                                                              Request_Id         RequestId,
                                                              String             Action,
                                                              JObject            JSONData,
                                                              DateTime           RequestTimeout,
                                                              CancellationToken  CancellationToken   = default)
        {

            try
            {

                var webSocketConnections  = LookupNetworkingNode(DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode      = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    var jsonRequestMessage  = new OCPP_JSONRequestMessage(
                                                  Timestamp.Now,
                                                  EventTracking_Id.New,
                                                  webSocketConnections.First().Item2,
                                                  DestinationNodeId,
                                                  NetworkPath,
                                                  RequestId,
                                                  Action,
                                                  JSONData
                                              );

                    var ocppTextMessage     = jsonRequestMessage.ToJSON().ToString(Formatting.None);


                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SendStatus.Success == await SendTextMessage(
                                                            webSocketConnection.Item1,
                                                            ocppTextMessage,
                                                            EventTrackingId,
                                                            CancellationToken
                                                        ))
                        {

                            requests.TryAdd(RequestId,
                                            SendRequestState.FromJSONRequest(
                                                Timestamp.Now,
                                                DestinationNodeId,
                                                RequestTimeout,
                                                jsonRequestMessage
                                            ));

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

                            break;

                        }

                        RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendOCPPMessageResult.Success;

                }
                else
                    return SendOCPPMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendOCPPMessageResult.TransmissionFailed;
            }

        }

        #endregion

        #region SendBinaryData   (EventTrackingId, DestinationNodeId, NetworkPath, RequestId, Action, BinaryData, RequestTimeout, ...)

        /// <summary>
        /// Send (and forget) the given binary data.
        /// </summary>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="DestinationNodeId">The networking node identification of the message destination.</param>
        /// <param name="NetworkPath">The network path.</param>
        /// <param name="RequestId">A unique request identification.</param>
        /// <param name="Action">An OCPP action.</param>
        /// <param name="BinaryData">The binary payload.</param>
        /// <param name="RequestTimeout">A request timeout.</param>
        public async Task<SendOCPPMessageResult> SendBinaryData(EventTracking_Id   EventTrackingId,
                                                                 NetworkingNode_Id  DestinationNodeId,
                                                                 NetworkPath        NetworkPath,
                                                                 Request_Id         RequestId,
                                                                 String             Action,
                                                                 Byte[]             BinaryData,
                                                                 DateTime           RequestTimeout,
                                                                 CancellationToken  CancellationToken   = default)
        {

            try
            {

                var webSocketConnections = LookupNetworkingNode(DestinationNodeId).ToArray();

                if (webSocketConnections.Length != 0)
                {

                    var networkingMode        = webSocketConnections.First().Item1.TryGetCustomDataAs<NetworkingMode>(networkingMode_WebSocketKey);

                    var binaryRequestMessage  = new OCPP_BinaryRequestMessage(
                                                    Timestamp.Now,
                                                    EventTracking_Id.New,
                                                    webSocketConnections.First().Item2,
                                                    DestinationNodeId,
                                                    NetworkPath,
                                                    RequestId,
                                                    Action,
                                                    BinaryData
                                                );

                    requests.TryAdd(RequestId,
                                    SendRequestState.FromBinaryRequest(
                                        Timestamp.Now,
                                        DestinationNodeId,
                                        RequestTimeout,
                                        binaryRequestMessage
                                    ));

                    var ocppBinaryMessage     = binaryRequestMessage.ToByteArray();

                    foreach (var webSocketConnection in webSocketConnections)
                    {

                        if (SendStatus.Success == await SendBinaryMessage(
                                                            webSocketConnection.Item1,
                                                            ocppBinaryMessage,
                                                            EventTrackingId,
                                                            CancellationToken
                                                        ))
                        {

                            #region OnBinaryMessageRequestSent

                            //var requestLogger = OnBinaryMessageRequestSent;
                            //if (requestLogger is not null)
                            //{

                            //    var loggerTasks = requestLogger.GetInvocationList().
                            //                                    OfType <OnWebSocketBinaryMessageDelegate>().
                            //                                    Select (loggingDelegate => loggingDelegate.Invoke(Timestamp.Now,
                            //                                                                                      this,
                            //                                                                                      webSocketConnection.Item1,
                            //                                                                                      EventTrackingId,
                            //                                                                                      ocppBinaryMessage,
                            //                                                                                      CancellationToken)).
                            //                                    ToArray();

                            //    try
                            //    {
                            //        await Task.WhenAll(loggerTasks);
                            //    }
                            //    catch (Exception e)
                            //    {
                            //        DebugX.Log(e, nameof(AOCPPWebSocketServer) + "." + nameof(OnBinaryMessageRequestSent));
                            //    }

                            //}

                            #endregion

                            break;

                        }

                        RemoveConnection(webSocketConnection.Item1);

                    }

                    return SendOCPPMessageResult.Success;

                }
                else
                    return SendOCPPMessageResult.UnknownClient;

            }
            catch (Exception)
            {
                return SendOCPPMessageResult.TransmissionFailed;
            }

        }

        #endregion


        #region SendJSONAndWait  (EventTrackingId, NetworkingNodeId, NetworkPath, RequestId, OCPPAction, JSONPayload,   RequestTimeout = null)

        public async Task<SendRequestState> SendJSONAndWait(EventTracking_Id   EventTrackingId,
                                                            NetworkingNode_Id  NetworkingNodeId,
                                                            NetworkPath        NetworkPath,
                                                            Request_Id         RequestId,
                                                            String             OCPPAction,
                                                            JObject            JSONPayload,
                                                            TimeSpan?          RequestTimeout,
                                                            CancellationToken  CancellationToken   = default)
        {

            var endTime         = Timestamp.Now + (RequestTimeout ?? this.RequestTimeout ?? DefaultRequestTimeout);

            var sendJSONResult  = await SendJSONData(
                                            EventTrackingId,
                                            NetworkingNodeId,
                                            NetworkPath,
                                            RequestId,
                                            OCPPAction,
                                            JSONPayload,
                                            endTime,
                                            CancellationToken
                                        );

            if (sendJSONResult == SendOCPPMessageResult.Success) {

                #region Wait for a response... till timeout

                do
                {

                    try
                    {

                        await Task.Delay(25, CancellationToken);

                        if (requests.TryGetValue(RequestId, out var sendRequestState) &&
                           (sendRequestState?.JSONResponse   is not null ||
                            sendRequestState?.BinaryResponse is not null ||
                            sendRequestState?.ErrorCode.HasValue == true))
                        {

                            requests.TryRemove(RequestId, out _);

                            return sendRequestState;

                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(String.Concat(nameof(AOCPPWebSocketServer), ".", nameof(SendJSONAndWait), " exception occured: ", e.Message));
                    }

                }
                while (Timestamp.Now < endTime);

                #endregion

                #region When timeout...

                if (requests.TryGetValue(RequestId, out var sendRequestState2) &&
                    sendRequestState2 is not null)
                {
                    sendRequestState2.ErrorCode = ResultCode.Timeout;
                    requests.TryRemove(RequestId, out _);
                    return sendRequestState2;
                }

                #endregion

            }

            #region ..., or client/network error(s)

            else
            {
                if (requests.TryGetValue(RequestId, out var sendRequestState3) &&
                    sendRequestState3 is not null)
                {
                    sendRequestState3.ErrorCode = ResultCode.Timeout;
                    requests.TryRemove(RequestId, out _);
                    return sendRequestState3;
                }
            }

            #endregion


            // Just in case...
            var now = Timestamp.Now;

            return SendRequestState.FromJSONRequest(

                       now,
                       NetworkingNodeId,
                       now,
                       new OCPP_JSONRequestMessage(
                           Timestamp.Now,
                           EventTracking_Id.New,
                           NetworkingMode.Standard,
                           NetworkingNodeId,
                           NetworkPath,
                           RequestId,
                           OCPPAction,
                           JSONPayload
                       ),
                       now,

                       ErrorCode:  ResultCode.InternalError

                   );

        }

        #endregion

        #region SendBinaryAndWait(EventTrackingId, NetworkingNodeId, NetworkPath, RequestId, OCPPAction, BinaryPayload, RequestTimeout = null)

        public async Task<SendRequestState> SendBinaryAndWait(EventTracking_Id    EventTrackingId,
                                                              NetworkingNode_Id   NetworkingNodeId,
                                                              NetworkPath         NetworkPath,
                                                              Request_Id          RequestId,
                                                              String              OCPPAction,
                                                              Byte[]              BinaryPayload,
                                                              TimeSpan?           RequestTimeout,
                                                              CancellationToken   CancellationToken   = default)
        {

            var endTime         = Timestamp.Now + (RequestTimeout ?? this.RequestTimeout ?? DefaultRequestTimeout);

            var sendJSONResult  = await SendBinaryData(
                                            EventTrackingId,
                                            NetworkingNodeId,
                                            NetworkPath,
                                            RequestId,
                                            OCPPAction,
                                            BinaryPayload,
                                            endTime,
                                            CancellationToken
                                        );

            if (sendJSONResult == SendOCPPMessageResult.Success) {

                #region Wait for a response... till timeout

                do
                {

                    try
                    {

                        await Task.Delay(25, CancellationToken);

                        if (requests.TryGetValue(RequestId, out var sendRequestState) &&
                           (sendRequestState?.JSONResponse   is not null ||
                            sendRequestState?.BinaryResponse is not null ||
                            sendRequestState?.ErrorCode.HasValue == true))
                        {

                            requests.TryRemove(RequestId, out _);

                            return sendRequestState;

                        }

                    }
                    catch (Exception e)
                    {
                        DebugX.Log(String.Concat(nameof(AOCPPWebSocketServer), ".", nameof(SendJSONAndWait), " exception occured: ", e.Message));
                    }

                }
                while (Timestamp.Now < endTime);

                #endregion

                #region When timeout...

                if (requests.TryGetValue(RequestId, out var sendRequestState2) &&
                    sendRequestState2 is not null)
                {
                    sendRequestState2.ErrorCode = ResultCode.Timeout;
                    requests.TryRemove(RequestId, out _);
                    return sendRequestState2;
                }

                #endregion

            }

            #region ..., or client/network error(s)

            else
            {
                if (requests.TryGetValue(RequestId, out var sendRequestState3) &&
                    sendRequestState3 is not null)
                {
                    sendRequestState3.ErrorCode = ResultCode.Timeout;
                    requests.TryRemove(RequestId, out _);
                    return sendRequestState3;
                }
            }

            #endregion


            // Just in case...
            var now = Timestamp.Now;

            return SendRequestState.FromBinaryRequest(

                       now,
                       NetworkingNodeId,
                       now,
                       new OCPP_BinaryRequestMessage(
                           Timestamp.Now,
                           EventTracking_Id.New,
                           NetworkingMode.Standard,
                           NetworkingNodeId,
                           NetworkPath,
                           RequestId,
                           OCPPAction,
                           BinaryPayload
                       ),
                       now,

                       ErrorCode:  ResultCode.InternalError

                   );

        }

        #endregion





        #region (protected) HandleErrors(Module, Caller, Exception, Description = null)
        protected Task HandleErrors(String     Module,
                                    String     Caller,
                                    Exception  Exception,
                                    String?    Description   = null)
        {

            DebugX.LogException(Exception, $"{Module}.{Caller}{(Description is not null ? $" {Description}" : "")}");

            return Task.CompletedTask;

        }

        #endregion


    }

}
