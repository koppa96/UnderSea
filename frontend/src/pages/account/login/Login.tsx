import * as React from "react";
import axios from "axios";

import qs from "qs";
import "./../../../assets/scss/forms.scss";

import { Form } from "reactstrap";
import { LoginProps, LoginState, LoginResponse } from "./Interface";
import { Link } from "react-router-dom";

export class Login extends React.Component<LoginProps> {
  state: LoginState = {
    model: {
      name: "",
      password: ""
    },
    error: null
  };

  handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    this.props.beginlogin({
      name: this.state.model.name,
      password: this.state.model.password
    });
  };

  /*handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const token =
      "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6ImMyMWMzYzQxMjgyMmY0MmVjZDcxYmY0ZDhiY2I5OWUwIiwidHlwIjoiSldUIn0.eyJuYmYiOjE1NjEzODM2MTMsImV4cCI6MTU2MTM4NzIxMywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNTUiLCJhdWQiOlsiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNTUvcmVzb3VyY2VzIiwidW5kZXJzZWFfYXBpIl0sImNsaWVudF9pZCI6InVuZGVyc2VhX2NsaWVudCIsInN1YiI6IjAyNmE0MDlmLTZlYjMtNDI5Mi1hZDViLWRlOWJmMDBlMjMyMyIsImF1dGhfdGltZSI6MTU2MTM4MzYxMywiaWRwIjoibG9jYWwiLCJuYW1lIjoiYXNkIiwiZW1haWwiOiJhc2RAYXNkLmFzZCIsInNjb3BlIjpbInVuZGVyc2VhX2FwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.rv1QdyVPHuwE5kQZIYf-95b5hkSAcVan9mZu-8KPga4EO1J1wQuq0v4sSoaBIqcstyZytOLbvuu9M264tElhmTABZG8imSF-6MykCf2Lcq1Af9AHq0tk25pIoJ2nBEWxl0aWtap1sHpnm27TAooKW2OeHHquUvbQHyhAe6mDfUfiaN0K_ykG0zya9_pGm4mquPhwfETVrvKT_7tOWkcFb1QGpWYRxPK8ntXCsOThE0th43rlNeBoCOg0XRXIN3-TX5gOY5A8a32uqAsvaDOrOPIx5W0MFUj6mxOlh3raPJvuvDSbPB6ST2x401J3LCw4C6Iy7iGQGYeDjo_7cWeABA";
  const header={

  }*/

  render() {
    const { error, loading } = this.props;
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
                type="password"
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
              <div className=" form-center">
                {loading ? (
                  <div className="loading-circle" />
                ) : (
                  <button className="form-button" type="submit">
                    Belépés
                  </button>
                )}

                {error && <p className="form-error">{error}</p>}
              </div>
            </Form>
            <div className=" text-center">
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
