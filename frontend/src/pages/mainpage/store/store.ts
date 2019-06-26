import { CountryInfo, ICountryInfo } from "../../../api/Client";
import { combineReducers } from "redux";
import { BuildingState } from "../buildings/store/store";
import { BuildingReducer } from "../buildings/store/reducer";

//STATE
export interface MainpageResponseState {
  model?: ICountryInfo;
  error?: string;
  loading: boolean;
}

export const initialMainpageResponseState: MainpageResponseState = {
  error: undefined,
  loading: false
};

//REDUCER
export interface MainpageState {
  buildingIds: BuildingState;
}

export const MainpageReducer = combineReducers<MainpageState>({
  buildingIds: BuildingReducer
});
