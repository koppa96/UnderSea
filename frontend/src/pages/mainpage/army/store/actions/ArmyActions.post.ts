import { ArmyUnit } from "../store";

export interface IRequestParamState {
  unitsToAdd: ArmyUnit[];
}

export interface IArmyActionsTypes {
  REQUEST: "ARMY_REQUEST_ADD_UNITS";
  SUCCESS: "ARMY_SUCCESS_ADD_UNITS";
  ERROR: "ARMY_ERROR_ADD_UNITS";
}

export const ArmyActions: IArmyActionsTypes = {
  REQUEST: "ARMY_REQUEST_ADD_UNITS",
  SUCCESS: "ARMY_SUCCESS_ADD_UNITS",
  ERROR: "ARMY_ERROR_ADD_UNITS"
};

export interface IActionAddArmyUnitRequest {
  type: IArmyActionsTypes["REQUEST"];
  params: IRequestParamState;
}
export interface IActionAddArmyUnitSucces {
  type: IArmyActionsTypes["SUCCESS"];
  data: IRequestParamState;
}

export interface IActionAddArmyUnitError {
  type: IArmyActionsTypes["ERROR"];
  params: string | null;
}

export type IActions =
  | IActionAddArmyUnitRequest
  | IActionAddArmyUnitSucces
  | IActionAddArmyUnitError;

export const ArmyUnitAddActionCreator = (
  params: IRequestParamState
): IActionAddArmyUnitRequest => ({
  type: ArmyActions.REQUEST,
  params
});

export const fetchError = (params: string): IActionAddArmyUnitError => ({
  type: ArmyActions.ERROR,
  params
});
export const fetchSuccess = (
  data: IRequestParamState
): IActionAddArmyUnitSucces => ({
  type: ArmyActions.SUCCESS,
  data
});
