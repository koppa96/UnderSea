import { LoginResponseState, initialLoginResponseState } from "./store";
import { IActions, LoginActions } from "./actions/LoginAction.post";

//Rename to reducer
export const LoginReducer = (
  state = initialLoginResponseState,
  action: IActions
): LoginResponseState => {
  switch (action.type) {
    case LoginActions.REQUEST:
      return {
        ...state,
        loading: true
      };

    case LoginActions.SUCCES:
      return {
        ...state,
        loading: false,
        model: {
          access_token: action.error.data.access_token,
          expires_in: action.error.data.expires_in,
          refresh_token: action.error.data.refresh_token,
          token_type: action.error.data.token_type
        },
        error: ""
      };
    case LoginActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
      };

    default:
      const check: never = action;
      return state;
  }
};
