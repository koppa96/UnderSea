import { ArmyState, ArmyInitialState } from "./store";
import { IArmyActions, ArmyActions } from "./actions/ArmyActions.post";
import {
  IGetActions,
  ArmyActions as getArmyActions
} from "./actions/ArmyActions.get";
import {
  RefreshActions,
  IRefreshActions
} from "../../store/actions/RefreshActions.update";

export const ArmyReducer = (
  state = ArmyInitialState,
  action: IArmyActions | IGetActions | IRefreshActions
): ArmyState => {
  switch (action.type) {
    case ArmyActions.REQUEST:
      return {
        ...state,
        isPostRequesting: true,
        isPostSuccessFull: false
      };
    case ArmyActions.SUCCESS:
      return {
        ...state,
        isPostRequesting: false,
        units: state.units,
        error: null,
        isPostSuccessFull: true
      };
    case ArmyActions.ERROR:
      return {
        ...state,
        isPostRequesting: false,
        error: action.params
      };
    case ArmyActions.RESET:
      return {
        ...state,
        isPostSuccessFull: false
      };
    case getArmyActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case getArmyActions.SUCCESS:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        units: action.data
      };
    case getArmyActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.reason
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
      const check: never = action;
      return state;
  }
};
