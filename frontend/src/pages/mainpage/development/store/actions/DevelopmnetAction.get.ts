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
  data: ISuccesParamState;
}
export interface IErrorActionGetDevelopment {
  type: IGetDevelopmentActionsTypes["ERROR"];
  reason?: string;
}

//REDUCERHEZ
export type IGetDevelopmentActions =
  | IRequestActionGetDevelopment
  | ISuccesActionGetDevelopment
  | IErrorActionGetDevelopment;

//ACTIONCREATORHOZ
export const GetDevelopmentRequestActionCreator = (): IRequestActionGetDevelopment => ({
  type: GetDevelopmentActions.REQUEST
});

export const GetDevelopmentErrorActionCreator = (
  params?: string
): IErrorActionGetDevelopment => ({
  type: GetDevelopmentActions.ERROR,
  reason: params
});
export const GetDevelopmentSuccessActionCreator = (
  params: ISuccesParamState
): ISuccesActionGetDevelopment => ({
  type: GetDevelopmentActions.SUCCES,
  data: params
});
