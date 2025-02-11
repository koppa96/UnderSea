import { buildingInitialState, BuildingState } from "./store";
import { GetBuildingActions, IActions } from "./actions/BuildingAction.get";

export const BuildingReducer = (
  state = buildingInitialState,
  action: IActions
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

    default:
      const check: never = action;
      return state;
  }
};
