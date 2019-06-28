import { ICommandInfo } from "../../../../../api/Client";

//ACTIONTYPES
export interface IPostTargetActionsTypes {
  REQUEST: "ATTACK_REQUEST_POST_ATTACK";
  SUCCES: "ATTACK_SUCCES_POST_ATTACK";
  ERROR: "ATTACK_ERROR_POST_ATTACK";
}

export const PostAttackActions: IPostTargetActionsTypes = {
  REQUEST: "ATTACK_REQUEST_POST_ATTACK",
  SUCCES: "ATTACK_SUCCES_POST_ATTACK",
  ERROR: "ATTACK_ERROR_POST_ATTACK"
};

//ACTIONHOZ
export interface IRequestActionPostTarget {
  type: IPostTargetActionsTypes["REQUEST"];
  params: ICommandInfo;
}
export interface ISuccesActionPostTarget {
  type: IPostTargetActionsTypes["SUCCES"];
}
export interface IErrorActionPostTarget {
  type: IPostTargetActionsTypes["ERROR"];
  params?: string;
}

//REDUCERHEZ
export type IPostTargetActions =
  | IRequestActionPostTarget
  | ISuccesActionPostTarget
  | IErrorActionPostTarget;

//ACTIONCREATORHOZ
export const PostTargetActionCreator = (
  params: ICommandInfo
): IRequestActionPostTarget => ({
  type: PostAttackActions.REQUEST,
  params
});

export const fetchError = (params?: string): IErrorActionPostTarget => ({
  type: PostAttackActions.ERROR,
  params
});
export const fetchSucces = (): ISuccesActionPostTarget => ({
  type: PostAttackActions.SUCCES
});
