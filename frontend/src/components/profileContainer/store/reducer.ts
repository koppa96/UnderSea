import {
  GetProfileActions,
  IProfileActions
} from "./actions/profileContainer.get";
import { ProfileState, profileInitialState } from "./store";

export const ProfileReducer = (
  state = profileInitialState,
  action: IProfileActions
): ProfileState => {
  switch (action.type) {
    case GetProfileActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case GetProfileActions.SUCCES:
      return {
        ...state,
        loading: false,
        profile: action.params.profile
      };
    case GetProfileActions.ERROR:
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
