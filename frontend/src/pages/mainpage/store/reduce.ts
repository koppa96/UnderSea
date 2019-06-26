import { IActions, MainpageActions } from "./actions";
import { MainpageResponseState, initialMainpageResponseState } from "./store";

//Rename to reducer
export const MainpageReducer = (
  state = initialMainpageResponseState,
  action: IActions
): MainpageResponseState => {
  switch (action.type) {
    case MainpageActions.REQUEST:
      return {
        ...state,
        loading: true
      };

    case MainpageActions.SUCCES:
      return {
        ...state,
        loading: false,
        model: action.params.country,
        error: ""
      };
    case MainpageActions.ERROR:
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
