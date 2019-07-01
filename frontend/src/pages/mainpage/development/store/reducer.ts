import {
  GetDevelopmentActions,
  IGetDevelopmentActions
} from "./actions/DevelopmnetAction.get";
import { DevelopmentState, developmentInitialState } from "./store";

export const DevelopmentReducer = (
  state = developmentInitialState,
  action: IGetDevelopmentActions
): DevelopmentState => {
  switch (action.type) {
    case GetDevelopmentActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case GetDevelopmentActions.SUCCES:
      return {
        ...state,
        loading: false,
        development: action.data.description
      };
    case GetDevelopmentActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.reason ? action.reason : "Ismeretlen hiba"
      };

    default:
      const check: never = action;
      return state;
  }
};
