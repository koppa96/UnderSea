import { ICountryInfo } from "../../Interface";

export interface IRefreshActionsTypes {
  REQUEST: "REFRESH_REQUEST_SUPER_DATA";
  SUCCES: "REFRESH_SUCCES_SUPER_DATA";
  ERROR: "REFRESH_ERROR_SUPER_DATA";
}

export const RefreshActions: IRefreshActionsTypes = {
  REQUEST: "REFRESH_REQUEST_SUPER_DATA",
  SUCCES: "REFRESH_SUCCES_SUPER_DATA",
  ERROR: "REFRESH_ERROR_SUPER_DATA"
};

//ACtionshoz
export interface IActionRefreshRequest {
  type: IRefreshActionsTypes["REQUEST"];
  params: ICountryInfo;
}
export interface IActionRefreshSucces {
  type: IRefreshActionsTypes["SUCCES"];
  data: ICountryInfo;
}
export interface IActionRefreshError {
  type: IRefreshActionsTypes["ERROR"];
  error?: string;
}

//Reducerhez
export type IRefreshActions =
  | IActionRefreshRequest
  | IActionRefreshSucces
  | IActionRefreshError;

//ActionCreators
export const RefreshRequestActionCreator = (
  params: ICountryInfo
): IActionRefreshRequest => ({
  type: RefreshActions.REQUEST,
  params
});

export const fetchError = (error?: string): IActionRefreshError => ({
  type: RefreshActions.ERROR,
  error
});
export const fetchSucces = (data: ICountryInfo): IActionRefreshSucces => ({
  type: RefreshActions.SUCCES,
  data
});
