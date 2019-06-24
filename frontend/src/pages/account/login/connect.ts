import { MappedProps, DispachedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import Login from "./Login";

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {},

    dispatch
  );
export const LoginConnected = connect(
  null,
  mapDispatchToProps
)(Login);
