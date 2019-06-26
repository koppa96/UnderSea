import { ArmyState, ArmyInitialState } from "./store";
import { IActions, ArmyActions } from "./actions/ArmyActions";

export const ArmyReducer = (
    state = ArmyInitialState,
    action: IActions
): ArmyState => {
    switch(action.type) {
        case ArmyActions.REQUEST:
            const temp = state.units
            temp.forEach(unit => {
                action.params.unitsToAdd.forEach(element => {
                    if(unit.id == element.unitId)
                        unit.count += element.count
                }); 
            });
            return {
                ...state,
                units : temp
            };
        default:
           // const check: never = action.type;
            return state;
    }
}