import { ManState } from "./gyakorlás/store/store";
import { combineReducers } from "redux";
import { ManReducer } from "./gyakorlás/store/reducers";
import { BuildingState } from "./mainpage/buildings/store/store";
import { BuildingReducer } from "./mainpage/buildings/store/reducer";
import { LoginReducer } from "./account/login/store/reducer";
import { LoginResponseState } from "./account/login/store/store";
import { ArmyState } from "./mainpage/army/store/store";
import { ArmyReducer } from "./mainpage/army/store/reducer";
import { MainpageResponseState } from "./mainpage/store/store";
import { MainpageReducer } from "./mainpage/store/reducer";
import { RankState } from "./mainpage/rank/store/store";
import { RankReducer } from "./mainpage/rank/store/reducer";
import { ProfileState } from "../components/profileContainer/store/store";
import { ProfileReducer } from "../components/profileContainer/store/reducer";
import { WarState } from "./mainpage/war/store/store";
import { WarReducer } from "./mainpage/war/store/reducer";

export interface PagesState {
  gyakorlas: ManState;
  loginDetails: LoginResponseState;
  Army: ArmyState;
  mainpage: MainpageResponseState;
  buildings: BuildingState;
  rank: RankState;
  profile: ProfileState;
  war: WarState;
}

export const PagesReducer = combineReducers<PagesState>({
  gyakorlas: ManReducer,
  loginDetails: LoginReducer,
  Army: ArmyReducer,
  mainpage: MainpageReducer,
  buildings: BuildingReducer,
  rank: RankReducer,
  profile: ProfileReducer,
  war: WarReducer
});
