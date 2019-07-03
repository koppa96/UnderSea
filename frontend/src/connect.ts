import { App } from "./App";
import { connect } from "react-redux";
import { Dispatch, bindActionCreators } from "redux";
import { DispachedProps, MappedProps } from "./Interface";
import { IApllicationState } from "./store";
import { CheckTokenActionCreator } from "./store/actions/CheckToken.get";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { token } = state.app.pages;
  return {
    serverResponseLogin: token.tokenCheck,
    loading: token.loading
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getTokenCheck: CheckTokenActionCreator
    },
    dispatch
  );

export const AppConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(App);
