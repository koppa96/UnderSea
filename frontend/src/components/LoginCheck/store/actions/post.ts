export interface ILoginActionsTypes {
  REQUEST: "LOGIN_REQUEST_CHECK_LOGGEDIN";
  SUCCES: "LOGIN_SUCCES_CHECK_LOGGEDIN";
  ERROR: "LOGIN_ERROR_CHECK_LOGGEDIN";
}

export const LoginActions: ILoginActionsTypes = {
  REQUEST: "LOGIN_REQUEST_CHECK_LOGGEDIN",
  SUCCES: "LOGIN_SUCCES_CHECK_LOGGEDIN",
  ERROR: "LOGIN_ERROR_CHECK_LOGGEDIN"
};

export interface IRequestParamState {
  access_token: string;
  refresh_token: string;
}

export interface ISuccesParamState {
  data: {
    access_token: string;
    refresh_token: string;
    token_type: string;
    expires_in: string;
  };
}

//ACtionshoz
export interface IActionLoginRequest {
  type: ILoginActionsTypes["REQUEST"];
  params: IRequestParamState;
}
export interface IActionLoginSucces {
  type: ILoginActionsTypes["SUCCES"];
  params: ISuccesParamState;
}

export interface IActionLoginError {
  type: ILoginActionsTypes["ERROR"];
  params?: string;
}
//Reducerhez
export type IActions =
  | IActionLoginRequest
  | IActionLoginSucces
  | IActionLoginError;

//ActionCreators

export const BeginLoginActionCreator = (
  params: IRequestParamState
): IActionLoginRequest => ({
  type: LoginActions.REQUEST,
  params
});

export const fetchError = (params?: string): IActionLoginError => ({
  type: LoginActions.ERROR,
  params
});
export const fetchSucces = (params: ISuccesParamState): IActionLoginSucces => ({
  type: LoginActions.SUCCES,
  params
});
