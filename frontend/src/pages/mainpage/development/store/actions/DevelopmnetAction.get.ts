import { ICreationInfo } from "../../../../../api/Client";

//ACTIONTYPES
export interface IGetDevelopmentActionsTypes {
  REQUEST: "DEVELOPMENT_REQUEST_GET_DEVELOPMENT";
  SUCCES: "DEVELOPMENT_SUCCES_GET_DEVELOPMENT";
  ERROR: "DEVELOPMENT_ERROR_GET_DEVELOPMENT";
}

export const GetDevelopmentActions: IGetDevelopmentActionsTypes = {
  REQUEST: "DEVELOPMENT_REQUEST_GET_DEVELOPMENT",
  SUCCES: "DEVELOPMENT_SUCCES_GET_DEVELOPMENT",
  ERROR: "DEVELOPMENT_ERROR_GET_DEVELOPMENT"
};

export interface ISuccesParamState {
  description: ICreationInfo[];
}

//ACTIONHOZ
export interface IRequestActionGetDevelopment {
  type: IGetDevelopmentActionsTypes["REQUEST"];
}
export interface ISuccesActionGetDevelopment {
  type: IGetDevelopmentActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IErrorActionGetDevelopment {
  type: IGetDevelopmentActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IGetDevelopmentActions =
  | IRequestActionGetDevelopment
  | ISuccesActionGetDevelopment
  | IErrorActionGetDevelopment;

//ACTIONCREATORHOZ
export const GetDevelopmentActionCreator = (): IRequestActionGetDevelopment => ({
  type: GetDevelopmentActions.REQUEST
});

export const fetchError = (params?: string): IErrorActionGetDevelopment => ({
  type: GetDevelopmentActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetDevelopment => ({
  type: GetDevelopmentActions.SUCCES,
  params
});
