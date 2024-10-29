﻿/*
 * Copyright (c) 2014-2024 GraphDefined GmbH <achim.friedland@graphdefined.com>
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

using org.GraphDefined.Vanaheimr.CLI;

using cloud.charging.open.protocols.WWCP.NetworkingNode;
using cloud.charging.open.protocols.OCPPv1_6.NetworkingNode;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6.CentralSystem.CommandLine
{

    public static class ICentralSystemCLIExtensions
    {

        public static Boolean RemoteSystemIdIsSet(this ICentralSystemCLI CLI)
        {

            if (CLI.Environment.TryGetValue(EnvironmentKey.RemoteSystemId, out var values) &&
                values.Count > 0)
            {
                return true;
            }

            return false;

        }

        public static NetworkingNode_Id? GetRemoteSystemId(this ICentralSystemCLI CLI)
        {

            if (CLI.Environment.TryGetValue(EnvironmentKey.RemoteSystemId, out var values) &&
                values.Count > 0 &&
                NetworkingNode_Id.TryParse(values.First(), out var remoteSystemId))
            {
                return remoteSystemId;
            }

            return null;

        }

        public static SourceRouting? GetRemoteSystemSourceRoute(this ICentralSystemCLI CLI)
        {

            if (CLI.Environment.TryGetValue(EnvironmentKey.RemoteSystemId, out var values) &&
                values.Count > 0 &&
                NetworkingNode_Id.TryParse(values.First(), out var remoteSystemId))
            {
                return SourceRouting.To(remoteSystemId);
            }

            return null;

        }

        public static String? GetRemoteSystemOCPPVersion(this ICentralSystemCLI CLI)
        {

            if (CLI.Environment.TryGetValue(EnvironmentKey.RemoteSystemOCPPVersion, out var values) &&
                values.Count > 0)
            {
                return values.First();
            }

            return null;

        }

    }


    public interface ICentralSystemCLI : ICLI
    {

        OCPPAdapter                     OCPP                          { get; }

        IEnumerable<NetworkingNode_Id>  ConnectedNetworkingNodeIds    { get; }


    }

}
