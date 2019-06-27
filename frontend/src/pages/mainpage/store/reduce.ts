import { IActions, MainpageActions } from "./actions/MainpageAction.get";
import { MainpageResponseState, initialMainpageResponseState } from "./store";
import {
  IAddBuildingActions,
  AddBuildingActions
} from "../buildings/store/actions/BuildingAction.post";

//Rename to reducer
export const MainpageReducer = (
  state = initialMainpageResponseState,
  action: IActions | IAddBuildingActions
): MainpageResponseState => {
  switch (action.type) {
    case MainpageActions.REQUEST:
      return {
        ...state,
        loading: true
      };

    case MainpageActions.SUCCES:
      return {
        ...state,
        loading: false,
        model: action.params.country,
        error: ""
      };
    case MainpageActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
      };
    case AddBuildingActions.REQUEST:
      return {
        ...state,
        loading: true,
        lastBuilding: action.params
      };
    case AddBuildingActions.SUCCES:
      const newBuilding = state.model
        ? state.model.buildings
          ? state.model.buildings
          : []
        : [];
      for (let index = 0; index < newBuilding.length; index++) {
        if (newBuilding[index].id === state.lastBuilding) {
          newBuilding[index].inProgressCount =
            newBuilding[index].inProgressCount + 1;
        }
      }

      return {
        ...state,
        loading: false,
        model: state.model
          ? {
              ...state.model,
              buildings: newBuilding
            }
          : undefined
      };
    case AddBuildingActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
          ? action.params
          : "Ismeretlen hiba" + state.lastBuilding + " hozzáadásnál"
      };

    default:
      const check: never = action;
      return state;
  }
};
