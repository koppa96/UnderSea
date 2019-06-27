import { IWarActions, GetWarActions } from "./actions/WarAction.get";
import { WarInitialState, WarState } from "./store";

export const WarReducer = (
  state = WarInitialState,
  action: IWarActions
): WarState => {
  switch (action.type) {
    case GetWarActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case GetWarActions.SUCCES:
      console.log("action.param.war", action.params.wars);
      return {
        ...state,
        loading: false,
        war: action.params.wars
      };
    case GetWarActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params ? action.params : "Ismeretlen hiba"
      };

    default:
      const check: never = action;
      return state;
  }
};
