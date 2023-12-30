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

using cloud.charging.open.protocols.OCPP;
using cloud.charging.open.protocols.OCPP.WebSockets;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.NetworkingNode
{

    public delegate Task OnJSONMessageRequestSentDelegate   (DateTime                    Timestamp,
                                                             OCPPWebSocketAdapterOUT     Server,
                                                             OCPP_JSONRequestMessage     JSONRequestMessage);

    public delegate Task OnJSONMessageResponseSentDelegate  (DateTime                    Timestamp,
                                                             OCPPWebSocketAdapterOUT     Server,
                                                             OCPP_JSONResponseMessage    JSONResponseMessage);

    public delegate Task OnJSONErrorMessageSentDelegate     (DateTime                    Timestamp,
                                                             OCPPWebSocketAdapterOUT     Server,
                                                             OCPP_JSONErrorMessage       JSONErrorMessage);



    public delegate Task OnBinaryMessageRequestSentDelegate (DateTime                    Timestamp,
                                                             OCPPWebSocketAdapterOUT     Server,
                                                             OCPP_BinaryRequestMessage   BinaryRequestMessage);

    public delegate Task OnBinaryMessageResponseSentDelegate(DateTime                    Timestamp,
                                                             OCPPWebSocketAdapterOUT     Server,
                                                             OCPP_BinaryResponseMessage  BinaryResponseMessage);

    //public delegate Task OnBinaryErrorMessageSentDelegate   (DateTime                    Timestamp,
    //                                                         OCPPWebSocketAdapterOUT     Server,
    //                                                         OCPP_BinaryErrorMessage     BinaryErrorMessage);



    /// <summary>
    /// An OCPP HTTP Web Socket adapter.
    /// </remarks>
    /// <param name="NetworkingNode">The parent networking node.</param>
    public partial class OCPPWebSocketAdapterOUT(TestNetworkingNode NetworkingNode) : IOCPPWebSocketAdapterOUT
    {

        #region Data

        private readonly TestNetworkingNode parentNetworkingNode = NetworkingNode;

        #endregion

        #region Events

        #region Generic JSON Messages

        /// <summary>
        /// An event sent whenever a JSON request was sent.
        /// </summary>
        public event OnJSONMessageRequestSentDelegate?          OnJSONMessageRequestSent;

        /// <summary>
        /// An event sent whenever a JSON response was sent.
        /// </summary>
        public event OnJSONMessageResponseSentDelegate?         OnJSONMessageResponseSent;

        /// <summary>
        /// An event sent whenever a JSON error response was sent.
        /// </summary>
        public event OnJSONErrorMessageSentDelegate?            OnJSONErrorResponseSent;

        #endregion

        #region Generic Binary Messages

        /// <summary>
        /// An event sent whenever a binary request was sent.
        /// </summary>
        public event OnBinaryMessageRequestSentDelegate?        OnBinaryMessageRequestSent;

        /// <summary>
        /// An event sent whenever a binary response was sent.
        /// </summary>
        public event OnBinaryMessageResponseSentDelegate?       OnBinaryMessageResponseSent;

        ///// <summary>
        ///// An event sent whenever a binary error response was sent.
        ///// </summary>
        //public event OnWebSocketBinaryErrorResponseDelegate?    OnBinaryErrorResponseSent;

        #endregion

        #endregion


        // Send requests...

        #region SendJSONRequest         (JSONRequestMessage)

        /// <summary>
        /// Send (and forget) the given JSON.
        /// </summary>
        /// <param name="JSONRequestMessage">A OCPP JSON request.</param>
        public async Task<SendOCPPMessageResult> SendJSONRequest(OCPP_JSONRequestMessage JSONRequestMessage)
        {

            #region OnJSONMessageRequestSent

            var logger = OnJSONMessageRequestSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnJSONMessageRequestSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         JSONRequestMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnJSONMessageRequestSent));
                }
            }

            #endregion

            return await parentNetworkingNode.SendJSONRequest(JSONRequestMessage);

        }

        #endregion

        #region SendJSONRequestAndWait  (JSONRequestMessage)

        /// <summary>
        /// Send (and forget) the given JSON.
        /// </summary>
        /// <param name="JSONRequestMessage">A OCPP JSON request.</param>
        public async Task<SendRequestState> SendJSONRequestAndWait(OCPP_JSONRequestMessage JSONRequestMessage)
        {

            #region OnJSONMessageRequestSent

            var logger = OnJSONMessageRequestSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnJSONMessageRequestSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         JSONRequestMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnJSONMessageRequestSent));
                }
            }

            #endregion

            return await parentNetworkingNode.SendJSONRequestAndWait(JSONRequestMessage);

        }

        #endregion


        #region SendBinaryRequest       (BinaryRequestMessage)

        /// <summary>
        /// Send (and forget) the given Binary.
        /// </summary>
        /// <param name="BinaryRequestMessage">A OCPP Binary request.</param>
        public async Task<SendOCPPMessageResult> SendBinaryRequest(OCPP_BinaryRequestMessage BinaryRequestMessage)
        {

            #region OnBinaryMessageRequestSent

            var logger = OnBinaryMessageRequestSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnBinaryMessageRequestSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         BinaryRequestMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnBinaryMessageRequestSent));
                }
            }

            #endregion

            return await parentNetworkingNode.SendBinaryRequest(BinaryRequestMessage);

        }

        #endregion

        #region SendBinaryRequestAndWait(BinaryRequestMessage)

        /// <summary>
        /// Send (and forget) the given Binary.
        /// </summary>
        /// <param name="BinaryRequestMessage">A OCPP Binary request.</param>
        public async Task<SendRequestState> SendBinaryRequestAndWait(OCPP_BinaryRequestMessage BinaryRequestMessage)
        {

            #region OnBinaryMessageRequestSent

            var logger = OnBinaryMessageRequestSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnBinaryMessageRequestSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         BinaryRequestMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnBinaryMessageRequestSent));
                }
            }

            #endregion

            return await parentNetworkingNode.SendBinaryRequestAndWait(BinaryRequestMessage);

        }

        #endregion


        // Response events...

        #region NotifyJSONMessageResponseSent  (JSONResponseMessage)

        public async Task NotifyJSONMessageResponseSent(OCPP_JSONResponseMessage JSONResponseMessage)
        {

            var logger = OnJSONMessageResponseSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnJSONMessageResponseSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         JSONResponseMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnJSONMessageRequestSent));
                }
            }

        }

        #endregion

        #region NotifyJSONErrorResponseSent    (JSONErrorMessage)

        public async Task NotifyJSONErrorResponseSent(OCPP_JSONErrorMessage JSONErrorMessage)
        {

            var logger = OnJSONErrorResponseSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnJSONErrorMessageSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         JSONErrorMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnJSONMessageRequestSent));
                }
            }

        }

        #endregion


        #region NotifyBinaryMessageResponseSent(BinaryResponseMessage)


        public async Task NotifyBinaryMessageResponseSent(OCPP_BinaryResponseMessage BinaryResponseMessage)
        {

            var logger = OnBinaryMessageResponseSent;
            if (logger is not null)
            {
                try
                {

                    await Task.WhenAll(logger.GetInvocationList().
                                           OfType<OnBinaryMessageResponseSentDelegate>().
                                           Select(loggingDelegate => loggingDelegate.Invoke(
                                                                         Timestamp.Now,
                                                                         this,
                                                                         BinaryResponseMessage
                                                                     )).
                                           ToArray());

                }
                catch (Exception e)
                {
                    DebugX.Log(e, nameof(OCPPWebSocketAdapterOUT) + "." + nameof(OnBinaryMessageRequestSent));
                }
            }

        }

        #endregion


    }

}