import { combineReducers, Reducer, AnyAction } from "redux";
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
    localStorage.clear();
    state = undefined;
  }

  return appReducer(state, action);
};
