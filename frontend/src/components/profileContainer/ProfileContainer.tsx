import * as React from "react";
import { Link } from "react-router-dom";
import { ProfileProps } from "./Interface";
import { BasePortUrl } from "../..";

export class ProfileContainer extends React.Component<ProfileProps> {
  render = () => {
    const { profile } = this.props.profile;
    console.log("profile container rendered");
    return (
      <div className="profile-bg">
        <div className="rectangle">
          <img alt="profile" src={BasePortUrl + profile.profileImageUrl} />
        </div>
        <span className="profil-margin">
          {profile.username ? profile.username : ""}
        </span>
        <Link className="profil-margin" to="/">
          Profil
        </Link>
        <span onClick={this.props.logout} className="logout">
          Kijelentkezés
        </span>
      </div>
    );
  };
}
