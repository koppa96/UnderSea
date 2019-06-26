import { IApllicationState } from "../../../store";
import { MappedProps, DispachedProps } from "./Interface";
import { Dispatch, bindActionCreators } from "redux";
import { Army } from "./Army";
import { connect } from "react-redux";
import { ArmyUnitAddActionCreator } from "./store/actions/ArmyActions";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
    ownedUnitState: state.app.pages.Army
});

const mapDispatchToProps = (dispatch: Dispatch): DispachedProps =>
    bindActionCreators({
        addUnits: ArmyUnitAddActionCreator
    },
    dispatch
    );

export const ArmyConnected = connect(
    mapStateToProps,
    mapDispatchToProps
)(Army);