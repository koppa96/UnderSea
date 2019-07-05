interface NativeProps {}

interface MappedProps {}

interface dispatchedProps {}

export type RegisterProps = NativeProps & MappedProps & dispatchedProps;

export interface RegisterState {
  model: {
    name: string;
    password: string;
    repassword: string;
    countryName: string;
    email: string;
  };
  redirect: boolean;
  error: string | null;
}
