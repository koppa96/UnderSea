import { RequestBuildingParams } from "../store";

//ACTIONTYPES
export interface IBuildingActionsTypes {
  REQUEST: "BUILDING_REQUEST_BUILDING_CREATE";
  SUCCES: "BUILDING_SUCCES_BUILDING_CREATE";
  ERROR: "BUILDING_ERROR_BUILDING_CREATE";
}

export const AddBuildingActions: IBuildingActionsTypes = {
  REQUEST: "BUILDING_REQUEST_BUILDING_CREATE",
  SUCCES: "BUILDING_SUCCES_BUILDING_CREATE",
  ERROR: "BUILDING_ERROR_BUILDING_CREATE"
};

//ACTIONHOZ
export interface IActionRequestAddBuilding {
  type: IBuildingActionsTypes["REQUEST"];
  params: RequestBuildingParams;
}

export interface IActionSuccesAddBuilding {
  type: IBuildingActionsTypes["SUCCES"];
  data: RequestBuildingParams;
}

export interface IActionErrorAddBuilding {
  type: IBuildingActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IAddBuildingActions =
  | IActionRequestAddBuilding
  | IActionSuccesAddBuilding
  | IActionErrorAddBuilding;

//ACTIONCREATORHOZ
export const BuildingAddActionCreator = (
  params: RequestBuildingParams
): IActionRequestAddBuilding => ({
  type: AddBuildingActions.REQUEST,
  params
});
export const fetchError = (error?: string): IActionErrorAddBuilding => ({
  type: AddBuildingActions.ERROR,
  error
});
export const fetchSucces = (
  data: RequestBuildingParams
): IActionSuccesAddBuilding => ({
  type: AddBuildingActions.SUCCES,
  data
});
