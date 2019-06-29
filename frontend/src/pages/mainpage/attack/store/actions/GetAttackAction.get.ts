export interface ITargetInfo {
  countryId: number;
  countryName?: string | undefined;
}

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

export type IGetTargetListResponse = ITargetInfo[];

//ACTIONHOZ
export interface IRequestActionGetTarget {
  type: IGetTargetActionsTypes["REQUEST"];
}
export interface ISuccesActionGetTarget {
  type: IGetTargetActionsTypes["SUCCES"];
  data: IGetTargetListResponse;
}
export interface IErrorActionGetTarget {
  type: IGetTargetActionsTypes["ERROR"];
  error?: string;
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

export const fetchError = (error?: string): IErrorActionGetTarget => ({
  type: GetTargetActions.ERROR,
  error
});
export const fetchSucces = (
  data: IGetTargetListResponse
): ISuccesActionGetTarget => ({
  type: GetTargetActions.SUCCES,
  data
});
