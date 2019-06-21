export interface ManState {
  name: string;
  age?: number;
  isOld: boolean;
  gender: string;
}

export const testInitialState: ManState = {
  name: "",
  age: undefined,
  isOld: false,
  gender: ""
};
