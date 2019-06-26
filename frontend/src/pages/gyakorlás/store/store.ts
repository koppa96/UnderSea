
export interface ManState {
  name: string;
  age?: number;
  isOld: boolean;
  gender: string;
  //  asd: alma;
}

export const testInitialState: ManState = {
  name: "",
  age: undefined,
  isOld: false,
  gender: ""
  //  asd: { asd: { ka: "asdasd" } }
};
