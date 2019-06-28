import { IApllicationState } from "../../../store";
import { bindActionCreators, Dispatch } from "redux";
import { connect } from "react-redux";
import { Attack } from "./Attack";
import { GetTargetActionCreator } from "./store/actions/GetAttackAction.get";
import { MappedProps, DispatchedProps } from "./interface";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  const tempUnit = model ? (model.armyInfo ? model.armyInfo : []) : [];

  return {
    unit: tempUnit,
    targets: state.app.pages.target.targets
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {
      getTargets: GetTargetActionCreator
    },

    dispatch
  );
export const AttackConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Attack);
