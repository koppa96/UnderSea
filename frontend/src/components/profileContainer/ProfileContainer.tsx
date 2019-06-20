import * as React from "react";
import { Link } from "react-router-dom";
import ProfileImg from "./../../assets/images/profile-bg.svg";

export class ProfileContainer extends React.Component {
  render = () => {
    return (
      <div className="profile-bg">
        <div className="rectangle">
          <img alt="profile" src={ProfileImg} />
        </div>
        <span className="profil-margin">jakabjancsi</span>
        <Link className="profil-margin" to="/profil">
          Profil
        </Link>
        <span>Kijelentkezés</span>
      </div>
    );
  };
}
