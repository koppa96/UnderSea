export interface ICreationInfo {
  id: number;
  name?: string | undefined;
  description?: string | undefined;
  imageUrl?: string | undefined;
  iconImageUrl?: string | undefined;
  cost: number;
}

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

export type IGetBuildingRespone = ICreationInfo[];

//ACTIONHOZ
export interface IRequestActionGetBuilding {
  type: IGetBuildingActionsTypes["REQUEST"];
}
export interface ISuccesActionGetBuilding {
  type: IGetBuildingActionsTypes["SUCCES"];
  data: IGetBuildingRespone;
}
export interface IErrorActionGetBuilding {
  type: IGetBuildingActionsTypes["ERROR"];
  error: string | null;
}

//REDUCERHEZ
export type IGetActions =
  | IRequestActionGetBuilding
  | ISuccesActionGetBuilding
  | IErrorActionGetBuilding;

//ACTIONCREATORHOZ
export const GetBuildingActionCreator = (): IRequestActionGetBuilding => ({
  type: GetBuildingActions.REQUEST
});

export const fetchError = (error: string | null): IErrorActionGetBuilding => ({
  type: GetBuildingActions.ERROR,
  error
});
export const fetchSucces = (
  data: IGetBuildingRespone
): ISuccesActionGetBuilding => ({
  type: GetBuildingActions.SUCCES,
  data
});
