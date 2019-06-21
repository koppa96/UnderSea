import { buildingInitialState, BuildingState } from "./store";
import { IActions, BuildingActions } from "./actions/buildingActions";

//Rename to reducer
export const BuildingReducer = (
  state = buildingInitialState,
  action: IActions
): BuildingState => {
  switch (action.type) {
    case BuildingActions.REQUEST:
      return {
        ...state,
        buildingIds: action.params.buildingIDs
      };

    default:
      //Mé nem teccik nekije
      const check: never = action;
      return state;
  }
};
