import {
  createStore,
  applyMiddleware,
  combineReducers,
  Reducer,
  AnyAction
} from "redux";
import saga from "redux-saga";
import { composeWithDevTools } from "redux-devtools-extension";
import { RouterState } from "connected-react-router";
import { PagesState, PagesReducer } from "./pages/store";

export interface AppState {
  pages: PagesState;
}

export interface IApllicationState {
  router: RouterState;
  app: AppState;
}

export interface ResetAction {
  type: "RESET_EVERYTHING";
}

export const resetEverything = (): ResetAction => ({
  type: "RESET_EVERYTHING"
});

export const appReducer = combineReducers<AppState>({
  pages: PagesReducer
});

export const appRootReducer: Reducer<AppState> = (
  state: AppState | undefined,
  action: AnyAction
): AppState => {
  if (action.type === "RESET_EVERYTHING") {
    state = undefined;
  }
  return appReducer(state, action);
};
