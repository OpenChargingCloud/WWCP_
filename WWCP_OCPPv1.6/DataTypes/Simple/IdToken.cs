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

using System;

using org.GraphDefined.Vanaheimr.Illias;

#endregion

namespace cloud.charging.open.protocols.OCPPv1_6
{

    /// <summary>
    /// Extention methods for identification tokens.
    /// </summary>
    public static class IdTokenExtentions
    {

        /// <summary>
        /// Indicates whether this identification token is null or empty.
        /// </summary>
        /// <param name="IdToken">An identification token.</param>
        public static Boolean IsNullOrEmpty(this IdToken? IdToken)
            => !IdToken.HasValue || IdToken.Value.IsNullOrEmpty;

        /// <summary>
        /// Indicates whether this identification token is null or empty.
        /// </summary>
        /// <param name="IdToken">An identification token.</param>
        public static Boolean IsNotNullOrEmpty(this IdToken? IdToken)
            => IdToken.HasValue && IdToken.Value.IsNotNullOrEmpty;

    }


    /// <summary>
    /// An identification token.
    /// </summary>
    public readonly struct IdToken : IId,
                                     IEquatable<IdToken>,
                                     IComparable<IdToken>
    {

        #region Data

        /// <summary>
        /// The internal identification token.
        /// </summary>
        private readonly String InternalId;

        /// <summary>
        /// Private non-cryptographic random number generator.
        /// </summary>
        private static readonly Random _random = new Random();

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
            => InternalId.IsNotNullOrEmpty();

        /// <summary>
        /// The length of the tag identification.
        /// </summary>
        public UInt64 Length
            => (UInt64) InternalId?.Length;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new identification token.
        /// </summary>
        /// <param name="Token">A string (20 characters).</param>
        private IdToken(String  Token)
        {
            this.InternalId = Token;
        }

        #endregion


        #region (static) Random(Length)

        /// <summary>
        /// Create a new random identification token.
        /// </summary>
        /// <param name="Length">The expected length of the random identification token.</param>
        public static IdToken Random(Byte Length = 8)

            => new IdToken(_random.RandomString(Length).ToUpper());

        #endregion

        #region (static) Parse   (Text)

        /// <summary>
        /// Parse the given string as an identification token.
        /// </summary>
        /// <param name="Text">A text representation of an identification token.</param>
        public static IdToken Parse(String Text)
        {

            if (TryParse(Text, out IdToken idToken))
                return idToken;

            throw new ArgumentException("Invalid text-representation of an identification token: '" + Text + "'!",
                                        nameof(Text));

        }

        #endregion

        #region (static) TryParse(Text)

        /// <summary>
        /// Try to parse the given string as an identification token.
        /// </summary>
        /// <param name="Text">A text representation of an identification token.</param>
        public static IdToken? TryParse(String Text)
        {

            if (TryParse(Text, out IdToken idToken))
                return idToken;

            return null;

        }

        #endregion

        #region (static) TryParse(Text, out IdToken)

        /// <summary>
        /// Try to parse the given string as an identification token.
        /// </summary>
        /// <param name="Text">A text representation of an identification token.</param>
        /// <param name="IdToken">The parsed identification token.</param>
        public static Boolean TryParse(String Text, out IdToken IdToken)
        {

            Text = Text?.Trim();

            if (Text.IsNotNullOrEmpty())
            {
                try
                {
                    IdToken = new IdToken(Text);
                    return true;
                }
                catch
                { }
            }

            IdToken = default;
            return false;

        }

        #endregion

        #region Clone

        /// <summary>
        /// Clone this identification token.
        /// </summary>
        public IdToken Clone
            => new IdToken(new String(InternalId.ToCharArray()));

        #endregion


        #region Operator overloading

        #region Operator == (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (IdToken IdToken1,
                                           IdToken IdToken2)

            => IdToken1.Equals(IdToken2);

        #endregion

        #region Operator != (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (IdToken IdToken1,
                                           IdToken IdToken2)

            => !IdToken1.Equals(IdToken2);

        #endregion

        #region Operator <  (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator < (IdToken IdToken1,
                                          IdToken IdToken2)

            => IdToken1.CompareTo(IdToken2) < 0;

        #endregion

        #region Operator <= (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator <= (IdToken IdToken1,
                                           IdToken IdToken2)

            => IdToken1.CompareTo(IdToken2) <= 0;

        #endregion

        #region Operator >  (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator > (IdToken IdToken1,
                                          IdToken IdToken2)

            => IdToken1.CompareTo(IdToken2) > 0;

        #endregion

        #region Operator >= (IdToken1, IdToken2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken1">A identification token.</param>
        /// <param name="IdToken2">Another identification token.</param>
        /// <returns>true|false</returns>
        public static Boolean operator >= (IdToken IdToken1,
                                           IdToken IdToken2)

            => IdToken1.CompareTo(IdToken2) >= 0;

        #endregion

        #endregion

        #region IComparable<IdToken> Members

        #region CompareTo(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        public Int32 CompareTo(Object Object)

            => Object is IdToken idToken
                   ? CompareTo(idToken)
                   : throw new ArgumentException("The given object is not an identification token!",
                                                 nameof(Object));

        #endregion

        #region CompareTo(IdToken)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="IdToken">An object to compare with.</param>
        public Int32 CompareTo(IdToken IdToken)

            => String.Compare(InternalId,
                              IdToken.InternalId,
                              StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region IEquatable<IdToken> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Object">An object to compare with.</param>
        /// <returns>true|false</returns>
        public override Boolean Equals(Object Object)

            => Object is IdToken idToken &&
                   Equals(idToken);

        #endregion

        #region Equals(IdToken)

        /// <summary>
        /// Compares two identification tokens for equality.
        /// </summary>
        /// <param name="IdToken">A identification token to compare with.</param>
        /// <returns>True if both match; False otherwise.</returns>
        public Boolean Equals(IdToken IdToken)

            => String.Equals(InternalId,
                             IdToken.InternalId,
                             StringComparison.OrdinalIgnoreCase);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()

            => InternalId?.ToUpper().GetHashCode() ?? 0;

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
