import { ITokenActions, CheckTokenActions } from "./actions/CheckToken.get";
import { tokencheckInitialState, TokenState } from "./store";
import {
  IActions,
  LoginActions
} from "../pages/account/login/store/actions/LoginAction.post";

export const TokenReducer = (
  state = tokencheckInitialState,
  action: ITokenActions | IActions
): TokenState => {
  switch (action.type) {
    case CheckTokenActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case CheckTokenActions.SUCCES:
      return {
        ...state,
        tokenCheck: true,
        loading: false
      };
    case CheckTokenActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.error ? action.error : "Ismeretlen hiba"
      };
    case LoginActions.REQUEST:
      return {
        ...state
      };

    case LoginActions.SUCCES:
      return {
        ...state,
        tokenCheck: true
      };
    case LoginActions.ERROR:
      return {
        ...state
      };
    default:
      const check: never = action;
      return state;
  }
};
