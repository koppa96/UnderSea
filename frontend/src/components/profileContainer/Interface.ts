import { ProfileState } from "./store/store";
import { ResetAction } from "../../store";
import { PostProfileInput } from "./store/actions/uploadImage.post";

interface NativeProps {
  togglePopup: Function;
}

export interface MappedProps {
  profile: ProfileState;
  event?: boolean;
}
export interface DispachedProps {
  getUserInfo: () => void;
  logout: () => ResetAction;
  uploadImage: (img: PostProfileInput) => void;
}

export type ProfileProps = NativeProps & MappedProps & DispachedProps;

export interface IEventInfo {
  id: number;
  name?: string | undefined;
  description?: string | undefined;
  flavourtext?: string | undefined;
  imageUrl?: string | undefined;
}

export interface IUserInfo {
  username?: string | undefined;
  email?: string | undefined;
  profileImageUrl?: string | undefined;
}
