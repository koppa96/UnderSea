import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../store";
import { Dispatch, bindActionCreators } from "redux";
import {
  TestAddActionCreator,
  TestDeleteActionCreator
} from "./store/actions/testAction";
import { connect } from "react-redux";
import { Test } from "./Test";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  humanState: state.app.pages.gyakorlas
});

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      addHuman: TestAddActionCreator,
      removeHUman: TestDeleteActionCreator
    },

    dispatch
  );
export const TestConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Test);
