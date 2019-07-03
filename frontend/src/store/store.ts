export interface TokenState {
  tokenCheck: boolean;
  loading: boolean;
  error?: string;
}

export const tokencheckInitialState: TokenState = {
  loading: false,
  tokenCheck: false
};
