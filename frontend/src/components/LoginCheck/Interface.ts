export interface NativeProps {
  login: boolean;
}
export interface MappedProps {
  serverLogin: boolean;
}

export type LogCheckProps = NativeProps & MappedProps;
