export interface ICommandInfo {
  id: number;
  targetCountryId: number;
  targetCountryName?: string | undefined;
  units?: IBriefUnitInfo[] | undefined;
}

export interface IBriefUnitInfo {
  id: number;
  name?: string | undefined;
  totalCount: number;
}

//ACTIONTYPES
export interface IGetWarActionsTypes {
  REQUEST: "WAR_REQUEST_GET_WAR";
  SUCCES: "WAR_SUCCES_GET_WAR";
  ERROR: "WAR_ERROR_GET_WAR";
}

export const GetWarActions: IGetWarActionsTypes = {
  REQUEST: "WAR_REQUEST_GET_WAR",
  SUCCES: "WAR_SUCCES_GET_WAR",
  ERROR: "WAR_ERROR_GET_WAR"
};

export interface ISuccesParamState {
  wars: ICommandInfo[];
}

//ACTIONHOZ
export interface IRequestActionGetWar {
  type: IGetWarActionsTypes["REQUEST"];
}
export interface ISuccesActionGetWar {
  type: IGetWarActionsTypes["SUCCES"];
  data: ISuccesParamState;
}
export interface IErrorActionGetWar {
  type: IGetWarActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IWarActions =
  | IRequestActionGetWar
  | ISuccesActionGetWar
  | IErrorActionGetWar;

//ACTIONCREATORHOZ
export const GetWarActionCreator = (): IRequestActionGetWar => ({
  type: GetWarActions.REQUEST
});

export const fetchError = (error?: string): IErrorActionGetWar => ({
  type: GetWarActions.ERROR,
  error
});
export const fetchSucces = (data: ISuccesParamState): ISuccesActionGetWar => ({
  type: GetWarActions.SUCCES,
  data
});
