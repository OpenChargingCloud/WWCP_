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
using static cloud.charging.open.protocols.OCPPv1_6.CS.CentralSystemWSServer;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6
{

    /// <summary>
    /// A general OCPP result.
    /// </summary>
    public readonly struct Result : IEquatable<Result>
    {

        #region Properties

        /// <summary>
        /// The machine-readable result code.
        /// </summary>
        public ResultCodes  ResultCode     { get; }

        /// <summary>
        /// A human-readable error description.
        /// </summary>
        public String?      Description    { get; }

        /// <summary>
        /// Human-readable error details.
        /// </summary>
        public String?      Details        { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new generic OCPP result.
        /// </summary>
        /// <param name="ResultCode">The machine-readable result code.</param>
        /// <param name="Description">A human-readable error description.</param>
        public Result(ResultCodes  ResultCode,
                      String?      Description   = null,
                      String?      Details       = null)
        {

            this.ResultCode   = ResultCode;
            this.Description  = Description?.Trim();
            this.Details      = Details?.Trim();

        }

        #endregion


        public static Result FromSendRequestState(SendRequestState SendRequestState)

            => new (
                   SendRequestState.ErrorCode ?? ResultCodes.GenericError,
                   SendRequestState.ErrorDescription,
                   SendRequestState.ErrorDetails?.ToString()
               );


        #region Static methods

        /// <summary>
        /// Unknown result code.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static Result GenericError(String? Description   = null,
                                          String? Details       = null)

            => new (ResultCodes.GenericError,
                    Description,
                    Details);


        /// <summary>
        /// Data accepted and processed.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static Result OK(String? Description = null)

            => new (ResultCodes.OK,
                    Description);


        ///// <summary>
        ///// Only part of the data was accepted.
        ///// </summary>
        ///// <param name="Description">A human-readable error description.</param>
        //public static Result Partly(String? Description = null)

        //    => new (ResultCodes.Partly,
        //            Description);


        ///// <summary>
        ///// Wrong username and/or password.
        ///// </summary>
        ///// <param name="Description">A human-readable error description.</param>
        //public static Result NotAuthorized(String? Description = null)

        //    => new (ResultCodes.NotAuthorized,
        //            Description);


        ///// <summary>
        ///// One or more ID (EVSE/Contract) were not valid for this user.
        ///// </summary>
        ///// <param name="Description">A human-readable error description.</param>
        //public static Result InvalidId(String? Description = null)

        //    => new (ResultCodes.InvalidId,
        //            Description);


        /// <summary>
        /// Internal server error.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static Result Server(String? Description = null)

            => new (ResultCodes.NetworkError,
                    Description);


        /// <summary>
        /// Data has technical errors.
        /// </summary>
        /// <param name="Description">A human-readable error description.</param>
        public static Result Format(String? Description = null)

            => new (ResultCodes.FormationViolation,
                    Description);

        #endregion


        #region Operator overloading

        #region Operator == (Result1, Result2)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="Result1">A result.</param>
        /// <param name="Result2">Another result.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public static Boolean operator == (Result Result1,
                                           Result Result2)

            => Result1.Equals(Result2);

        #endregion

        #region Operator != (Result1, Result2)

        /// <summary>
        /// Compares two results for inequality.
        /// </summary>
        /// <param name="Result1">A result.</param>
        /// <param name="Result2">Another result.</param>
        /// <returns>False if both match; True otherwise.</returns>
        public static Boolean operator != (Result Result1,
                                           Result Result2)

            => !Result1.Equals(Result2);

        #endregion

        #endregion

        #region IEquatable<Result> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="Object">A result to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Result result &&
                   Equals(result);

        #endregion

        #region Equals(Result)

        /// <summary>
        /// Compares two results for equality.
        /// </summary>
        /// <param name="Result">A result to compare with.</param>
        public Boolean Equals(Result Result)

            => ResultCode.Equals(Result.ResultCode) &&

             ((Description is     null && Result.Description is     null) ||
              (Description is not null && Result.Description is not null && Description.Equals(Result.Description)));

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return ResultCode.  GetHashCode() * 3 ^
                       Description?.GetHashCode() ?? 0;

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => ResultCode +

              (Description.IsNotNullOrEmpty()
                  ? " - " + Description
                  : "");

        #endregion

    }

}