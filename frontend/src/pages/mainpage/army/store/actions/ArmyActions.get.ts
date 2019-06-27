import { ArmyItemResponse } from "../../ArmyItem/Interface";

export type IGetArmyRespone = ArmyItemResponse[];

export interface IArmyActionsTypes {
  REQUEST: "ARMY_REQUEST_GET_UNITS";
  SUCCESS: "ARMY_SUCCESS_GET_UNITS";
  ERROR: "ARMY_ERROR_GET_UNITS";
}

export const ArmyActions: IArmyActionsTypes = {
  REQUEST: "ARMY_REQUEST_GET_UNITS",
  SUCCESS: "ARMY_SUCCESS_GET_UNITS",
  ERROR: "ARMY_ERROR_GET_UNITS"
};

export interface IActionGetArmyUnitRequest {
  type: IArmyActionsTypes["REQUEST"];
  // param
}
export interface IActionGetArmyUnitSucces {
  type: IArmyActionsTypes["SUCCESS"];
  data: IGetArmyRespone;
}

export interface IActionGetArmyUnitError {
  type: IArmyActionsTypes["ERROR"];
  reason: string | null;
}

export type IGetActions =
  | IActionGetArmyUnitRequest
  | IActionGetArmyUnitSucces
  | IActionGetArmyUnitError;

export const ArmyUnitGetRequestActionCreator = (): IActionGetArmyUnitRequest => ({
  type: ArmyActions.REQUEST
});
export const getArmy = ArmyUnitGetRequestActionCreator;

export const ArmyUnitGetErrortActionCreator = (
  reason: string | null
): IActionGetArmyUnitError => ({
  type: ArmyActions.ERROR,
  reason
});
export const ArmyUnitGetSuccessActionCreator = (
  data: IGetArmyRespone
): IActionGetArmyUnitSucces => ({
  type: ArmyActions.SUCCESS,
  data
});
