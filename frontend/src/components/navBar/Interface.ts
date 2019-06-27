import { NavBarIconProp } from "../navBarIcons/Interface";
import { NavbarState } from "./store/store";

interface NativeProps {}

export interface MappedProps {
  navbar: NavbarState;
}

export type NavBarProps = NativeProps & MappedProps;
