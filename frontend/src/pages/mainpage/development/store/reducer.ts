import {
  GetDevelopmentActions,
  IGetDevelopmentActions
} from "./actions/DevelopmnetAction.get";
import { DevelopmentState, developmentInitialState } from "./store";
import {
  IAddDevelopmentActions,
  AddDevelopmentActions
} from "./actions/DevelopmentAction.post";

export const DevelopmentReducer = (
  state = developmentInitialState,
  action: IGetDevelopmentActions | IAddDevelopmentActions
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
    case AddDevelopmentActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case AddDevelopmentActions.SUCCES:
      return {
        ...state,
        loading: false
      };
    case AddDevelopmentActions.ERROR:
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
