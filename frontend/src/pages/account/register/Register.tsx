import * as React from "react";

import { Form } from "reactstrap";
import { RegisterProps, RegisterState } from "./Interface";
import { Link } from "react-router-dom";

export class Register extends React.Component<RegisterProps, RegisterState> {
  state: RegisterState = {
    model: {
      name: "",
      password: "",
      repassword: "",
      countryname: ""
    },
    error: null
  };

  /* handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const config = {
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
        "Access-Control-Allow-Origin": "*",
        "Access-Control-Allow-Headers": "Origin, Content-Type, X-Auth-Token",
      }
    };

    const requestBody = qs.stringify({
      username: this.state.model.name,
      password: this.state.model.password,
      email: "undersea_client",
      client_secret: "undersea_client_secret",
      scope: "offline_access undersea_api",
      grant_type: "password"
    });
    const url = "https://localhost:44355/api/accounts/me";

    axios
      .post(url, requestBody, config)
      .then(response => {
        console.log(response);
        const resp: LoginResponse = response.data as LoginResponse;
        const connectToken = resp.token_type + " " + resp.access_token;
        const refreshToken = resp.refresh_token;

        localStorage.setItem("connection_header", connectToken);
        localStorage.setItem("refresh_token", refreshToken);
        console.log(localStorage.getItem("refresh_token"));
      })
      .catch(error => {
        if (error.response.status === "408") {
          this.setState({ error: "Connection timeout" });
        } else {
          this.setState({ error: "Helytelen jelszó, vagy felhasználó" });
        }
      });
  };

  */

  render() {
    return (
      <div className="mainpage-width">
        <h1 className="undersea-font-form">UNDERSEA</h1>

        <div className="form-bg mainpage-width">
          <h3 className="form-font">Regisztráció</h3>
          <Form onSubmit={this.handleSubmit}>
            <input
              className="form-input"
              required
              onChange={e =>
                this.setState({
                  ...this.state
                  // name: e.target.value
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
                  ...this.state
                  // name: e.target.value
                })
              }
              placeholder="Jelszó"
              name="password"
            />
            <input
              className="form-input"
              required
              onChange={e =>
                this.setState({
                  ...this.state
                  //  repassword: e.target.value
                })
              }
              placeholder="Jelszó megerősítése"
              name="repassword"
            />
            <input
              className="form-input"
              required
              onChange={e =>
                this.setState({
                  ...this.state
                  //  countryname: e.target.value
                })
              }
              placeholder="A városod neve, amit építesz"
              name="countryname"
            />
            <div className="button-container text-center">
              <button className="form-button" type="submit">
                Regisztráció
              </button>
            </div>
          </Form>
          <div className="button-container text-center">
            <Link className="form-link" to="/login">
              Bejelentkezés
            </Link>
          </div>
        </div>
      </div>
    );
  }

  handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();

    console.log(this.state);
  };

  /* handleChange = (e: React.FormEvent<HTMLInputElement>) => {
    this.setState({
      [e.currentTarget.name]: e.currentTarget.value
    });
  };
*/
}
export default Register;
