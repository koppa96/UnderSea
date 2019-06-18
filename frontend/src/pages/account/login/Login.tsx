import * as React from "react";

import "./../../../assets/scss/forms.scss";

import { Form } from "reactstrap";
import { LoginProps, LoginState } from "./Interface";
import { Link } from "react-router-dom";

export class Login extends React.Component<LoginProps, LoginState> {
  state: LoginState = {
    name: "",
    password: ""
  };

  render() {
    return (
      <div className="form-bg">
        <h2 className="form-font">Belépés</h2>
        <Form onSubmit={this.handleSubmit}>
          <input
            className="form-input"
            required
            onChange={e =>
              this.setState({
                ...this.state,
                name: e.target.value
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
                name: e.target.value
              })
            }
            placeholder="Jelszó"
            name="password"
          />
          <div className="button-container text-center">
            <button className="form-button" type="submit">
              Belépés
            </button>
          </div>
        </Form>
        <div className="button-container text-center">
          <Link className="form-link" to="/register">
            Regisztráció
          </Link>
        </div>
      </div>
    );
  }

  handleSubmit = async (e: React.FormEvent<HTMLFormElement>): Promise<void> => {
    e.preventDefault();
    console.log(this.state);
  };

  //   handleChange = (e: React.FormEvent<HTMLInputElement>) => {
  //     e.preventDefault();
  //     this.setState({
  //       [e.currentTarget.name]: e.currentTarget.value
  //     });
  //   };
}
export default Login;
