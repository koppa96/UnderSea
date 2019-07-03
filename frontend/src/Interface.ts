import { ResetAction } from "./store";

export interface DispachedProps {
  getUserInfo: () => void;
}
export interface MappedProps {
  serverResponseLogin: boolean;
  loading: boolean;
}
interface NativeProps {}

export type LoginProps = NativeProps & MappedProps & DispachedProps;
