import { IWarActions, GetWarActions } from "./actions/WarAction.get";
import { WarInitialState, WarState } from "./store";
import {
  IPostTargetActions,
  PostAttackActions
} from "../../attack/store/actions/AddAttackAction.post";
import {
  IDeleteWarActions,
  DeleteWarActions
} from "./actions/WarAction.delete";
import {
  RefreshActions,
  IRefreshActions
} from "../../store/actions/RefreshActions.update";

export const WarReducer = (
  state = WarInitialState,
  action: IWarActions | IPostTargetActions | IDeleteWarActions | IRefreshActions
): WarState => {
  switch (action.type) {
    case GetWarActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case GetWarActions.SUCCES:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        war: action.data.wars
      };
    case GetWarActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.error ? action.error : "Beállítási hiba"
      };
    case PostAttackActions.REQUEST:
      return {
        ...state
      };
    case PostAttackActions.ERROR:
      return {
        ...state
      };
    case PostAttackActions.SUCCES:
      return {
        ...state,
        war: [...state.war, action.data]
      };
    case DeleteWarActions.REQUEST:
      return {
        ...state,

        isPostRequesting: true
      };
    case DeleteWarActions.ERROR:
      return {
        ...state,
        isPostRequesting: false,
        error: action.error ? action.error : "Ismeretlen hiba beállításnál"
      };
    case DeleteWarActions.SUCCES:
      return {
        ...state,
        isPostRequesting: false,
        war: state.war.filter(item => item.id !== action.data.id)
      };
    case RefreshActions.REQUEST:
      return {
        ...state,
        isLoaded: false
      };
    case RefreshActions.SUCCES:
      return {
        ...state
      };
    case RefreshActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Frissítési hiba"
      };
    default:
      const _check: never = action;
      return state;
  }
};
