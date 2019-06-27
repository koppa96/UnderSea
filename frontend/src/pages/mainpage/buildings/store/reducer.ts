import { buildingInitialState, BuildingState } from "./store";
import { GetBuildingActions, IActions } from "./actions/BuildingAction.get";
import {
  IAddBuildingActions,
  AddBuildingActions
} from "./actions/BuildingAction.post";

export const BuildingReducer = (
  state = buildingInitialState,
  action: IActions | IAddBuildingActions
): BuildingState => {
  switch (action.type) {
    case GetBuildingActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case GetBuildingActions.SUCCES:
      console.log("action.param.building", action.params.buildings);
      return {
        ...state,
        loading: false,
        buildings: action.params.buildings
      };
    case GetBuildingActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params ? action.params : "Ismeretlen hiba"
      };

    case AddBuildingActions.REQUEST:
      return {
        ...state,
        isPostRequesting: true
      };
    case AddBuildingActions.SUCCES:
      return {
        ...state,
        isPostRequesting: false
      };
    case AddBuildingActions.ERROR:
      return {
        ...state,
        isPostRequesting: false
      };
    default:
      const check: never = action;
      return state;
  }
};
