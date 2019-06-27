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
  params: number;
}

export interface IActionSuccesAddBuilding {
  type: IBuildingActionsTypes["SUCCES"];
}

export interface IActionErrorAddBuilding {
  type: IBuildingActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IAddBuildingActions =
  | IActionRequestAddBuilding
  | IActionSuccesAddBuilding
  | IActionErrorAddBuilding;

//ACTIONCREATORHOZ
export const BuildingAddActionCreator = (
  params: number
): IActionRequestAddBuilding => ({
  type: AddBuildingActions.REQUEST,
  params
});
export const fetchError = (params?: string): IActionErrorAddBuilding => ({
  type: AddBuildingActions.ERROR,
  params
});
export const fetchSucces = (): IActionSuccesAddBuilding => ({
  type: AddBuildingActions.SUCCES
});
