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

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    /// <summary>
    /// A notify customer information request.
    /// </summary>
    public class NotifyCustomerInformationRequest : ARequest<NotifyCustomerInformationRequest>,
                                                    IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/cs/notifyCustomerInformationRequest");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext  Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The unique identification of the notify customer information request.
        /// </summary>
        [Mandatory]
        public Int64          NotifyCustomerInformationRequestId    { get; }

        /// <summary>
        /// The requested data or a part of the requested data.
        /// No format specified in which the data is returned.
        /// </summary>
        [Mandatory]
        public String         Data                                  { get; }

        /// <summary>
        /// The sequence number of this message.
        /// First message starts at 0.
        /// </summary>
        [Mandatory]
        public UInt32         SequenceNumber                        { get; }

        /// <summary>
        /// The timestamp of the moment this message was generated at the charging station.
        /// </summary>
        [Mandatory]
        public DateTime       GeneratedAt                           { get; }

        /// <summary>
        /// The optional "to be continued" indicator whether another part of the monitoring
        /// data follows in an upcoming NotifyCustomerInformationRequest message.
        /// Default value when omitted is false.
        /// </summary>
        [Optional]
        public Boolean?       ToBeContinued                         { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a notify customer information request.
        /// </summary>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="NotifyCustomerInformationRequestId">The unique identification of the notify customer information request.</param>
        /// <param name="Data">The requested data or a part of the requested data. No format specified in which the data is returned.</param>
        /// <param name="SequenceNumber">The sequence number of this message. First message starts at 0.</param>
        /// <param name="GeneratedAt">The timestamp of the moment this message was generated at the charging station.</param>
        /// <param name="ToBeContinued">The optional "to be continued" indicator whether another part of the monitoring data follows in an upcoming NotifyCustomerInformationRequest message. Default value when omitted is false.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">The timeout of this request.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public NotifyCustomerInformationRequest(ChargingStation_Id       ChargingStationId,
                                                Int64                    NotifyCustomerInformationRequestId,
                                                String                   Data,
                                                UInt32                   SequenceNumber,
                                                DateTime                 GeneratedAt,
                                                Boolean?                 ToBeContinued       = null,

                                                IEnumerable<KeyPair>?    SignKeys            = null,
                                                IEnumerable<SignInfo>?   SignInfos           = null,
                                                IEnumerable<Signature>?  Signatures          = null,

                                                CustomData?              CustomData          = null,

                                                Request_Id?              RequestId           = null,
                                                DateTime?                RequestTimestamp    = null,
                                                TimeSpan?                RequestTimeout      = null,
                                                EventTracking_Id?        EventTrackingId     = null,
                                                CancellationToken        CancellationToken   = default)

            : base(ChargingStationId,
                   "NotifyCustomerInformation",

                   SignKeys,
                   SignInfos,
                   Signatures,

                   CustomData,

                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   CancellationToken)

        {

            this.NotifyCustomerInformationRequestId  = NotifyCustomerInformationRequestId;
            this.Data                                = Data;
            this.SequenceNumber                      = SequenceNumber;
            this.GeneratedAt                         = GeneratedAt;
            this.ToBeContinued                       = ToBeContinued;

            unchecked
            {

                hashCode = this.NotifyCustomerInformationRequestId.GetHashCode()       * 13 ^
                           this.Data.                              GetHashCode()       * 11 ^
                           this.SequenceNumber.                    GetHashCode()       *  7 ^
                           this.GeneratedAt.                       GetHashCode()       *  5 ^
                          (this.ToBeContinued?.                    GetHashCode() ?? 0) *  3 ^
                           base.                                   GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:2:2020:3:NotifyCustomerInformationRequest",
        //   "comment": "OCPP 2.0.1 FINAL",
        //   "definitions": {
        //     "CustomDataType": {
        //       "description": "This class does not get 'AdditionalProperties = false' in the schema generation, so it can be extended with arbitrary JSON properties to allow adding custom data.",
        //       "javaType": "CustomData",
        //       "type": "object",
        //       "properties": {
        //         "vendorId": {
        //           "type": "string",
        //           "maxLength": 255
        //         }
        //       },
        //       "required": [
        //         "vendorId"
        //       ]
        //     }
        //   },
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "customData": {
        //       "$ref": "#/definitions/CustomDataType"
        //     },
        //     "data": {
        //       "description": "(Part of) the requested data. No format specified in which the data is returned. Should be human readable.\r\n",
        //       "type": "string",
        //       "maxLength": 512
        //     },
        //     "tbc": {
        //       "description": "“to be continued” indicator. Indicates whether another part of the monitoringData follows in an upcoming notifyMonitoringReportRequest message. Default value when omitted is false.\r\n",
        //       "type": "boolean",
        //       "default": false
        //     },
        //     "seqNo": {
        //       "description": "Sequence number of this message. First message starts at 0.\r\n",
        //       "type": "integer"
        //     },
        //     "generatedAt": {
        //       "description": " Timestamp of the moment this message was generated at the Charging Station.\r\n",
        //       "type": "string",
        //       "format": "date-time"
        //     },
        //     "requestId": {
        //       "description": "The Id of the request.\r\n\r\n",
        //       "type": "integer"
        //     }
        //   },
        //   "required": [
        //     "data",
        //     "seqNo",
        //     "generatedAt",
        //     "requestId"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, RequestId, ChargingStationId, CustomNotifyCustomerInformationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a notify customer information request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="CustomNotifyCustomerInformationRequestParser">A delegate to parse custom notify customer information requests.</param>
        public static NotifyCustomerInformationRequest Parse(JObject                                                         JSON,
                                                             Request_Id                                                      RequestId,
                                                             ChargingStation_Id                                              ChargingStationId,
                                                             CustomJObjectParserDelegate<NotifyCustomerInformationRequest>?  CustomNotifyCustomerInformationRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         ChargingStationId,
                         out var notifyCustomerInformationRequest,
                         out var errorResponse,
                         CustomNotifyCustomerInformationRequestParser) &&
                notifyCustomerInformationRequest is not null)
            {
                return notifyCustomerInformationRequest;
            }

            throw new ArgumentException("The given JSON representation of a notify customer information request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, ChargingStationId, out NotifyCustomerInformationRequest, out ErrorResponse, CustomNotifyCustomerInformationRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a notify customer information request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="NotifyCustomerInformationRequest">The parsed notify customer information request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                JSON,
                                       Request_Id                             RequestId,
                                       ChargingStation_Id                     ChargingStationId,
                                       out NotifyCustomerInformationRequest?  NotifyCustomerInformationRequest,
                                       out String?                            ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        ChargingStationId,
                        out NotifyCustomerInformationRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a notify customer information request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="NotifyCustomerInformationRequest">The parsed notify customer information request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomNotifyCustomerInformationRequestParser">A delegate to parse custom notify customer information requests.</param>
        public static Boolean TryParse(JObject                                                         JSON,
                                       Request_Id                                                      RequestId,
                                       ChargingStation_Id                                              ChargingStationId,
                                       out NotifyCustomerInformationRequest?                           NotifyCustomerInformationRequest,
                                       out String?                                                     ErrorResponse,
                                       CustomJObjectParserDelegate<NotifyCustomerInformationRequest>?  CustomNotifyCustomerInformationRequestParser)
        {

            try
            {

                NotifyCustomerInformationRequest = null;

                #region NotifyCustomerInformationRequestId    [mandatory]

                if (!JSON.ParseMandatory("requestId",
                                         "notify customer information request identification",
                                         out Int32 NotifyCustomerInformationRequestId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Data                                  [mandatory]

                if (!JSON.ParseMandatoryText("data",
                                             "customer data",
                                             out String Data,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region SequenceNumber                        [mandatory]

                if (!JSON.ParseMandatory("seqNo",
                                         "sequence number",
                                         out UInt32 SequenceNumber,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region GeneratedAt                           [mandatory]

                if (!JSON.ParseMandatory("generatedAt",
                                         "generated at",
                                         out DateTime GeneratedAt,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ToBeContinued                         [optional]

                if (JSON.ParseOptional("tbc",
                                       "to be continued",
                                       out Boolean? ToBeContinued,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Signatures                            [optional, OCPP_CSE]

                if (JSON.ParseOptionalHashSet("signatures",
                                              "cryptographic signatures",
                                              Signature.TryParse,
                                              out HashSet<Signature> Signatures,
                                              out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region CustomData                            [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           OCPPv2_1.CustomData.TryParse,
                                           out CustomData CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region ChargingStationId                     [optional, OCPP_CSE]

                if (JSON.ParseOptional("chargingStationId",
                                       "charging station identification",
                                       ChargingStation_Id.TryParse,
                                       out ChargingStation_Id? chargingStationId_PayLoad,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (chargingStationId_PayLoad.HasValue)
                        ChargingStationId = chargingStationId_PayLoad.Value;

                }

                #endregion


                NotifyCustomerInformationRequest = new NotifyCustomerInformationRequest(
                                                       ChargingStationId,
                                                       NotifyCustomerInformationRequestId,
                                                       Data,
                                                       SequenceNumber,
                                                       GeneratedAt,
                                                       ToBeContinued,
                                                       null,
                                                       null,
                                                       Signatures,
                                                       CustomData,
                                                       RequestId
                                                   );

                if (CustomNotifyCustomerInformationRequestParser is not null)
                    NotifyCustomerInformationRequest = CustomNotifyCustomerInformationRequestParser(JSON,
                                                                                                    NotifyCustomerInformationRequest);

                return true;

            }
            catch (Exception e)
            {
                NotifyCustomerInformationRequest  = null;
                ErrorResponse                     = "The given JSON representation of a notify customer information request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomNotifyCustomerInformationRequestSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomNotifyCustomerInformationRequestSerializer">A delegate to serialize custom NotifyCustomerInformation requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<NotifyCustomerInformationRequest>?  CustomNotifyCustomerInformationRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<Signature>?                         CustomSignatureSerializer                          = null,
                              CustomJObjectSerializerDelegate<CustomData>?                        CustomCustomDataSerializer                         = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("requestId",     NotifyCustomerInformationRequestId),
                                 new JProperty("data",          Data),
                                 new JProperty("seqNo",         SequenceNumber),
                                 new JProperty("generatedAt",   GeneratedAt.ToIso8601()),

                           ToBeContinued.HasValue
                               ? new JProperty("tbc",           ToBeContinued)
                               : null,

                           Signatures.Any()
                               ? new JProperty("signatures",    new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                           CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",    CustomData. ToJSON(CustomCustomDataSerializer))
                               : null);

            return CustomNotifyCustomerInformationRequestSerializer is not null
                       ? CustomNotifyCustomerInformationRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (NotifyCustomerInformationRequest1, NotifyCustomerInformationRequest2)

        /// <summary>
        /// Compares two notify customer information requests for equality.
        /// </summary>
        /// <param name="NotifyCustomerInformationRequest1">A notify customer information request.</param>
        /// <param name="NotifyCustomerInformationRequest2">Another notify customer information request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (NotifyCustomerInformationRequest? NotifyCustomerInformationRequest1,
                                           NotifyCustomerInformationRequest? NotifyCustomerInformationRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(NotifyCustomerInformationRequest1, NotifyCustomerInformationRequest2))
                return true;

            // If one is null, but not both, return false.
            if (NotifyCustomerInformationRequest1 is null || NotifyCustomerInformationRequest2 is null)
                return false;

            return NotifyCustomerInformationRequest1.Equals(NotifyCustomerInformationRequest2);

        }

        #endregion

        #region Operator != (NotifyCustomerInformationRequest1, NotifyCustomerInformationRequest2)

        /// <summary>
        /// Compares two notify customer information requests for inequality.
        /// </summary>
        /// <param name="NotifyCustomerInformationRequest1">A notify customer information request.</param>
        /// <param name="NotifyCustomerInformationRequest2">Another notify customer information request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (NotifyCustomerInformationRequest? NotifyCustomerInformationRequest1,
                                           NotifyCustomerInformationRequest? NotifyCustomerInformationRequest2)

            => !(NotifyCustomerInformationRequest1 == NotifyCustomerInformationRequest2);

        #endregion

        #endregion

        #region IEquatable<NotifyCustomerInformationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two notify customer information requests for equality.
        /// </summary>
        /// <param name="Object">A notify customer information request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is NotifyCustomerInformationRequest notifyCustomerInformationRequest &&
                   Equals(notifyCustomerInformationRequest);

        #endregion

        #region Equals(NotifyCustomerInformationRequest)

        /// <summary>
        /// Compares two notify customer information requests for equality.
        /// </summary>
        /// <param name="NotifyCustomerInformationRequest">A notify customer information request to compare with.</param>
        public override Boolean Equals(NotifyCustomerInformationRequest? NotifyCustomerInformationRequest)

            => NotifyCustomerInformationRequest is not null &&

               NotifyCustomerInformationRequestId.Equals(NotifyCustomerInformationRequest.NotifyCustomerInformationRequestId) &&
               Data.                              Equals(NotifyCustomerInformationRequest.Data)                               &&
               SequenceNumber.                    Equals(NotifyCustomerInformationRequest.SequenceNumber)                     &&
               GeneratedAt.                       Equals(NotifyCustomerInformationRequest.GeneratedAt)                        &&

            ((!ToBeContinued.HasValue && !NotifyCustomerInformationRequest.ToBeContinued.HasValue) ||
               ToBeContinued.HasValue &&  NotifyCustomerInformationRequest.ToBeContinued.HasValue && ToBeContinued.Value.Equals(NotifyCustomerInformationRequest.ToBeContinued.Value)) &&

               base.GenericEquals(NotifyCustomerInformationRequest);

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

                   NotifyCustomerInformationRequestId, ", ",
                   Data.SubstringMax(20),              ", ",
                   SequenceNumber,
                   ToBeContinued.HasValue == true
                       ? ", to be continued, "
                       : ", ",
                   GeneratedAt.ToIso8601()

               );

        #endregion

    }

}
