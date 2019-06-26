import { NavBarIconProp } from "../navBarIcons/Interface";
import { BuildingProps } from "../../pages/mainpage/buildings/buildingItem/Interface";
import { IBriefCreationInfo } from "../../api/Client";
export interface NavbarState {
  navBarIcons?: IBriefCreationInfo[];
}
