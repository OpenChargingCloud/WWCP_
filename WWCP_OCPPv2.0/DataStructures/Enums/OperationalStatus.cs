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

namespace cloud.charging.open.protocols.OCPPv2_0
{

    /// <summary>
    /// Extentions methods for the operational status.
    /// </summary>
    public static class OperationalStatusExtentions
    {

        #region Parse(Text)

        public static OperationalStatus Parse(String Text)

            => Text.Trim() switch {
                   "Inoperative"  => OperationalStatus.Inoperative,
                   "Operative"    => OperationalStatus.Operative,
                   _              => OperationalStatus.Unknown
               };

        #endregion

        #region AsText(this OperationalStatus)

        public static String AsText(this OperationalStatus OperationalStatus)

            => OperationalStatus switch {
                   OperationalStatus.Inoperative  => "Inoperative",
                   OperationalStatus.Operative    => "Operative",
                   _                              => "Unknown"
               };

        #endregion

    }


    /// <summary>
    /// Defines the operational status of a charging station or EVSE.
    /// </summary>
    public enum OperationalStatus
    {

        /// <summary>
        /// Unknown operational status.
        /// </summary>
        Unknown,

        /// <summary>
        /// The charging station or EVSE is not available for charging.
        /// </summary>
        Inoperative,

        /// <summary>
        /// The charging station or EVSE is available for charging.
        /// </summary>
        Operative

    }

}
