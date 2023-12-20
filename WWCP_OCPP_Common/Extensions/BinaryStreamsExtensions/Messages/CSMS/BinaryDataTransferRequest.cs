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

#endregion

namespace cloud.charging.open.protocols.OCPP.CSMS
{

    /// <summary>
    /// A binary data transfer request.
    /// </summary>
    public class BinaryDataTransferRequest : ARequest<BinaryDataTransferRequest>,
                                             IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/csms/binaryDataTransferRequest");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext  Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The vendor identification or namespace of the given message.
        /// </summary>
        [Mandatory]
        public Vendor_Id      VendorId     { get; }

        /// <summary>
        /// An optional message identification field.
        /// </summary>
        [Optional]
        public Message_Id?    MessageId    { get; }

        /// <summary>
        /// Optional message binary data without specified length or format.
        /// </summary>
        [Optional]
        public Byte[]?        Data         { get; }

        /// <summary>
        /// The binary format of the given message.
        /// </summary>
        [Mandatory]
        public BinaryFormats  Format       { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new binary data transfer request.
        /// </summary>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="VendorId">The vendor identification or namespace of the given message.</param>
        /// <param name="MessageId">An optional message identification.</param>
        /// <param name="Data">Optional vendor-specific binary data.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">The timeout of this request.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public BinaryDataTransferRequest(NetworkingNode_Id        NetworkingNodeId,
                                         Vendor_Id                VendorId,
                                         Message_Id?              MessageId           = null,
                                         Byte[]?                  Data                = null,
                                         BinaryFormats?           Format              = null,

                                         IEnumerable<KeyPair>?    SignKeys            = null,
                                         IEnumerable<SignInfo>?   SignInfos           = null,
                                         IEnumerable<Signature>?  Signatures          = null,

                                         Request_Id?              RequestId           = null,
                                         DateTime?                RequestTimestamp    = null,
                                         TimeSpan?                RequestTimeout      = null,
                                         EventTracking_Id?        EventTrackingId     = null,
                                         NetworkPath?             NetworkPath         = null,
                                         CancellationToken        CancellationToken   = default)

            : base(NetworkingNodeId,
                   nameof(BinaryDataTransferRequest)[..^7],

                   SignKeys,
                   SignInfos,
                   Signatures,

                   null, //CustomData,

                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   NetworkPath,
                   CancellationToken)

