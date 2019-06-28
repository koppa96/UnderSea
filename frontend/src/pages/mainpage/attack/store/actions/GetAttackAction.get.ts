import { ITargetInfo } from "../../../../../api/Client";

//ACTIONTYPES
export interface IGetTargetActionsTypes {
  REQUEST: "TARGET_REQUEST_GET_TARGET";
  SUCCES: "TARGET_SUCCES_GET_TARGET";
  ERROR: "TARGET_ERROR_GET_TARGET";
}

export const GetTargetActions: IGetTargetActionsTypes = {
  REQUEST: "TARGET_REQUEST_GET_TARGET",
  SUCCES: "TARGET_SUCCES_GET_TARGET",
  ERROR: "TARGET_ERROR_GET_TARGET"
};

export interface ISuccesParamState {
  targets: ITargetInfo[];
}

//ACTIONHOZ
export interface IRequestActionGetTarget {
  type: IGetTargetActionsTypes["REQUEST"];
}
export interface ISuccesActionGetTarget {
  type: IGetTargetActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IErrorActionGetTarget {
  type: IGetTargetActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type ITargetActions =
  | IRequestActionGetTarget
  | ISuccesActionGetTarget
  | IErrorActionGetTarget;

//ACTIONCREATORHOZ
export const GetTargetActionCreator = (): IRequestActionGetTarget => ({
  type: GetTargetActions.REQUEST
});

export const fetchError = (params?: string): IErrorActionGetTarget => ({
  type: GetTargetActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetTarget => ({
  type: GetTargetActions.SUCCES,
  params
});
