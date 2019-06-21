import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { BuildingAddActionCreator } from "./store/actions/buildingActions";
import { connect } from "react-redux";
import { Buildings } from "./Buildings";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  buildingState: state.app.pages.buildingIds
});

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      addBuilding: BuildingAddActionCreator
    },

    dispatch
  );

export const BuildingsConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Buildings);
