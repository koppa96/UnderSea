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
        loading: true
      };
    case GetTargetActions.SUCCES:
      console.log("action.param.targets", action.params.targets);
      return {
        ...state,
        loading: false,
        targets: action.params.targets
      };
    case GetTargetActions.ERROR:
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
