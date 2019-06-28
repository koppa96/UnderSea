import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Development } from "./Development";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  const researches = model && model.researches ? model.researches : [];

  return {
    totalDevelopment: {
      development: researches,
      loading: state.app.pages.mainpage.loading,
      isPostRequesting: false
    }
  };
};
/*
const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
       addDevelopment: ,
    },

    dispatch
  );
*/
export const DevelopmentConnected = connect(
  mapStateToProps,
  {}
)(Development);
