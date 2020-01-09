﻿/*
 * Copyright (c) 2014-2020 GraphDefined GmbH
 * This file is part of WWCP OCPP <https://github.com/GraphDefined/WWCP_OCPP>
 *
 * Licensed under the Affero GPL license, Version 3.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.gnu.org/licenses/agpl.html
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

namespace org.GraphDefined.WWCP.OCPPv1_5
{

    /// <summary>
    /// Defines the registration-status-value
    /// </summary>
    public enum RegistrationStatus
    {

        /// <summary>
        /// Charge point is accepted by Central System.
        /// </summary>
        Accepted,

        /// <summary>
        /// Charge point is not accepted by Central System.
        /// This may happen when the Charge Point id is not
        /// known by Central System.
        /// </summary>
        Rejected

    }

}
