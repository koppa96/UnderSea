import { IUserInfo } from "../../api/Client";
import { ProfileState } from "./store/store";
import { ResetAction } from "../../store";

interface NativeProps {}

export interface MappedProps {
  profile: ProfileState;
}
export interface DispachedProps {
  getUserInfo: () => void;
  logout: () => ResetAction;
}

export type ProfileProps = NativeProps & MappedProps & DispachedProps;
