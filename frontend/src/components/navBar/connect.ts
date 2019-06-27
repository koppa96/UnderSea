import { MappedProps } from "./Interface";

import { connect } from "react-redux";
import { NavBar } from "./NavBar";
import { IApllicationState } from "../../store";

const mapStateToProps = (state: IApllicationState): MappedProps => ({
  navbar: {
    navBarIcons: state.app.pages.mainpage.model
  }
});
export const NavBarConnected = connect(
  mapStateToProps,
  {}
)(NavBar);
