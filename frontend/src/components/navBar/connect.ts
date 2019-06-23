import { MappedProps } from "./Interface";

import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { NavBar } from "./NavBar";
import { IApllicationState } from "../../store";
import { NavBarIconProp } from "../navBarIcons/Interface";
import { NavbarState } from "./store";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  navbar: {
    navBarIcons: state.app.pages.buildingIds.buildings
  }
});

const mapDispatchToProps = (dispatch: Dispatch) =>
  bindActionCreators(
    {},

    dispatch
  );

export const NavBarConnected = connect(
  mapStateToProps,
  mapDispatchToProps
)(NavBar);
