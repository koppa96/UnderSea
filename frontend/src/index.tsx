import "bootstrap/dist/css/bootstrap.min.css";
import React from "react";
import ReactDOM from "react-dom";
import { App, registeraxios } from "./App";
import axios from "axios";
import * as serviceWorker from "./serviceWorker";
import { Provider } from "react-redux";
import { configureStore } from "./config/ConfigureStore";
import { createBrowserHistory } from "history";
import { ConnectedRouter } from "connected-react-router";
import qs from "qs";

export const BasePortUrl = "https://localhost:44355/";

const history = createBrowserHistory({ basename: "/" });
const { store } = configureStore(history);

// codegen({

//   methodNameMode: 'operationId',
//   source:require('./config/swagger.json'),
//   outputDir: '.',
//   useStaticMethod:true
// });
registeraxios();
ReactDOM.render(
  <Provider store={store}>
    <ConnectedRouter history={history}>
      <React.StrictMode>
        <App />
      </React.StrictMode>
    </ConnectedRouter>
  </Provider>,
  document.getElementById("root")
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
