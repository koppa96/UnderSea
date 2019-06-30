import {
  ITargetActions,
  GetTargetActions
} from "./actions/GetAttackAction.get";
import { targetInitialState, TargetState } from "./store";
import {
  IPostTargetActions,
  PostAttackActions
} from "./actions/AddAttackAction.post";

export const TargetReducer = (
  state = targetInitialState,
  action: ITargetActions | IPostTargetActions
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
        isLoaded: false,
        error: action.error ? action.error : "Beállítási hiba"
      };
    case PostAttackActions.REQUEST:
      return {
        ...state,
        isPostRequesting: true
      };
    case PostAttackActions.SUCCES:
      return {
        ...state,
        isPostRequesting: false
      };
    case PostAttackActions.ERROR:
      return {
        ...state,
        isPostRequesting: false,
        error: action.error ? action.error : "Ismeretlen hiba beállításnál"
      };
    default:
      const check: never = action;
      return state;
  }
};
