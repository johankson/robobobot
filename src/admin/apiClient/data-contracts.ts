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

export enum PlayerType {
  ServerBot = "ServerBot",
  RemoteBot = "RemoteBot",
}

export enum MoveDirection {
  North = "North",
  NorthEast = "NorthEast",
  East = "East",
  SouthEast = "SouthEast",
  South = "South",
  SouthWest = "SouthWest",
  West = "West",
  NorthWest = "NorthWest",
}

export enum BattleType {
  Sandbox = "Sandbox",
  Regular = "Regular",
}

export interface Battle {
  renderer?: BattleRenderer;
  battleField?: BattleField;
  type?: BattleType;
  battleToken?: string | null;
  settings?: BattleSettings;
  /** @format date-time */
  startTime?: string;
  duration?: TimeSpan;
  players?: Player[] | null;
  isStale?: boolean;
}

export interface BattleField {
  /** @format int32 */
  width?: number;
  /** @format int32 */
  height?: number;
}

export interface BattleFieldOptions {
  seed?: string | null;
  /** @format int32 */
  width?: number;
  /** @format int32 */
  height?: number;
  predefined?: string | null;
}

export type BattleRenderer = object;

export interface BattleSettings {
  executionDurations?: ExecutionDurations;
  lockedTurret?: boolean;
  /** @format int32 */
  staleTimeoutInMinutes?: number;
  randomizeStartPositionAssignment?: boolean;
}

export interface BattleView {
  battleField?: string | null;
  /** @format int32 */
  width?: number;
  /** @format int32 */
  height?: number;
  players?: Player[] | null;
}

export interface ExecutionDurations {
  /** @format int32 */
  moveOverLandInMilliseconds?: number;
  /** @format int32 */
  moveThroughForrestInMilliseconds?: number;
  /** @format int32 */
  moveFailureInMilliseconds?: number;
  /** @format int32 */
  aimDurationPerDegree?: number;
  /** @format int32 */
  getReadingInMilliseconds?: number;
}

export interface JoinRequest {
  battleId?: string | null;
  playerToken?: string | null;
}

export interface JoinResponse {
  battleToken?: string | null;
  playerToken?: string | null;
  playerName?: string | null;
}

export interface JoinSandboxRequest {
  name?: string | null;
  battleFieldOptions?: BattleFieldOptions;
  sandboxOptions?: SandboxOptions;
}

export interface Location {
  /** @format int32 */
  x?: number;
  /** @format int32 */
  y?: number;
}

export interface Player {
  token?: string | null;
  name?: string | null;
  type?: PlayerType;
  location?: Location;
  /** @format int32 */
  turretDegree?: number;
  /** @format int32 */
  tankHeading?: number;
  shortToken?: string;
}

export interface SandboxOptions {
  /** @format float */
  speedModifier?: number;
  /** @format int32 */
  numberOfBots?: number;
  playerStartPosition?: Location;
}

export interface TimeSpan {
  /** @format int64 */
  ticks?: number;
  /** @format int32 */
  days?: number;
  /** @format int32 */
  hours?: number;
  /** @format int32 */
  milliseconds?: number;
  /** @format int32 */
  microseconds?: number;
  /** @format int32 */
  nanoseconds?: number;
  /** @format int32 */
  minutes?: number;
  /** @format int32 */
  seconds?: number;
  /** @format double */
  totalDays?: number;
  /** @format double */
  totalHours?: number;
  /** @format double */
  totalMilliseconds?: number;
  /** @format double */
  totalMicroseconds?: number;
  /** @format double */
  totalNanoseconds?: number;
  /** @format double */
  totalMinutes?: number;
  /** @format double */
  totalSeconds?: number;
}
