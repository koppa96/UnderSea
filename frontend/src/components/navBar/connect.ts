import { MappedProps } from "./Interface";

import { Dispatch, bindActionCreators } from "redux";
import { connect } from "react-redux";
import { NavBar } from "./NavBar";
import { IApllicationState } from "../../store";
import { NavBarIconProp } from "../navBarIcons/Interface";
import { NavbarState } from "./store/store";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  navbar: {
    navBarIcons: state.app.pages.mainpage.model
  }
});
export const NavBarConnected = connect(
  mapStateToProps,
  {}
)(NavBar);
