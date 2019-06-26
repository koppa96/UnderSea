import { CountryInfo, ICountryInfo } from "../../../api/Client";

export interface MainpageResponseState {
  model: ICountryInfo | null;
  error?: string;
  loading: boolean;
}

export const initialMainpageResponseState: MainpageResponseState = {
  error: undefined,
  loading: false,
  model: null
};
