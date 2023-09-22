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

namespace cloud.charging.open.protocols.OCPPv2_1
{

    /// <summary>
    /// A OCPP CSE cryptographic signature.
    /// </summary>
    public class Signature : ACustomData,
                             IEquatable<Signature>
    {

        #region Properties

        /// <summary>
        /// The unique key identification, e.g. the prefix of the public key.
        /// </summary>
        [Mandatory]
        public String   KeyId              { get; }

        /// <summary>
        /// The signature value.
        /// </summary>
        [Mandatory]
        public String   Value              { get; }

        /// <summary>
        /// The optional method used to create the digital signature.
        /// </summary>
        [Optional]
        public String?  SigningMethod      { get; }

        /// <summary>
        /// The optional encoding method.
        /// </summary>
        [Optional]
        public String?  EncodingMethod     { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new OCPP CSE cryptographic signature.
        /// </summary>
        /// <param name="KeyId">An unique key identification, e.g. a prefix of a public key.</param>
        /// <param name="Value">A signature value.</param>
        /// <param name="SigningMethod">An optional method used to create the digital signature.</param>
        /// <param name="EncodingMethod">An optional encoding method.</param>
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public Signature(String       KeyId,
                         String       Value,
                         String?      SigningMethod    = null,
                         String?      EncodingMethod   = null,
                         CustomData?  CustomData       = null)

            : base(CustomData)

        {

            this.KeyId            = KeyId;
            this.Value            = Value;
            this.SigningMethod    = SigningMethod;
            this.EncodingMethod   = EncodingMethod;

            unchecked
            {

                hashCode = KeyId.          GetHashCode()       * 11 ^
                           Value.          GetHashCode()       *  7 ^
                          (SigningMethod?. GetHashCode() ?? 0) *  5 ^
                          (EncodingMethod?.GetHashCode() ?? 0) *  3 ^

                           base.           GetHashCode();

            }

        }

        #endregion


        #region Documentation

        // tba.

        #endregion

        #region (static) Parse   (JSON, CustomSignatureParser = null)

        /// <summary>
        /// Parse the given JSON representation of a cryptographic signature.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomSignatureParser">A delegate to parse custom cryptographic signatures.</param>
        public static Signature Parse(JObject                                  JSON,
                                      CustomJObjectParserDelegate<Signature>?  CustomSignatureParser   = null)
        {

            if (TryParse(JSON,
                         out var signature,
                         out var errorResponse,
                         CustomSignatureParser))
            {
                return signature!;
            }

            throw new ArgumentException("The given JSON representation of a signature is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out Signature, out ErrorResponse, CustomSignatureParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of a signature.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="Signature">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject         JSON,
                                       out Signature?  Signature,
                                       out String?     ErrorResponse)

            => TryParse(JSON,
                        out Signature,
                        out ErrorResponse,
                        null);


        /// <summary>
        /// Try to parse the given JSON representation of a signature.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="Signature">The parsed connector type.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomSignatureParser">A delegate to parse custom signatures.</param>
        public static Boolean TryParse(JObject                                  JSON,
                                       out Signature?                           Signature,
                                       out String?                              ErrorResponse,
                                       CustomJObjectParserDelegate<Signature>?  CustomSignatureParser   = null)
        {

            try
            {

                Signature = default;

                #region KeyId             [mandatory]

                if (!JSON.ParseMandatoryText("keyId",
                                             "key identification",
                                             out String KeyId,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region Value             [mandatory]

                if (!JSON.ParseMandatoryText("value",
                                             "signature value",
                                             out String Value,
                                             out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region SigningMethod     [optional]

                var SigningMethod   = JSON.GetString("signingMethod");

                #endregion

                #region EncodingMethod    [optional]

                var EncodingMethod  = JSON.GetString("encodingMethod");

                #endregion

                #region CustomData        [optional]

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


                Signature = new Signature(
                                KeyId,
                                Value,
                                SigningMethod,
                                EncodingMethod,
                                CustomData
                            );

                if (CustomSignatureParser is not null)
                    Signature = CustomSignatureParser(JSON,
                                                      Signature);

                return true;

            }
            catch (Exception e)
            {
                Signature      = default;
                ErrorResponse  = "The given JSON representation of a signature is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomSignatureSerializer = null, CustomCustomDataSerializer = null)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomSignatureSerializer">A delegate to serialize cryptographic signature objects.</param>
        /// <param name="CustomCustomDataSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<Signature>?   CustomSignatureSerializer    = null,
                              CustomJObjectSerializerDelegate<CustomData>?  CustomCustomDataSerializer   = null)
        {

            var json = JSONObject.Create(

                                 new JProperty("keyId",            KeyId),
                                 new JProperty("Value",            Value),
                                 new JProperty("signingMethod",    SigningMethod),
                                 new JProperty("encodingMethod",   EncodingMethod),

                           CustomData is not null
                               ? new JProperty("customData",       CustomData.ToJSON(CustomCustomDataSerializer))
                               : null

                       );

            return CustomSignatureSerializer is not null
                       ? CustomSignatureSerializer(this, json)
                       : json;

        }

        #endregion


        #region Operator overloading

        #region Operator == (Signature1, Signature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Signature1">A signature.</param>
        /// <param name="Signature2">Another signature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (Signature? Signature1,
                                           Signature? Signature2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(Signature1, Signature2))
                return true;

            // If one is null, but not both, return false.
            if (Signature1 is null || Signature2 is null)
                return false;

            return Signature1.Equals(Signature2);

        }

        #endregion

        #region Operator != (Signature1, Signature2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="Signature1">A signature.</param>
        /// <param name="Signature2">Another signature.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (Signature? Signature1,
                                           Signature? Signature2)

            => !(Signature1 == Signature2);

        #endregion

        #endregion

        #region IEquatable<Signature> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two signatures for equality.
        /// </summary>
        /// <param name="Object">A signature to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is Signature signature &&
                   Equals(signature);

        #endregion

        #region Equals(Signature)

        /// <summary>
        /// Compares two signatures for equality.
        /// </summary>
        /// <param name="Signature">A signature to compare with.</param>
        public Boolean Equals(Signature? Signature)

            => Signature is not null &&

               String.Equals(KeyId,           Signature.KeyId,           StringComparison.Ordinal) &&
               String.Equals(Value,           Signature.EncodingMethod,  StringComparison.Ordinal) &&
               String.Equals(SigningMethod,   Signature.SigningMethod,   StringComparison.Ordinal) &&
               String.Equals(EncodingMethod,  Signature.EncodingMethod,  StringComparison.Ordinal) &&

               base.  Equals(Signature);

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

            => $"{KeyId}: {Value}";

        #endregion

    }

}
