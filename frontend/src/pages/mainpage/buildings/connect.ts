import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Buildings } from "./Buildings";
import { BuildingAddActionCreator } from "./store/actions/buildingActions";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  buildingState: state.app.pages.buildingIds
});

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      addBuildings: BuildingAddActionCreator
    },

    dispatch
  );
export const BuildingsConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Buildings);
