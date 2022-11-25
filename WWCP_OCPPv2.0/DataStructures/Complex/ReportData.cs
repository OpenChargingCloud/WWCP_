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

#endregion

namespace cloud.charging.open.protocols.OCPPv2_0
{

    /// <summary>
    /// Report data.
    /// </summary>
    public class ReportData : ACustomData,
                              IEquatable<ReportData>
    {

        #region Properties

        /// <summary>
        /// The component for which a report of the monitoring report was requested.
        /// </summary>
        [Mandatory]
        public Component                       Component                  { get; }

        /// <summary>
        /// The variable for which the monitoring report was is requested.
        /// </summary>
        [Mandatory]
        public Variable                        Variable                   { get; }

        /// <summary>
        /// The attribute data of the variable.
        /// </summary>
        [Mandatory]
        public IEnumerable<VariableAttribute>  VariableAttributes         { get; }

        /// <summary>
        /// Optional fixed read-only parameters of a variable.
        /// </summary>
        [Optional]
        public VariableCharacteristics         VariableCharacteristics    { get; }

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create new monitoring data.
        /// </summary>
        /// <param name="Component">The component for which a report of the monitoring report was requested.</param>
        /// <param name="Variable">The variable for which the monitoring report was is requested.</param>
        /// <param name="VariableAttributes">An enumeration of monitors for the given report data pair.</param>
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public ReportData(Component                       Component,
                          Variable                        Variable,
                          IEnumerable<VariableAttribute>  VariableAttributes,
                          VariableCharacteristics         VariableCharacteristics,
                          CustomData?                     CustomData   = null)

            : base(CustomData)

        {

            if (!VariableAttributes.Any())
                throw new ArgumentException("The given enumeration of variable attributes must not be empty!",
                                            nameof(VariableAttributes));

            this.Component                = Component;
            this.Variable                 = Variable;
            this.VariableAttributes       = VariableAttributes.Distinct();
            this.VariableCharacteristics  = VariableCharacteristics;

        }

        #endregion


        #region Documentation

        // "ReportDataType": {
        //   "description": "Class to report components, variables and variable attributes and characteristics.\r\n",
        //   "javaType": "ReportData",
        //   "type": "object",
        //   "additionalProperties": false,
        //   "properties": {
        //     "customData": {
        //       "$ref": "#/definitions/CustomDataType"
        //     },
        //     "component": {
        //       "$ref": "#/definitions/ComponentType"
        //     },
        //     "variable": {
        //       "$ref": "#/definitions/VariableType"
        //     },
        //     "variableAttribute": {
        //       "type": "array",
        //       "additionalItems": false,
        //       "items": {
        //         "$ref": "#/definitions/VariableAttributeType"
        //       },
        //       "minItems": 1,
        //       "maxItems": 4
        //     },
        //     "variableCharacteristics": {
        //       "$ref": "#/definitions/VariableCharacteristicsType"
        //     }
        //   },
        //   "required": [
        //     "component",
        //     "variable",
        //     "variableAttribute"
        //   ]
        // }

        #endregion

        #region (static) Parse   (JSON, CustomReportDataParser = null)

        /// <summary>
        /// Parse the given JSON representation of report data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="CustomReportDataParser">A delegate to parse custom report data JSON objects.</param>
        public static ReportData Parse(JObject                                   JSON,
                                       CustomJObjectParserDelegate<ReportData>?  CustomReportDataParser   = null)
        {

            if (TryParse(JSON,
                         out var reportData,
                         out var errorResponse,
                         CustomReportDataParser))
            {
                return reportData!;
            }

            throw new ArgumentException("The given JSON representation of report data is invalid: " + errorResponse,
                                        nameof(JSON));

        }

        #endregion

        #region (static) TryParse(JSON, out ReportData, CustomReportDataParser = null)

        // Note: The following is needed to satisfy pattern matching delegates! Do not refactor it!

        /// <summary>
        /// Try to parse the given JSON representation of report data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ReportData">The parsed report data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        public static Boolean TryParse(JObject          JSON,
                                       out ReportData?  ReportData,
                                       out String?      ErrorResponse)

            => TryParse(JSON,
                        out ReportData,
                        out ErrorResponse);


