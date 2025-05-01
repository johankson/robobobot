/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

import {
  Battle,
  BattleView,
  JoinRequest,
  JoinResponse,
  JoinSandboxRequest,
  MoveDirection,
} from "./data-contracts";
import { ContentType, HttpClient, RequestParams } from "./http-client";

export class Api<
  SecurityDataType = unknown,
> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Admin
   * @name AdminPauseServerList
   * @request GET:/api/Admin/pause-server
   */
  adminPauseServerList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Admin/pause-server`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminResumeServerList
   * @request GET:/api/Admin/resume-server
   */
  adminResumeServerList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Admin/resume-server`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminStatsList
   * @request GET:/api/Admin/stats
   */
  adminStatsList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Admin/stats`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminSetFpsDetail
   * @request GET:/api/Admin/set-fps/{fps}
   */
  adminSetFpsDetail = (fps: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Admin/set-fps/${fps}`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminViewLogList
   * @summary View the server log
   * @request GET:/api/Admin/view-log
   */
  adminViewLogList = (params: RequestParams = {}) =>
    this.request<string, any>({
      path: `/api/Admin/view-log`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminMovePlayerList
   * @request GET:/api/Admin/move-player
   */
  adminMovePlayerList = (
    query?: {
      playerToken?: string;
      /** @format int32 */
      x?: number;
      /** @format int32 */
      y?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<void, any>({
      path: `/api/Admin/move-player`,
      method: "GET",
      query: query,
      ...params,
    });
  /**
   * No description
   *
   * @tags Admin
   * @name AdminListBattlesList
   * @request GET:/api/Admin/list-battles
   */
  adminListBattlesList = (params: RequestParams = {}) =>
    this.request<Battle[], any>({
      path: `/api/Admin/list-battles`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleJoinCreate
   * @request POST:/api/Battle/join
   */
  battleJoinCreate = (data: JoinRequest, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Battle/join`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      ...params,
    });
  /**
   * @description The token to use is the player token. The Battle Id is representing the entire battle.
   *
   * @tags Battle
   * @name BattleJoinSandboxCreate
   * @summary Joins a new simulated battle for you to try practice your bot on.
   * @request POST:/api/Battle/join-sandbox
   */
  battleJoinSandboxCreate = (
    data: JoinSandboxRequest,
    params: RequestParams = {},
  ) =>
    this.request<JoinResponse, any>({
      path: `/api/Battle/join-sandbox`,
      method: "POST",
      body: data,
      type: ContentType.Json,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleViewBattleRawList
   * @request GET:/api/Battle/view-battle-raw
   */
  battleViewBattleRawList = (
    query?: {
      battleId?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<string, any>({
      path: `/api/Battle/view-battle-raw`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleViewBattleList
   * @request GET:/api/Battle/view-battle
   */
  battleViewBattleList = (
    query?: {
      battleId?: string;
    },
    params: RequestParams = {},
  ) =>
    this.request<BattleView, any>({
      path: `/api/Battle/view-battle`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleGetVisualList
   * @summary Gets the playing fields visual representation seen from your tank.
   * @request GET:/api/Battle/get-visual
   */
  battleGetVisualList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Battle/get-visual`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleMoveDetail
   * @request GET:/api/Battle/move/{direction}
   */
  battleMoveDetail = (direction: MoveDirection, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Battle/move/${direction}`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleAimDetail
   * @request GET:/api/Battle/aim/{deltaangle}
   */
  battleAimDetail = (deltaangle: number, params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Battle/aim/${deltaangle}`,
      method: "GET",
      ...params,
    });
  /**
   * No description
   *
   * @tags Battle
   * @name BattleGetReadingsList
   * @summary Gets the readings from the instruments in your tank.
   * @request GET:/api/Battle/get-readings
   */
  battleGetReadingsList = (params: RequestParams = {}) =>
    this.request<void, any>({
      path: `/api/Battle/get-readings`,
      method: "GET",
      ...params,
    });
}
