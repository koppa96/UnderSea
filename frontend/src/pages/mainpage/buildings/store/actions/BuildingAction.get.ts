import { ICreationInfo } from "../../../../../api/Client";

//ACTIONTYPES
export interface IGetBuildingActionsTypes {
  REQUEST: "BUILDING_REQUEST_GET_BUILDING";
  SUCCES: "BUILDING_SUCCES_GET_BUILDING";
  ERROR: "BUILDING_ERROR_GET_BUILDING";
}

export const GetBuildingActions: IGetBuildingActionsTypes = {
  REQUEST: "BUILDING_REQUEST_GET_BUILDING",
  SUCCES: "BUILDING_SUCCES_GET_BUILDING",
  ERROR: "BUILDING_ERROR_GET_BUILDING"
};

export interface ISuccesParamState {
  buildings: ICreationInfo[];
}

//ACTIONHOZ
export interface IRequestActionGetBuilding {
  type: IGetBuildingActionsTypes["REQUEST"];
}
export interface ISuccesActionGetBuilding {
  type: IGetBuildingActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IErrorActionGetBuilding {
  type: IGetBuildingActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IActions =
  | IRequestActionGetBuilding
  | ISuccesActionGetBuilding
  | IErrorActionGetBuilding;

//ACTIONCREATORHOZ
export const GetBuildingActionCreator = (): IRequestActionGetBuilding => ({
  type: GetBuildingActions.REQUEST
});

export const fetchError = (params?: string): IErrorActionGetBuilding => ({
  type: GetBuildingActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetBuilding => ({
  type: GetBuildingActions.SUCCES,
  params
});
