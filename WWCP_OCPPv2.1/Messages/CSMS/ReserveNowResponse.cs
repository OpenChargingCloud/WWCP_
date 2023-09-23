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
    /// A reserve now response.
    /// </summary>
    public class ReserveNowResponse : AResponse<CSMS.ReserveNowRequest,
                                                ReserveNowResponse>
    {

        #region Properties

        /// <summary>
        /// The success or failure of the reservation.
        /// </summary>
        [Mandatory]
        public ReservationStatus  Status        { get; }

        /// <summary>
        /// Optional detailed status information.
        /// </summary>
        [Optional]
        public StatusInfo?        StatusInfo    { get; }

        #endregion

        #region Constructor(s)

        #region ReserveNowResponse(Request, Status, StatusInfo = null, CustomData = null)

        /// <summary>
        /// Create a new reserve now response.
        /// </summary>
        /// <param name="Request">The reserve now request leading to this response.</param>
        /// <param name="Status">The success or failure of the reservation.</param>
        /// <param name="StatusInfo">Optional detailed status information.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        public ReserveNowResponse(CSMS.ReserveNowRequest   Request,
                                  ReservationStatus        Status,
                                  StatusInfo?              StatusInfo   = null,

                                  IEnumerable<Signature>?  Signatures   = null,
                                  CustomData?              CustomData   = null)

            : base(Request,
                   Result.OK(),
                   Signatures,
                   CustomData)

        {

            this.Status      = Status;
            this.StatusInfo  = StatusInfo;

        }

        #endregion

        #region ReserveNowResponse(Request, Result)

        /// <summary>
        /// Create a new reserve now response.
        /// </summary>
        /// <param name="Request">The reserve now request leading to this response.</param>
        /// <param name="Result">The result.</param>
        public ReserveNowResponse(CSMS.ReserveNowRequest  Request,
                                  Result                  Result)

            : base(Request,
                   Result,
                   Timestamp.Now)

        { }

        #endregion

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:2:2020:3:ReserveNowResponse",
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
        //     },
        //     "ReserveNowStatusEnumType": {
        //       "description": "This indicates the success or failure of the reservation.\r\n",
        //       "javaType": "ReserveNowStatusEnum",
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "Accepted",
        //         "Faulted",
        //         "Occupied",
        //         "Rejected",
        //         "Unavailable"
        //       ]
        //     },
        //     "StatusInfoType": {
        //       "description": "Element providing more information about the status.\r\n",
        //       "javaType": "StatusInfo",
        //       "type": "object",
        //       "additionalProperties": false,
        //       "properties": {
        //         "customData": {
        //           "$ref": "#/definitions/CustomDataType"
        //         },
        //         "reasonCode": {
        //           "description": "A predefined code for the reason why the status is returned in this response. The string is case-insensitive.\r\n",
        //           "type": "string",
        //           "maxLength": 20
        //         },
        //         "additionalInfo": {
        //           "description": "Additional text to provide detailed information.\r\n",
        //           "type": "string",
        //           "maxLength": 512
        //         }
        //       },
        //       "required": [
        //         "reasonCode"
        //       ]
        //     }
        //   },
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "customData": {
        //       "$ref": "#/definitions/CustomDataType"
        //     },
        //     "status": {
        //       "$ref": "#/definitions/ReserveNowStatusEnumType"
        //     },
        //     "statusInfo": {
        //       "$ref": "#/definitions/StatusInfoType"
        //     }
        //   },
        //   "required": [
        //     "status"
        //   ]
        // }

        #endregion

        #region (static) Parse   (Request, JSON, CustomReserveNowResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a reserve now response.
        /// </summary>
        /// <param name="Request">The reserve now request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomReserveNowResponseParser">A delegate to parse custom reserve now responses.</param>
        public static ReserveNowResponse Parse(CSMS.ReserveNowRequest                            Request,
                                               JObject                                           JSON,
                                               CustomJObjectParserDelegate<ReserveNowResponse>?  CustomReserveNowResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out var reserveNowResponse,
                         out var errorResponse,
                         CustomReserveNowResponseParser))
            {
                return reserveNowResponse!;
            }

            throw new ArgumentException("The given JSON representation of a reserve now response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Request, JSON, out ReserveNowResponse, out ErrorResponse, CustomReserveNowResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a reserve now response.
        /// </summary>
        /// <param name="Request">The reserve now request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ReserveNowResponse">The parsed reserve now response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomReserveNowResponseParser">A delegate to parse custom reserve now responses.</param>
        public static Boolean TryParse(CSMS.ReserveNowRequest                            Request,
                                       JObject                                           JSON,
                                       out ReserveNowResponse?                           ReserveNowResponse,
                                       out String?                                       ErrorResponse,
                                       CustomJObjectParserDelegate<ReserveNowResponse>?  CustomReserveNowResponseParser   = null)
        {

            try
            {

                ReserveNowResponse = null;

                #region ReservationStatus    [mandatory]

                if (!JSON.ParseMandatory("status",
                                         "reservation status",
                                         ReservationStatusExtensions.TryParse,
                                         out ReservationStatus ReservationStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region StatusInfo           [optional]

                if (JSON.ParseOptionalJSON("statusInfo",
                                           "detailed status info",
                                           OCPPv2_1.StatusInfo.TryParse,
                                           out StatusInfo? StatusInfo,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Signatures           [optional, OCPP_CSE]

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

                #region CustomData           [optional]

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


                ReserveNowResponse = new ReserveNowResponse(
                                         Request,
                                         ReservationStatus,
                                         StatusInfo,
                                         Signatures,
                                         CustomData
                                     );

                if (CustomReserveNowResponseParser is not null)
                    ReserveNowResponse = CustomReserveNowResponseParser(JSON,
                                                                        ReserveNowResponse);

                return true;

            }
            catch (Exception e)
            {
                ReserveNowResponse  = null;
                ErrorResponse       = "The given JSON representation of a reserve now response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomReserveNowResponseSerializer = null, CustomStatusInfoSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomReserveNowResponseSerializer">A delegate to serialize custom reserve now responses.</param>
        /// <param name="CustomStatusInfoSerializer">A delegate to serialize a custom status infos.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ReserveNowResponse>?  CustomReserveNowResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusInfo>?          CustomStatusInfoSerializer           = null,
                              CustomJObjectSerializerDelegate<Signature>?           CustomSignatureSerializer            = null,
                              CustomJObjectSerializerDelegate<CustomData>?          CustomCustomDataSerializer           = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("status",       Status.    AsText()),

                           StatusInfo is not null
                               ? new JProperty("statusInfo",   StatusInfo.ToJSON(CustomStatusInfoSerializer,
                                                                                 CustomCustomDataSerializer))
                               : null,

                           Signatures is not null
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomReserveNowResponseSerializer is not null
                       ? CustomReserveNowResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Static methods

        /// <summary>
        /// The reserve now failed.
        /// </summary>
        /// <param name="Request">The reserve now request leading to this response.</param>
        public static ReserveNowResponse Failed(CSMS.ReserveNowRequest Request)

            => new (Request,
                    Result.Server());

        #endregion


        #region Operator overloading

        #region Operator == (ReserveNowResponse1, ReserveNowResponse2)

        /// <summary>
        /// Compares two reserve now responses for equality.
        /// </summary>
        /// <param name="ReserveNowResponse1">A reserve now response.</param>
        /// <param name="ReserveNowResponse2">Another reserve now response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (ReserveNowResponse? ReserveNowResponse1,
                                           ReserveNowResponse? ReserveNowResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ReserveNowResponse1, ReserveNowResponse2))
                return true;

            // If one is null, but not both, return false.
            if (ReserveNowResponse1 is null || ReserveNowResponse2 is null)
                return false;

            return ReserveNowResponse1.Equals(ReserveNowResponse2);

        }

        #endregion

        #region Operator != (ReserveNowResponse1, ReserveNowResponse2)

        /// <summary>
        /// Compares two reserve now responses for inequality.
        /// </summary>
        /// <param name="ReserveNowResponse1">A reserve now response.</param>
        /// <param name="ReserveNowResponse2">Another reserve now response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (ReserveNowResponse? ReserveNowResponse1,
                                           ReserveNowResponse? ReserveNowResponse2)

            => !(ReserveNowResponse1 == ReserveNowResponse2);

        #endregion

        #endregion

        #region IEquatable<ReserveNowResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two reserve now responses for equality.
        /// </summary>
        /// <param name="Object">A reserve now response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ReserveNowResponse reserveNowResponse &&
                   Equals(reserveNowResponse);

        #endregion

        #region Equals(ReserveNowResponse)

        /// <summary>
        /// Compares two reserve now responses for equality.
        /// </summary>
        /// <param name="ReserveNowResponse">A reserve now response to compare with.</param>
        public override Boolean Equals(ReserveNowResponse? ReserveNowResponse)

            => ReserveNowResponse is not null &&

               Status.     Equals(ReserveNowResponse.Status) &&

             ((StatusInfo is     null && ReserveNowResponse.StatusInfo is     null) ||
               StatusInfo is not null && ReserveNowResponse.StatusInfo is not null && StatusInfo.Equals(ReserveNowResponse.StatusInfo)) &&

               base.GenericEquals(ReserveNowResponse);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Status.     GetHashCode()       * 5 ^
                      (StatusInfo?.GetHashCode() ?? 0) * 3 ^

                       base.       GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => Status.AsText();

        #endregion

    }

}
