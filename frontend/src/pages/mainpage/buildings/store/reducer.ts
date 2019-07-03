import { buildingInitialState, BuildingState } from "./store";
import { GetBuildingActions, IGetActions } from "./actions/BuildingAction.get";
import {
  IAddBuildingActions,
  AddBuildingActions
} from "./actions/BuildingAction.post";
import {
  RefreshActions,
  IRefreshActions
} from "../../store/actions/RefreshActions.update";

export const BuildingReducer = (
  state = buildingInitialState,
  action: IGetActions | IAddBuildingActions | IRefreshActions
): BuildingState => {
  switch (action.type) {
    case GetBuildingActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case GetBuildingActions.SUCCES:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        buildings: action.data
      };
    case GetBuildingActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.error ? action.error : "Beállítási hiba"
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
        isPostRequesting: false,
        error: action.error ? action.error : "Ismeretlen hiba beállításnál"
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
