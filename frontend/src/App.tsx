import * as React from "react";
import { Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";

import axios from "axios";
import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { LoginConnected } from "./pages/account/login/connect";
import { MainPageConnected } from "./pages/mainpage/connect";
import { BasePortUrl } from ".";
import { LoginProps } from "./Interface";
import { LoginCheck } from "./components/LoginCheck/LoginCheck";
import { LoginCheckConnected } from "./components/LoginCheck/connect";

export class App extends React.Component<LoginProps> {
  //TODO: Router kiszervezés
  componentDidMount() {
    this.props.getUserInfo();
    this.setState({ loadingPage: this.props.loading });
  }
  state = {
    mountedMainpage: false,
    loadingPage: null
  };
  mounted = (mount: boolean) => {
    this.setState({ mountedMainpage: mount });
  };

  render() {
    const { serverResponseLogin, loading } = this.props;
    console.log("loadingPage", this.state.loadingPage);
    console.log("loading", loading);
    return (
      <div className="App">
        <div
          className={
            this.state.mountedMainpage ? "bg-image-loggedin" : "bg-image"
          }
        >
          <Switch>
            <Route exact path="/">
              <LoginCheckConnected
                login={true}
                serverResponseLogin={serverResponseLogin}
              >
                <MainPageConnected mounted={this.mounted} />
              </LoginCheckConnected>
            </Route>
            <Route path="/account">
              <LoginCheckConnected
                login={true}
                serverResponseLogin={serverResponseLogin}
              >
                <MainPageConnected mounted={this.mounted} />
              </LoginCheckConnected>
            </Route>
            <Route path="/register" component={Register} />

            <Route path="/login">
              <LoginCheckConnected
                login={false}
                serverResponseLogin={serverResponseLogin}
              >
                <LoginConnected />
              </LoginCheckConnected>
            </Route>
            <Route component={NotFound} />
          </Switch>
        </div>
      </div>
    );
  }
}
