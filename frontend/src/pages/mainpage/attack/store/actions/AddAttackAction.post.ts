import { IUnitDetails } from "../../interface";

export interface ICommandDetails {
  targetCountryId: number;
  units?: IUnitDetails[] | undefined;
}

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
  params: ICommandDetails;
}
export interface ISuccesActionPostTarget {
  type: IPostTargetActionsTypes["SUCCES"];
  data: ICommandDetails;
}
export interface IErrorActionPostTarget {
  type: IPostTargetActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type IPostTargetActions =
  | IRequestActionPostTarget
  | ISuccesActionPostTarget
  | IErrorActionPostTarget;

//ACTIONCREATORHOZ
export const PostTargetActionCreator = (
  params: ICommandDetails
): IRequestActionPostTarget => ({
  type: PostAttackActions.REQUEST,
  params
});

export const fetchError = (error?: string): IErrorActionPostTarget => ({
  type: PostAttackActions.ERROR,
  error
});
export const fetchSucces = (
  data: ICommandDetails
): ISuccesActionPostTarget => ({
  type: PostAttackActions.SUCCES,
  data
});
