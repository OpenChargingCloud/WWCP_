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

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.WebSockets
{

    public class OCPP_WebSocket_ErrorMessage
    {

        public Request_Id                 RequestId           { get; }

        public OCPP_WebSocket_ErrorCodes  ErrorCode           { get; }

        public String                     ErrorDescription    { get; }

        public JObject                    ErrorDetails        { get; }


        public OCPP_WebSocket_ErrorMessage(Request_Id                 RequestId,
                                           OCPP_WebSocket_ErrorCodes  ErrorCode,
                                           String                     ErrorDescription   = null,
                                           JObject                    ErrorDetails       = null)

        {

            this.RequestId         = RequestId;
            this.ErrorCode         = ErrorCode;
            this.ErrorDescription  = ErrorDescription ?? "";
            this.ErrorDetails      = ErrorDetails     ?? new JObject();

        }


        public JArray ToJSON()

            // [
            //     4,            // MessageType: CALLERROR (Server-to-Client)
            //    "19223201",    // RequestId from request
            //    "<errorCode>",
            //    "<errorDescription>",
            //    {
            //        <errorDetails>
            //    }
            // ]

            // Error Code                    Description
            // -----------------------------------------------------------------------------------------------
            // NotImplemented                Requested Action is not known by receiver
            // NotSupported                  Requested Action is recognized but not supported by the receiver
            // InternalError                 An internal error occurred and the receiver was not able to process the requested Action successfully
            // ProtocolError                 Payload for Action is incomplete
            // SecurityError                 During the processing of Action a security issue occurred preventing receiver from completing the Action successfully
            // FormationViolation            Payload for Action is syntactically incorrect or not conform the PDU structure for Action
            // PropertyConstraintViolation   Payload is syntactically correct but at least one field contains an invalid value
            // OccurenceConstraintViolation  Payload for Action is syntactically correct but at least one of the fields violates occurence constraints
            // TypeConstraintViolation       Payload for Action is syntactically correct but at least one of the fields violates data type constraints (e.g. “somestring”: 12)
            // GenericError                  Any other error not covered by the previous ones

            => new JArray(4,
                          RequestId.ToString(),
                          ErrorCode.ToString(),
                          ErrorDescription,
                          ErrorDetails);


        public static Boolean TryParse(String Text, out OCPP_WebSocket_ErrorMessage? RequestFrame)
        {

            RequestFrame = null;

            if (Text is null)
                return false;

            // [
            //     4,
            //    "100007",
            //    "InternalError",
            //    "An internal error occurred and the receiver was not able to process the requested Action successfully",
            //    {}
            // ]

            // [
            //     2,                  // MessageType: CALL (Client-to-Server)
            //    "19223201",          // RequestId
            //    "BootNotification",  // Action
            //    {
            //        "chargePointVendor": "VendorX",
            //        "chargePointModel":  "SingleSocketCharger"
            //    }
            // ]

            try
            {

                var JSON = JArray.Parse(Text);

                if (JSON.Count != 5)
                    return false;

                if (!Byte.TryParse(JSON[0].Value<String>(), out Byte messageType))
                    return false;

                var requestId    = Request_Id.TryParse(JSON[1]?.Value<String>() ?? "");
                var error        = Enum.TryParse(JSON[2].Value<String>(), out OCPP_WebSocket_ErrorCodes wsErrorCodes);
                var description  = JSON[3]?.Value<String>();
                var details      = JSON[4] as JObject;

                if (!requestId.HasValue || description is null || details is null)
                    return false;

                RequestFrame = new OCPP_WebSocket_ErrorMessage(requestId.Value,
                                                               error ? wsErrorCodes : OCPP_WebSocket_ErrorCodes.GenericError,
                                                               description,
                                                               details);

                return true;

            }
            catch
            {
                return false;
            }

        }


        public override String ToString()

            => String.Concat(RequestId,
                             " => ",
                             ErrorCode.ToString());


    }

}
