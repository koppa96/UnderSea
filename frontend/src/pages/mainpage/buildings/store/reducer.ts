import { buildingInitialState, BuildingState } from "./store";
import { IActions, BuildingActions } from "./actions/buildingActions";
import { BuildingProps } from "../Interface";

//Rename to reducer
export const BuildingReducer = (
  state = buildingInitialState,
  action: IActions
): BuildingState => {
  switch (action.type) {
    case BuildingActions.REQUEST:
      const tempState = state.buildings;
      tempState.forEach(item => {
        action.params.buildingIDs.forEach(element => {
          console.log("elementer: ", element);
          console.log("item.id: ", item.id);
          if (item.id == element) {
            item.amount = item.amount + 1;
          }
        });
      });
      return {
        ...state,
        buildings: tempState
      };

    default:
      const check: never = action.type;
      return state;
  }
};
