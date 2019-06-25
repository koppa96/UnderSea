import { ManState } from "./gyakorlás/store/store";
import { combineReducers } from "redux";
import { ManReducer } from "./gyakorlás/store/reducers";
import { BuildingState } from "./mainpage/buildings/store/store";
import { BuildingReducer } from "./mainpage/buildings/store/reducer";
import { NavbarState } from "../components/navBar/store";
import { LoginReducer } from "./account/login/store/reducer";
import { LoginResponseState } from "./account/login/store/store";
import { ArmyState } from "./mainpage/army/store/store";
import { ArmyReducer } from "./mainpage/army/store/reducer";

export interface PagesState {
  gyakorlas: ManState;
  buildingIds: BuildingState;
  loginDetails: LoginResponseState;
  Army: ArmyState
}

export const PagesReducer = combineReducers<PagesState>({
  gyakorlas: ManReducer,
  buildingIds: BuildingReducer,
  loginDetails: LoginReducer,
  Army: ArmyReducer
});
