import { CountryInfo, ICountryInfo } from "../../../../../api/Client";

export interface IMainpageActionsTypes {
  REQUEST: "MAINPAGE_REQUEST_SUPER_DATA";
  SUCCES: "MAINPAGE_SUCCES_SUPER_DATA";
  ERROR: "MAINPAGE_ERROR_SUPER_DATA";
}

export const MainpageActions: IMainpageActionsTypes = {
  REQUEST: "MAINPAGE_REQUEST_SUPER_DATA",
  SUCCES: "MAINPAGE_SUCCES_SUPER_DATA",
  ERROR: "MAINPAGE_ERROR_SUPER_DATA"
};

export interface ISuccesParamState {
  country: ICountryInfo;
}
//ACtionshoz
export interface IActionMainpageRequest {
  type: IMainpageActionsTypes["REQUEST"];
}
export interface IActionMainpageSucces {
  type: IMainpageActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IActionMainpageError {
  type: IMainpageActionsTypes["ERROR"];
  params?: string;
}

//Reducerhez
export type IActions =
  | IActionMainpageRequest
  | IActionMainpageSucces
  | IActionMainpageError;

//ActionCreators
export const MainpageRequestActionCreator = (): IActionMainpageRequest => ({
  type: MainpageActions.REQUEST
});

export const fetchError = (params?: string): IActionMainpageError => ({
  type: MainpageActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): IActionMainpageSucces => ({
  type: MainpageActions.SUCCES,
  params
});
