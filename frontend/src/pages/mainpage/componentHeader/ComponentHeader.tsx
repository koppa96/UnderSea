import * as React from "react";
import { HeaderProps } from "./Interface";

export const ComponentHeader = (props: HeaderProps) => {
  return (
    <header className="component-header">
      <h2>{props.title}</h2>
      <p className="main-font-important">{props.mainDescription}</p>
      <p>{props.description}</p>
    </header>
  );
};
