import { testInitialState, ManState } from "./store";
import { IActions, TestActions } from "./actions/testAction";

//Rename to reducer
export const ManReducer = (
  state = testInitialState,
  action: IActions
): ManState => {
  switch (action.type) {
    case TestActions.REQUEST:
      return {
        ...state,
        name: action.params.name,
        age: action.params.age,
        gender: action.params.gender,
        isOld: action.params.age > 18 ? true : false
      };

    case TestActions.DELETE:
      return {
        ...testInitialState
      };

    default:
      const _check: never = action;
      return state;
  }
};
