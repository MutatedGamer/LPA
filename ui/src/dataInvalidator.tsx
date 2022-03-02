import React, { FC } from "react";
import { useDispatch } from "react-redux";
import { applicationApi } from "./services/application";

const { ipcRenderer } = require("electron");

const DataInvalidator: FC<unknown> = ({ children }) => {
  const dispatch = useDispatch();

  ipcRenderer.on("invalidate", (_: any, arg: any) => {
    console.log(arg);
    dispatch(applicationApi.util.invalidateTags([arg]));
  });

  return <>{children}</>;
};

export default DataInvalidator;
