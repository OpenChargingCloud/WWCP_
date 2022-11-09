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

using Newtonsoft.Json.Linq;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CP
{

    /// <summary>
    /// The log status notification request.
    /// </summary>
    public class LogStatusNotificationRequest : ARequest<LogStatusNotificationRequest>
    {

        #region Properties

        /// <summary>
        /// The status of the log upload.
        /// </summary>
        [Mandatory]
        public UploadLogStatus  Status          { get; }

        /// <summary>
        /// The request id that was provided in the GetLog.req that started this log upload.
        /// </summary>
        [Optional]
        public Int32?           LogRequestId    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new log status notification request.
        /// </summary>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// 
        /// <param name="Status">The status of the log upload.</param>
        /// <param name="LogRquestId">The request id that was provided in the GetLog.req that started this log upload.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        public LogStatusNotificationRequest(ChargeBox_Id        ChargeBoxId,
                                            UploadLogStatus     Status,
                                            Int32?              LogRquestId           = null,

                                            Request_Id?         RequestId                 = null,
                                            DateTime?           RequestTimestamp          = null,
                                            TimeSpan?           RequestTimeout            = null,
                                            EventTracking_Id?   EventTrackingId           = null,
                                            CancellationToken?  CancellationToken         = null)

            : base(ChargeBoxId,
                   "LogStatusNotification",
                   RequestId,
                   RequestTimestamp,
                   RequestTimeout,
                   EventTrackingId,
                   CancellationToken)

        {

            this.Status       = Status;
            this.LogRequestId  = LogRquestId;

        }

        #endregion


        #region Documentation

        // {
        //   "$schema": "http://json-schema.org/draft-06/schema#",
        //   "$id": "urn:OCPP:Cp:1.6:2020:3:LogStatusNotification.req",
        //   "definitions": {
        //     "UploadLogStatusEnumType": {
        //       "type": "string",
        //       "additionalProperties": false,
        //       "enum": [
        //         "BadMessage",
        //         "Idle",
        //         "NotSupportedOperation",
        //         "PermissionDenied",
        //         "Uploaded",
        //         "UploadFailure",
        //         "Uploading"
        //       ]
        //     }
        //   },
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "status": {
        //       "$ref": "#/definitions/UploadLogStatusEnumType"
        //     },
        //     "requestId": {
        //       "type": "integer"
        //     }
        //   },
        //   "required": [
        //     "status"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, RequestId, ChargeBoxId, CustomLogStatusNotificationRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of a log status notification request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="CustomLogStatusNotificationRequestParser">A delegate to parse custom log status notification requests.</param>
        public static LogStatusNotificationRequest Parse(JObject                                                     JSON,
                                                         Request_Id                                                  RequestId,
                                                         ChargeBox_Id                                                ChargeBoxId,
                                                         CustomJObjectParserDelegate<LogStatusNotificationRequest>?  CustomLogStatusNotificationRequestParser   = null)
        {

            if (TryParse(JSON,
                         RequestId,
                         ChargeBoxId,
                         out var logStatusNotificationRequest,
                         out var errorResponse,
                         CustomLogStatusNotificationRequestParser))
            {
                return logStatusNotificationRequest!;
            }

            throw new ArgumentException("The given JSON representation of a log status notification request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, ChargeBoxId, out LogStatusNotificationRequest, OnException = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a log status notification request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="LogStatusNotificationRequest">The parsed log status notification request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                            JSON,
                                       Request_Id                         RequestId,
                                       ChargeBox_Id                       ChargeBoxId,
                                       out LogStatusNotificationRequest?  LogStatusNotificationRequest,
                                       out String?                        ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        ChargeBoxId,
                        out LogStatusNotificationRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a log status notification request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargeBoxId">The charge box identification.</param>
        /// <param name="LogStatusNotificationRequest">The parsed log status notification request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomLogStatusNotificationRequestParser">A delegate to parse custom log status notification requests.</param>
        public static Boolean TryParse(JObject                                                     JSON,
                                       Request_Id                                                  RequestId,
                                       ChargeBox_Id                                                ChargeBoxId,
                                       out LogStatusNotificationRequest?                           LogStatusNotificationRequest,
                                       out String?                                                 ErrorResponse,
                                       CustomJObjectParserDelegate<LogStatusNotificationRequest>?  CustomLogStatusNotificationRequestParser)
        {

            try
            {

                LogStatusNotificationRequest = null;

                #region Status          [mandatory]

                if (!JSON.MapMandatory("status",
                                       "status",
                                       UploadLogStatusExtentions.Parse,
                                       out UploadLogStatus Status,
                                       out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region LogRequestId    [optional]

                if (!JSON.ParseOptional("requestId",
                                        "request identification",
                                        out Int32? LogRequestId,
                                        out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region ChargeBoxId     [optional, OCPP_CSE]

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


                LogStatusNotificationRequest = new LogStatusNotificationRequest(ChargeBoxId,
                                                                                Status,
                                                                                LogRequestId,
                                                                                RequestId);

                if (CustomLogStatusNotificationRequestParser is not null)
                    LogStatusNotificationRequest = CustomLogStatusNotificationRequestParser(JSON,
                                                                          LogStatusNotificationRequest);

                return true;

            }
            catch (Exception e)
            {
                LogStatusNotificationRequest  = null;
                ErrorResponse                 = "The given JSON representation of a log status notification request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomLogStatusNotificationSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        public override JObject ToJSON()
            => ToJSON(null);


        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomLogStatusNotificationSerializer">A delegate to serialize custom log status notification requests.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<LogStatusNotificationRequest>? CustomLogStatusNotificationSerializer)
        {

            var json = JSONObject.Create(

                           new JProperty("status",           Status.AsText()),

                           LogRequestId.HasValue
                               ? new JProperty("requestId",  LogRequestId.Value)
                               : null

                       );

            return CustomLogStatusNotificationSerializer is not null
                       ? CustomLogStatusNotificationSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (LogStatusNotificationRequest1, LogStatusNotificationRequest2)

        /// <summary>
        /// Compares two log status notification requests for equality.
        /// </summary>
        /// <param name="LogStatusNotificationRequest1">A log status notification request.</param>
        /// <param name="LogStatusNotificationRequest2">Another log status notification request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (LogStatusNotificationRequest LogStatusNotificationRequest1,
                                           LogStatusNotificationRequest LogStatusNotificationRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(LogStatusNotificationRequest1, LogStatusNotificationRequest2))
                return true;

            // If one is null, but not both, return false.
            if (LogStatusNotificationRequest1 is null || LogStatusNotificationRequest2 is null)
                return false;

            return LogStatusNotificationRequest1.Equals(LogStatusNotificationRequest2);

        }

        #endregion

        #region Operator != (LogStatusNotificationRequest1, LogStatusNotificationRequest2)

        /// <summary>
        /// Compares two log status notification requests for inequality.
        /// </summary>
        /// <param name="LogStatusNotificationRequest1">A log status notification request.</param>
        /// <param name="LogStatusNotificationRequest2">Another log status notification request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (LogStatusNotificationRequest LogStatusNotificationRequest1,
                                           LogStatusNotificationRequest LogStatusNotificationRequest2)

            => !(LogStatusNotificationRequest1 == LogStatusNotificationRequest2);

        #endregion

        #endregion

        #region IEquatable<LogStatusNotificationRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two log status notification requests for equality.
        /// </summary>
        /// <param name="Object">A log status notification request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is LogStatusNotificationRequest logStatusNotificationRequest &&
                   Equals(logStatusNotificationRequest);

        #endregion

        #region Equals(LogStatusNotificationRequest)

        /// <summary>
        /// Compares two log status notification requests for equality.
        /// </summary>
        /// <param name="LogStatusNotificationRequest">A log status notification request to compare with.</param>
        public override Boolean Equals(LogStatusNotificationRequest? LogStatusNotificationRequest)

            => LogStatusNotificationRequest is not null &&

               Status.Equals(LogStatusNotificationRequest.Status) &&

            ((!LogRequestId.HasValue && !LogStatusNotificationRequest.LogRequestId.HasValue) ||
              (LogRequestId.HasValue &&  LogStatusNotificationRequest.LogRequestId.HasValue && LogRequestId.Value.Equals(LogStatusNotificationRequest.LogRequestId.Value)));

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

                return Status.      GetHashCode() * 3 ^
                      (LogRequestId?.GetHashCode() ?? 0);

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(Status,
                             LogRequestId.HasValue
                                 ? " (" + LogRequestId + ")"
                                 :  ""
               );

        #endregion

    }

}