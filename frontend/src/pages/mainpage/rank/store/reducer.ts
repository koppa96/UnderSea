import { IRankActions, GetRankActions } from "./actions/RankAction.get";
import { rankInitialState, RankState } from "./store";

export const RankReducer = (
  state = rankInitialState,
  action: IRankActions
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

    default:
      const check: never = action;
      return state;
  }
};
