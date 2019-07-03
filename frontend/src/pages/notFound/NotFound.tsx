import React from "react";
import Wave from "./../../assets/images/wave.svg";
import { goBack } from "connected-react-router";
import { Redirect } from "react-router";

export class NotFound extends React.Component {
  state = {
    redirect: false
  };
  goBack = () => {
    this.setState({ redirect: true });
  };

  render() {
    console.log("redir: " + this.state.redirect);
    return this.state.redirect ? (
      <Redirect to="/" />
    ) : (
      <div className="main-component notfound-width">
        <div className="img-container">
          <img className="wave-notfound" src={Wave} alt="wave" />
          <h3 className="undersea-font-mainpage">UNDERSEA</h3>
        </div>
        <span className="not-found-text undersea-font">
          Ajjaj, valami hiba történt!
        </span>
        <button onClick={this.goBack} className="form-button">
          Vissza a főoldalra
        </button>
      </div>
    );
  }
}
