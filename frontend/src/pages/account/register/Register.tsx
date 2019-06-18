import * as React from "react";

import { Button, Form, FormGroup, Input } from "reactstrap";
import { RegisterProps, RegisterState } from "./Interface";

export class Register extends React.Component<RegisterProps, RegisterState> {
  state: RegisterState = {
    name: "",
    password: "",
    repassword: "",
    countryname: ""
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
                password: e.target.value
              })
            }
            placeholder="Jelszó"
            name="password"
            id="password"
          />
        </FormGroup>
        <FormGroup>
          <Input
            required
            onChange={e =>
              this.setState({
                ...this.state,
                repassword: e.target.value
              })
            }
            placeholder="Jelszó megerősítése"
            name="repassword"
            id="repassword"
          />
        </FormGroup>
        <FormGroup>
          <Input
            required
            onChange={e =>
              this.setState({
                ...this.state,
                countryname: e.target.value
              })
            }
            placeholder="A városod neve amit építesz"
            name="countryname"
            id="countryname"
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

  /* handleChange = (e: React.FormEvent<HTMLInputElement>) => {
    this.setState({
      [e.currentTarget.name]: e.currentTarget.value
    });
  };
*/
}
export default Register;
