//ACTIONTYPES
export interface IBuildingActionsTypes {
  REQUEST: "BUILDING_REQUEST_BUILDING_CREATE";
}

export const BuildingActions: IBuildingActionsTypes = {
  REQUEST: "BUILDING_REQUEST_BUILDING_CREATE"
};

export interface IRequestParamState {
  buildingIDs: number[];
}

//ACTIONHOZ
export interface IActionAddBuilding {
  type: IBuildingActionsTypes["REQUEST"];
  params: IRequestParamState;
}

//REDUCERHEZ
export type IActions = IActionAddBuilding;

//ACTIONCREATORHOZ
export const BuildingAddActionCreator = (
  params: IRequestParamState
): IActionAddBuilding => ({
  type: BuildingActions.REQUEST,
  params
});
