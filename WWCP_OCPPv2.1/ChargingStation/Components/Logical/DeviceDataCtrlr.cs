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

using org.GraphDefined.Vanaheimr.Illias;

using cloud.charging.open.protocols.OCPP;
using System.Runtime.InteropServices;
using org.GraphDefined.Vanaheimr.Hermod.Mail;
using System.Drawing;

#endregion

namespace cloud.charging.open.protocols.OCPPv2_1
{

    /// <summary>
    /// Logical Component responsible for configuration relating to the
    /// exchange and storage of charging station device model data.
    /// </summary>
    public class DeviceDataCtrlr : ALogicalComponentConfig
    {

        #region Properties

        #region BytesPerMessage

        /// <summary>
        /// Maximum number of entries that can be sent in one message.
        /// </summary>
        [Mandatory]
        public BytesPerMessageClass  BytesPerMessage    { get; }

        public class BytesPerMessageClass
        {

            /// <summary>
            /// Message Size (in bytes) - puts constraint on GetReportRequest message size.
            /// </summary>
            [Mandatory]
            public UInt32  GetReport       { get; set; }

            /// <summary>
            /// Message Size (in bytes) - puts constraint on GetVariablesRequest message size.
            /// </summary>
            [Mandatory]
            public UInt32  GetVariables    { get; set; }

            /// <summary>
            /// Message Size (in bytes) - puts constraint on SetVariablesRequest message size.
            /// </summary>
            [Mandatory]
            public UInt32  SetVariables    { get; set; }

        }

        #endregion

        #region ItemsPerMessage

        /// <summary>
        /// Maximum number of entries that can be sent in one message.
        /// </summary>
        [Mandatory]
        public ItemsPerMessageClass  ItemsPerMessage    { get; }

        public class ItemsPerMessageClass
        {

            /// <summary>
            /// Maximum number of ComponentVariable entries that can be sent in one GetReportRequest message.
            /// </summary>
            [Mandatory]
            public UInt32  GetReport       { get; set; }

            /// <summary>
            /// Maximum number of GetVariableData objects in GetVariablesRequest.
            /// </summary>
            [Mandatory]
            public UInt32  GetVariables    { get; set; }

            /// <summary>
            /// Maximum number of SetVariableData objects in SetVariablesRequest.
            /// </summary>
            [Mandatory]
            public UInt32  SetVariables    { get; set; }

        }

        #endregion

        #region ValueSize

        /// <summary>
        /// Limit a field.
        /// </summary>
        [Mandatory]
        public ValueSizeClass ValueSize { get; }

        public class ValueSizeClass
        {

            /// <summary>
            /// This Configuration Variable can be used to limit the following fields: SetVariableData.attributeValue and VariableCharacteristics.valueList.
            /// The max size of these values will always remain equal.
            /// </summary>
            public UInt32?  Configuration    { get; }

            /// <summary>
            /// This Configuration Variable can be used to limit the following fields: GetVariableResult.attributeValue, VariableAttribute.value and EventData.actualValue.
            /// The max size of these values will always remain equal.
            /// </summary>
            public UInt32?  Reporting        { get; }

        }

        #endregion

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Create a new device data controller.
        /// </summary>
        /// <param name="BytesPerMessage">Message Size (in bytes) - maxLimit used to report constraint on message size.</param>
        /// <param name="ItemsPerMessage">Maximum number of entries that can be sent in one message.</param>
        /// <param name="ValueSize">Can be used to limit the following fields: SetVariableData.attributeValue, GetVariableResult.attributeValue, VariableAttribute.value, VariableCharacteristics.valueList and EventData.actualValue.</param>
        /// 
        /// <param name="Instance">The optional case insensitive name of the instance in case the component exists as multiple instances.</param>
        /// <param name="CustomData">An optional custom data object to allow to store any kind of customer specific data.</param>
        public DeviceDataCtrlr(BytesPerMessageClass  BytesPerMessage,
                               ItemsPerMessageClass  ItemsPerMessage,
                               ValueSizeClass        ValueSize,

                               String?               Instance     = null,
                               CustomData?           CustomData   = null)

            : base(nameof(DeviceDataCtrlr),
                   Instance,
                   new[] {

                       #region BytesPerMessage (GetReport)

                       new VariableConfig(

                           Name:              "BytesPerMessage",
                           Instance:          "GetReport",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Message Size (in bytes) - puts constraint on GetReportRequest message size."),

                           CustomData:        null

                       ),

                       #endregion

                       #region BytesPerMessage (GetVariables)

                       new VariableConfig(

                           Name:              "BytesPerMessage",
                           Instance:          "GetVariables",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Message Size (in bytes) - puts constraint on GetVariablesRequest message size."),

                           CustomData:        null

                       ),

                       #endregion

                       #region BytesPerMessage (GetVariables)

                       new VariableConfig(

                           Name:              "BytesPerMessage",
                           Instance:          "SetVariables",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Message Size (in bytes) - puts constraint on SetVariablesRequest message size."),

                           CustomData:        null

                       ),

                       #endregion


                       #region ItemsPerMessage (GetReport)

                       new VariableConfig(

                           Name:              "ItemsPerMessage",
                           Instance:          "GetReport",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Maximum number of ComponentVariable entries that can be sent in one GetReportRequest message."),

                           CustomData:        null

                       ),

                       #endregion

                       #region ItemsPerMessage (GetVariables)

                       new VariableConfig(

                           Name:              "ItemsPerMessage",
                           Instance:          "GetVariables",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Maximum number of GetVariableData objects in GetVariablesRequest."),

                           CustomData:        null

                       ),

                       #endregion

                       #region ItemsPerMessage (SetVariables)

                       new VariableConfig(

                           Name:              "ItemsPerMessage",
                           Instance:          "SetVariables",

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer
                                                   )
                                               },

                           Description:       I18NString.Create("Maximum number of SetVariableData objects in SetVariablesRequest."),

                           CustomData:        null

                       ),

                       #endregion


                       #region ValueSize (Configuration)

                       new VariableConfig(

                           Name:              "ConfigurationValueSize",
                           Instance:          null,

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer,
                                                       MaxLimit:    1000
                                                   )
                                               },

                           Description:       I18NString.Create("This Configuration Variable can be used to limit the following fields: SetVariableData.attributeValue and\r\nVariableCharacteristics.valueList. The max size of these values will always remain equal."),

                           CustomData:        null

                       ),

                       #endregion

                       #region ValueSize (Reporting)

                       new VariableConfig(

                           Name:              "ReportingValueSize",
                           Instance:          null,

                           Attributes:        new[] {
                                                   new VariableAttribute(
                                                       Mutability:  MutabilityTypes.ReadOnly
                                                   )
                                               },

                           Characteristics:   new[] {
                                                   new VariableCharacteristics(
                                                       DataType:    DataTypes.Integer,
                                                       MaxLimit:    2500
                                                   )
                                               },

                           Description:       I18NString.Create("This Configuration Variable can be used to limit the following fields: GetVariableResult.attributeValue,\r\nVariableAttribute.value and EventData.actualValue. The max size of these values will always remain equal.\r\n"),

                           CustomData:        null

                       ),

                       #endregion

                   },
                   I18NString.Create("Logical Component responsible for configuration relating to the exchange and storage of charging station device model data."),
                   CustomData)

        {

            this.BytesPerMessage  = BytesPerMessage;
            this.ItemsPerMessage  = ItemsPerMessage;
            this.ValueSize        = ValueSize;

        }

        #endregion


    }

}
