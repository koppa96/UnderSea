import * as React from "react";
import { Route, Switch } from "react-router-dom";
import { Register } from "./pages/account/register/index";

import "./app.scss";
import { NotFound } from "./pages/notFound/index";
import { LoginConnected } from "./pages/account/login/connect";
import { MainPageConnected } from "./pages/mainpage/connect";
import { LoginProps } from "./Interface";
import { LoginCheckConnected } from "./components/LoginCheck/connect";

export class App extends React.Component<LoginProps> {
  //TODO: Router kiszervezés
  componentDidMount() {
    this.props.getTokenCheck();
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
    return (
      <div className="App">
        <div
          className={
            this.state.mountedMainpage ? "bg-image-loggedin" : "bg-image"
          }
        >
          {loading ? (
            <div className="loading-circle main-loading" />
          ) : (
            <Switch>
              <Route exact path="/">
                <LoginCheckConnected
                  login={true}
                  serverResponseLogin={serverResponseLogin}
                  loading={loading}
                >
                  {loading ? (
                    <div className="loading-circle loading-button" />
                  ) : (
                    <MainPageConnected mounted={this.mounted} />
                  )}
                </LoginCheckConnected>
              </Route>
              <Route path="/account">
                <LoginCheckConnected
                  login={true}
                  serverResponseLogin={serverResponseLogin}
                  loading={loading}
                >
                  <MainPageConnected mounted={this.mounted} />)
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
          )}
        </div>
      </div>
    );
  }
}
