import { ICommandInfo } from "../../../../../api/Client";

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
  params: ISuccesParamState;
}
export interface IErrorActionGetWar {
  type: IGetWarActionsTypes["ERROR"];
  params?: string;
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

export const fetchError = (params?: string): IErrorActionGetWar => ({
  type: GetWarActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetWar => ({
  type: GetWarActions.SUCCES,
  params
});
