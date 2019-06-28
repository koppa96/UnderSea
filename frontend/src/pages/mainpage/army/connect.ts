import { IApllicationState } from "../../../store";
import { MappedProps, DispachedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { Army } from "./Army";
import { connect } from "react-redux";
import { ArmyUnitAddActionCreator } from "./store/actions/ArmyActions.post";
import { getArmy } from "./store/actions/ArmyActions.get";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  return {
    ownedUnitState: state.app.pages.Army,
    count: model
      ? model.armyInfo
        ? model.armyInfo.map(info => ({ id: info.id, count: info.count }))
        : []
      : []
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
  bindActionCreators(
    {
      addUnits: ArmyUnitAddActionCreator,
      getArmy
    },
    dispatch
  );

export const ArmyConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Army);
