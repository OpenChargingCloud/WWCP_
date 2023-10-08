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
    /// A set display message response.
    /// </summary>
    public class SetDisplayMessageResponse : AResponse<CSMS.SetDisplayMessageRequest,
                                                       SetDisplayMessageResponse>,
                                             IResponse
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/cs/setDisplayMessageResponse");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext         Context
            => DefaultJSONLDContext;

        /// <summary>
        /// Whether the charging station is able to display the message.
        /// </summary>
        [Mandatory]
        public DisplayMessageStatus  Status        { get; }

        /// <summary>
        /// Optional detailed status information.
        /// </summary>
        [Optional]
        public StatusInfo?           StatusInfo    { get; }

        #endregion

        #region Constructor(s)

        #region SetDisplayMessageResponse(Request, Status, StatusInfo = null, CustomData = null)

        /// <summary>
        /// Create a new set display message response.
        /// </summary>
        /// <param name="Request">The set display message request leading to this response.</param>
        /// <param name="Status">Whether the charging station is able to display the message.</param>
        /// <param name="StatusInfo">Optional detailed status information.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        public SetDisplayMessageResponse(CSMS.SetDisplayMessageRequest  Request,
                                         DisplayMessageStatus           Status,
                                         StatusInfo?                    StatusInfo        = null,

                                         IEnumerable<KeyPair>?          SignKeys          = null,
                                         IEnumerable<SignInfo>?         SignInfos         = null,
                                         SignaturePolicy?               SignaturePolicy   = null,
                                         IEnumerable<Signature>?        Signatures        = null,

                                         DateTime?                      Timestamp         = null,
                                         CustomData?                    CustomData        = null)

            : base(Request,
                   Result.OK(),
                   SignKeys,
                   SignInfos,
                   SignaturePolicy,
                   Signatures,
                   Timestamp,
                   CustomData)

        {

            this.Status      = Status;
            this.StatusInfo  = StatusInfo;

        }

        #endregion

        #region SetDisplayMessageResponse(Request, Result)

        /// <summary>
        /// Create a new set display message response.
        /// </summary>
        /// <param name="Request">The set display message request leading to this response.</param>
        /// <param name="Result">The result.</param>
        public SetDisplayMessageResponse(CSMS.SetDisplayMessageRequest  Request,
                                         Result                         Result)

            : base(Request,
                   Result)

        { }

        #endregion

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:2:2020:3:SetDisplayMessageResponse",
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
        //     "DisplayMessageStatusEnumType": {
        //       "description": "This indicates whether the Charging Station is able to display the message.\r\n",
        //       "javaType": "DisplayMessageStatusEnum",
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "Accepted",
        //         "NotSupportedMessageFormat",
        //         "Rejected",
        //         "NotSupportedPriority",
        //         "NotSupportedState",
        //         "UnknownTransaction"
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
        //       "$ref": "#/definitions/DisplayMessageStatusEnumType"
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

        #region (static) Parse   (Request, JSON, CustomSetDisplayMessageResponseParser = null)

        /// <summary>
        /// Parse the given JSON representation of a set display message response.
        /// </summary>
        /// <param name="Request">The set display message request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomSetDisplayMessageResponseParser">A delegate to parse custom set display message responses.</param>
        public static SetDisplayMessageResponse Parse(CSMS.SetDisplayMessageRequest                              Request,
                                                      JObject                                                  JSON,
                                                      CustomJObjectParserDelegate<SetDisplayMessageResponse>?  CustomSetDisplayMessageResponseParser   = null)
        {

            if (TryParse(Request,
                         JSON,
                         out var setDisplayMessageResponse,
                         out var errorResponse,
                         CustomSetDisplayMessageResponseParser))
            {
                return setDisplayMessageResponse!;
            }

            throw new ArgumentException("The given JSON representation of a set display message response is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(Request, JSON, out SetDisplayMessageResponse, out ErrorResponse, CustomBootNotificationResponseParser = null)

        /// <summary>
        /// Try to parse the given JSON representation of a set display message response.
        /// </summary>
        /// <param name="Request">The set display message request leading to this response.</param>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="SetDisplayMessageResponse">The parsed set display message response.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSetDisplayMessageResponseParser">A delegate to parse custom set display message responses.</param>
        public static Boolean TryParse(CSMS.SetDisplayMessageRequest                              Request,
                                       JObject                                                  JSON,
                                       out SetDisplayMessageResponse?                           SetDisplayMessageResponse,
                                       out String?                                              ErrorResponse,
                                       CustomJObjectParserDelegate<SetDisplayMessageResponse>?  CustomSetDisplayMessageResponseParser   = null)
        {

            try
            {

                SetDisplayMessageResponse = null;

                #region Status        [mandatory]

                if (!JSON.ParseMandatory("status",
                                         "display message status",
                                         DisplayMessageStatusExtensions.TryParse,
                                         out DisplayMessageStatus Status,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region StatusInfo    [optional]

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
                                           OCPPv2_1.CustomData.TryParse,
                                           out CustomData CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                SetDisplayMessageResponse = new SetDisplayMessageResponse(
                                                Request,
                                                Status,
                                                StatusInfo,
                                                null,
                                                null,
                                                null,
                                                Signatures,
                                                null,
                                                CustomData
                                            );

                if (CustomSetDisplayMessageResponseParser is not null)
                    SetDisplayMessageResponse = CustomSetDisplayMessageResponseParser(JSON,
                                                                                      SetDisplayMessageResponse);

                return true;

            }
            catch (Exception e)
            {
                SetDisplayMessageResponse  = null;
                ErrorResponse              = "The given JSON representation of a set display message response is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSetDisplayMessageResponseSerializer = null, CustomStatusInfoSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSetDisplayMessageResponseSerializer">A delegate to serialize custom charging profile responses.</param>
        /// <param name="CustomStatusInfoSerializer">A delegate to serialize a custom status infos.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<SetDisplayMessageResponse>?  CustomSetDisplayMessageResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<StatusInfo>?                 CustomStatusInfoSerializer                  = null,
                              CustomJObjectSerializerDelegate<Signature>?                  CustomSignatureSerializer                   = null,
                              CustomJObjectSerializerDelegate<CustomData>?                 CustomCustomDataSerializer                  = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("status",      Status.    AsText()),

                           StatusInfo is not null
                               ? new JProperty("statusInfo",  StatusInfo.ToJSON(CustomStatusInfoSerializer,
                                                                                CustomCustomDataSerializer))
                               : null,

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",  CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomSetDisplayMessageResponseSerializer is not null
                       ? CustomSetDisplayMessageResponseSerializer(this, json)
                       : json;

        }

        #endregion


        #region Static methods

        /// <summary>
        /// The set display message command failed.
        /// </summary>
        /// <param name="Request">The set display message request leading to this response.</param>
        public static SetDisplayMessageResponse Failed(CSMS.SetDisplayMessageRequest Request)

            => new (Request,
                    Result.Server());

        #endregion


        #region Operator overloading

        #region Operator == (SetDisplayMessageResponse1, SetDisplayMessageResponse2)

        /// <summary>
        /// Compares two set display message responses for equality.
        /// </summary>
        /// <param name="SetDisplayMessageResponse1">A set display message response.</param>
        /// <param name="SetDisplayMessageResponse2">Another set display message response.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (SetDisplayMessageResponse? SetDisplayMessageResponse1,
                                           SetDisplayMessageResponse? SetDisplayMessageResponse2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(SetDisplayMessageResponse1, SetDisplayMessageResponse2))
                return true;

            // If one is null, but not both, return false.
            if (SetDisplayMessageResponse1 is null || SetDisplayMessageResponse2 is null)
                return false;

            return SetDisplayMessageResponse1.Equals(SetDisplayMessageResponse2);

        }

        #endregion

        #region Operator != (SetDisplayMessageResponse1, SetDisplayMessageResponse2)

        /// <summary>
        /// Compares two set display message responses for inequality.
        /// </summary>
        /// <param name="SetDisplayMessageResponse1">A set display message response.</param>
        /// <param name="SetDisplayMessageResponse2">Another set display message response.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (SetDisplayMessageResponse? SetDisplayMessageResponse1,
                                           SetDisplayMessageResponse? SetDisplayMessageResponse2)

            => !(SetDisplayMessageResponse1 == SetDisplayMessageResponse2);

        #endregion

        #endregion

        #region IEquatable<SetDisplayMessageResponse> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two set display message responses for equality.
        /// </summary>
        /// <param name="Object">A set display message response to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is SetDisplayMessageResponse setDisplayMessageResponse &&
                   Equals(setDisplayMessageResponse);

        #endregion

        #region Equals(SetDisplayMessageResponse)

        /// <summary>
        /// Compares two set display message responses for equality.
        /// </summary>
        /// <param name="SetDisplayMessageResponse">A set display message response to compare with.</param>
        public override Boolean Equals(SetDisplayMessageResponse? SetDisplayMessageResponse)

            => SetDisplayMessageResponse is not null &&

               Status.     Equals(SetDisplayMessageResponse.Status) &&

             ((StatusInfo is     null && SetDisplayMessageResponse.StatusInfo is     null) ||
               StatusInfo is not null && SetDisplayMessageResponse.StatusInfo is not null && StatusInfo.Equals(SetDisplayMessageResponse.StatusInfo)) &&

               base.GenericEquals(SetDisplayMessageResponse);

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