        /// <summary>
        /// Try to parse the given JSON representation of report data.
        /// </summary>
        /// <param name="JSON">The JSON to be parsed.</param>
        /// <param name="ReportData">The parsed report data.</param>
        /// <param name="ErrorResponse">An optional error response.</param>
        /// <param name="CustomReportDataParser">A delegate to parse custom report data JSON objects.</param>
        public static Boolean TryParse(JObject                                   JSON,
                                       out ReportData?                           ReportData,
                                       out String?                               ErrorResponse,
                                       CustomJObjectParserDelegate<ReportData>?  CustomReportDataParser)
        {

            try
            {

                ReportData = default;

                #region Component                  [mandatory]

                if (!JSON.ParseMandatoryJSON("component",
                                             "component",
                                             OCPPv2_0.Component.TryParse,
                                             out Component? Component,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (Component is null)
                    return false;

                #endregion

                #region Variable                   [mandatory]

                if (!JSON.ParseMandatoryJSON("variable",
                                             "variable",
                                             OCPPv2_0.Variable.TryParse,
                                             out Variable? Variable,
                                             out ErrorResponse))
                {
                    return false;
                }

                if (Variable is null)
                    return false;

                #endregion

                #region VariableAttributes         [mandatory]

                if (!JSON.ParseMandatoryHashSet("variableAttribute",
                                                "variable attribute",
                                                VariableAttribute.TryParse,
                                                out HashSet<VariableAttribute> VariableAttributes,
                                                out ErrorResponse))
                {
                    return false;
                }

                #endregion

                #region VariableCharacteristics    [optional]

                if (JSON.ParseOptionalJSON("variableCharacteristics",
                                           "variable characteristics",
                                           OCPPv2_0.VariableCharacteristics.TryParse,
                                           out VariableCharacteristics? VariableCharacteristics,
                                           out ErrorResponse))
                {
                    return false;
                }

                if (VariableCharacteristics is null)
                    return false;

                #endregion

                #region CustomData                 [optional]

                if (JSON.ParseOptionalJSON("customData",
                                           "custom data",
                                           OCPPv2_0.CustomData.TryParse,
                                           out CustomData CustomData,
                                           out ErrorResponse))
                {
                    if (ErrorResponse is not null)
                        return false;
                }

                #endregion


                ReportData = new ReportData(Component,
                                            Variable,
                                            VariableAttributes,
                                            VariableCharacteristics,
                                            CustomData);

                if (CustomReportDataParser is not null)
                    ReportData = CustomReportDataParser(JSON,
                                                        ReportData);

                return true;

            }
            catch (Exception e)
            {
                ReportData     = default;
                ErrorResponse  = "The given JSON representation of report data is invalid: " + e.Message;
                return false;
            }

        }

        #endregion

        #region ToJSON(CustomReportDataResponseSerializer = null, CustomComponentResponseSerializer = null, ...)

        /// <summary>
        /// Return a JSON representation of this object.
        /// </summary>
        /// <param name="CustomReportDataResponseSerializer">A delegate to serialize custom report data objects.</param>
        /// <param name="CustomComponentResponseSerializer">A delegate to serialize custom component objects.</param>
        /// <param name="CustomEVSEResponseSerializer">A delegate to serialize custom EVSE objects.</param>
        /// <param name="CustomVariableResponseSerializer">A delegate to serialize custom variable objects.</param>
        /// <param name="CustomVariableAttributeResponseSerializer">A delegate to serialize custom variable attribute objects.</param>
        /// <param name="CustomVariableCharacteristicsResponseSerializer">A delegate to serialize custom variable characteristics objects.</param>
        /// <param name="CustomCustomDataResponseSerializer">A delegate to serialize CustomData objects.</param>
        public JObject ToJSON(CustomJObjectSerializerDelegate<ReportData>?               CustomReportDataResponseSerializer                = null,
                              CustomJObjectSerializerDelegate<Component>?                CustomComponentResponseSerializer                 = null,
                              CustomJObjectSerializerDelegate<EVSE>?                     CustomEVSEResponseSerializer                      = null,
                              CustomJObjectSerializerDelegate<Variable>?                 CustomVariableResponseSerializer                  = null,
                              CustomJObjectSerializerDelegate<VariableAttribute>?        CustomVariableAttributeResponseSerializer         = null,
                              CustomJObjectSerializerDelegate<VariableCharacteristics>?  CustomVariableCharacteristicsResponseSerializer   = null,
                              CustomJObjectSerializerDelegate<CustomData>?               CustomCustomDataResponseSerializer                = null)
        {

            var JSON = JSONObject.Create(

                                 new JProperty("component",                Component.              ToJSON(CustomComponentResponseSerializer,
                                                                                                          CustomEVSEResponseSerializer,
                                                                                                          CustomCustomDataResponseSerializer)),

                                 new JProperty("variable",                 Variable.               ToJSON(CustomVariableResponseSerializer,
                                                                                                          CustomCustomDataResponseSerializer)),

                                 new JProperty("variableAttribute",        new JArray(VariableAttributes.Select(variableAttribute => variableAttribute.ToJSON(CustomVariableAttributeResponseSerializer,
                                                                                                                                                              CustomCustomDataResponseSerializer)))),

                           VariableCharacteristics is not null
                               ? new JProperty("variableCharacteristics",  VariableCharacteristics.ToJSON(CustomVariableCharacteristicsResponseSerializer,
                                                                                                          CustomCustomDataResponseSerializer))
                               : null,

                           CustomData is not null
                               ? new JProperty("customData",               CustomData.             ToJSON(CustomCustomDataResponseSerializer))
                               : null

                       );

            return CustomReportDataResponseSerializer is not null
                       ? CustomReportDataResponseSerializer(this, JSON)
                       : JSON;

        }

        #endregion


        #region Operator overloading

        #region Operator == (ReportData1, ReportData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReportData1">Report data.</param>
        /// <param name="ReportData2">Other report data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator == (ReportData? ReportData1,
                                           ReportData? ReportData2)
        {

            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(ReportData1, ReportData2))
                return true;

            // If one is null, but not both, return false.
            if (ReportData1 is null || ReportData2 is null)
                return false;

            return ReportData1.Equals(ReportData2);

        }

