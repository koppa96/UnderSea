export interface NativeProps {}

export interface MappedProps {}

export interface dispatchedProps {}

export type LoginProps = NativeProps & MappedProps & dispatchedProps;

export interface LoginState {
  model: { name: string; password: string };
  error: string | null;
}
