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
        loading: true
      };
    case GetRankActions.SUCCES:
      console.log("action.param.building", action.params.ranks);
      return {
        ...state,
        loading: false,
        rank: action.params.ranks
      };
    case GetRankActions.ERROR:
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
