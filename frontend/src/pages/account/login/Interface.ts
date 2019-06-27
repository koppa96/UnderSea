import { IRequestParamState } from "./store/actions/LoginAction.post";

export interface NativeProps {}

export interface MappedProps {
  error?: string;
  loading: boolean;
}

export interface DispatchedProps {
  beginlogin: (params: IRequestParamState) => void;
}

export type LoginProps = NativeProps & MappedProps & DispatchedProps;

export interface LoginState {
  model: { name: string; password: string };
  error: string | null;
}

export interface LoginResponse {
  access_token: string;
  refresh_token: string;
  token_type: string;
}
