import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { BuildingAddActionCreator } from "./store/actions/buildingActions";
import { connect } from "react-redux";
import { Buildings } from "./Buildings";
import { GetBuildingActionCreator } from "./store/actions/get";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  return {
    boughtBuildingState: model ? model.buildings : []
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      addBuilding: BuildingAddActionCreator,
      getAllBuilding: GetBuildingActionCreator
    },

    dispatch
  );

export const BuildingsConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Buildings);
