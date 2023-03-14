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

namespace cloud.charging.open.protocols.OCPPv2_0.CS
{

    #region OnReset

    /// <summary>
    /// A delegate called whenever a reset request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnResetRequestDelegate(DateTime               LogTimestamp,
                                                ICentralSystemClient   Sender,
                                                ResetRequest           Request);

    /// <summary>
    /// A delegate called whenever a response to a reset request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnResetResponseDelegate(DateTime               LogTimestamp,
                                                 ICentralSystemClient   Sender,
                                                 CS.ResetRequest        Request,
                                                 CP.ResetResponse       Response,
                                                 TimeSpan               Runtime);

    #endregion

    #region OnChangeAvailability

    /// <summary>
    /// A delegate called whenever a change availability request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnChangeAvailabilityRequestDelegate(DateTime                    LogTimestamp,
                                                             ICentralSystemClient        Sender,
                                                             ChangeAvailabilityRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a change availability request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnChangeAvailabilityResponseDelegate(DateTime                        LogTimestamp,
                                                              ICentralSystemClient            Sender,
                                                              CS.ChangeAvailabilityRequest    Request,
                                                              CP.ChangeAvailabilityResponse   Response,
                                                              TimeSpan                        Runtime);

    #endregion

    #region OnDataTransfer

    /// <summary>
    /// A delegate called whenever a data transfer request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnDataTransferRequestDelegate(DateTime                 LogTimestamp,
                                                       ICentralSystemClient     Sender,
                                                       CS.DataTransferRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a data transfer request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnDataTransferResponseDelegate(DateTime                  LogTimestamp,
                                                        ICentralSystemClient      Sender,
                                                        CS.DataTransferRequest    Request,
                                                        CP.DataTransferResponse   Response,
                                                        TimeSpan                  Runtime);

    #endregion

    #region OnTriggerMessage

    /// <summary>
    /// A delegate called whenever a trigger message request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnTriggerMessageRequestDelegate(DateTime                LogTimestamp,
                                                         ICentralSystemClient    Sender,
                                                         TriggerMessageRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a trigger message request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnTriggerMessageResponseDelegate(DateTime                    LogTimestamp,
                                                          ICentralSystemClient        Sender,
                                                          CS.TriggerMessageRequest    Request,
                                                          CP.TriggerMessageResponse   Response,
                                                          TimeSpan                    Runtime);

    #endregion

    #region OnUpdateFirmware

    /// <summary>
    /// A delegate called whenever a update firmware request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnUpdateFirmwareRequestDelegate(DateTime                LogTimestamp,
                                                         ICentralSystemClient    Sender,
                                                         UpdateFirmwareRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a update firmware request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnUpdateFirmwareResponseDelegate(DateTime                    LogTimestamp,
                                                          ICentralSystemClient        Sender,
                                                          CS.UpdateFirmwareRequest    Request,
                                                          CP.UpdateFirmwareResponse   Response,
                                                          TimeSpan                    Runtime);

    #endregion


    #region OnReserveNow

    /// <summary>
    /// A delegate called whenever a reserve now request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnReserveNowRequestDelegate(DateTime               LogTimestamp,
                                                     ICentralSystemClient   Sender,
                                                     ReserveNowRequest      Request);

    /// <summary>
    /// A delegate called whenever a response to a reserve now request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnReserveNowResponseDelegate(DateTime               LogTimestamp,
                                                      ICentralSystemClient   Sender,
                                                      CS.ReserveNowRequest   Request,
                                                      CP.ReserveNowResponse  Response,
                                                      TimeSpan               Runtime);

    #endregion

    #region OnCancelReservation

    /// <summary>
    /// A delegate called whenever a cancel reservation request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnCancelReservationRequestDelegate(DateTime                   LogTimestamp,
                                                            ICentralSystemClient       Sender,
                                                            CancelReservationRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a cancel reservation request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnCancelReservationResponseDelegate(DateTime                       LogTimestamp,
                                                             ICentralSystemClient           Sender,
                                                             CS.CancelReservationRequest    Request,
                                                             CP.CancelReservationResponse   Response,
                                                             TimeSpan                       Runtime);

    #endregion

    #region OnUnlockConnector

    /// <summary>
    /// A delegate called whenever a unlock connector request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnUnlockConnectorRequestDelegate(DateTime                 LogTimestamp,
                                                          ICentralSystemClient     Sender,
                                                          UnlockConnectorRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a unlock connector request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnUnlockConnectorResponseDelegate(DateTime                     LogTimestamp,
                                                           ICentralSystemClient         Sender,
                                                           CS.UnlockConnectorRequest    Request,
                                                           CP.UnlockConnectorResponse   Response,
                                                           TimeSpan                     Runtime);

    #endregion

    #region OnSetChargingProfile

    /// <summary>
    /// A delegate called whenever a set charging profile request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnSetChargingProfileRequestDelegate(DateTime                    LogTimestamp,
                                                             ICentralSystemClient        Sender,
                                                             SetChargingProfileRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a set charging profile request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnSetChargingProfileResponseDelegate(DateTime                        LogTimestamp,
                                                              ICentralSystemClient            Sender,
                                                              CS.SetChargingProfileRequest    Request,
                                                              CP.SetChargingProfileResponse   Response,
                                                              TimeSpan                        Runtime);

    #endregion

    #region OnClearChargingProfile

    /// <summary>
    /// A delegate called whenever a clear charging profile request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnClearChargingProfileRequestDelegate(DateTime                      LogTimestamp,
                                                               ICentralSystemClient          Sender,
                                                               ClearChargingProfileRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a clear charging profile request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnClearChargingProfileResponseDelegate(DateTime                          LogTimestamp,
                                                                ICentralSystemClient              Sender,
                                                                CS.ClearChargingProfileRequest    Request,
                                                                CP.ClearChargingProfileResponse   Response,
                                                                TimeSpan                          Runtime);

    #endregion

    #region OnGetCompositeSchedule

    /// <summary>
    /// A delegate called whenever a get composite schedule request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetCompositeScheduleRequestDelegate(DateTime                      LogTimestamp,
                                                               ICentralSystemClient          Sender,
                                                               GetCompositeScheduleRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a get composite schedule request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetCompositeScheduleResponseDelegate(DateTime                          LogTimestamp,
                                                                ICentralSystemClient              Sender,
                                                                CS.GetCompositeScheduleRequest    Request,
                                                                CP.GetCompositeScheduleResponse   Response,
                                                                TimeSpan                          Runtime);

    #endregion


    #region OnGetLocalListVersion

    /// <summary>
    /// A delegate called whenever a get local list version request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetLocalListVersionRequestDelegate(DateTime                     LogTimestamp,
                                                              ICentralSystemClient         Sender,
                                                              GetLocalListVersionRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a get local list version request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetLocalListVersionResponseDelegate(DateTime                         LogTimestamp,
                                                               ICentralSystemClient             Sender,
                                                               CS.GetLocalListVersionRequest    Request,
                                                               CP.GetLocalListVersionResponse   Response,
                                                               TimeSpan                         Runtime);

    #endregion

    #region OnSendLocalList

    /// <summary>
    /// A delegate called whenever a send local list request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnSendLocalListRequestDelegate(DateTime               LogTimestamp,
                                                        ICentralSystemClient   Sender,
                                                        SendLocalListRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a send local list request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnSendLocalListResponseDelegate(DateTime                   LogTimestamp,
                                                         ICentralSystemClient       Sender,
                                                         CS.SendLocalListRequest    Request,
                                                         CP.SendLocalListResponse   Response,
                                                         TimeSpan                   Runtime);

    #endregion

    #region OnClearCache

    /// <summary>
    /// A delegate called whenever a clear cache request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnClearCacheRequestDelegate(DateTime               LogTimestamp,
                                                     ICentralSystemClient   Sender,
                                                     ClearCacheRequest      Request);

    /// <summary>
    /// A delegate called whenever a response to a clear cache request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnClearCacheResponseDelegate(DateTime                LogTimestamp,
                                                      ICentralSystemClient    Sender,
                                                      CS.ClearCacheRequest    Request,
                                                      CP.ClearCacheResponse   Response,
                                                      TimeSpan                Runtime);

    #endregion


    // Security extensions...

    #region OnCertificateSigned

    /// <summary>
    /// A delegate called whenever an install certificate request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnCertificateSignedRequestDelegate(DateTime                   LogTimestamp,
                                                            ICentralSystemClient       Sender,
                                                            CertificateSignedRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to an install certificate request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnCertificateSignedResponseDelegate(DateTime                       LogTimestamp,
                                                             ICentralSystemClient           Sender,
                                                             CS.CertificateSignedRequest    Request,
                                                             CP.CertificateSignedResponse   Response,
                                                             TimeSpan                       Runtime);

    #endregion

    #region OnDeleteCertificate

    /// <summary>
    /// A delegate called whenever a delete certificate request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnDeleteCertificateRequestDelegate(DateTime                   LogTimestamp,
                                                            ICentralSystemClient       Sender,
                                                            DeleteCertificateRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a delete certificate request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnDeleteCertificateResponseDelegate(DateTime                       LogTimestamp,
                                                             ICentralSystemClient           Sender,
                                                             CS.DeleteCertificateRequest    Request,
                                                             CP.DeleteCertificateResponse   Response,
                                                             TimeSpan                       Runtime);

    #endregion

    #region OnGetInstalledCertificateIds

    /// <summary>
    /// A delegate called whenever a get installed certificate ids request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetInstalledCertificateIdsRequestDelegate(DateTime                            LogTimestamp,
                                                                     ICentralSystemClient                Sender,
                                                                     GetInstalledCertificateIdsRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to a get installed certificate ids request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetInstalledCertificateIdsResponseDelegate(DateTime                                LogTimestamp,
                                                                      ICentralSystemClient                    Sender,
                                                                      CS.GetInstalledCertificateIdsRequest    Request,
                                                                      CP.GetInstalledCertificateIdsResponse   Response,
                                                                      TimeSpan                                Runtime);

    #endregion

    #region OnGetLog

    /// <summary>
    /// A delegate called whenever a get log request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnGetLogRequestDelegate(DateTime               LogTimestamp,
                                                 ICentralSystemClient   Sender,
                                                 GetLogRequest          Request);

    /// <summary>
    /// A delegate called whenever a response to a get log request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnGetLogResponseDelegate(DateTime               LogTimestamp,
                                                  ICentralSystemClient   Sender,
                                                  CS.GetLogRequest       Request,
                                                  CP.GetLogResponse      Response,
                                                  TimeSpan               Runtime);

    #endregion

    #region OnInstallCertificate

    /// <summary>
    /// A delegate called whenever an install certificate request will be send to a charge point.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    public delegate Task OnInstallCertificateRequestDelegate(DateTime                    LogTimestamp,
                                                             ICentralSystemClient        Sender,
                                                             InstallCertificateRequest   Request);

    /// <summary>
    /// A delegate called whenever a response to an install certificate request was received.
    /// </summary>
    /// <param name="LogTimestamp">The timestamp of the log request.</param>
    /// <param name="Sender">The sender of the request.</param>
    /// <param name="Request">The request.</param>
    /// <param name="Response">The response.</param>
    /// <param name="Runtime">The runtime of the request.</param>
    public delegate Task OnInstallCertificateResponseDelegate(DateTime                        LogTimestamp,
                                                              ICentralSystemClient            Sender,
                                                              CS.InstallCertificateRequest    Request,
                                                              CP.InstallCertificateResponse   Response,
                                                              TimeSpan                        Runtime);

    #endregion


}