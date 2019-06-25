import { ManState } from "./gyakorlás/store/store";
import { combineReducers } from "redux";
import { ManReducer } from "./gyakorlás/store/reducers";
import { BuildingState } from "./mainpage/buildings/store/store";
import { BuildingReducer } from "./mainpage/buildings/store/reducer";
import { NavbarState } from "../components/navBar/store";
import { LoginReducer } from "./account/login/store/reducer";
import { LoginResponseState } from "./account/login/store/store";

export interface PagesState {
  gyakorlas: ManState;
  buildingIds: BuildingState;
  loginDetails: LoginResponseState;
}

export const PagesReducer = combineReducers<PagesState>({
  gyakorlas: ManReducer,
  buildingIds: BuildingReducer,
  loginDetails: LoginReducer
});
