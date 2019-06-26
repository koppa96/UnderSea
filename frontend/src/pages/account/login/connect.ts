import { MappedProps, DispatchedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import Login from "./Login";
import { BeginLoginActionCreator } from "./store/actions/post";
import { IApllicationState } from "../../../store";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  error: state.app.pages.loginDetails.error,
  loading: state.app.pages.loginDetails.loading
});

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {
      beginlogin: BeginLoginActionCreator
    },

    dispatch
  );
export const LoginConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Login);
