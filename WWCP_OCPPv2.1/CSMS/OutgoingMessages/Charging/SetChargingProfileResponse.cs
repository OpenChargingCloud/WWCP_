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

using cloud.charging.open.protocols.OCPP;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CS
{

    /// <summary>
    /// A set charging profile response.
    /// </summary>
    public class SetChargingProfileResponse : AResponse<CSMS.SetChargingProfileRequest,
                                                        SetChargingProfileResponse>,
                                              IResponse
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/v2.1/cs/setChargingProfileResponse");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext          Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The success or failure of the set charging profile command.
        /// </summary>
        [Mandatory]
        public ChargingProfileStatus  Status        { get; }

        /// <summary>
        /// Optional detailed status information.
        /// </summary>
        [Optional]
        public StatusInfo?            StatusInfo    { get; }

        #endregion

        #region Constructor(s)

        #region SetChargingProfileResponse(Request, Status, StatusInfo = null, ...)

        /// <summary>
        /// Create a new set charging profile response.
        /// </summary>
        /// <param name="Request">The set charging profile request leading to this response.</param>
        /// <param name="Status">The success or failure of the set charging profile command.</param>
        /// <param name="StatusInfo">Optional detailed status information.</param>
        /// <param name="ResponseTimestamp">An optional response timestamp.</param>
        /// 
        /// <param name="SignKeys">An optional enumeration of keys to be used for signing this response.</param>
        /// <param name="SignInfos">An optional enumeration of information to be used for signing this response.</param>
        /// <param name="Signatures">An optional enumeration of cryptographic signatures.</param>
        /// 
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public SetChargingProfileResponse(CSMS.SetChargingProfileRequest  Request,
                                          ChargingProfileStatus           Status,
                                          StatusInfo?                     StatusInfo          = null,
                                          DateTime?                       ResponseTimestamp   = null,

                                          IEnumerable<KeyPair>?           SignKeys            = null,
                                          IEnumerable<SignInfo>?          SignInfos           = null,
                                          IEnumerable<OCPP.Signature>?    Signatures          = null,

                                          CustomData?                     CustomData          = null)

            : base(Request,
                   Result.OK(),
                   ResponseTimestamp,

                   null,
                   null,

                   SignKeys,
                   SignInfos,
                   Signatures,

                   CustomData)

        {

            this.Status      = Status;
            this.StatusInfo  = StatusInfo;

        }

        #endregion

        #region SetChargingProfileResponse(Request, Result)

        /// <summary>
        /// Create a new set charging profile response.
        /// </summary>
        /// <param name="Request">The set charging profile request leading to this response.</param>
        /// <param name="Result">The result.</param>
        public SetChargingProfileResponse(CSMS.SetChargingProfileRequest  Request,
                                          Result                          Result)

            : base(Request,
                   Result)

        { }

        #endregion

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:2:2020:3:SetChargingProfileResponse",
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
        //     "ChargingProfileStatusEnumType": {
        //       "description": "Returns whether the Charging Station has been able to process the message successfully. This does not guarantee the schedule will be followed to the letter. There might be other constraints the Charging Station may need to take into account.\r\n",
        //       "javaType": "ChargingProfileStatusEnum",
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "Accepted",
        //         "Rejected"
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
        //       "$ref": "#/definitions/ChargingProfileStatusEnumType"
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

        #region (static) Parse   (Request, JSON, CustomSetChargingProfileResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a set charging profile response.
        /// </summary>
        /// <param name="Request">The set charging profile request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomSetChargingProfileResponseParser">A delegate to parse custom set charging profile responses.</param>
        public static SetChargingProfileResponse Parse(CSMS.SetChargingProfileRequest                            Request,
                                                       JObject                                                   JSON,
                                                       CustomJObjectParserDelegate<SetChargingProfileResponse>?  CustomSetChargingProfileResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out var setChargingProfileResponse,
                         out var errorResponse,
                         CustomSetChargingProfileResponseParser))
            {
                return setChargingProfileResponse;
            }

            throw new ArgumentException("The given JSON representation of a set charging profile response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Request, JSON, out SetChargingProfileResponse, out ErrorResponse, CustomBootNotificationResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a set charging profile response.
        /// </summary>
        /// <param name="Request">The set charging profile request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SetChargingProfileResponse">The parsed set charging profile response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSetChargingProfileResponseParser">A delegate to parse custom set charging profile responses.</param>
        public static Boolean TryParse(CSMS.SetChargingProfileRequest                            Request,
                                       JObject                                                   JSON,
                                       [NotNullWhen(true)]  out SetChargingProfileResponse?      SetChargingProfileResponse,
                                       [NotNullWhen(false)] out String?                          ErrorResponse,
                                       CustomJObjectParserDelegate<SetChargingProfileResponse>?  CustomSetChargingProfileResponseParser   = null)
        {

            try
            {

                SetChargingProfileResponse = null;

                #region ChargingProfileStatus    [mandatory]

                if (!JSON.ParseMandatory("status",
                                         "charging profile status",
                                         ChargingProfileStatusExtensions.TryParse,
                                         out ChargingProfileStatus ChargingProfileStatus,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region StatusInfo               [optional]

                if (JSON.ParseOptionalJSON("statusInfo",
                                           "detailed status info",
                                           OCPP.StatusInfo.TryParse,
                                           out StatusInfo? StatusInfo,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Signatures               [optional, OCPP_CSE]

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

                #region CustomData               [optional]

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


                SetChargingProfileResponse = new SetChargingProfileResponse(
                                                 Request,
                                                 ChargingProfileStatus,
                                                 StatusInfo,
                                                 null,
                                                 null,
                                                 null,
                                                 Signatures,
                                                 CustomData
                                             );

                if (CustomSetChargingProfileResponseParser is not null)
                    SetChargingProfileResponse = CustomSetChargingProfileResponseParser(JSON,
                                                                                        SetChargingProfileResponse);

                return true;

            }
            catch (Exception e)
            {
                SetChargingProfileResponse  = null;
                ErrorResponse               = "The given JSON representation of a set charging profile response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSetChargingProfileResponseSerializer = null, CustomStatusInfoSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSetChargingProfileResponseSerializer">A delegate to serialize custom charging profile responses.</param>
        /// <param name="CustomStatusInfoSerializer">A delegate to serialize a custom status infos.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<SetChargingProfileResponse>?  CustomSetChargingProfileResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusInfo>?                  CustomStatusInfoSerializer                   = null,
                              CustomJObjectSerializerDelegate<OCPP.Signature>?              CustomSignatureSerializer                    = null,
                              CustomJObjectSerializerDelegate<CustomData>?                  CustomCustomDataSerializer                   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("status",       Status.    AsText()),

                           StatusInfo is not null
                               ? new JProperty("statusInfo",   StatusInfo.ToJSON(CustomStatusInfoSerializer,
                                                                                 CustomCustomDataSerializer))
                               : null,

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomSetChargingProfileResponseSerializer is not null
                       ? CustomSetChargingProfileResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Static methods

        /// <summary>
        /// The set charging profile command failed.
        /// </summary>
        /// <param name="Request">The set charging profile request leading to this response.</param>
        public static SetChargingProfileResponse Failed(CSMS.SetChargingProfileRequest Request)

            => new (Request,
                    Result.Server());

        #endregion


        #region Operator overloading

        #region Operator == (SetChargingProfileResponse1, SetChargingProfileResponse2)

        /// <summary>
        /// Compares two set charging profile responses for equality.
        /// </summary>
        /// <param name="SetChargingProfileResponse1">A set charging profile response.</param>
        /// <param name="SetChargingProfileResponse2">Another set charging profile response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetChargingProfileResponse? SetChargingProfileResponse1,
                                           SetChargingProfileResponse? SetChargingProfileResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetChargingProfileResponse1, SetChargingProfileResponse2))
                return true;

            // If one is null, but not both, return false.
            if (SetChargingProfileResponse1 is null || SetChargingProfileResponse2 is null)
                return false;

            return SetChargingProfileResponse1.Equals(SetChargingProfileResponse2);

        }

        #endregion

        #region Operator != (SetChargingProfileResponse1, SetChargingProfileResponse2)

        /// <summary>
        /// Compares two set charging profile responses for inequality.
        /// </summary>
        /// <param name="SetChargingProfileResponse1">A set charging profile response.</param>
        /// <param name="SetChargingProfileResponse2">Another set charging profile response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetChargingProfileResponse? SetChargingProfileResponse1,
                                           SetChargingProfileResponse? SetChargingProfileResponse2)

            => !(SetChargingProfileResponse1 == SetChargingProfileResponse2);

        #endregion

        #endregion

        #region IEquatable<SetChargingProfileResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two set charging profile responses for equality.
        /// </summary>
        /// <param name="Object">A set charging profile response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SetChargingProfileResponse setChargingProfileResponse &&
                   Equals(setChargingProfileResponse);

        #endregion

        #region Equals(SetChargingProfileResponse)

        /// <summary>
        /// Compares two set charging profile responses for equality.
        /// </summary>
        /// <param name="SetChargingProfileResponse">A set charging profile response to compare with.</param>
        public override Boolean Equals(SetChargingProfileResponse? SetChargingProfileResponse)

            => SetChargingProfileResponse is not null &&

               Status.     Equals(SetChargingProfileResponse.Status) &&

             ((StatusInfo is     null && SetChargingProfileResponse.StatusInfo is     null) ||
               StatusInfo is not null && SetChargingProfileResponse.StatusInfo is not null && StatusInfo.Equals(SetChargingProfileResponse.StatusInfo)) &&

               base.GenericEquals(SetChargingProfileResponse);

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
