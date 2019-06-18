import * as React from "react";

import { Button, Form, FormGroup, Input } from "reactstrap";
import { LoginProps, LoginState } from "./Interface";

export class Login extends React.Component<LoginProps, LoginState> {
  state: LoginState = {
    name: "",
    password: ""
  };

  render() {
    return (
      <Form onSubmit={this.handleSubmit}>
        <FormGroup>
          <Input
            required
            onChange={e =>
              this.setState({
                ...this.state,
                name: e.target.value
              })
            }
            placeholder="Felhasználó"
            name="name"
            id="name"
          />
        </FormGroup>
        <FormGroup>
          <Input
            required
            onChange={e =>
              this.setState({
                ...this.state,
                name: e.target.value
              })
            }
            placeholder="Jelszó"
            name="password"
            id="password"
          />
        </FormGroup>
        <FormGroup>
          <Button variant={"success"} color="primary" type="submit">
            Regisztráció
          </Button>
        </FormGroup>
      </Form>
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
