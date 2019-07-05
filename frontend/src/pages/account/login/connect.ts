import { MappedProps, DispatchedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { BeginLoginActionCreator } from "./store/actions/LoginAction.post";
import { IApllicationState } from "../../../store";
import { GetProfileActionCreator } from "../../../components/profileContainer/store/actions/profileContainer.get";
import { Login } from "./Login";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  error: state.app.pages.loginDetails.error,
  loading: state.app.pages.loginDetails.loading,
  succes:
    state.app.pages.loginDetails.model.access_token.length > 10 ? true : false
});

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {
      beginlogin: BeginLoginActionCreator,
      getUserInfo: GetProfileActionCreator
    },

    dispatch
  );
export const LoginConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Login);
