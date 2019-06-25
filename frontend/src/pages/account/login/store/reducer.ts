import { LoginResponseState, initialLoginResponseState } from "./store";
import { IActions, LoginActions } from "./actions/post";

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
        loading: false
        /*  model: {
          access_token: action.params.access_token,
          expires_in: action.params.expires_in,
          refresh_token: action.params.refresh_token,
          token_type: action.params.token_type
        },
        error: ""
      */
      };
    case LoginActions.ERROR:
      return {
        ...state,
        loading: false,
        error: "Hiba történt"
      };

    default:
      const check: never = action.type;
      return state;
  }
};
