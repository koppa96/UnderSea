import { bindActionCreators, Dispatch } from "redux";
import { connect } from "react-redux";
import { IApllicationState } from "../../store";
import { Menu } from "./Menu";
import { MappedProps } from "./Interface";

const mapStateToProps = (state: IApllicationState): MappedProps => {
  const { model } = state.app.pages.mainpage;

  return {
    unseenReports: model ? model.unseenReports : 0
  };
};

export const MenuConnected = connect(
  mapStateToProps,
  []
)(Menu);
