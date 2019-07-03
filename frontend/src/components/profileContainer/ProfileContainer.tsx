import * as React from "react";
import { Link } from "react-router-dom";
import { ProfileProps } from "./Interface";
import { BasePortUrl } from "../..";

export class ProfileContainer extends React.Component<ProfileProps> {
  componentDidMount() {
    document.title = "Profil";
    this.props.getUserInfo();
  }

  render = () => {
    const { profile } = this.props.profile;
    const { event } = this.props;
    return (
      <div className="profile-bg">
        {event && <p onClick={() => this.props.togglePopup()}>+</p>}

        <div className="rectangle">
          <Link to="/">
            <img alt="profile" src={BasePortUrl + profile.profileImageUrl} />
          </Link>
        </div>
        <span className="profil-margin">
          {profile.username ? profile.username : ""}
        </span>
        <Link className="profil-margin" to="/account/profile">
          Profil
        </Link>
        <span onClick={this.props.logout} className="logout">
          Kijelentkezés
        </span>
      </div>
    );
  };
}
