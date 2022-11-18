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

namespace cloud.charging.open.protocols.OCPPv1_6
{

    /// <summary>
    /// Extentions methods for log types.
    /// </summary>
    public static class LogTypesExtentions
    {

        #region Parse(Text)

        public static LogTypes Parse(String Text)

            => Text.Trim() switch {
                   "DiagnosticsLog"  => LogTypes.DiagnosticsLog,
                   "SecurityLog"     => LogTypes.SecurityLog,
                   _                 => LogTypes.Unknown
               };

        #endregion

        #region AsText(this LogType)

        public static String AsText(this LogTypes LogType)

            => LogType switch {
                   LogTypes.DiagnosticsLog  => "DiagnosticsLog",
                   LogTypes.SecurityLog     => "SecurityLog",
                   _                        => "Unknown"
               };

        #endregion

    }


    /// <summary>
    /// Log types.
    /// </summary>
    public enum LogTypes
    {

        /// <summary>
        /// Unknown generic status.
        /// </summary>
        Unknown,

        /// <summary>
        /// This contains the field definition of a diagnostics log file.
        /// </summary>
        DiagnosticsLog,

        /// <summary>
        /// Sent by the Central System to the Charge Point to request that the Charge Point uploads the security log.
        /// </summary>
        SecurityLog

    }

}
