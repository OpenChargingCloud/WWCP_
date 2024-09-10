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

using System.Xml.Linq;
using System.Diagnostics.CodeAnalysis;

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OCPPv1_6.CS;
using cloud.charging.open.protocols.WWCP;
using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.OCPP;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CP
{

    /// <summary>
    /// A reset response.
    /// </summary>
    public class ResetResponse : AResponse<CS.ResetRequest,
                                              ResetResponse>,
                                 IResponse
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/v1.6/cp/resetResponse");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext  Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The success or failure of the reset command.
        /// </summary>
        public ResetStatus    Status    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new Reset response.
        /// </summary>
        /// <param name="Request">The Reset request leading to this response.</param>
        /// <param name="Status">The success or failure of the Reset command.</param>
        /// 
        /// <param name="Result">The machine-readable result code.</param>
        /// <param name="ResponseTimestamp">The timestamp of the response message.</param>
        /// 
        /// <param name="Destination">The destination identification of the message within the overlay network.</param>
        /// <param name="NetworkPath">The networking path of the message through the overlay network.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this message.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this message.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures of this message.</param>
        /// 
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public ResetResponse(ResetRequest             Request,
                             ResetStatus              Status,

                             Result?                  Result                = null,
                             DateTime?                ResponseTimestamp     = null,

                             SourceRouting?           Destination           = null,
                             NetworkPath?             NetworkPath           = null,

                             IEnumerable<KeyPair>?    SignKeys              = null,
                             IEnumerable<SignInfo>?   SignInfos             = null,
                             IEnumerable<Signature>?  Signatures            = null,

                             CustomData?              CustomData            = null,

                             SerializationFormats?    SerializationFormat   = null,
                             CancellationToken        CancellationToken     = default)

            : base(Request,
                   Result ?? Result.OK(),
                   ResponseTimestamp,

                   Destination,
                   NetworkPath,

                   SignKeys,
                   SignInfos,
                   Signatures,

                   CustomData,

                   SerializationFormat,
                   CancellationToken)

        {

            this.Status = Status;

            unchecked
            {

                hashCode = this.Status.GetHashCode() * 3 ^
                           base.       GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:ns   = "urn://Ocpp/Cp/2015/10/">
        //    <soap:Header/>
        //    <soap:Body>
        //       <ns:resetResponse>
        //
        //          <ns:status>?</ns:status>
        //
        //       </ns:resetResponse>
        //    </soap:Body>
        // </soap:Envelope>

        // {
        //     "$schema": "http://json-schema.org/draft-04/schema#",
        //     "id":      "urn:OCPP:1.6:2019:12:ResetResponse",
        //     "title":   "ResetResponse",
        //     "type":    "object",
        //     "properties": {
        //         "status": {
        //             "type": "string",
        //             "additionalProperties": false,
        //             "enum": [
        //                 "Accepted",
        //                 "Rejected"
        //             ]
        //         }
        //     },
        //     "additionalProperties": false,
        //     "required": [
        //         "status"
        //     ]
        // }

        #endregion

        #region (static) Parse   (Request, XML)

        /// <summary>
        /// Parse the given XML representation of a reset response.
        /// </summary>
        /// <param name="Request">The reset request leading to this response.</param>
        /// <param name="XML">The XML to be parsed.</param>
        public static ResetResponse Parse(ResetRequest  Request,
                                          XElement      XML)
        {

            if (TryParse(Request,
                         XML,
                         out var resetResponse,
                         out var errorResponse))
            {
                return resetResponse;
            }

            throw new ArgumentException("The given XML representation of a reset response is invalid: " + errorResponse,
                                        nameof(XML));

        }

        #endregion

        #region (static) Parse   (Request, JSON, CustomResetResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a Reset response.
        /// </summary>
        /// <param name="Request">The Reset request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomResetResponseParser">An optional delegate to parse custom Reset responses.</param>
        public static ResetResponse Parse(ResetRequest                                 Request,
                                          JObject                                      JSON,
                                          SourceRouting                                Destination,
                                          NetworkPath                                  NetworkPath,
                                          DateTime?                                    ResponseTimestamp           = null,
                                          CustomJObjectParserDelegate<ResetResponse>?  CustomResetResponseParser   = null,
                                          CustomJObjectParserDelegate<Signature>?      CustomSignatureParser       = null,
                                          CustomJObjectParserDelegate<CustomData>?     CustomCustomDataParser      = null)
        {

            if (TryParse(Request,
                         JSON,
                         Destination,
                         NetworkPath,
                         out var resetResponse,
                         out var errorResponse,
                         ResponseTimestamp,
                         CustomResetResponseParser,
                         CustomSignatureParser,
                         CustomCustomDataParser))
            {
                return resetResponse;
            }

            throw new ArgumentException("The given JSON representation of a Reset response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Request, XML,  out ResetResponse, out ErrorResponse)

        /// <summary>
        /// Try to parse the given XML representation of a reset response.
        /// </summary>
        /// <param name="Request">The reset request leading to this response.</param>
        /// <param name="XML">The XML to be parsed.</param>
        /// <param name="ResetResponse">The parsed reset response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(ResetRequest                             Request,
                                       XElement                                 XML,
                                       [NotNullWhen(true)]  out ResetResponse?  ResetResponse,
                                       [NotNullWhen(false)] out String?         ErrorResponse)
        {

            try
            {

                ResetResponse = new ResetResponse(

                                    Request,

                                    XML.MapValueOrFail(OCPPNS.OCPPv1_6_CP + "status",
                                                       ResetStatusExtensions.Parse)

                                );

                ErrorResponse = null;
                return true;

            }
            catch (Exception e)
            {
                ResetResponse  = null;
                ErrorResponse  = "The given JSON representation of a reset response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(Request, JSON, out ResetResponse, out ErrorResponse, CustomResetResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a Reset response.
        /// </summary>
        /// <param name="Request">The Reset request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ResetResponse">The parsed Reset response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomResetResponseParser">An optional delegate to parse custom Reset responses.</param>
        public static Boolean TryParse(ResetRequest                                 Request,
                                       JObject                                      JSON,
                                       SourceRouting                                Destination,
                                       NetworkPath                                  NetworkPath,
                                       [NotNullWhen(true)]  out ResetResponse?      ResetResponse,
                                       [NotNullWhen(false)] out String?             ErrorResponse,
                                       DateTime?                                    ResponseTimestamp           = null,
                                       CustomJObjectParserDelegate<ResetResponse>?  CustomResetResponseParser   = null,
                                       CustomJObjectParserDelegate<Signature>?      CustomSignatureParser       = null,
                                       CustomJObjectParserDelegate<CustomData>?     CustomCustomDataParser      = null)
        {

            try
            {

                ResetResponse = null;

                #region Status        [optional]

                if (!JSON.MapMandatory("status",
                                       "reset status",
                                       ResetStatusExtensions.Parse,
                                       out ResetStatus Status,
                                       out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Signatures    [optional, OCPP_CSE]

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

                #region CustomData    [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           WWCP.CustomData.TryParse,
                                           out CustomData? CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ResetResponse = new ResetResponse(

                                    Request,
                                    Status,

                                    null,
                                    ResponseTimestamp,

                                    Destination,
                                    NetworkPath,

                                    null,
                                    null,
                                    Signatures,

                                    CustomData

                                );

                if (CustomResetResponseParser is not null)
                    ResetResponse = CustomResetResponseParser(JSON,
                                                              ResetResponse);

                return true;

            }
            catch (Exception e)
            {
                ResetResponse  = null;
                ErrorResponse  = "The given JSON representation of a reset response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new (OCPPNS.OCPPv1_6_CP + "resetResponse",

                   new XElement(OCPPNS.OCPPv1_6_CP + "status",  Status.AsText())

               );

        #endregion

        #region ToJSON(CustomResetResponseSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomResetResponseSerializer">A delegate to serialize custom reset responses.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ResetResponse>?  CustomResetResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<Signature>? CustomSignatureSerializer       = null,
                              CustomJObjectSerializerDelegate<CustomData>?     CustomCustomDataSerializer      = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("status",       Status.    AsText()),

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomResetResponseSerializer is not null
                       ? CustomResetResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Static methods

        /// <summary>
        /// The Reset failed because of a request error.
        /// </summary>
        /// <param name="Request">The Reset request.</param>
        public static ResetResponse RequestError(ResetRequest             Request,
                                                 EventTracking_Id         EventTrackingId,
                                                 ResultCode               ErrorCode,
                                                 String?                  ErrorDescription    = null,
                                                 JObject?                 ErrorDetails        = null,
                                                 DateTime?                ResponseTimestamp   = null,

                                                 SourceRouting?           Destination         = null,
                                                 NetworkPath?             NetworkPath         = null,

                                                 IEnumerable<KeyPair>?    SignKeys            = null,
                                                 IEnumerable<SignInfo>?   SignInfos           = null,
                                                 IEnumerable<Signature>?  Signatures          = null,

                                                 CustomData?              CustomData          = null)

            => new (

                   Request,
                   ResetStatus.Rejected,
                   Result.FromErrorResponse(
                       ErrorCode,
                       ErrorDescription,
                       ErrorDetails
                   ),
                   ResponseTimestamp,

                   Destination,
                   NetworkPath,

                   SignKeys,
                   SignInfos,
                   Signatures,

                   CustomData

               );


        /// <summary>
        /// The Reset failed.
        /// </summary>
        /// <param name="Request">The Reset request.</param>
        /// <param name="ErrorDescription">An optional error description.</param>
        public static ResetResponse FormationViolation(ResetRequest  Request,
                                                       String        ErrorDescription)

            => new (Request,
                    ResetStatus.Rejected,
                    Result:  Result.FormationViolation(
                                 $"Invalid data format: {ErrorDescription}"
                             ));


        /// <summary>
        /// The Reset failed.
        /// </summary>
        /// <param name="Request">The Reset request.</param>
        /// <param name="ErrorDescription">An optional error description.</param>
        public static ResetResponse SignatureError(ResetRequest  Request,
                                                   String        ErrorDescription)

            => new (Request,
                    ResetStatus.Rejected,
                    Result:  Result.SignatureError(
                                 $"Invalid signature(s): {ErrorDescription}"
                             ));


        /// <summary>
        /// The Reset failed.
        /// </summary>
        /// <param name="Request">The Reset request.</param>
        /// <param name="Description">An optional error description.</param>
        public static ResetResponse Failed(ResetRequest  Request,
                                           String?       Description   = null)

            => new (Request,
                    ResetStatus.Rejected,
                    Result:  Result.Server(Description));


        /// <summary>
        /// The Reset failed because of an exception.
        /// </summary>
        /// <param name="Request">The Reset request.</param>
        /// <param name="Exception">The exception.</param>
        public static ResetResponse ExceptionOccured(ResetRequest  Request,
                                                     Exception     Exception)

            => new (Request,
                    ResetStatus.Rejected,
                    Result:  Result.FromException(Exception));

        #endregion


        #region Operator overloading

        #region Operator == (ResetResponse1, ResetResponse2)

        /// <summary>
        /// Compares two reset responses for equality.
        /// </summary>
        /// <param name="ResetResponse1">A reset response.</param>
        /// <param name="ResetResponse2">Another reset response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ResetResponse? ResetResponse1,
                                           ResetResponse? ResetResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ResetResponse1, ResetResponse2))
                return true;

            // If one is null, but not both, return false.
            if (ResetResponse1 is null || ResetResponse2 is null)
                return false;

            return ResetResponse1.Equals(ResetResponse2);

        }

        #endregion

        #region Operator != (ResetResponse1, ResetResponse2)

        /// <summary>
        /// Compares two reset responses for inequality.
        /// </summary>
        /// <param name="ResetResponse1">A reset response.</param>
        /// <param name="ResetResponse2">Another reset response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ResetResponse? ResetResponse1,
                                           ResetResponse? ResetResponse2)

            => !(ResetResponse1 == ResetResponse2);

        #endregion

        #endregion

        #region IEquatable<ResetResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two reset responses for equality.
        /// </summary>
        /// <param name="Object">A reset response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ResetResponse resetResponse &&
                   Equals(resetResponse);

        #endregion

        #region Equals(ResetResponse)

        /// <summary>
        /// Compares two reset responses for equality.
        /// </summary>
        /// <param name="ResetResponse">A reset response to compare with.</param>
        public override Boolean Equals(ResetResponse? ResetResponse)

            => ResetResponse is not null &&
                   Status.Equals(ResetResponse.Status);

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

            => Status.ToString();

        #endregion

    }

}