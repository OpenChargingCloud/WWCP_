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

using cloud.charging.open.protocols.OCPPv2_1.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1
{

    public static class E2ESecurityExtensions_OutgoingMessageExtensions
    {

        #region TransferSecureData                    (Parameter, Payload, KeyId = null, Key = null, Nonce = null, Counter = null, DestinationId = null, ...)

        /// <summary>
        /// Transfer the given binary data to the given charging station.
        /// </summary>
        /// <param name="Parameter">Encryption parameters.</param>
        /// <param name="KeyId">The unique identification of the encryption key.</param>
        /// <param name="Payload">The unencrypted encapsulated security payload.</param>
        /// <param name="Key"></param>
        /// <param name="Nonce"></param>
        /// <param name="Counter"></param>
        /// 
        /// <param name="DestinationId">The optional networking node identification.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<SecureDataTransferResponse>

            TransferSecureData(this INetworkingNodeButNotCSMS  NetworkingNode,
                               UInt16                          Parameter,
                               Byte[]                          Payload,
                               UInt16?                         KeyId               = null,
                               Byte[]?                         Key                 = null,
                               UInt64?                         Nonce               = null,
                               UInt64?                         Counter             = null,

                               NetworkingNode_Id?              DestinationId       = null,

                               IEnumerable<KeyPair>?           SignKeys            = null,
                               IEnumerable<SignInfo>?          SignInfos           = null,
                               IEnumerable<Signature>?         Signatures          = null,

                               Request_Id?                     RequestId           = null,
                               DateTime?                       RequestTimestamp    = null,
                               TimeSpan?                       RequestTimeout      = null,
                               EventTracking_Id?               EventTrackingId     = null,
                               CancellationToken               CancellationToken   = default)

        {

            try
            {

                return NetworkingNode.OCPP.OUT.SecureDataTransfer(
                           SecureDataTransferRequest.Encrypt(
                               DestinationId ?? NetworkingNode_Id.CSMS,
                               Parameter,
                               KeyId   ?? 0,
                               Key     ?? NetworkingNode.GetEncryptionKey    (DestinationId ?? NetworkingNode_Id.CSMS, KeyId),
                               Nonce   ?? NetworkingNode.GetEncryptionNonce  (DestinationId ?? NetworkingNode_Id.CSMS, KeyId),
                               Counter ?? NetworkingNode.GetEncryptionCounter(DestinationId ?? NetworkingNode_Id.CSMS, KeyId),
                               Payload,

                               SignKeys,
                               SignInfos,
                               Signatures,

                               RequestId        ?? NetworkingNode.NextRequestId,
                               RequestTimestamp ?? Timestamp.Now,
                               RequestTimeout   ?? NetworkingNode.OCPP.DefaultRequestTimeout,
                               EventTrackingId  ?? EventTracking_Id.New,
                               NetworkPath.From(NetworkingNode.Id),
                               CancellationToken
                           )
                       );

            }
            catch (Exception e)
            {
                return Task.FromResult(
                           SecureDataTransferResponse.ExceptionOccured(null, e)
                       );
            }

        }

        #endregion

        #region TransferSecureData          (DestinationId, Parameter, Payload, KeyId = null, Key = null, Nonce = null, Counter = null, ...)

        /// <summary>
        /// Transfer the given binary data to the given charging station.
        /// </summary>
        /// <param name="DestinationId">The networking node identification.</param>
        /// <param name="Parameter">Encryption parameters.</param>
        /// <param name="KeyId">The unique identification of the encryption key.</param>
        /// <param name="Payload">The unencrypted encapsulated security payload.</param>
        /// <param name="Key"></param>
        /// <param name="Nonce"></param>
        /// <param name="Counter"></param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">An optional timeout for this request.</param>
        /// <param name="EventTrackingId">An optional event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public static Task<SecureDataTransferResponse>

            TransferSecureData(this INetworkingNode     NetworkingNode,
                               NetworkingNode_Id        DestinationId,
                               UInt16                   Parameter,
                               Byte[]                   Payload,
                               UInt16?                  KeyId               = null,
                               Byte[]?                  Key                 = null,
                               UInt64?                  Nonce               = null,
                               UInt64?                  Counter             = null,

                               IEnumerable<KeyPair>?    SignKeys            = null,
                               IEnumerable<SignInfo>?   SignInfos           = null,
                               IEnumerable<Signature>?  Signatures          = null,

                               Request_Id?              RequestId           = null,
                               DateTime?                RequestTimestamp    = null,
                               TimeSpan?                RequestTimeout      = null,
                               EventTracking_Id?        EventTrackingId     = null,
                               CancellationToken        CancellationToken   = default)

        {

            try
            {

                return NetworkingNode.OCPP.OUT.SecureDataTransfer(
                           SecureDataTransferRequest.Encrypt(
                               DestinationId,
                               Parameter,
                               KeyId   ?? 0,
                               Key     ?? NetworkingNode.GetEncryptionKey    (DestinationId, KeyId),
                               Nonce   ?? NetworkingNode.GetEncryptionNonce  (DestinationId, KeyId),
                               Counter ?? NetworkingNode.GetEncryptionCounter(DestinationId, KeyId),
                               Payload,

                               SignKeys,
                               SignInfos,
                               Signatures,

                               RequestId        ?? NetworkingNode.NextRequestId,
                               RequestTimestamp ?? Timestamp.Now,
                               RequestTimeout   ?? NetworkingNode.OCPP.DefaultRequestTimeout,
                               EventTrackingId  ?? EventTracking_Id.New,
                               NetworkPath.From(NetworkingNode.Id),
                               CancellationToken
                           )
                       );

            }
            catch (Exception e)
            {
                return Task.FromResult(
                           SecureDataTransferResponse.ExceptionOccured(null, e)
                       );
            }

        }

        #endregion


    }

}