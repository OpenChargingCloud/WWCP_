﻿/*
 * Copyright (c) 2014-2022 GraphDefined GmbH
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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CS
{

    /// <summary>
    /// The GetDiagnostics request.
    /// </summary>
    public class GetDiagnosticsRequest : ARequest<GetDiagnosticsRequest>
    {

        #region Properties

        /// <summary>
        /// The URI where the diagnostics file shall be uploaded to.
        /// </summary>
        public String     Location         { get; }

        /// <summary>
        /// The timestamp of the oldest logging information to include in
        /// the diagnostics.
        /// </summary>
        public DateTime?  StartTime        { get; }

        /// <summary>
        /// The timestamp of the latest logging information to include in
        /// the diagnostics.
        /// </summary>
        public DateTime?  StopTime         { get; }

        /// <summary>
        /// The optional number of retries of a charge point for trying to
        /// upload the diagnostics before giving up. If this field is not
        /// present, it is left to the charge point to decide how many times
        /// it wants to retry.
        /// </summary>
        public Byte?      Retries          { get; }

        /// <summary>
        /// The interval after which a retry may be attempted. If this field
        /// is not present, it is left to charge point to decide how long to
        /// wait between attempts.
        /// </summary>
        public TimeSpan?  RetryInterval    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new GetDiagnostics request.
        /// </summary>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="Location">The URI where the diagnostics file shall be uploaded to.</param>
        /// <param name="StartTime">The timestamp of the oldest logging information to include in the diagnostics.</param>
        /// <param name="StopTime">The timestamp of the latest logging information to include in the diagnostics.</param>
        /// <param name="Retries">The optional number of retries of a charge point for trying to upload the diagnostics before giving up. If this field is not present, it is left to the charge point to decide how many times it wants to retry.</param>
        /// <param name="RetryInterval">The interval after which a retry may be attempted. If this field is not present, it is left to charge point to decide how long to wait between attempts.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        public GetDiagnosticsRequest(ChargeBox_Id        ChargeBoxId,
                                     String              Location,
                                     DateTime?           StartTime           = null,
                                     DateTime?           StopTime            = null,
                                     Byte?               Retries             = null,
                                     TimeSpan?           RetryInterval       = null,

                                     Request_Id?         RequestId           = null,
                                     DateTime?           RequestTimestamp    = null,
                                     TimeSpan?           RequestTimeout      = null,
                                     EventTracking_Id?   EventTrackingId     = null,
                                     CancellationToken?  CancellationToken   = null)

            : base(ChargeBoxId,
                   "GetDiagnostics",
                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   CancellationToken)

        {

            #region Initial checks

            Location = Location.Trim();

            if (Location.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Location), "The given location must not be null or empty!");

            #endregion

            this.Location       = Location;
            this.StartTime      = StartTime;
            this.StopTime       = StopTime;
            this.Retries        = Retries;
            this.RetryInterval  = RetryInterval;

        }

        #endregion


        #region Documentation

        // <soap:Envelope xmlns:soap = "http://www.w3.org/2003/05/soap-envelope"
        //                xmlns:wsa  = "http://www.w3.org/2005/08/addressing"
        //                xmlns:ns   = "urn://Ocpp/Cp/2015/10/">
        //
        //    <soap:Header>
        //       ...
        //    </soap:Header>
        //
        //    <soap:Body>
        //       <ns:getDiagnosticsRequest>
        //
        //          <ns:location>?</ns:location>
        //
        //          <!--Optional:-->
        //          <ns:startTime>?</ns:startTime>
        //
        //          <!--Optional:-->
        //          <ns:stopTime>?</ns:stopTime>
        //
        //          <!--Optional:-->
        //          <ns:retries>?</ns:retries>
        //
        //          <!--Optional:-->
        //          <ns:retryInterval>?</ns:retryInterval>
        //
        //       </ns:getDiagnosticsRequest>
        //    </soap:Body>
        //
        // </soap:Envelope>

        // {
        //     "$schema": "http://json-schema.org/draft-04/schema#",
        //     "id":      "urn:OCPP:1.6:2019:12:GetDiagnosticsRequest",
        //     "title":   "GetDiagnosticsRequest",
        //     "type":    "object",
        //     "properties": {
        //         "location": {
        //             "type": "string",
        //             "format": "uri"
        //         },
        //         "retries": {
        //             "type": "integer"
        //         },
        //         "retryInterval": {
        //             "type": "integer"
        //         },
        //         "startTime": {
        //             "type": "string",
        //             "format": "date-time"
        //         },
        //         "stopTime": {
        //             "type": "string",
        //             "format": "date-time"
        //         }
        //     },
        //     "additionalProperties": false,
        //     "required": [
        //         "location"
        //     ]
        // }

        #endregion

        #region (static) Parse   (XML,  RequestId, ChargeBoxId)

        /// <summary>
        /// Parse the given XML representation of a get diagnostics request.
        /// </summary>
        /// <param name="XML">The XML to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        public static GetDiagnosticsRequest Parse(XElement      XML,
                                                  Request_Id    RequestId,
                                                  ChargeBox_Id  ChargeBoxId)
        {

            if (TryParse(XML,
                         RequestId,
                         ChargeBoxId,
                         out var getDiagnosticsRequest,
                         out var errorResponse))
            {
                return getDiagnosticsRequest!;
            }

            throw new ArgumentException("The given XML representation of a get diagnostics request is invalid: " + errorResponse,
                                        nameof(XML));

        }

        #endregion

        #region (static) Parse   (JSON, RequestId, ChargeBoxId, CustomGetDiagnosticsRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a get diagnostics request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="CustomGetDiagnosticsRequestParser">A delegate to parse custom GetDiagnostics requests.</param>
        public static GetDiagnosticsRequest Parse(JObject                                              JSON,
                                                  Request_Id                                           RequestId,
                                                  ChargeBox_Id                                         ChargeBoxId,
                                                  CustomJObjectParserDelegate<GetDiagnosticsRequest>?  CustomGetDiagnosticsRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         ChargeBoxId,
                         out var getDiagnosticsRequest,
                         out var errorResponse,
                         CustomGetDiagnosticsRequestParser))
            {
                return getDiagnosticsRequest!;
            }

            throw new ArgumentException("The given JSON representation of a get diagnostics request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(XML,  RequestId, ChargeBoxId, out GetDiagnosticsRequest, out ErrorResponse)

        /// <summary>
        /// Try to parse the given XML representation of a get diagnostics request.
        /// </summary>
        /// <param name="XML">The XML to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="GetDiagnosticsRequest">The parsed GetDiagnostics request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(XElement                    XML,
                                       Request_Id                  RequestId,
                                       ChargeBox_Id                ChargeBoxId,
                                       out GetDiagnosticsRequest?  GetDiagnosticsRequest,
                                       out String?                 ErrorResponse)
        {

            try
            {

                GetDiagnosticsRequest = new GetDiagnosticsRequest(

                                            ChargeBoxId,

                                            XML.ElementValueOrFail(OCPPNS.OCPPv1_6_CP + "location"),

                                            XML.MapValueOrNullable(OCPPNS.OCPPv1_6_CP + "startTime",
                                                                   DateTime.Parse),

                                            XML.MapValueOrNullable(OCPPNS.OCPPv1_6_CP + "stopTime",
                                                                   DateTime.Parse),

                                            XML.MapValueOrNullable(OCPPNS.OCPPv1_6_CP + "retries",
                                                                   Byte.Parse),

                                            XML.MapValueOrNullable(OCPPNS.OCPPv1_6_CP + "retryInterval",
                                                                   s => TimeSpan.FromSeconds(UInt32.Parse(s))),

                                            RequestId

                                        );

                ErrorResponse = null;
                return true;

            }
            catch (Exception e)
            {
                GetDiagnosticsRequest  = null;
                ErrorResponse          = "The given XML representation of a get diagnostics request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, ChargeBoxId, out GetDiagnosticsRequest, out ErrorResponse, CustomGetDiagnosticsRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a get diagnostics request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="GetDiagnosticsRequest">The parsed GetDiagnostics request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                     JSON,
                                       Request_Id                  RequestId,
                                       ChargeBox_Id                ChargeBoxId,
                                       out GetDiagnosticsRequest?  GetDiagnosticsRequest,
                                       out String?                 ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        ChargeBoxId,
                        out GetDiagnosticsRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a get diagnostics request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="GetDiagnosticsRequest">The parsed GetDiagnostics request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomGetDiagnosticsRequestParser">A delegate to parse custom GetDiagnostics requests.</param>
        public static Boolean TryParse(JObject                                              JSON,
                                       Request_Id                                           RequestId,
                                       ChargeBox_Id                                         ChargeBoxId,
                                       out GetDiagnosticsRequest?                           GetDiagnosticsRequest,
                                       out String?                                          ErrorResponse,
                                       CustomJObjectParserDelegate<GetDiagnosticsRequest>?  CustomGetDiagnosticsRequestParser)
        {

            try
            {

                GetDiagnosticsRequest = null;

                #region Location         [mandatory]

                if (!JSON.ParseMandatoryText("location",
                                             "location",
                                             out String Location,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region StartTime        [optional]

                if (JSON.ParseOptional("startTime",
                                       "start time",
                                       out DateTime? StartTime,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region StopTime         [optional]

                if (JSON.ParseOptional("stopTime",
                                       "stop time",
                                       out DateTime? StopTime,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region Retries          [optional]

                if (JSON.ParseOptional("retries",
                                       "retries",
                                       out Byte? Retries,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region RetryInterval    [optional]

                if (JSON.ParseOptional("retryInterval",
                                       "retry interval",
                                       out TimeSpan? RetryInterval,
                                       out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion

                #region ChargeBoxId      [optional, OCPP_CSE]

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


                GetDiagnosticsRequest = new GetDiagnosticsRequest(ChargeBoxId,
                                                                  Location,
                                                                  StartTime,
                                                                  StopTime,
                                                                  Retries,
                                                                  RetryInterval,
                                                                  RequestId);

                if (CustomGetDiagnosticsRequestParser is not null)
                    GetDiagnosticsRequest = CustomGetDiagnosticsRequestParser(JSON,
                                                                              GetDiagnosticsRequest);

                return true;

            }
            catch (Exception e)
            {
                GetDiagnosticsRequest  = null;
                ErrorResponse          = "The given JSON representation of a get diagnostics request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToXML()

        /// <summary>
        /// Return a XML representation of this object.
        /// </summary>
        public XElement ToXML()

            => new (OCPPNS.OCPPv1_6_CP + "getDiagnosticsRequest",

                   new XElement(OCPPNS.OCPPv1_6_CP + "location",             Location),

                   StartTime.HasValue
                       ? new XElement(OCPPNS.OCPPv1_6_CP + "startTime",      StartTime.Value.ToIso8601())
                       : null,

                   StopTime.HasValue
                       ? new XElement(OCPPNS.OCPPv1_6_CP + "stopTime",       StopTime.Value.ToIso8601())
                       : null,

                   Retries.HasValue
                       ? new XElement(OCPPNS.OCPPv1_6_CP + "retries",        Retries.Value)
                       : null,

                   RetryInterval.HasValue
                       ? new XElement(OCPPNS.OCPPv1_6_CP + "retryInterval",  RetryInterval.Value.TotalSeconds)
                       : null

               );

        #endregion

        #region ToJSON(CustomGetDiagnosticsRequestSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomGetDiagnosticsRequestSerializer">A delegate to serialize custom start transaction requests.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<GetDiagnosticsRequest>? CustomGetDiagnosticsRequestSerializer = null)
        {

            var json = JSONObject.Create(

                           new JProperty("location",             Location),

                           StartTime.HasValue
                               ? new JProperty("startTime",      StartTime.Value.ToIso8601())
                               : null,

                           StopTime.HasValue
                               ? new JProperty("stopTime",       StopTime. Value.ToIso8601())
                               : null,

                           Retries.HasValue
                               ? new JProperty("retries",        Retries.  Value.ToString())
                               : null,

                           RetryInterval.HasValue
                               ? new JProperty("retryInterval",  (UInt64) RetryInterval.Value.TotalSeconds)
                               : null

                       );

            return CustomGetDiagnosticsRequestSerializer is not null
                       ? CustomGetDiagnosticsRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (GetDiagnosticsRequest1, GetDiagnosticsRequest2)

        /// <summary>
        /// Compares two GetDiagnostics requests for equality.
        /// </summary>
        /// <param name="GetDiagnosticsRequest1">A GetDiagnostics request.</param>
        /// <param name="GetDiagnosticsRequest2">Another GetDiagnostics request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (GetDiagnosticsRequest GetDiagnosticsRequest1,
                                           GetDiagnosticsRequest GetDiagnosticsRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(GetDiagnosticsRequest1, GetDiagnosticsRequest2))
                return true;

            // If one is null, but not both, return false.
            if (GetDiagnosticsRequest1 is null || GetDiagnosticsRequest2 is null)
                return false;

            return GetDiagnosticsRequest1.Equals(GetDiagnosticsRequest2);

        }

        #endregion

        #region Operator != (GetDiagnosticsRequest1, GetDiagnosticsRequest2)

        /// <summary>
        /// Compares two GetDiagnostics requests for inequality.
        /// </summary>
        /// <param name="GetDiagnosticsRequest1">A GetDiagnostics request.</param>
        /// <param name="GetDiagnosticsRequest2">Another GetDiagnostics request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (GetDiagnosticsRequest GetDiagnosticsRequest1,
                                           GetDiagnosticsRequest GetDiagnosticsRequest2)

            => !(GetDiagnosticsRequest1 == GetDiagnosticsRequest2);

        #endregion

        #endregion

        #region IEquatable<GetDiagnosticsRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two get diagnostics requests for equality.
        /// </summary>
        /// <param name="Object">A get diagnostics request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is GetDiagnosticsRequest getDiagnosticsRequest &&
                   Equals(getDiagnosticsRequest);

        #endregion

        #region Equals(GetDiagnosticsRequest)

        /// <summary>
        /// Compares two get diagnostics requests for equality.
        /// </summary>
        /// <param name="GetDiagnosticsRequest">A get diagnostics request to compare with.</param>
        public override Boolean Equals(GetDiagnosticsRequest? GetDiagnosticsRequest)

            => GetDiagnosticsRequest is not null &&

               Location.Equals(GetDiagnosticsRequest.Location) &&

            ((!StartTime.    HasValue && !GetDiagnosticsRequest.StartTime.    HasValue) ||
              (StartTime.    HasValue &&  GetDiagnosticsRequest.StartTime.    HasValue && StartTime.    Value.Equals(GetDiagnosticsRequest.StartTime.    Value))) &&

            ((!StopTime.     HasValue && !GetDiagnosticsRequest.StopTime.     HasValue) ||
              (StopTime.     HasValue &&  GetDiagnosticsRequest.StopTime.     HasValue && StopTime.     Value.Equals(GetDiagnosticsRequest.StopTime.     Value))) &&

            ((!Retries.      HasValue && !GetDiagnosticsRequest.Retries.      HasValue) ||
              (Retries.      HasValue &&  GetDiagnosticsRequest.Retries.      HasValue && Retries.      Value.Equals(GetDiagnosticsRequest.Retries.      Value))) &&

            ((!RetryInterval.HasValue && !GetDiagnosticsRequest.RetryInterval.HasValue) ||
              (RetryInterval.HasValue &&  GetDiagnosticsRequest.RetryInterval.HasValue && RetryInterval.Value.Equals(GetDiagnosticsRequest.RetryInterval.Value))) &&

               base.GenericEquals(GetDiagnosticsRequest);

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

                return Location.      GetHashCode()       * 13 ^

                      (StartTime?.    GetHashCode() ?? 0) * 11 ^
                      (StopTime?.     GetHashCode() ?? 0) *  7 ^
                      (Retries?.      GetHashCode() ?? 0) *  5 ^
                      (RetryInterval?.GetHashCode() ?? 0) *  3 ^

                       base.          GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Location,

                             StartTime.HasValue
                                 ? ", from " + StartTime.Value.ToIso8601()
                                 : "",

                             StopTime.HasValue
                                 ? ", to "   + StopTime. Value.ToIso8601()
                                 : "",

                             Retries.HasValue
                                 ? ", " + Retries.Value + " retries"
                                 : "",

                             RetryInterval.HasValue
                                 ? ", retry interval " + RetryInterval.Value.TotalSeconds + " sec(s)"
                                 : "");

        #endregion

    }

}
