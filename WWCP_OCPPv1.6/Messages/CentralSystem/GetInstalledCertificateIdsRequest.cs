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

namespace cloud.charging.open.protocols.OCPPv1_6.CS
{

    /// <summary>
    /// The get installed certificate ids request.
    /// </summary>
    public class GetInstalledCertificateIdsRequest : ARequest<GetInstalledCertificateIdsRequest>
    {

        #region Properties

        /// <summary>
        /// The type of the certificates requested.
        /// </summary>
        public CertificateUse  CertificateType    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new get installed certificate ids request.
        /// </summary>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="CertificateType">The type of the certificates requested.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        public GetInstalledCertificateIdsRequest(ChargeBox_Id       ChargeBoxId,
                                                 CertificateUse     CertificateType,

                                                 Request_Id?        RequestId           = null,
                                                 DateTime?          RequestTimestamp    = null,
                                                 TimeSpan?          RequestTimeout      = null,
                                                 EventTracking_Id?  EventTrackingId     = null,
                                                 CancellationToken  CancellationToken   = default)

            : base(ChargeBoxId,
                   "GetInstalledCertificateIds",
                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   CancellationToken)

        {

            this.CertificateType = CertificateType;

        }

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:1.6:2020:3:GetInstalledCertificateIds.req",
        //   "definitions": {
        //     "CertificateUseEnumType": {
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "CentralSystemRootCertificate",
        //         "ManufacturerRootCertificate"
        //       ]
        //     }
        // },
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "certificateType": {
        //         "$ref": "#/definitions/CertificateUseEnumType"
        //     }
        // },
        //   "required": [
        //     "certificateType"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, RequestId, ChargeBoxId, CustomGetInstalledCertificateIdsRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a get installed certificate ids request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="CustomGetInstalledCertificateIdsRequestParser">A delegate to parse custom get installed certificate ids requests.</param>
        public static GetInstalledCertificateIdsRequest Parse(JObject                                                          JSON,
                                                              Request_Id                                                       RequestId,
                                                              ChargeBox_Id                                                     ChargeBoxId,
                                                              CustomJObjectParserDelegate<GetInstalledCertificateIdsRequest>?  CustomGetInstalledCertificateIdsRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         ChargeBoxId,
                         out var getInstalledCertificateIdsRequest,
                         out var errorResponse,
                         CustomGetInstalledCertificateIdsRequestParser))
            {
                return getInstalledCertificateIdsRequest!;
            }

