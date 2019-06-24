export interface NativeProps {}

export interface MappedProps {}

export interface dispatchedProps {}

export type RegisterProps = NativeProps & MappedProps & dispatchedProps;

export interface RegisterState {
  model: {
    name: string;
    password: string;
    repassword: string;
    countryname: string;
  };
  error: string | null;
}
