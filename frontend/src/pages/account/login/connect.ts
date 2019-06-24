import { MappedProps, DispatchedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import Login from "./Login";

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {},

    dispatch
  );
export const LoginConnected = connect(
  null,
  mapDispatchToProps
)(Login);
