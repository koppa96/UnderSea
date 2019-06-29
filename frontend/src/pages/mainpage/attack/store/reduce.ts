import {
  ITargetActions,
  GetTargetActions
} from "./actions/GetAttackAction.get";
import { targetInitialState, TargetState } from "./store";

export const TargetReducer = (
  state = targetInitialState,
  action: ITargetActions
): TargetState => {
  switch (action.type) {
    case GetTargetActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case GetTargetActions.SUCCES:
      console.log("action.data", action.data);
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        targets: action.data
      };
    case GetTargetActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.error ? action.error : "Beállítási hiba"
      };

    default:
      const check: never = action;
      return state;
  }
};
