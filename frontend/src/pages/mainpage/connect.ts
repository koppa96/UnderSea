import { IApllicationState } from "../../store";
import { bindActionCreators, Dispatch } from "redux";
import { MainPage } from "./Mainpage";
import { connect } from "react-redux";
import { MappedProps, DispatchedProps } from "./Interface";
import { MainpageRequestActionCreator } from "./store/actions/MainpageAction.get";
import { RefreshRequestActionCreator } from "./store/actions/RefreshActions.update";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  error: state.app.pages.mainpage.error,
  loading: state.app.pages.mainpage.loading,
  building: state.app.pages.mainpage.model
    ? state.app.pages.mainpage.model.buildings
      ? state.app.pages.mainpage.model.buildings
      : []
    : [],
  event:
    state.app.pages.mainpage.model &&
    state.app.pages.mainpage.model.event &&
    state.app.pages.mainpage.model.event,
  researches: state.app.pages.mainpage.model
    ? state.app.pages.mainpage.model.researches
      ? state.app.pages.mainpage.model.researches
      : []
    : []
});

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {
      beginFetchMainpage: MainpageRequestActionCreator,
      refreshCountryInfo: RefreshRequestActionCreator
    },

    dispatch
  );
export const MainPageConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(MainPage);
