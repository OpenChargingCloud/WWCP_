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

using System.Security.Policy;

namespace cloud.charging.open.protocols.OCPPv1_6
{

    /// <summary>
    /// Extentions methods for cryptographic hash algorithms.
    /// </summary>
    public static class HashAlgorithmsExtentions
    {

        #region Parse(Text)

        public static HashAlgorithms Parse(String Text)

            => Text.Trim() switch {
                   "SHA256"  => HashAlgorithms.SHA256,
                   "SHA384"  => HashAlgorithms.SHA384,
                   "SHA512"  => HashAlgorithms.SHA512,
                   _         => HashAlgorithms.Unknown
               };

        public static HashAlgorithms? TryParse(String Text)

            => Text.Trim() switch {
                   "SHA256"  => HashAlgorithms.SHA256,
                   "SHA384"  => HashAlgorithms.SHA384,
                   "SHA512"  => HashAlgorithms.SHA512,
                   _         => null
               };

        public static Boolean TryParse(String Text, out HashAlgorithms? HashAlgorithm)
        {

            HashAlgorithm = TryParse(Text);

            return HashAlgorithm.HasValue;

        }

        #endregion

        #region AsText(this HashAlgorithms)

        public static String AsText(this HashAlgorithms HashAlgorithms)

            => HashAlgorithms switch {
                   HashAlgorithms.SHA256  => "SHA256",
                   HashAlgorithms.SHA384  => "SHA384",
                   HashAlgorithms.SHA512  => "SHA512",
                   _                      => "unknown"
               };

        #endregion

    }


    /// <summary>
    /// Cryptographic hash algorithms.
    /// </summary>
    public enum HashAlgorithms
    {

        /// <summary>
        /// Unknown generic status.
        /// </summary>
        Unknown,

        /// <summary>
        /// SHA-256 hash algorithm.
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA-384 hash algorithm.
        /// </summary>
        SHA384,

        /// <summary>
        /// SHA-512 hash algorithm.
        /// </summary>
        SHA512

    }

}