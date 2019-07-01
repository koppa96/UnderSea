import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { GetWarActionCreator } from "./store/actions/WarAction.get";
import { War } from "./War";
import { DispachedProps, MappedProps } from "./Interface";
import { DeleteWarActionCreator } from "./store/actions/WarAction.delete";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  return {
    totalWar: state.app.pages.war
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      getAllWar: GetWarActionCreator,
      deleteById: DeleteWarActionCreator
    },

    dispatch
  );

export const WarConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(War);
