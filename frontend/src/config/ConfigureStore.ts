import createSagaMiddleware from "redux-saga";
import { routerMiddleware, connectRouter } from "connected-react-router";
import { History } from "history";
import { compose, combineReducers, createStore, applyMiddleware } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";
import { appRootReducer } from "../store";
import { rootSaga } from "../saga";

export function configureStore(history: History) {
  const sagaMiddleWare = createSagaMiddleware();
  const middleware = [sagaMiddleWare, routerMiddleware(history)];

  let enhancers = compose;
  const windowIfDefined =
    typeof window === "undefined" ? null : (window as any);
  const isDevelopment = process.env.NODE_ENV === "development";
  const devToolsExtension =
    windowIfDefined &&
    isDevelopment &&
    windowIfDefined.__REDUX_DEVTOOLS_EXTENSION__;
  if (
    isDevelopment &&
    devToolsExtension &&
    typeof devToolsExtension === "function"
  ) {
    enhancers = composeWithDevTools({
      // actionsBlacklist: [''], maxAge: 500
    });
  }

  const DeepRootReducer = combineReducers({
    router: connectRouter(history),
    app: appRootReducer
  });

  const store = createStore(
    DeepRootReducer,
    enhancers(applyMiddleware(...middleware))
  );

  sagaMiddleWare.run(rootSaga);

  return { store };
}
