export interface NativeProps {
  serverResponseLogin: boolean;
  login: boolean;
}
export interface MappedProps {
  serverToken: boolean;
}

export type LogCheckProps = NativeProps & MappedProps;

export interface LoginCheckInterface {
  serverResponseLogin: boolean;
  login: boolean;
}
