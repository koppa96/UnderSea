import * as React from "react";

import { Form } from "reactstrap";
import { RegisterProps, RegisterState } from "./Interface";
import { Link } from "react-router-dom";

export class Register extends React.Component<RegisterProps, RegisterState> {
  state: RegisterState = {
    name: "",
    password: "",
    repassword: "",
    countryname: ""
  };

  render() {
    return (
      <div className="form-bg">
        <h3 className="form-font">Regisztráció</h3>
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
          <input
            className="form-input"
            required
            onChange={e =>
              this.setState({
                ...this.state,
                repassword: e.target.value
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
                ...this.state,
                countryname: e.target.value
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
