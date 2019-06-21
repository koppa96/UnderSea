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

export interface AppState {
  test: any;
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
  test: x => x || null
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
