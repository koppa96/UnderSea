export interface NativeProps {}

export interface MappedProps {}

export interface DispatchedProps {}

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
