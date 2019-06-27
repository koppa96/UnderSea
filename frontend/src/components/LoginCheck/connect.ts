import { MappedProps } from "./Interface";
import { connect } from "react-redux";
import { IApllicationState } from "../../store";
import { LoginCheck } from "./LoginCheck";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  serverLogin:
    state.app.pages.loginDetails.model.access_token.length > 10 ? true : false
});

export const LoginCheckConnected = connect(
  mapStateToProps,
  {}
)(LoginCheck);
