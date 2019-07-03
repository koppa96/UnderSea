import { IRankActions, GetRankActions } from "./actions/RankAction.get";
import { rankInitialState, RankState } from "./store";
import {
  RefreshActions,
  IRefreshActions
} from "../../store/actions/RefreshActions.update";

export const RankReducer = (
  state = rankInitialState,
  action: IRankActions | IRefreshActions
): RankState => {
  switch (action.type) {
    case GetRankActions.REQUEST:
      return {
        ...state,
        isRequesting: true
      };
    case GetRankActions.SUCCES:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        rank: action.params.ranks
      };
    case GetRankActions.ERROR:
      return {
        ...state,
        isRequesting: false,
        isLoaded: true,
        error: action.params ? action.params : "Ismeretlen hiba"
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