        #endregion

        #region Operator != (ReportData1, ReportData2)

        /// <summary>
        /// Compares two instances of this object.
        /// </summary>
        /// <param name="ReportData1">Report data.</param>
        /// <param name="ReportData2">Other report data.</param>
        /// <returns>true|false</returns>
        public static Boolean operator != (ReportData? ReportData1,
                                           ReportData? ReportData2)

            => !(ReportData1 == ReportData2);

        #endregion

        #endregion

        #region IEquatable<ReportData> Members

        #region Equals(Object)

        /// <summary>
        /// Compares two report data for equality.
        /// </summary>
        /// <param name="Object">Report data to compare with.</param>
        public override Boolean Equals(Object? Object)

            => Object is ReportData reportData &&
                   Equals(reportData);

        #endregion

        #region Equals(ReportData)

        /// <summary>
        /// Compares two report data for equality.
        /// </summary>
        /// <param name="ReportData">Report data to compare with.</param>
        public Boolean Equals(ReportData? ReportData)

            => ReportData is not null &&

               Component.Equals(ReportData.Component) &&
               Variable. Equals(ReportData.Variable)  &&

               VariableAttributes.Count().Equals(ReportData.VariableAttributes.Count())     &&
               VariableAttributes.All(data => ReportData.VariableAttributes.Contains(data)) &&

             ((VariableCharacteristics is     null && ReportData.VariableCharacteristics is     null) ||
              (VariableCharacteristics is not null && ReportData.VariableCharacteristics is not null && VariableCharacteristics.Equals(ReportData.VariableCharacteristics))) &&

               base.     Equals(ReportData);

        #endregion

        #endregion

        #region (override) GetHashCode()

        /// <summary>
        /// Return the HashCode of this object.
        /// </summary>
        /// <returns>The HashCode of this object.</returns>
        public override Int32 GetHashCode()
        {
            unchecked
            {

                return Component.               GetHashCode()       * 11 ^
                       Variable.                GetHashCode()       *  7 ^
                       //ToDo: Add VariableAttributes!
                      (VariableCharacteristics?.GetHashCode() ?? 0) *  3 ^

                       base.                    GetHashCode();

            }
        }

        #endregion

        #region (override) ToString()

        /// <summary>
        /// Return a text representation of this object.
        /// </summary>
        public override String ToString()

            => String.Concat(

                   "Component: ", Component.ToString(), ", ",
                   "Variable: ",  Variable. ToString(), ", ",

                   VariableAttributes.Count(), " variable attribute(s)",

                   VariableCharacteristics is not null
                       ? ", " + VariableCharacteristics.ToString()
                       : ""

               );

        #endregion

    }

}