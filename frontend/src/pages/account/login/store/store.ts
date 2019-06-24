export interface LoginResponseState {
  model: {
    refresh_token: string;
    access_token: string;
    token_type: string;
    expires_in: string;
  };
  error: string | null;
  loading: boolean;
}

export const initialLoginResponseState: LoginResponseState = {
  error: null,
  loading: false,
  model: {
    access_token: "",
    expires_in: "",
    refresh_token: "",
    token_type: ""
  }
};
