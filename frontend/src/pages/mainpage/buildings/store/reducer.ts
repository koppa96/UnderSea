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
      console.log("added");
      const tempState = state.buildings;
      tempState.forEach(item => {
        console.log("item: ", item);
        action.params.buildingIDs.forEach(element => {
          console.log("elementer: ", element);
          console.log("item.id: ", item.id);
          if (item.id == element) {
            item.amount = item.amount + 1;
          }
        });
      });
      console.log(tempState);
      return {
        ...state,
        buildings: tempState
      };

    default:
      //Mé nem teccik nekije
      const check: never = action.type;
      return state;
  }
};
