import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { Form, Label, FormGroup } from "reactstrap";
import axios from "axios";
import { registerAxiosConfig } from "../../../config/axiosConfig";
import { BasePortUrl } from "../../..";
export default class Profile extends React.Component {
  state = {
    password: "",
    repassword: "",
    oldpw: "",
    error: ""
  };

  validate(): boolean {
    const { password, repassword, oldpw } = this.state;
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

    if (password.length < 6) {
      this.setState({ error: "A jelszónak minimuma 6 karakter" });
      return false;
    }
    if (oldpw.length < 6) {
      this.setState({ error: "Régi jelszó túl rövid" });
      return false;
    }
    if (oldpw === password) {
      this.setState({
        error: "Az új jelszó nem egyezhet meg a régi jelszóval"
      });
      return false;
    }
    this.setState({ error: "" });
    return true;
  }

  submit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    if (this.validate()) {
      const instance = axios.create();
      const configured = registerAxiosConfig(instance);
      const data = {
        oldPassword: this.state.oldpw,
        newPassword: this.state.password
      };

      const response = await configured
        .post(BasePortUrl + "api/Accounts/me/password", data)
        .then(response => {
          this.setState({
            error: "Sikeres jelszóváltoztatás",
            password: "",
            repassword: "",
            oldpw: ""
          });
        })
        .catch(error => {
          this.setState({ error: "Hibás régi jelszó!" });
        });
    }
  };

  render() {
    return (
      <div className="main-component profile-width">
        <ComponentHeader title={title} />
        {
          //TODO: autocomplete off on chrome
        }
        <Form
          autoComplete="false"
          onSubmit={e => this.submit(e)}
          className="form-font-profile"
        >
          <FormGroup autoComplete={"false"}>
            <Label className="white" for="oldpw">
              Old password:
            </Label>
            <input
              type="password"
              autoComplete="false"
              className="form-input"
              id="oldpw"
              placeholder="**********"
              value={this.state.oldpw}
              onChange={e => {
                this.setState({ oldpw: e.target.value });
              }}
            />
          </FormGroup>

          <FormGroup autoComplete={"off"}>
            <Label className="white" for="newpw">
              New password:
            </Label>
            <input
              type="password"
              autoComplete="off"
              className="form-input"
              id="newpw"
              placeholder="**********"
              value={this.state.password}
              onChange={e => {
                this.setState({ password: e.target.value });
              }}
            />
          </FormGroup>

          <FormGroup autoComplete={"off"}>
            <Label className="white" for="newpw2">
              New password again:
            </Label>
            <input
              type="password"
              autoComplete="off"
              className="form-input"
              id="newpw2"
              placeholder="**********"
              value={this.state.repassword}
              onChange={e => {
                this.setState({ repassword: e.target.value });
              }}
            />
          </FormGroup>
          <div className="button-container text-center">
            <button className="form-button">Változtat</button>
          </div>
        </Form>
        {this.state.error !== "" && (
          <span className="white">{this.state.error}</span>
        )}
      </div>
    );
  }
}
const title = "Profilom";
