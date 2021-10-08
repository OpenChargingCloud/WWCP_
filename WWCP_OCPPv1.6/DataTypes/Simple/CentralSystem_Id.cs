﻿/*
 * Copyright (c) 2014-2021 GraphDefined GmbH
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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6
{

    /// <summary>
    /// Extention methods for central system identifications.
    /// </summary>
    public static class CentralSystemIdExtentions
    {

        /// <summary>
        /// Indicates whether this central system identification is null or empty.
        /// </summary>
        /// <param name="CentralSystemId">A central system identification.</param>
        public static Boolean IsNullOrEmpty(this CentralSystem_Id? CentralSystemId)
            => !CentralSystemId.HasValue || CentralSystemId.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this central system identification is null or empty.
        /// </summary>
        /// <param name="CentralSystemId">A central system identification.</param>
        public static Boolean IsNotNullOrEmpty(this CentralSystem_Id? CentralSystemId)
            => CentralSystemId.HasValue && CentralSystemId.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// A central system identification.
    /// </summary>
    public readonly struct CentralSystem_Id : IId,
                                              IEquatable<CentralSystem_Id>,
                                              IComparable<CentralSystem_Id>
    {

        #region Data

        /// <summary>
        /// The internal identification.
        /// </summary>
        private readonly String InternalId;

        private static readonly Random random = new Random();

        #endregion

        #region Properties

        /// <summary>
        /// Indicates whether this identification is null or empty.
        /// </summary>
        public Boolean IsNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// Indicates whether this identification is NOT null or empty.
        /// </summary>
        public Boolean IsNotNullOrEmpty
            => InternalId.IsNullOrEmpty();

        /// <summary>
        /// The length of the central system identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) (InternalId?.Length ?? 0);

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new central system identification based on the given string.
        /// </summary>
        /// <param name="String">The string representation of the central system identification.</param>
        private CentralSystem_Id(String String)
        {
            this.InternalId  = String;
        }

        #endregion


        #region (static) Random  (Length = 30)

        /// <summary>
        /// Create a new random central system identification.
        /// </summary>
        /// <param name="Length">The expected length of the central system identification.</param>
        public static CentralSystem_Id Random(Byte Length = 30)

            => new CentralSystem_Id(random.RandomString(Length));

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as central system identification.
        /// </summary>
        /// <param name="Text">A text representation of a central system identification.</param>
        public static CentralSystem_Id Parse(String Text)
        {

            if (TryParse(Text, out CentralSystem_Id chargeBoxId))
                return chargeBoxId;

            if (Text.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(Text), "The given text representation of a central system identification must not be null or empty!");

            throw new ArgumentException("The given text representation of a central system identification is invalid!", nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given text as central system identification.
        /// </summary>
        /// <param name="Text">A text representation of a central system identification.</param>
        public static CentralSystem_Id? TryParse(String Text)
        {

            if (TryParse(Text, out CentralSystem_Id chargeBoxId))
                return chargeBoxId;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out CentralSystemId)

        /// <summary>
        /// Try to parse the given text as central system identification.
        /// </summary>
        /// <param name="Text">A text representation of a central system identification.</param>
        /// <param name="CentralSystemId">The parsed central system identification.</param>
        public static Boolean TryParse(String Text, out CentralSystem_Id CentralSystemId)
        {

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    CentralSystemId = new CentralSystem_Id(Text.Trim());
                    return true;
                }
                catch (Exception)
                { }
            }

            CentralSystemId = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this central system identification.
        /// </summary>
        public CentralSystem_Id Clone

            => new CentralSystem_Id(
                   new String(InternalId?.ToCharArray())
               );

        #endregion


        #region Operator overloading

        #region Operator == (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (CentralSystem_Id CentralSystemId1,
                                           CentralSystem_Id CentralSystemId2)

            => CentralSystemId1.Equals(CentralSystemId2);

        #endregion

        #region Operator != (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (CentralSystem_Id CentralSystemId1,
                                           CentralSystem_Id CentralSystemId2)

            => !CentralSystemId1.Equals(CentralSystemId2);

        #endregion

        #region Operator <  (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (CentralSystem_Id CentralSystemId1,
                                          CentralSystem_Id CentralSystemId2)

            => CentralSystemId1.CompareTo(CentralSystemId2) < 0;

        #endregion

        #region Operator <= (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (CentralSystem_Id CentralSystemId1,
                                           CentralSystem_Id CentralSystemId2)

            => CentralSystemId1.CompareTo(CentralSystemId2) <= 0;

        #endregion

        #region Operator >  (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (CentralSystem_Id CentralSystemId1,
                                          CentralSystem_Id CentralSystemId2)

            => CentralSystemId1.CompareTo(CentralSystemId2) > 0;

        #endregion

        #region Operator >= (CentralSystemId1, CentralSystemId2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId1">A central system identification.</param>
        /// <param name="CentralSystemId2">Another central system identification.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (CentralSystem_Id CentralSystemId1,
                                           CentralSystem_Id CentralSystemId2)

            => CentralSystemId1.CompareTo(CentralSystemId2) >= 0;

        #endregion

        #endregion

        #region IComparable<CentralSystemId> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is CentralSystem_Id chargeBoxId
                   ? CompareTo(chargeBoxId)
                   : throw new ArgumentException("The given object is not a central system identification!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(CentralSystemId)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="CentralSystemId">An object to compare with.</param>
        public Int32 CompareTo(CentralSystem_Id CentralSystemId)

            => String.Compare(InternalId,
                              CentralSystemId.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<CentralSystemId> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is CentralSystem_Id chargeBoxId &&
                   Equals(chargeBoxId);

        #endregion

        #region Equals(CentralSystemId)

        /// <summary>
        /// Compares two central system identifications for equality.
        /// </summary>
        /// <param name="CentralSystemId">A central system identification to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(CentralSystem_Id CentralSystemId)

            => String.Equals(InternalId,
                             CentralSystemId.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region GetHashCode()

        /// <summary>
        /// Return the hash code of this object.
        /// </summary>
        /// <returns>The hash code of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToLower().GetHashCode() ?? 0;

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => InternalId ?? "";

        #endregion

    }

}
