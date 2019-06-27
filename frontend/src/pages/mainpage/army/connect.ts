import { IApllicationState } from "../../../store";
import { MappedProps, DispachedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { Army } from "./Army";
import { connect } from "react-redux";
import { ArmyUnitAddActionCreator } from "./store/actions/ArmyActions.post";
import { getArmy } from "./store/actions/ArmyActions.get";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  ownedUnitState: state.app.pages.Army
});

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
