import {
  GetProfileActions,
  IProfileActions
} from "./actions/profileContainer.get";
import { ProfileState, profileInitialState } from "./store";
import {
  IPostProfileActions,
  PostProfileImgActions
} from "./actions/uploadImage.post";

export const ProfileReducer = (
  state = profileInitialState,
  action: IProfileActions | IPostProfileActions
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

    case PostProfileImgActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case PostProfileImgActions.SUCCES:
      return {
        ...state,
        loading: false,
        profile: {
          ...state.profile,
          profileImageUrl: action.data
        }
      };
    case PostProfileImgActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.error ? action.error : "Ismeretlen hiba"
      };

    default:
      const _check: never = action;
      return state;
  }
};
