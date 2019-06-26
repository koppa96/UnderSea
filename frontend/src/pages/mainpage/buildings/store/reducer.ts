import { buildingInitialState, BuildingState } from "./store";
import { BuildingProps } from "../Interface";
import { GetBuildingActions, IActions } from "./actions/get";

//Rename to reducer
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