        {

            this.VendorId   = VendorId;
            this.MessageId  = MessageId;
            this.Data       = Data;
            this.Format     = Format ?? BinaryFormats.TextIds;

            unchecked
            {

                hashCode = this.VendorId.  GetHashCode()       * 11 ^
                          (this.MessageId?.GetHashCode() ?? 0) *  7 ^
                          (this.Data?.     GetHashCode() ?? 0) *  5 ^
                           this.Format.    GetHashCode()       *  3 ^
                           base.           GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse   (Binary, RequestId, NetworkingNodeId, NetworkPath, CustomDataTransferRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a binary data transfer request.
        /// </summary>
        /// <param name="Binary">The binary to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="CustomDataTransferRequestParser">A delegate to parse custom binary data transfer requests.</param>
        public static BinaryDataTransferRequest Parse(Byte[]                                                  Binary,
                                                      Request_Id                                              RequestId,
                                                      NetworkingNode_Id                                       NetworkingNodeId,
                                                      NetworkPath                                             NetworkPath,
                                                      CustomBinaryParserDelegate<BinaryDataTransferRequest>?  CustomDataTransferRequestParser   = null)
        {

            if (TryParse(Binary,
                         RequestId,
                         NetworkingNodeId,
                         NetworkPath,
                         out var binaryDataTransferRequest,
                         out var errorResponse,
                         CustomDataTransferRequestParser) &&
                binaryDataTransferRequest is not null)
            {
                return binaryDataTransferRequest;
            }

            throw new ArgumentException("The given binary representation of a binary data transfer request is invalid: " + errorResponse,
                                        nameof(Binary));

        }

        #endregion

        #region (static) TryParse(Binary, RequestId, NetworkingNodeId, NetworkPath, out BinaryDataTransferRequest, OnException = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given binary representation of a binary data transfer request.
        /// </summary>
        /// <param name="Binary">The binary data to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="BinaryDataTransferRequest">The parsed BinaryDataTransfer request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(Byte[]                          Binary,
                                       Request_Id                      RequestId,
                                       NetworkingNode_Id               NetworkingNodeId,
                                       NetworkPath                     NetworkPath,
                                       out BinaryDataTransferRequest?  BinaryDataTransferRequest,
                                       out String?                     ErrorResponse)

            => TryParse(Binary,
                        RequestId,
                        NetworkingNodeId,
                        NetworkPath,
                        out BinaryDataTransferRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given binary representation of a binary data transfer request.
        /// </summary>
        /// <param name="Binary">The binary to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="BinaryDataTransferRequest">The parsed BinaryDataTransfer request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomBinaryDataTransferRequestParser">A delegate to parse custom BinaryDataTransfer requests.</param>
        public static Boolean TryParse(Byte[]                                                  Binary,
                                       Request_Id                                              RequestId,
                                       NetworkingNode_Id                                       NetworkingNodeId,
                                       NetworkPath                                             NetworkPath,
                                       out BinaryDataTransferRequest?                          BinaryDataTransferRequest,
                                       out String?                                             ErrorResponse,
                                       CustomBinaryParserDelegate<BinaryDataTransferRequest>?  CustomBinaryDataTransferRequestParser)
        {

            try
            {

                BinaryDataTransferRequest = null;

                var stream  = new MemoryStream(Binary);
                var format  = BinaryFormatsExtensions.Parse(stream.ReadUInt16());

                #region Compact Format

                if (format == BinaryFormats.Compact)
                {

                    //var vendorId         = Vendor_Id. Parse(BitConverter.ToUInt32(span.Slice( 2, 4)));
                    //var messageId        = Message_Id.Parse(BitConverter.ToUInt32(span.Slice( 6, 4)));
                    //var dataLength       =                  BitConverter.ToInt32 (span.Slice(10, 4));
                    //var data             = span.Slice(14, dataLength).ToArray();

                    //var text             = data.ToUTF8String();

                    //var signaturesCount  = BitConverter.ToUInt16(span.Slice(14 + dataLength, 2));

                    //for (var sigi = 0; sigi < signaturesCount; sigi++)
                    //{

                    //    var signatureLength = BitConverter.ToUInt16(span.Slice(16 + dataLength + (sigi * 2), 2));

                    //    if (Signature.TryParse(span.Slice(18 + dataLength + (sigi * 2), signatureLength).ToArray(),
                    //                           out var signature,
                    //                           out var err) &&
                    //        signature is not null)
                    //    {
                    //        signatures.Add(signature);
                    //    }

                    //}


                    //BinaryDataTransferRequest = new BinaryDataTransferRequest(

                    //                                NetworkPath,
                    //                                vendorId,
                    //                                messageId,
                    //                                data,
                    //                                format,

                    //                                null,
                    //                                null,
                    //                                signatures,

                    //                                null,
                    //                                RequestId

                    //                            );

                }

                #endregion

                else
                {

                    var vendorIdLength    = stream.ReadUInt16();
                    var vendorIdText      = stream.ReadUTF8String(vendorIdLength);

                    if (!Vendor_Id.TryParse(vendorIdText, out var vendorId))
                    {
                        ErrorResponse = $"The received vendor identification '{vendorIdText}' is invalid!";
                        return false;
                    }

                    Message_Id? messageId = null;
                    var messageIdLength   = stream.ReadUInt16();
                    if (messageIdLength > 0)
                    {

                        var messageIdText = stream.ReadUTF8String(messageIdLength);

                        if (Message_Id.TryParse(messageIdText, out var _messageId))
                            messageId     = _messageId;
                        else
                        {
                            ErrorResponse = $"The received message identification '{messageIdText}' is invalid!";
                            return false;
                        }

                    }

                    var dataLength       = stream.ReadUInt64();
                    var data             = stream.ReadBytes(dataLength);


                    BinaryDataTransferRequest = new BinaryDataTransferRequest(

                                                    NetworkingNodeId,
                                                    vendorId,
                                                    messageId,
                                                    data,
                                                    format,

                                                    null,
                                                    null,
                                                    null, //signatures,

                                                    RequestId,
                                                    null,
                                                    null,
                                                    null,
                                                    NetworkPath

                                                );

                }

                ErrorResponse = null;

                if (CustomBinaryDataTransferRequestParser is not null)
                    BinaryDataTransferRequest = CustomBinaryDataTransferRequestParser(Binary,
                                                                                      BinaryDataTransferRequest);

                return true;

            }
            catch (Exception e)
            {
                BinaryDataTransferRequest  = null;
                ErrorResponse              = "The given binary representation of a binary data transfer request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToBinary(CustomBinaryDataTransferRequestSerializer = null, CustomBinarySignatureSerializer = null, ...)

        /// <summary>
        /// Return a binary representation of this object.
        /// </summary>
        /// <param name="CustomBinaryDataTransferRequestSerializer">A delegate to serialize custom binary data transfer requests.</param>
        /// <param name="CustomBinarySignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="IncludeSignatures">Whether to include the digital signatures (default), or not.</param>
        public Byte[] ToBinary(CustomBinarySerializerDelegate<BinaryDataTransferRequest>?  CustomBinaryDataTransferRequestSerializer   = null,
                               CustomBinarySerializerDelegate<Signature>?                  CustomBinarySignatureSerializer             = null,
                               Boolean                                                     IncludeSignatures                           = true)
        {

            var binaryStream = new MemoryStream();

            binaryStream.Write(Format.AsBytes(), 0, 2);

            switch (Format)
            {

                case BinaryFormats.Compact: {

                    binaryStream.Write(BitConverter.GetBytes(VendorId.  NumericId));
                    binaryStream.Write(BitConverter.GetBytes(MessageId?.NumericId ?? 0));
                    binaryStream.Write(BitConverter.GetBytes((UInt32) (Data?.LongLength ?? 0)));

                    if (Data is not null)
                        binaryStream.Write(Data,                                          0, (Int32) (Data?.LongLength ?? 0));

                    var signaturesCount = (UInt16) (IncludeSignatures ? Signatures.Count() : 0);
                    binaryStream.Write(BitConverter.GetBytes(signaturesCount),            0, 2);

                    if (IncludeSignatures) {
                        foreach (var signature in Signatures)
                        {
                            var binarySignature = signature.ToBinary();
                            binaryStream.Write(BitConverter.GetBytes((UInt16) binarySignature.Length));
                            binaryStream.Write(binarySignature);
                        }
                    }

                }
                break;

                case BinaryFormats.TextIds: {

                    var vendorIdBytes  = VendorId.  TextId.ToUTF8Bytes();
                    binaryStream.WriteUInt16((UInt16) vendorIdBytes. Length);
                    binaryStream.Write(vendorIdBytes,       0, vendorIdBytes. Length);

                    var messageIdBytes = MessageId?.TextId.ToUTF8Bytes() ?? [];
                    binaryStream.WriteUInt16((UInt16) messageIdBytes.Length);
                    if (messageIdBytes.Length > 0)
                        binaryStream.Write(messageIdBytes,  0, messageIdBytes.Length);

                    var data = Data                                          ?? [];
                    binaryStream.WriteUInt64((UInt64) data.          LongLength);
                    binaryStream.Write(data,                0, data.          Length); //ToDo: Fix me for >2 GB!

                    var signaturesCount = IncludeSignatures ? Signatures.Count() : 0;
                    binaryStream.WriteUInt16((UInt16) signaturesCount);

                    if (signaturesCount > 0)
                    {
                        
                    }

                }
                break;

                case BinaryFormats.TagLengthValue: {

                    var vendorIdBytes  = VendorId.  ToString().ToUTF8Bytes();
                    binaryStream.Write(BitConverter.GetBytes((UInt16) BinaryTags.VendorId),   0, 2);
                    binaryStream.Write(BitConverter.GetBytes((UInt16) vendorIdBytes.Length),  0, 2);
                    binaryStream.Write(vendorIdBytes,                                         0, vendorIdBytes. Length);

                    if (MessageId.HasValue) {
                        var messageIdBytes = MessageId?.ToString().ToUTF8Bytes() ?? [];
                        binaryStream.Write(BitConverter.GetBytes((UInt16) BinaryTags.MessageId),  0, 2);
                        binaryStream.Write(BitConverter.GetBytes((UInt16) messageIdBytes.Length), 0, 2);
                        binaryStream.Write(messageIdBytes,                                        0, messageIdBytes.Length);
                    }

                    var data = Data                                          ?? [];
                    binaryStream.Write(BitConverter.GetBytes((UInt16) BinaryTags.Data),       0, 2);
                    binaryStream.Write(BitConverter.GetBytes((UInt16) 0),                     0, 2);
                    binaryStream.Write(BitConverter.GetBytes((UInt32) data.Length),           0, 4);
                    binaryStream.Write(data,                                                  0, data.          Length);

                    var signaturesCount = (UInt16) (IncludeSignatures ? Signatures.Count() : 0);
                    binaryStream.Write(BitConverter.GetBytes(signaturesCount),            0, 2);

                    if (signaturesCount > 0)
                    {
                        
                    }

                }
                break;

            }

            var binary = binaryStream.ToArray();

            return CustomBinaryDataTransferRequestSerializer is not null
                       ? CustomBinaryDataTransferRequestSerializer(this, binary)
                       : binary;

        }

        #endregion


        #region Operator overloading

        #region Operator == (BinaryDataTransferRequest1, BinaryDataTransferRequest2)

        /// <summary>
        /// Compares two BinaryDataTransfer requests for equality.
        /// </summary>
        /// <param name="BinaryDataTransferRequest1">A BinaryDataTransfer request.</param>
        /// <param name="BinaryDataTransferRequest2">Another BinaryDataTransfer request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (BinaryDataTransferRequest? BinaryDataTransferRequest1,
                                           BinaryDataTransferRequest? BinaryDataTransferRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(BinaryDataTransferRequest1, BinaryDataTransferRequest2))
                return true;

            // If one is null, but not both, return false.
            if (BinaryDataTransferRequest1 is null || BinaryDataTransferRequest2 is null)
                return false;

            return BinaryDataTransferRequest1.Equals(BinaryDataTransferRequest2);

        }

        #endregion

        #region Operator != (BinaryDataTransferRequest1, BinaryDataTransferRequest2)

        /// <summary>
        /// Compares two BinaryDataTransfer requests for inequality.
        /// </summary>
        /// <param name="BinaryDataTransferRequest1">A BinaryDataTransfer request.</param>
        /// <param name="BinaryDataTransferRequest2">Another BinaryDataTransfer request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (BinaryDataTransferRequest? BinaryDataTransferRequest1,
                                           BinaryDataTransferRequest? BinaryDataTransferRequest2)

            => !(BinaryDataTransferRequest1 == BinaryDataTransferRequest2);

        #endregion

        #endregion

        #region IEquatable<BinaryDataTransferRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two binary data transfer requests for equality.
        /// </summary>
        /// <param name="Object">A binary data transfer request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is BinaryDataTransferRequest binaryDataTransferRequest &&
                   Equals(binaryDataTransferRequest);

        #endregion

        #region Equals(BinaryDataTransferRequest)

        /// <summary>
        /// Compares two binary data transfer requests for equality.
        /// </summary>
        /// <param name="BinaryDataTransferRequest">A binary data transfer request to compare with.</param>
        public override Boolean Equals(BinaryDataTransferRequest? BinaryDataTransferRequest)

            => BinaryDataTransferRequest is not null               &&

               VendorId.Equals(BinaryDataTransferRequest.VendorId) &&

             ((MessageId is     null && BinaryDataTransferRequest.MessageId is     null) ||
              (MessageId is not null && BinaryDataTransferRequest.MessageId is not null && MessageId.  Equals(BinaryDataTransferRequest.MessageId))) &&

             ((Data      is     null && BinaryDataTransferRequest.Data      is     null) ||
              (Data      is not null && BinaryDataTransferRequest.Data      is not null && Data.SequenceEqual(BinaryDataTransferRequest.Data)))      &&

               base.GenericEquals(BinaryDataTransferRequest);

        #endregion

        #endregion

        #region (override) GetHashCode()

        private readonly Int32 hashCode;

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        public override Int32 GetHashCode()
            => hashCode;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   VendorId,

                   MessageId.IsNotNullOrEmpty()
                        ? $" ({MessageId})"
                        : "",

                   Data?.Length > 0
                       ? $": '{Data.ToBase64().SubstringMax(100)}' [{Data.Length} bytes]"
                       : ""

                );

        #endregion


    }

}
