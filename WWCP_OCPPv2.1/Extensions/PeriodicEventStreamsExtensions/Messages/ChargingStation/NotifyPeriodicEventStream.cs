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
    /// A notify periodic event stream request.
    /// </summary>
    /// <remarks>There will be NO RESPONSE to this request!</remarks>
    public class NotifyPeriodicEventStream : ARequest<NotifyPeriodicEventStream>,
                                             IRequest
    {

        #region Data

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public readonly static JSONLDContext UserJSONLDContext = JSONLDContext.Parse("https://open.charging.cloud/context/ocpp/cs/notifyPeriodicEventStream");

        #endregion

        #region Properties

        /// <summary>
        /// The JSON-LD context of this object.
        /// </summary>
        public JSONLDContext                   Context
            => UserJSONLDContext;

        /// <summary>
        /// The unique identification of the periodic event stream.
        /// </summary>
        [Mandatory]
        public PeriodicEventStream_Id          Id                    { get; }

        /// <summary>
        /// The enumeration of periodic event stream data elements.
        /// </summary>
        [Mandatory]
        public IEnumerable<StreamDataElement>  StreamDataElements    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new notify periodic event stream request.
        /// </summary>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="Id">The unique identification of the periodic event stream.</param>
        /// <param name="StreamDataElements">An enumeration of periodic event stream data elements.</param>
        /// 
        /// <param name="Signatures">An optional enumeration of cryptographic signatures for this message.</param>
        /// <param name="CustomData">The custom data object to allow to store any kind of customer specific data.</param>
        /// 
        /// <param name="RequestId">An optional request identification.</param>
        /// <param name="RequestTimestamp">An optional request timestamp.</param>
        /// <param name="RequestTimeout">The timeout of this request.</param>
        /// <param name="EventTrackingId">An event tracking identification for correlating this request with other events.</param>
        /// <param name="CancellationToken">An optional token to cancel this request.</param>
        public NotifyPeriodicEventStream(ChargingStation_Id              ChargingStationId,
                                         PeriodicEventStream_Id          Id,
                                         IEnumerable<StreamDataElement>  StreamDataElements,

                                         IEnumerable<KeyPair>?           SignKeys            = null,
                                         IEnumerable<SignInfo>?          SignInfos           = null,
                                         IEnumerable<Signature>?         Signatures          = null,

                                         CustomData?                     CustomData          = null,

                                         Request_Id?                     RequestId           = null,
                                         DateTime?                       RequestTimestamp    = null,
                                         TimeSpan?                       RequestTimeout      = null,
                                         EventTracking_Id?               EventTrackingId     = null,
                                         CancellationToken               CancellationToken   = default)

            : base(ChargingStationId,
                   "NotifyPeriodicEventStream",

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

            if (!StreamDataElements.Any())
                throw new ArgumentException("The given enumeration of periodic event stream data elements must not be empty!",
                                            nameof(StreamDataElements));

            this.Id                  = Id;
            this.StreamDataElements  = StreamDataElements.Distinct();


            unchecked
            {

                hashCode = this.Id.                GetHashCode()  * 5 ^
                           this.StreamDataElements.CalcHashCode() * 3 ^
                           base.                   GetHashCode();

            }

        }

        #endregion


        //ToDo: Update schema documentation after the official release of OCPP v2.1!

        #region Documentation


        #endregion

        #region (static) Parse   (JSON, RequestId, ChargingStationId, CustomNotifyPeriodicEventStreamRequestParser = null)

        /// <summary>
        /// Parse the given JSON representation of an NotifyPeriodicEventStream request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="CustomNotifyPeriodicEventStreamRequestParser">A delegate to parse custom NotifyPeriodicEventStream requests.</param>
        public static NotifyPeriodicEventStream Parse(JObject                                                         JSON,
                                                             Request_Id                                                      RequestId,
                                                             ChargingStation_Id                                              ChargingStationId,
                                                             CustomJObjectParserDelegate<NotifyPeriodicEventStream>?  CustomNotifyPeriodicEventStreamRequestParser   = null)
        {


            if (TryParse(JSON,
                         RequestId,
                         ChargingStationId,
                         out var notifyPeriodicEventStreamRequest,
                         out var errorResponse,
                         CustomNotifyPeriodicEventStreamRequestParser) &&
                notifyPeriodicEventStreamRequest is not null)
            {
                return notifyPeriodicEventStreamRequest;
            }

            throw new ArgumentException("The given JSON representation of a NotifyPeriodicEventStream request is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, RequestId, ChargingStationId, out NotifyPeriodicEventStreamRequest, out ErrorResponse, CustomAuthorizeRequestParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a NotifyPeriodicEventStream request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="NotifyPeriodicEventStreamRequest">The parsed NotifyPeriodicEventStream request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject                                JSON,
                                       Request_Id                             RequestId,
                                       ChargingStation_Id                     ChargingStationId,
                                       out NotifyPeriodicEventStream?  NotifyPeriodicEventStreamRequest,
                                       out String?                            ErrorResponse)

            => TryParse(JSON,
                        RequestId,
                        ChargingStationId,
                        out NotifyPeriodicEventStreamRequest,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a NotifyPeriodicEventStream request.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="RequestId">The request identification.</param>
        /// <param name="ChargingStationId">The charging station identification.</param>
        /// <param name="NotifyPeriodicEventStreamRequest">The parsed NotifyPeriodicEventStream request.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomNotifyPeriodicEventStreamRequestParser">A delegate to parse custom NotifyPeriodicEventStream requests.</param>
        public static Boolean TryParse(JObject                                                         JSON,
                                       Request_Id                                                      RequestId,
                                       ChargingStation_Id                                              ChargingStationId,
                                       out NotifyPeriodicEventStream?                           NotifyPeriodicEventStreamRequest,
                                       out String?                                                     ErrorResponse,
                                       CustomJObjectParserDelegate<NotifyPeriodicEventStream>?  CustomNotifyPeriodicEventStreamRequestParser)
        {

            try
            {

                NotifyPeriodicEventStreamRequest = null;

                #region Id                    [mandatory]

                if (!JSON.ParseMandatory("id",
                                         "periodic event stream identification",
                                         PeriodicEventStream_Id.TryParse,
                                         out PeriodicEventStream_Id Id,
                                         out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region StreamDataElements    [mandatory]

                if (!JSON.ParseMandatoryHashSet("data",
                                                "periodic event stream data elements",
                                                StreamDataElement.TryParse,
                                                out HashSet<StreamDataElement> StreamDataElements,
                                                out ErrorResponse))
                {
                    return false;
                }

                #endregion


                #region Signatures            [optional, OCPP_CSE]

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

                #region CustomData            [optional]

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

                #region ChargingStationId     [optional, OCPP_CSE]

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


                NotifyPeriodicEventStreamRequest = new NotifyPeriodicEventStream(

                                                       ChargingStationId,
                                                       Id,
                                                       StreamDataElements,

                                                       null,
                                                       null,
                                                       Signatures,

                                                       CustomData,
                                                       RequestId

                                                   );

                if (CustomNotifyPeriodicEventStreamRequestParser is not null)
                    NotifyPeriodicEventStreamRequest = CustomNotifyPeriodicEventStreamRequestParser(JSON,
                                                                                                    NotifyPeriodicEventStreamRequest);

                return true;

            }
            catch (Exception e)
            {
                NotifyPeriodicEventStreamRequest  = null;
                ErrorResponse                     = "The given JSON representation of a NotifyPeriodicEventStream request is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomNotifyPeriodicEventStreamRequestSerializer = null, CustomStreamDataElementSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomNotifyPeriodicEventStreamRequestSerializer">A delegate to serialize custom NotifyPeriodicEventStream requests.</param>
        /// <param name="CustomStreamDataElementSerializer">A delegate to serialize custom stream data elements.</param>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<NotifyPeriodicEventStream>?  CustomNotifyPeriodicEventStreamRequestSerializer   = null,
                              CustomJObjectSerializerDelegate<StreamDataElement>?                 CustomStreamDataElementSerializer                  = null,
                              CustomJObjectSerializerDelegate<Signature>?                         CustomSignatureSerializer                          = null,
                              CustomJObjectSerializerDelegate<CustomData>?                        CustomCustomDataSerializer                         = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("id",           Id.   ToString()),
                                 new JProperty("data",         new JArray(StreamDataElements.Select(streamDataElement => streamDataElement.ToJSON(CustomStreamDataElementSerializer)))),

                           Signatures.Any()
                               ? new JProperty("signatures",   new JArray(Signatures.Select(signature => signature.ToJSON(CustomSignatureSerializer,
                                                                                                                          CustomCustomDataSerializer))))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",   CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomNotifyPeriodicEventStreamRequestSerializer is not null
                       ? CustomNotifyPeriodicEventStreamRequestSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (NotifyPeriodicEventStreamRequest1, NotifyPeriodicEventStreamRequest2)

        /// <summary>
        /// Compares two NotifyPeriodicEventStream requests for equality.
        /// </summary>
        /// <param name="NotifyPeriodicEventStreamRequest1">A NotifyPeriodicEventStream request.</param>
        /// <param name="NotifyPeriodicEventStreamRequest2">Another NotifyPeriodicEventStream request.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (NotifyPeriodicEventStream? NotifyPeriodicEventStreamRequest1,
                                           NotifyPeriodicEventStream? NotifyPeriodicEventStreamRequest2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(NotifyPeriodicEventStreamRequest1, NotifyPeriodicEventStreamRequest2))
                return true;

            // If one is null, but not both, return false.
            if (NotifyPeriodicEventStreamRequest1 is null || NotifyPeriodicEventStreamRequest2 is null)
                return false;

            return NotifyPeriodicEventStreamRequest1.Equals(NotifyPeriodicEventStreamRequest2);

        }

        #endregion

        #region Operator != (NotifyPeriodicEventStreamRequest1, NotifyPeriodicEventStreamRequest2)

        /// <summary>
        /// Compares two NotifyPeriodicEventStream requests for inequality.
        /// </summary>
        /// <param name="NotifyPeriodicEventStreamRequest1">A NotifyPeriodicEventStream request.</param>
        /// <param name="NotifyPeriodicEventStreamRequest2">Another NotifyPeriodicEventStream request.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (NotifyPeriodicEventStream? NotifyPeriodicEventStreamRequest1,
                                           NotifyPeriodicEventStream? NotifyPeriodicEventStreamRequest2)

            => !(NotifyPeriodicEventStreamRequest1 == NotifyPeriodicEventStreamRequest2);

        #endregion

        #endregion

        #region IEquatable<NotifyPeriodicEventStreamRequest> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two NotifyPeriodicEventStreamRequest requests for equality.
        /// </summary>
        /// <param name="Object">A NotifyPeriodicEventStreamRequest request to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is NotifyPeriodicEventStream notifyPeriodicEventStreamRequest &&
                   Equals(notifyPeriodicEventStreamRequest);

        #endregion

        #region Equals(NotifyPeriodicEventStreamRequest)

        /// <summary>
        /// Compares two NotifyPeriodicEventStreamRequest requests for equality.
        /// </summary>
        /// <param name="NotifyPeriodicEventStreamRequest">A NotifyPeriodicEventStreamRequest request to compare with.</param>
        public override Boolean Equals(NotifyPeriodicEventStream? NotifyPeriodicEventStreamRequest)

            => NotifyPeriodicEventStreamRequest is not null &&

               Id.                        Equals(NotifyPeriodicEventStreamRequest.Id) &&

               StreamDataElements.Count().Equals(NotifyPeriodicEventStreamRequest.StreamDataElements.Count()) &&
               StreamDataElements.All(NotifyPeriodicEventStreamRequest.StreamDataElements.Contains)           &&

               base.               GenericEquals(NotifyPeriodicEventStreamRequest);

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

            => $"{Id}: {StreamDataElements.AggregateWith(", ")}";

        #endregion

    }

}
