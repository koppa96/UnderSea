import { IApllicationState } from "../../../store";
import { bindActionCreators, Dispatch } from "redux";
import { connect } from "react-redux";
import { Attack } from "./Attack";
import { GetTargetActionCreator } from "./store/actions/GetAttackAction.get";
import { PostTargetActionCreator } from "./store/actions/AddAttackAction.post";
import { DispatchedProps, MappedProps, defendingTrop } from "./interface";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;
  const tempUnit = model ? model.armyInfo : [];
  var addUnit: defendingTrop[] = [];
  tempUnit &&
    tempUnit.forEach(item =>
      addUnit.push({
        count: item.totalCount,
        id: item.id,
        defendingCount: item.defendingCount,
        name: item.name ? item.name : "ismeretlen név",
        imageUrl: item.imageUrl ? item.imageUrl : null
      })
    );

  return {
    units: addUnit,
    targets: state.app.pages.target
  };
};

const mapDispatchToProps = (dispatch: Dispatch): DispatchedProps =>
  bindActionCreators(
    {
      getTargets: GetTargetActionCreator,
      attackTarget: PostTargetActionCreator
    },

    dispatch
  );

export const AttackConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(Attack);
