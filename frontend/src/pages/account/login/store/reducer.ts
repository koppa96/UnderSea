import { LoginResponseState, initialLoginResponseState } from "./store";
import { IActions } from "./actions/post";

//Rename to reducer
export const LoginReducer = (
  state = initialLoginResponseState,
  action: IActions
): LoginResponseState => {
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
      const check: never = action;
      return state;
  }
};
