import { App } from "./App";
import { connect } from "react-redux";
import { Dispatch, bindActionCreators } from "redux";
import { GetProfileActionCreator } from "./components/profileContainer/store/actions/profileContainer.get";
import { DispachedProps, MappedProps } from "./Interface";
import { IApllicationState } from "./store";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { username } = state.app.pages.profile.profile;
  return {
    serverResponseLogin: username && username.length > 1 ? true : false
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getUserInfo: GetProfileActionCreator
    },
    dispatch
  );

export const AppConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(App);
