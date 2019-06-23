import { NavBarIconProp } from "../navBarIcons/Interface";

export interface NavbarState {
  navBarIcons: NavBarIconProp[];
}

export const initialNabarState: NavbarState = {
  navBarIcons: [
    {
      id: 0,
      amount: 0,
      imageUrl: "asd"
    }
  ]
};
