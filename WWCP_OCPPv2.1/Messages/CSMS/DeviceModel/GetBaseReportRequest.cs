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

using cloud.charging.open.protocols.OCPP;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1.CSMS
{

    /// <summary>
    /// The get base report request.
    /// </summary>
    public class GetBaseReportRequest : ARequest<GetBaseReportRequest>,
                                        IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext DefaultJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/csms/getBaseReportRequest");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext  Context
            => DefaultJSONLDContext;

        /// <summary>
        /// The unique identification of the get base report request.
        /// </summary>
        [Mandatory]
        public Int64          GetBaseReportRequestId    { get; }

        /// <summary>
        /// The requested reporting base.
        /// </summary>
        [Mandatory]
        public ReportBase     ReportBase                { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a get base report request.
        /// </summary>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="GetBaseReportRequestId">An unique identification of the get base report request.</param>
        /// <param name="ReportBase">The requested reporting base.</param>
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
        public GetBaseReportRequest(NetworkingNode_Id        NetworkingNodeId,
                                    Int64                    GetBaseReportRequestId,
                                    ReportBase               ReportBase,

                                    IEnumerable<KeyPair>?    SignKeys            = null,
                                    IEnumerable<SignInfo>?   SignInfos           = null,
                                    IEnumerable<OCPP.Signature>?  Signatures          = null,

                                    CustomData?              CustomData          = null,

                                    Request_Id?              RequestId           = null,
                                    DateTime?                RequestTimestamp    = null,
                                    TimeSpan?                RequestTimeout      = null,
                                    EventTracking_Id?        EventTrackingId     = null,
                                    NetworkPath?             NetworkPath         = null,
                                    CancellationToken        CancellationToken   = default)

            : base(NetworkingNodeId,
                   nameof(GetBaseReportRequest)[..^7],

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

            this.GetBaseReportRequestId  = GetBaseReportRequestId;
            this.ReportBase              = ReportBase;

            unchecked
            {
                hashCode = this.GetBaseReportRequestId.GetHashCode() * 5 ^
                           this.ReportBase.            GetHashCode() * 3 ^
                           base.                       GetHashCode();
            }

        }

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:2:2020:3:GetBaseReportRequest",
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
        //     "ReportBaseEnumType": {
        //       "description": "This field specifies the report base.\r\n",
        //       "javaType": "ReportBaseEnum",
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "ConfigurationInventory",
        //         "FullInventory",
        //         "SummaryInventory"
        //       ]
        //     }
        //   },
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "customData": {
        //       "$ref": "#/definitions/CustomDataType"
        //     },
        //     "requestId": {
        //       "description": "The Id of the request.\r\n",
        //       "type": "integer"
        //     },
        //     "reportBase": {
        //       "$ref": "#/definitions/ReportBaseEnumType"
        //     }
        //   },
        //   "required": [
        //     "requestId",
        //     "reportBase"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, RequestId, NetworkingNodeId, NetworkPath, CustomGetBaseReportRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a get base report request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="CustomGetBaseReportRequestParser">A delegate to parse custom get base report requests.</param>
        public static GetBaseReportRequest Parse(JObject                                             JSON,
                                                 Request_Id                                          RequestId,
                                                 NetworkingNode_Id                                   NetworkingNodeId,
                                                 NetworkPath                                         NetworkPath,
                                                 CustomJObjectParserDelegate<GetBaseReportRequest>?  CustomGetBaseReportRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         NetworkingNodeId,
                         NetworkPath,
                         out var getBaseReportRequest,
                         out var errorResponse,
                         CustomGetBaseReportRequestParser) &&
                getBaseReportRequest is not null)
            {
                return getBaseReportRequest;
            }

            throw new ArgumentException("The given JSON representation of a get base report request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, NetworkingNodeId, NetworkPath, out GetBaseReportRequest, out ErrorResponse, CustomRemoteStartTransactionRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a get base report request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="GetBaseReportRequest">The parsed GetBaseReport request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                    JSON,
                                       Request_Id                 RequestId,
                                       NetworkingNode_Id          NetworkingNodeId,
                                       NetworkPath                NetworkPath,
                                       out GetBaseReportRequest?  GetBaseReportRequest,
                                       out String?                ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        NetworkingNodeId,
                        NetworkPath,
                        out GetBaseReportRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a get base report request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="NetworkingNodeId">The charging station/networking node identification.</param>
        /// <param name="NetworkPath">The network path of the request.</param>
        /// <param name="GetBaseReportRequest">The parsed get base report request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomGetBaseReportRequestParser">A delegate to parse custom get base report requests.</param>
        public static Boolean TryParse(JObject                                             JSON,
                                       Request_Id                                          RequestId,
                                       NetworkingNode_Id                                   NetworkingNodeId,
                                       NetworkPath                                         NetworkPath,
                                       out GetBaseReportRequest?                           GetBaseReportRequest,
                                       out String?                                         ErrorResponse,
                                       CustomJObjectParserDelegate<GetBaseReportRequest>?  CustomGetBaseReportRequestParser)
        {

            try
            {

                GetBaseReportRequest = null;

                #region GetBaseReportRequestId    [mandatory]

                if (!JSON.ParseMandatory("requestId",
                                         "certificate chain",
                                         out Int64 GetBaseReportRequestId,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ReportBase                [mandatory]

                if (!JSON.ParseMandatory("reportBase",
                                         "report base",
                                         OCPPv2_1.ReportBase.TryParse,
                                         out ReportBase ReportBase,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Signatures                [optional, OCPP_CSE]

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

                #region CustomData                [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           OCPP.CustomData.TryParse,
                                           out CustomData CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                GetBaseReportRequest = new GetBaseReportRequest(

                                           NetworkingNodeId,
                                           GetBaseReportRequestId,
                                           ReportBase,

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

                if (CustomGetBaseReportRequestParser is not null)
                    GetBaseReportRequest = CustomGetBaseReportRequestParser(JSON,
                                                                            GetBaseReportRequest);

                return true;

            }
            catch (Exception e)
            {
                GetBaseReportRequest  = null;
                ErrorResponse         = "The given JSON representation of a get base report request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomGetBaseReportRequestSerializer = null, CustomSignatureSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomGetBaseReportRequestSerializer">A delegate to serialize custom get base report requests.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetBaseReportRequest>?  CustomGetBaseReportRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<OCPP.Signature>?        CustomSignatureSerializer              = null,
                              CustomJObjectSerializerDelegate<CustomData>?            CustomCustomDataSerializer             = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("requestId",    GetBaseReportRequestId),
                                 new JProperty("reportBase",   ReportBase.ToString()),

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomGetBaseReportRequestSerializer is not null
                       ? CustomGetBaseReportRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetBaseReportRequest1, GetBaseReportRequest2)

        /// <summary>
        /// Compares two get base report requests for equality.
        /// </summary>
        /// <param name="GetBaseReportRequest1">A get base report request.</param>
        /// <param name="GetBaseReportRequest2">Another get base report request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetBaseReportRequest? GetBaseReportRequest1,
                                           GetBaseReportRequest? GetBaseReportRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetBaseReportRequest1, GetBaseReportRequest2))
                return true;

            // If one is null, but not both, return false.
            if (GetBaseReportRequest1 is null || GetBaseReportRequest2 is null)
                return false;

            return GetBaseReportRequest1.Equals(GetBaseReportRequest2);

        }

        #endregion

        #region Operator != (GetBaseReportRequest1, GetBaseReportRequest2)

        /// <summary>
        /// Compares two get base report requests for inequality.
        /// </summary>
        /// <param name="GetBaseReportRequest1">A get base report request.</param>
        /// <param name="GetBaseReportRequest2">Another get base report request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetBaseReportRequest? GetBaseReportRequest1,
                                           GetBaseReportRequest? GetBaseReportRequest2)

            => !(GetBaseReportRequest1 == GetBaseReportRequest2);

        #endregion

        #endregion

        #region IEquatable<GetBaseReportRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two get base report requests for equality.
        /// </summary>
        /// <param name="Object">A get base report request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is GetBaseReportRequest getBaseReportRequest &&
                   Equals(getBaseReportRequest);

        #endregion

        #region Equals(GetBaseReportRequest)

        /// <summary>
        /// Compares two get base report requests for equality.
        /// </summary>
        /// <param name="GetBaseReportRequest">A get base report request to compare with.</param>
        public override Boolean Equals(GetBaseReportRequest? GetBaseReportRequest)

            => GetBaseReportRequest is not null &&

               GetBaseReportRequestId.Equals(GetBaseReportRequest.GetBaseReportRequestId) &&
               ReportBase.            Equals(GetBaseReportRequest.ReportBase)             &&

               base.           GenericEquals(GetBaseReportRequest);

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

            => $"{ReportBase} / {GetBaseReportRequestId}";

        #endregion

    }

}
