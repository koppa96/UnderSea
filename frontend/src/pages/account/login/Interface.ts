export interface NativeProps{}

export interface MappedProps{}

export interface dispatchedProps{}

export type LoginProps = NativeProps & MappedProps & dispatchedProps;

export interface LoginState{
    name: string;
    password: string;
}