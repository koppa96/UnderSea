import * as React from "react";
import axios from "axios";

import { Form } from "reactstrap";
import { RegisterProps, RegisterState } from "./Interface";
import { Link } from "react-router-dom";

export class Register extends React.Component<RegisterProps, RegisterState> {
  state: RegisterState = {
    model: {
      name: "",
      password: "",
      repassword: "",
      countryName: "",
      email: ""
    },
    error: null
  };

  handleValidation(): boolean {
    const { name, password, countryName, repassword, email } = this.state.model;
    if (password !== repassword) {
      this.setState({ error: "A jelszavak nem egyeznek" });
      return false;
    }
    if (/[a-z]/.test(password) === false) {
      this.setState({ error: "A jelszónak kisbetűt kell tartalmazni" });
      return false;
    }

    if (/[A-Z]/.test(password) === false) {
      this.setState({ error: "A jelszónak nagybetűt kell tartalmazni" });
      return false;
    }
    if (/[0-9]/.test(password) === false) {
      this.setState({ error: "A jelszónak számot kell tartalmazni" });
      return false;
    }
    if (/[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(name) === true) {
      this.setState({ error: "A felhasználó nem tartalmazhat spec karaktert" });
      return false;
    }
    if (/[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/.test(countryName) === true) {
      this.setState({
        error: "Az ország nem tartalmazhat speciális karaktereket"
      });
      return false;
    }
    if (/[ @.]/.test(email) === false) {
      this.setState({ error: "Érvénytelen e-mail" });
      return false;
    }
    if (password.length < 6) {
      this.setState({ error: "A jelszónak minimuma 6 karakter" });
      return false;
    }
    this.setState({ error: "Sikeres regisztráció" });
    return true;
  }

  handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    console.log(this.state);
    var validation = false;

    validation = this.handleValidation();

    console.log(validation, "validation");
    if (validation) {
      const config = {
        headers: {
          "Content-Type": "application/json"
        }
      };

      const requestBody = {
        username: this.state.model.name,
        password: this.state.model.password,
        email: this.state.model.email,
        countryName: this.state.model.countryName
      };
      const url = "https://localhost:44355/api/accounts";
      console.log(requestBody, "requestBody");
      axios.post(url, requestBody, config).catch(error => {
        console.log(error, "error");
        if (error.response.status === "408") {
          this.setState({ error: "Connection timeout" });
        } else if (error.response.status === "405") {
          this.setState({ error: "Nem engedélyezett" });
        }
      });
    }
  };

  render() {
    const { error } = this.state;
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

            <input
              className="form-input"
              required
              onChange={e =>
                this.setState({
                  ...this.state,
                  model: {
                    ...this.state.model,
                    repassword: e.target.value
                  }
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
                  model: {
                    ...this.state.model,
                    email: e.target.value
                  }
                })
              }
              placeholder="E-mail"
              name="email"
            />
            <input
              className="form-input"
              required
              onChange={e =>
                this.setState({
                  ...this.state,
                  model: {
                    ...this.state.model,
                    countryName: e.target.value
                  }
                })
              }
              placeholder="A városod neve, amit építesz"
              name="countryname"
            />
            <div className="button-container text-center">
              <button className="form-button" type="submit">
                Regisztráció
              </button>
              {error && <p className="form-error">{error}</p>}
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

  /* handleChange = (e: React.FormEvent<HTMLInputElement>) => {
    this.setState({
      [e.currentTarget.name]: e.currentTarget.value
    });
  };
*/
}
export default Register;
