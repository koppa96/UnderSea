import { MappedProps, DispachedProps } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Buildings } from "./Buildings";
import { GetBuildingActionCreator } from "./store/actions/BuildingAction.get";
import { BuildingAddActionCreator } from "./store/actions/BuildingAction.post";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { buildings } = state.app.pages;
  const { model } = state.app.pages.mainpage;

  return {
    ownedBuildingState: buildings,
    count: model
      ? model.buildings
        ? model.buildings.map(info => ({
            id: info.id,
            count: info.count,
            inProgress: info.inProgressCount > 0 ? true : false
          }))
        : []
      : [],
    totalpearl: model ? model.pearls : 0,
    totalcoral: model ? model.corals : 0
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
