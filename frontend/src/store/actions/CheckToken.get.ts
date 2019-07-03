//ACTIONTYPES
export interface ICheckTokenActionsTypes {
  REQUEST: "TOKEN_REQUEST_CHECK_TOKEN";
  SUCCES: "TOKEN_SUCCES_CHECK_TOKEN";
  ERROR: "TOKEN_ERROR_CHECK_TOKEN";
}

export const CheckTokenActions: ICheckTokenActionsTypes = {
  REQUEST: "TOKEN_REQUEST_CHECK_TOKEN",
  SUCCES: "TOKEN_SUCCES_CHECK_TOKEN",
  ERROR: "TOKEN_ERROR_CHECK_TOKEN"
};

//ACTIONHOZ
export interface IRequestActionCheckToken {
  type: ICheckTokenActionsTypes["REQUEST"];
}
export interface ISuccesActionCheckToken {
  type: ICheckTokenActionsTypes["SUCCES"];
}
export interface IErrorActionCheckToken {
  type: ICheckTokenActionsTypes["ERROR"];
  error?: string;
}

//REDUCERHEZ
export type ITokenActions =
  | IRequestActionCheckToken
  | ISuccesActionCheckToken
  | IErrorActionCheckToken;

//ACTIONCREATORHOZ
export const CheckTokenActionCreator = (): IRequestActionCheckToken => ({
  type: CheckTokenActions.REQUEST
});

export const fetchError = (error?: string): IErrorActionCheckToken => ({
  type: CheckTokenActions.ERROR,
  error
});
export const fetchSucces = (): ISuccesActionCheckToken => ({
  type: CheckTokenActions.SUCCES
});
