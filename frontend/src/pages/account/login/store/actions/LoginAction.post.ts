export interface ILoginActionsTypes {
  REQUEST: "LOGIN_REQUEST_LOGIN_LOGIN";
  SUCCES: "LOGIN_SUCCES_LOGIN_LOGIN";
  ERROR: "LOGIN_ERROR_LOGIN_LOGIN";
}

export const LoginActions: ILoginActionsTypes = {
  REQUEST: "LOGIN_REQUEST_LOGIN_LOGIN",
  SUCCES: "LOGIN_SUCCES_LOGIN_LOGIN",
  ERROR: "LOGIN_ERROR_LOGIN_LOGIN"
};

export interface IRequestParamState {
  name: string;
  password: string;
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
  error: ISuccesParamState;
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
export const fetchSucces = (error: ISuccesParamState): IActionLoginSucces => ({
  type: LoginActions.SUCCES,
  error: error
});
