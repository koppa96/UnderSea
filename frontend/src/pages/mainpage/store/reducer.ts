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

import {
  IPostTargetActions,
  PostAttackActions
} from "../attack/store/actions/AddAttackAction.post";
import {
  IDeleteWarActions,
  DeleteWarActions
} from "../war/store/actions/WarAction.delete";
import {
  IAddDevelopmentActions,
  AddDevelopmentActions
} from "../development/store/actions/DevelopmentAction.post";
import { BriefCreationInfo, BriefUnitInfo } from "../../../api/Client";
import {
  IPostReportActions,
  PostReportActions
} from "../reports/store/actions/ReportAction.post";
import {
  IDeleteReportActions,
  DeleteReportActions
} from "../reports/store/actions/ReportAction.delete";
import {
  IRefreshActions,
  RefreshActions
} from "./actions/RefreshActions.update";

export const MainpageReducer = (
  state = initialMainpageResponseState,
  action:
    | IActions
    | IAddBuildingActions
    | IArmyActions
    | IPostTargetActions
    | IAddDevelopmentActions
    | IDeleteWarActions
    | IPostReportActions
    | IDeleteReportActions
    | IRefreshActions
): MainpageResponseState => {
  switch (action.type) {
    case MainpageActions.REQUEST:
      return {
        ...state,
        loading: true
      };

    case MainpageActions.SUCCES:
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
    case AddDevelopmentActions.SUCCES:
      var devs =
        state.model && state.model.researches && state.model.researches;
      var tempDev: BriefCreationInfo | undefined;
      var tempPearls = state.model && state.model.pearls;

      if (devs !== undefined) {
        tempDev = devs.find(r => r.id === action.data);
        if (tempDev !== undefined) {
          devs.forEach(dev => {
            if (tempDev !== undefined)
              if (dev.id === tempDev.id) {
                dev.inProgressCount = 1;
              }
          });
        }
        if (tempPearls && tempDev !== undefined) {
          //TODO mockolt adat at kell kotni a researchstoreba
          tempPearls -= 1000;
        }
      }
      return {
        ...state,
        loading: false,
        model: state.model
          ? {
              ...state.model,
              researches: devs,
              pearls: tempPearls ? tempPearls : 0
            }
          : undefined
      };
    case AddBuildingActions.REQUEST:
      return {
        ...state,
        loading: true
      };
    case AddBuildingActions.SUCCES:
      const newBuilding = state.model
        ? state.model.buildings
          ? state.model.buildings
          : []
        : [];
      var totalBought = 0;
      for (let index = 0; index < newBuilding.length; index++) {
        if (newBuilding[index].id === action.data.id) {
          newBuilding[index].inProgressCount =
            newBuilding[index].inProgressCount + 1;
          totalBought = totalBought + action.data.cost;
        }
      }

      return {
        ...state,
        loading: false,
        model: state.model
          ? {
              ...state.model,
              buildings: newBuilding,
              pearls: state.model.pearls - totalBought
            }
          : undefined
      };
    case AddBuildingActions.ERROR:
      return {
        ...state,
        loading: false,
        error: action.error ? action.error : "Ismeretlen hiba hozzáadásnál"
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
                armyunit.defendingCount += unit.count;
                costPearl += unit.price * unit.count;
              }
            });
          });
        }
        costPearl = state.model.pearls - costPearl;
      }
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
    case PostAttackActions.REQUEST:
      return {
        ...state
      };
    case PostAttackActions.ERROR:
      return {
        ...state
      };
    case PostAttackActions.SUCCES:
      const newUnits: BriefUnitInfo[] = [];

      state.model &&
        state.model.armyInfo &&
        state.model.armyInfo.forEach(x => {
          var tempIdUnit =
            action.data.units &&
            action.data.units.find(item => item.id === x.id);
          if (tempIdUnit) {
            x.defendingCount = x.defendingCount - tempIdUnit.totalCount;
          }
          newUnits.push(x);
        });
      return {
        ...state,
        model: state.model
          ? {
              ...state.model,
              armyInfo: newUnits
            }
          : undefined
      };
    case DeleteWarActions.REQUEST:
      return {
        ...state
      };
    case DeleteWarActions.ERROR:
      return {
        ...state
      };
    case DeleteWarActions.SUCCES:
      const backUnits: BriefUnitInfo[] = [];
      state.model &&
        state.model.armyInfo &&
        state.model.armyInfo.forEach(x => {
          var tempIdUnit =
            action.data.units &&
            action.data.units.find(item => item.id === x.id);
          if (tempIdUnit) {
            x.defendingCount = x.defendingCount + tempIdUnit.totalCount;
          }
          backUnits.push(x);
        });

      return {
        ...state,
        model: state.model
          ? {
              ...state.model,
              armyInfo: backUnits
            }
          : undefined
      };
    case PostReportActions.REQUEST:
      return {
        ...state
      };
    case PostReportActions.SUCCES:
      return {
        ...state,
        model: state.model
          ? { ...state.model, unseenReports: state.model.unseenReports - 1 }
          : undefined
      };
    case PostReportActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Beállítási hiba"
      };
    case DeleteReportActions.REQUEST:
      return {
        ...state
      };
    case DeleteReportActions.SUCCES:
      return {
        ...state,
        model: state.model
          ? {
              ...state.model,
              unseenReports: action.data.isSeen
                ? state.model.unseenReports
                : state.model.unseenReports - 1
            }
          : undefined
      };
    case DeleteReportActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Beállítási hiba"
      };
    case RefreshActions.REQUEST:
      return {
        ...state,
        model: action.params
      };
    case RefreshActions.SUCCES:
      return {
        ...state
      };
    case RefreshActions.ERROR:
      return {
        ...state,
        error: action.error ? action.error : "Frissítési hiba"
      };
    default:
      // const check: never = action.type;
      return state;
  }
};
