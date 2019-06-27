import { ICountryInfo } from "../../../api/Client";
import { combineReducers } from "redux";
import { BuildingState } from "../buildings/store/store";
import { BuildingReducer } from "../buildings/store/reducer";

//STATE
export interface MainpageResponseState {
  model?: ICountryInfo;
  error?: string;
  loading: boolean;
  lastBuilding?: number;
}

export const initialMainpageResponseState: MainpageResponseState = {
  error: undefined,
  loading: false
};

//REDUCER
