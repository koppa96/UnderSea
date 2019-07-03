import React from "react";
import Wave from "./../../assets/images/wave.svg";

export const NotFound = () => {
  return (
    <div className="main-component notfound-width">
      <div className="img-container">
        <img className="wave-notfound" src={Wave} alt="wave" />
        <h3 className="undersea-font-mainpage">UNDERSEA</h3>
      </div>
      <span className="not-found-text undersea-font">
        Ajjaj, valami hiba történt!
      </span>
    </div>
  );
};
