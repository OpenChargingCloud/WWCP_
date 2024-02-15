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

using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;
using org.GraphDefined.Vanaheimr.Hermod.HTTP;

using cloud.charging.open.protocols.OCPP;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    /// <summary>
    /// A NotifySettlement request.
    /// </summary>
    public class NotifySettlementRequest : ARequest<NotifySettlementRequest>,
                                           IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/v2.1/cs/notifySettlementRequest");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext     Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The optional transaction for which priority charging is requested.
        /// </summary>
        [Optional]
        public Transaction_Id?   TransactionId          { get; }

        /// <summary>
        /// The payment reference received from the payment terminal
        /// and is used as the value for idToken.
        /// </summary>
        [Mandatory]
        public PaymentReference  PaymentReference       { get; }

        /// <summary>
        /// The status of the settlement attempt.
        /// </summary>
        [Mandatory]
        public PaymentStatus     PaymentStatus          { get; }

        /// <summary>
        /// Optional additional information from payment terminal/payment process.
        /// </summary>
        [Optional]
        public String?           StatusInfo             { get; }

        /// <summary>
        /// The amount that was settled, or attempted to be settled (in case of failure).
        /// </summary>
        [Mandatory]
        public Decimal           SettlementAmount       { get; }

        /// <summary>
        /// The time when the settlement was done.
        /// </summary>
        [Mandatory]
        public DateTime          SettlementTimestamp    { get; }

        /// <summary>
        /// The optional receipt id, to be used if the receipt is generated by the
        /// payment terminal or the charging station.
        /// </summary>
        [Optional]
        public ReceiptId?        ReceiptId              { get; }

        /// <summary>
        /// The optional receipt URL, to be used if the receipt is generated by the
        /// payment terminal or the charging station.
        /// </summary>
        [Optional]
        public URL?              ReceiptURL             { get; }

        /// <summary>
        /// The optional invoice number inside the e-receipt.
        /// </summary>
        [Optional]
        public InvoiceNumber?    InvoiceNumber          { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a NotifySettlement request.
        /// </summary>
        /// <param name="DestinationNodeId">The destination networking node identification.</param>
        /// 
        /// <param name="PaymentReference">The payment reference received from the payment terminal and is used as the value for idToken.</param>
        /// <param name="PaymentStatus">The status of the settlement attempt.</param>
        /// <param name="SettlementAmount">The amount that was settled, or attempted to be settled (in case of failure).</param>
        /// <param name="SettlementTimestamp">The time when the settlement was done.</param>
        /// 
        /// <param name="TransactionId">The optional transaction for which priority charging is requested.</param>
        /// <param name="StatusInfo">Optional additional information from payment terminal/payment process.</param>
        /// <param name="ReceiptId">The optional receipt id, to be used if the receipt is generated by the payment terminal or the charging station.</param>
        /// <param name="ReceiptURL">The optional receipt URL, to be used if the receipt is generated by the payment terminal or the charging station.</param>
        /// <param name="InvoiceNumber">The optional invoice number inside the e-receipt.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">The timeout of this request.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public NotifySettlementRequest(NetworkingNode_Id             DestinationNodeId,

                                       PaymentReference              PaymentReference,
                                       PaymentStatus                 PaymentStatus,
                                       Decimal                       SettlementAmount,
                                       DateTime                      SettlementTimestamp,

                                       Transaction_Id?               TransactionId       = null,
                                       String?                       StatusInfo          = null,
                                       ReceiptId?                    ReceiptId           = null,
                                       URL?                          ReceiptURL          = null,
                                       InvoiceNumber?                InvoiceNumber       = null,

                                       IEnumerable<KeyPair>?         SignKeys            = null,
                                       IEnumerable<SignInfo>?        SignInfos           = null,
                                       IEnumerable<OCPP.Signature>?  Signatures          = null,

                                       CustomData?                   CustomData          = null,

                                       Request_Id?                   RequestId           = null,
                                       DateTime?                     RequestTimestamp    = null,
                                       TimeSpan?                     RequestTimeout      = null,
                                       EventTracking_Id?             EventTrackingId     = null,
                                       NetworkPath?                  NetworkPath         = null,
                                       CancellationToken             CancellationToken   = default)

            : base(DestinationNodeId,
                   nameof(NotifySettlementRequest)[..^7],

                   SignKeys,
                   SignInfos,
                   Signatures,

                   CustomData,

                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   NetworkPath,
                   CancellationToken)

        {

            this.PaymentReference     = PaymentReference;
            this.PaymentStatus        = PaymentStatus;
            this.SettlementAmount     = SettlementAmount;
            this.SettlementTimestamp  = SettlementTimestamp;

            this.TransactionId        = TransactionId;
            this.StatusInfo           = StatusInfo;
            this.ReceiptId            = ReceiptId;
            this.ReceiptURL           = ReceiptURL;
            this.InvoiceNumber        = InvoiceNumber;


            unchecked
            {

                hashCode = this.PaymentReference.   GetHashCode()       * 27 ^
                           this.PaymentStatus.      GetHashCode()       * 23 ^
                           this.SettlementAmount.   GetHashCode()       * 19 ^
                           this.SettlementTimestamp.GetHashCode()       * 17 ^

                          (this.TransactionId?.     GetHashCode() ?? 0) * 13 ^
                          (this.StatusInfo?.        GetHashCode() ?? 0) * 11 ^
                          (this.ReceiptId?.         GetHashCode() ?? 0) *  7 ^
                          (this.ReceiptURL?.        GetHashCode() ?? 0) *  5 ^
                          (this.InvoiceNumber?.     GetHashCode() ?? 0) *  3 ^

                           base.                    GetHashCode();

            }

        }

        #endregion


        //ToDo: Update schema documentation after the official release of OCPP v2.1!

        #region Documentation


        #endregion

        #region (static) Parse   (JSON, RequestId, DestinationNodeId, NetworkPath, CustomNotifySettlementRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a NotifySettlement request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="DestinationNodeId">The destination networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="CustomNotifySettlementRequestParser">A delegate to parse custom NotifySettlement requests.</param>
        public static NotifySettlementRequest Parse(JObject                                                JSON,
                                                    Request_Id                                             RequestId,
                                                    NetworkingNode_Id                                      DestinationNodeId,
                                                    NetworkPath                                            NetworkPath,
                                                    CustomJObjectParserDelegate<NotifySettlementRequest>?  CustomNotifySettlementRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         DestinationNodeId,
                         NetworkPath,
                         out var notifySettlementRequest,
                         out var errorResponse,
                         CustomNotifySettlementRequestParser))
            {
                return notifySettlementRequest;
            }

            throw new ArgumentException("The given JSON representation of a NotifySettlement request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, DestinationNodeId, NetworkPath, out NotifySettlementRequest, out ErrorResponse, CustomNotifySettlementRequestParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a NotifySettlement request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="DestinationNodeId">The destination networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="NotifySettlementRequest">The parsed NotifySettlement request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomNotifySettlementRequestParser">A delegate to parse custom NotifySettlement requests.</param>
        public static Boolean TryParse(JObject                                                JSON,
                                       Request_Id                                             RequestId,
                                       NetworkingNode_Id                                      DestinationNodeId,
                                       NetworkPath                                            NetworkPath,
                                       [NotNullWhen(true)]  out NotifySettlementRequest?      NotifySettlementRequest,
                                       [NotNullWhen(false)] out String?                       ErrorResponse,
                                       CustomJObjectParserDelegate<NotifySettlementRequest>?  CustomNotifySettlementRequestParser)
        {

            try
            {

                NotifySettlementRequest = null;

                #region PaymentReference       [mandatory]

                if (!JSON.ParseMandatory("pspRef",
                                         "payment reference identification",
                                         OCPPv2_1.PaymentReference.TryParse,
                                         out PaymentReference PaymentReference,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region PaymentStatus          [mandatory]

                if (!JSON.ParseMandatory("status",
                                         "payment status",
                                         PaymentStatusExtensions.TryParse,
                                         out PaymentStatus PaymentStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region SettlementAmount       [mandatory]

                if (!JSON.ParseMandatory("settlementAmount",
                                         "settlement amount",
                                         out Decimal SettlementAmount,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region SettlementTimestamp    [mandatory]

                if (!JSON.ParseMandatory("settlementTime",
                                         "settlement timestamp",
                                         out DateTime SettlementTimestamp,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region TransactionId          [optional]

                if (JSON.ParseOptional("transactionId",
                                       "transaction identification",
                                       Transaction_Id.TryParse,
                                       out Transaction_Id? TransactionId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region StatusInfo             [optional]

                var StatusInfo = JSON["statusInfo"]?.Value<String>();

                #endregion

                #region ReceiptId              [optional]

                if (JSON.ParseOptional("receiptId",
                                       "receipt identification",
                                       OCPPv2_1.ReceiptId.TryParse,
                                       out ReceiptId? ReceiptId,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region ReceiptURL             [optional]

                if (JSON.ParseOptional("receiptUrl",
                                       "receipt URL",
                                       URL.TryParse,
                                       out URL? ReceiptURL,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region InvoiceNumber          [optional]

                if (JSON.ParseOptional("invoiceNumber",
                                       "invoice number",
                                       OCPPv2_1.InvoiceNumber.TryParse,
                                       out InvoiceNumber? InvoiceNumber,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                #region Signatures             [optional, OCPP_CSE]

                if (JSON.ParseOptionalHashSet("signatures",
                                              "cryptographic signatures",
                                              OCPP.Signature.TryParse,
                                              out HashSet<OCPP.Signature> Signatures,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData             [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           OCPP.CustomData.TryParse,
                                           out CustomData? CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                NotifySettlementRequest = new NotifySettlementRequest(

                                              DestinationNodeId,

                                              PaymentReference,
                                              PaymentStatus,
                                              SettlementAmount,
                                              SettlementTimestamp,

                                              TransactionId,
                                              StatusInfo,
                                              ReceiptId,
                                              ReceiptURL,
                                              InvoiceNumber,

                                              null,
                                              null,
                                              Signatures,

                                              CustomData,

                                              RequestId,
                                              null,
                                              null,
                                              null,
                                              NetworkPath

                                          );

                if (CustomNotifySettlementRequestParser is not null)
                    NotifySettlementRequest = CustomNotifySettlementRequestParser(JSON,
                                                                                  NotifySettlementRequest);

                return true;

            }
            catch (Exception e)
            {
                NotifySettlementRequest  = null;
                ErrorResponse            = "The given JSON representation of a NotifySettlement request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomNotifySettlementRequestSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomNotifySettlementRequestSerializer">A delegate to serialize custom NotifySettlement requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<NotifySettlementRequest>?  CustomNotifySettlementRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<OCPP.Signature>?           CustomSignatureSerializer                 = null,
                              CustomJObjectSerializerDelegate<CustomData>?               CustomCustomDataSerializer                = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("pspRef",             PaymentReference.   ToString()),
                                 new JProperty("status",             PaymentStatus.      ToString()),
                                 new JProperty("settlementAmount",   SettlementAmount),
                                 new JProperty("settlementTime",     SettlementTimestamp),

                           TransactionId.HasValue
                               ? new JProperty("transactionId",      TransactionId.Value.ToString())
                               : null,

                           StatusInfo is not null
                               ? new JProperty("statusInfo",         StatusInfo)
                               : null,

                           ReceiptId.HasValue
                               ? new JProperty("receiptId",          ReceiptId.    Value.ToString())
                               : null,

                           ReceiptURL.HasValue
                               ? new JProperty("receiptUrl",         ReceiptURL.   Value.ToString())
                               : null,

                           InvoiceNumber.HasValue
                               ? new JProperty("invoiceNumber",      InvoiceNumber.Value.ToString())
                               : null,


                           Signatures.Any()
                               ? new JProperty("signatures",         new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                                CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",         CustomData.   ToJSON(CustomCustomDataSerializer))
                               : null);

            return CustomNotifySettlementRequestSerializer is not null
                       ? CustomNotifySettlementRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (NotifySettlementRequest1, NotifySettlementRequest2)

        /// <summary>
        /// Compares two NotifySettlement requests for equality.
        /// </summary>
        /// <param name="NotifySettlementRequest1">A NotifySettlement request.</param>
        /// <param name="NotifySettlementRequest2">Another NotifySettlement request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (NotifySettlementRequest? NotifySettlementRequest1,
                                           NotifySettlementRequest? NotifySettlementRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(NotifySettlementRequest1, NotifySettlementRequest2))
                return true;

            // If one is null, but not both, return false.
            if (NotifySettlementRequest1 is null || NotifySettlementRequest2 is null)
                return false;

            return NotifySettlementRequest1.Equals(NotifySettlementRequest2);

        }

        #endregion

        #region Operator != (NotifySettlementRequest1, NotifySettlementRequest2)

        /// <summary>
        /// Compares two NotifySettlement requests for inequality.
        /// </summary>
        /// <param name="NotifySettlementRequest1">A NotifySettlement request.</param>
        /// <param name="NotifySettlementRequest2">Another NotifySettlement request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (NotifySettlementRequest? NotifySettlementRequest1,
                                           NotifySettlementRequest? NotifySettlementRequest2)

            => !(NotifySettlementRequest1 == NotifySettlementRequest2);

        #endregion

        #endregion

        #region IEquatable<NotifySettlementRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two NotifySettlement requests for equality.
        /// </summary>
        /// <param name="Object">A NotifySettlement request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is NotifySettlementRequest notifySettlementRequest &&
                   Equals(notifySettlementRequest);

        #endregion

        #region Equals(NotifySettlementRequest)

        /// <summary>
        /// Compares two NotifySettlement requests for equality.
        /// </summary>
        /// <param name="NotifySettlementRequest">A NotifySettlement request to compare with.</param>
        public override Boolean Equals(NotifySettlementRequest? NotifySettlementRequest)

            => NotifySettlementRequest is not null &&

               PaymentReference.   Equals(NotifySettlementRequest.PaymentReference)    &&
               PaymentStatus.      Equals(NotifySettlementRequest.PaymentStatus)       &&
               SettlementAmount.   Equals(NotifySettlementRequest.SettlementAmount)    &&
               SettlementTimestamp.Equals(NotifySettlementRequest.SettlementTimestamp) &&

               base.GenericEquals(NotifySettlementRequest);

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

            => $"'{PaymentReference}'{(TransactionId.HasValue ? $" for {TransactionId.Value}" : "")} {PaymentStatus} with {SettlementAmount} at '{SettlementTimestamp}'";

        #endregion

    }

}
