import * as React from "react";
import axios from "axios";

import "./../../../assets/scss/forms.scss";

import { Form } from "reactstrap";
import { LoginProps, LoginState, LoginResponse } from "./Interface";
import { Link } from "react-router-dom";

export class Login extends React.Component {
  state: LoginState = {
    model: {
      name: "",
      password: ""
    },
    error: null
  };

  handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    console.log(this.state);
    e.preventDefault();
    axios
      .post("/connect/token", {
        username: this.state.model.name,
        password: this.state.model.password
      })
      .then(response => {
        const resp: LoginResponse = response.data as LoginResponse;
        const connectToken = resp.token_type + " " + resp.access_token;
        const refreshToken = resp.refresh_token;

        localStorage.setItem("connection_header", connectToken);
        localStorage.setItem("refresh_token", refreshToken);
      })
      .catch(error => {
        if (error.response.status === "408") {
          this.setState({ error: "Connection timeout" });
        } else {
          this.setState({ error: "Helytelen jelszó, vagy felhasználó" });
        }
      });
  };

  render() {
    const { error } = this.state;
    return (
      <div className="mainpage-width">
        <h1 className="undersea-font-form">UNDERSEA</h1>

        <div className="form-wrapper">
          <div className="form-blur" />
          <div className="form-bg">
            <h2 className="form-font">Belépés</h2>
            <Form onSubmit={this.handleSubmit}>
              <input
                className="form-input"
                required
                onChange={e =>
                  this.setState({
                    ...this.state,
                    model: {
                      ...this.state.model,
                      name: e.target.value
                    }
                  })
                }
                placeholder="Felhasználó"
                name="name"
              />

              <input
                className="form-input"
                required
                onChange={e =>
                  this.setState({
                    ...this.state,
                    model: {
                      ...this.state.model,
                      password: e.target.value
                    }
                  })
                }
                placeholder="Jelszó"
                name="password"
              />
              <div className="button-container text-center">
                <button className="form-button" type="submit">
                  Belépés
                </button>
                {error && <p className="form-error">{error}</p>}
              </div>
            </Form>
            <div className="button-container text-center">
              <Link className="form-link" to="/register">
                Regisztráció
              </Link>
            </div>
          </div>
        </div>
      </div>
    );
  }

  //   handleChange = (e: React.FormEvent<HTMLInputElement>) => {
  //     e.preventDefault();
  //     this.setState({
  //       [e.currentTarget.name]: e.currentTarget.value
  //     });
  //   };
}
export default Login;
