import { MappedProps, DispachedProps, countProp } from "./Interface";
import { IApllicationState } from "../../../store";
import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { Buildings } from "./Buildings";
import { GetBuildingActionCreator } from "./store/actions/get";
import { ICreationInfo } from "../../../api/Client";
import { BuildingAddActionCreator } from "../store/actions/post/addBuilding";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { buildings } = state.app.pages.buildings;
  const { model } = state.app.pages.mainpage;

  const temp: ICreationInfo[] = [];
  const newModel = model ? (model.buildings ? model.buildings : []) : [];
  for (let index = 0; index < newModel.length; index++) {
    for (let index2 = 0; index2 < buildings.length; index2++) {
      if (newModel[index].id === buildings[index2].id) {
        temp.push({
          count: newModel[index].count,
          cost: buildings[index2].cost,
          description: buildings[index2].description,
          id: buildings[index2].id,
          imageUrl: buildings[index2].imageUrl,
          name: buildings[index2].name
        });
      }
    }
  }

  return {
    boughtBuildingState: temp,
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
