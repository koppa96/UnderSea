import { ManState } from "./gyakorlás/store/store";
import { combineReducers } from "redux";
import { ManReducer } from "./gyakorlás/store/reducers";
import { BuildingState } from "./mainpage/buildings/store/store";
import { BuildingReducer } from "./mainpage/buildings/store/reducer";

export interface PagesState {
  gyakorlas: ManState;
  buildingIds: BuildingState;
}

export const PagesReducer = combineReducers<PagesState>({
  gyakorlas: ManReducer,
  buildingIds: BuildingReducer
});
