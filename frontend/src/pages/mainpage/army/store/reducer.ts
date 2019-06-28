import { ArmyState, ArmyInitialState } from "./store";
import { IArmyActions, ArmyActions } from "./actions/ArmyActions.post";
import {
  IGetActions,
  ArmyActions as getArmyActions
} from "./actions/ArmyActions.get";

export const ArmyReducer = (
  state = ArmyInitialState,
  action: IArmyActions | IGetActions
): ArmyState => {
  switch (action.type) {
    case ArmyActions.REQUEST:
      return {
        ...state,
        isPostRequesting: true
      };
    case ArmyActions.SUCCESS:
      return {
        ...state,
        isPostRequesting: false,
        units: state.units
      };
    case ArmyActions.ERROR:
      return {
        ...state,
        isPostRequesting: false,
        error: action.params
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
    default:
      const check: never = action;
      return state;
  }
};
