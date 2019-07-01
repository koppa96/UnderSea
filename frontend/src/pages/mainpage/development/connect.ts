import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Development } from "./Development";
import { GetDevelopmentRequestActionCreator } from "./store/actions/DevelopmnetAction.get";
import { AddDevelopmentRequestActionCreator } from "./store/actions/DevelopmentAction.post";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  const researchs = model && model.researches ? model.researches : [];

  return {
    totalResourcesDesc: researchs,
    totalDevelopment: state.app.pages.development
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getAllDevelopment: GetDevelopmentRequestActionCreator,
      addDevelopment: AddDevelopmentRequestActionCreator
    },

    dispatch
  );

export const DevelopmentConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Development);
