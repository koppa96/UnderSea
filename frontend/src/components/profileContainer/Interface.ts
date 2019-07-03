import { IUserInfo } from "../../api/Client";
import { ProfileState } from "./store/store";
import { ResetAction } from "../../store";

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
}

export type ProfileProps = NativeProps & MappedProps & DispachedProps;

export interface IEventInfo {
  id: number;
  name?: string | undefined;
  description?: string | undefined;
  flavourtext?: string | undefined;
  imageUrl?: string | undefined;
}
