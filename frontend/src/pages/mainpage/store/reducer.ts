import { IActions, MainpageActions } from "./actions/MainpageAction.get";
import { MainpageResponseState, initialMainpageResponseState } from "./store";
import {
  IAddBuildingActions,
  AddBuildingActions
} from "../buildings/store/actions/BuildingAction.post";
import {
  IArmyActions,
  ArmyActions
} from "../army/store/actions/ArmyActions.post";
import { BriefUnitInfo } from "../../../api/Client";

export const MainpageReducer = (
  state = initialMainpageResponseState,
  action: IActions | IAddBuildingActions | IArmyActions
): MainpageResponseState => {
  switch (action.type) {
    case MainpageActions.REQUEST:
      return {
        ...state,
        loading: true
      };

    case MainpageActions.SUCCES:
      console.log("Mainpage reducer army", action.params.country);
      return {
        ...state,
        loading: false,
        model: action.params.country,
        error: ""
      };
    case MainpageActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
      };
    case AddBuildingActions.REQUEST:
      return {
        ...state,
        loading: true,
        lastBuilding: action.params
      };
    case AddBuildingActions.SUCCES:
      const newBuilding = state.model
        ? state.model.buildings
          ? state.model.buildings
          : []
        : [];
      for (let index = 0; index < newBuilding.length; index++) {
        if (newBuilding[index].id === state.lastBuilding) {
          newBuilding[index].inProgressCount =
            newBuilding[index].inProgressCount + 1;
        }
      }

      return {
        ...state,
        loading: false,
        model: state.model
          ? {
              ...state.model,
              buildings: newBuilding
            }
          : undefined
      };
    case AddBuildingActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
          ? action.params
          : "Ismeretlen hiba" + state.lastBuilding + " hozzáadásnál"
      };
    case ArmyActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case ArmyActions.SUCCESS:
      let temp: BriefUnitInfo[] = [];
      let costPearl: number = 0;
      if (state.model) {
        if (state.model.armyInfo) {
          temp = state.model.armyInfo;
          temp.forEach(armyunit => {
            action.data.unitsToAdd.forEach(unit => {
              if (unit.unitId === armyunit.id) {
                armyunit.totalCount += unit.count;
                costPearl += unit.price * unit.count;
              }
            });
          });
        }
        costPearl = state.model.pearls - costPearl;
      }
      console.log(costPearl);
      return {
        ...state,
        loading: false,
        model: state.model
          ? {
              ...state.model,
              armyInfo: temp,
              pearls: costPearl
            }
          : undefined
      };
    case ArmyActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.params
          ? action.params
          : "Ismeretlen hiba az egységek hozzáadásnál"
      };

    default:
      const check: never = action;
      return state;
  }
};
