import React from "react";
import { ComponentHeader } from "../../../components/componentHeader";
import { Form, Label, FormGroup } from "reactstrap";
import { FormControl, Input } from "@material-ui/core";

export default class Profile extends React.Component {
  render() {
    return (
      <div className="main-component profile-width">
        <ComponentHeader title={title} />
        <Form>
          <FormGroup>
            <Label for="oldpw">Old password: </Label>
            <Input id="oldpw" placeholder="**********" />
          </FormGroup>

          <FormGroup>
            <Label for="newpw">New password: </Label>
            <Input id="newpw" placeholder="**********" />
          </FormGroup>

          <FormGroup>
            <Label for="newpw2">New password again: </Label>
            <Input id="newpw2" placeholder="**********" />
          </FormGroup>
        </Form>
      </div>
    );
  }
}
const title = "Profilom";