            throw new ArgumentException("The given JSON representation of a get installed certificate ids request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, ChargeBoxId, out GetInstalledCertificateIdsRequest, out ErrorResponse, CustomGetInstalledCertificateIdsRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a get installed certificate ids request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="GetInstalledCertificateIdsRequest">The parsed get installed certificate ids request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                 JSON,
                                       Request_Id                              RequestId,
                                       ChargeBox_Id                            ChargeBoxId,
                                       out GetInstalledCertificateIdsRequest?  GetInstalledCertificateIdsRequest,
                                       out String?                             ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        ChargeBoxId,
                        out GetInstalledCertificateIdsRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a get installed certificate ids request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="GetInstalledCertificateIdsRequest">The parsed get installed certificate ids request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomGetInstalledCertificateIdsRequestParser">A delegate to parse custom get installed certificate ids requests.</param>
        public static Boolean TryParse(JObject                                                          JSON,
                                       Request_Id                                                       RequestId,
                                       ChargeBox_Id                                                     ChargeBoxId,
                                       out GetInstalledCertificateIdsRequest?                           GetInstalledCertificateIdsRequest,
                                       out String?                                                      ErrorResponse,
                                       CustomJObjectParserDelegate<GetInstalledCertificateIdsRequest>?  CustomGetInstalledCertificateIdsRequestParser)
        {

            try
            {

                GetInstalledCertificateIdsRequest = null;

                #region CertificateType    [mandatory]

                if (!JSON.MapMandatory("certificateType",
                                       "certificate type",
                                       CertificateUseExtensions.Parse,
                                       out CertificateUse CertificateType,
                                       out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ChargeBoxId        [optional, OCPP_CSE]

                if (JSON.ParseOptional("chargeBoxId",
                                       "charge box identification",
                                       ChargeBox_Id.TryParse,
                                       out ChargeBox_Id? chargeBoxId_PayLoad,
                                       out ErrorResponse))
                {

                    if (ErrorResponse is not null)
                        return false;

                    if (chargeBoxId_PayLoad.HasValue)
                        ChargeBoxId = chargeBoxId_PayLoad.Value;

                }

                #endregion


                GetInstalledCertificateIdsRequest = new GetInstalledCertificateIdsRequest(ChargeBoxId,
                                                                                          CertificateType,
                                                                                          RequestId);

                if (CustomGetInstalledCertificateIdsRequestParser is not null)
                    GetInstalledCertificateIdsRequest = CustomGetInstalledCertificateIdsRequestParser(JSON,
                                                                                                      GetInstalledCertificateIdsRequest);

                return true;

            }
            catch (Exception e)
            {
                GetInstalledCertificateIdsRequest  = null;
                ErrorResponse                      = "The given JSON representation of a get installed certificate ids request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomGetInstalledCertificateIdsRequestSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomGetInstalledCertificateIdsRequestSerializer">A delegate to serialize custom start transaction requests.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetInstalledCertificateIdsRequest>? CustomGetInstalledCertificateIdsRequestSerializer = null)
        {

            var json = JSONObject.Create(
                           new JProperty("certificateType", CertificateType.AsText())
                       );

            return CustomGetInstalledCertificateIdsRequestSerializer is not null
                       ? CustomGetInstalledCertificateIdsRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetInstalledCertificateIdsRequest1, GetInstalledCertificateIdsRequest2)

        /// <summary>
        /// Compares two get installed certificate ids requests for equality.
        /// </summary>
        /// <param name="GetInstalledCertificateIdsRequest1">A get installed certificate ids request.</param>
        /// <param name="GetInstalledCertificateIdsRequest2">Another get installed certificate ids request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetInstalledCertificateIdsRequest? GetInstalledCertificateIdsRequest1,
                                           GetInstalledCertificateIdsRequest? GetInstalledCertificateIdsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetInstalledCertificateIdsRequest1, GetInstalledCertificateIdsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (GetInstalledCertificateIdsRequest1 is null || GetInstalledCertificateIdsRequest2 is null)
                return false;

            return GetInstalledCertificateIdsRequest1.Equals(GetInstalledCertificateIdsRequest2);

        }

        #endregion

        #region Operator != (GetInstalledCertificateIdsRequest1, GetInstalledCertificateIdsRequest2)

        /// <summary>
        /// Compares two get installed certificate ids requests for inequality.
        /// </summary>
        /// <param name="GetInstalledCertificateIdsRequest1">A get installed certificate ids request.</param>
        /// <param name="GetInstalledCertificateIdsRequest2">Another get installed certificate ids request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetInstalledCertificateIdsRequest? GetInstalledCertificateIdsRequest1,
                                           GetInstalledCertificateIdsRequest? GetInstalledCertificateIdsRequest2)

            => !(GetInstalledCertificateIdsRequest1 == GetInstalledCertificateIdsRequest2);

        #endregion

        #endregion

        #region IEquatable<GetInstalledCertificateIdsRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two get installed certificate ids requests for equality.
        /// </summary>
        /// <param name="Object">A get installed certificate ids request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is GetInstalledCertificateIdsRequest getInstalledCertificateIdsRequest &&
                   Equals(getInstalledCertificateIdsRequest);

        #endregion

        #region Equals(GetInstalledCertificateIdsRequest)

        /// <summary>
        /// Compares two get installed certificate ids requests for equality.
        /// </summary>
        /// <param name="GetInstalledCertificateIdsRequest">A get installed certificate ids request to compare with.</param>
        public override Boolean Equals(GetInstalledCertificateIdsRequest? GetInstalledCertificateIdsRequest)

            => GetInstalledCertificateIdsRequest is not null &&

               CertificateType.Equals(GetInstalledCertificateIdsRequest.CertificateType) &&

               base.    GenericEquals(GetInstalledCertificateIdsRequest);

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

                return CertificateType.GetHashCode() * 3 ^
                       base.           GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => CertificateType.AsText();

        #endregion

    }

}
