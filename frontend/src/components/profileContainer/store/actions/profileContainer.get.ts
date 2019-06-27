import { IUserInfo } from "../../../../api/Client";

//ACTIONTYPES
export interface IGetProfileActionsTypes {
  REQUEST: "PROFILE_REQUEST_GET_PROFILE";
  SUCCES: "PROFILE_SUCCES_GET_PROFILE";
  ERROR: "PROFILE_ERROR_GET_PROFILE";
}

export const GetProfileActions: IGetProfileActionsTypes = {
  REQUEST: "PROFILE_REQUEST_GET_PROFILE",
  SUCCES: "PROFILE_SUCCES_GET_PROFILE",
  ERROR: "PROFILE_ERROR_GET_PROFILE"
};

export interface ISuccesParamState {
  profile: IUserInfo;
}

//ACTIONHOZ
export interface IRequestActionGetProfile {
  type: IGetProfileActionsTypes["REQUEST"];
}
export interface ISuccesActionGetProfile {
  type: IGetProfileActionsTypes["SUCCES"];
  params: ISuccesParamState;
}
export interface IErrorActionGetProfile {
  type: IGetProfileActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IProfileActions =
  | IRequestActionGetProfile
  | ISuccesActionGetProfile
  | IErrorActionGetProfile;

//ACTIONCREATORHOZ
export const GetProfileActionCreator = (): IRequestActionGetProfile => ({
  type: GetProfileActions.REQUEST
});

export const fetchError = (params?: string): IErrorActionGetProfile => ({
  type: GetProfileActions.ERROR,
  params
});
export const fetchSucces = (
  params: ISuccesParamState
): ISuccesActionGetProfile => ({
  type: GetProfileActions.SUCCES,
  params
});
