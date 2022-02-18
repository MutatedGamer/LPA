import React from "react";
import { Button } from "@mui/material";
import { useLoadPluginMutation } from "./services/application";

const LoadPlugin = () => {
  const [loadPlugin] = useLoadPluginMutation();

  return (
    <Button variant="contained" onClick={() => loadPlugin()}>
      Load Plugin Directory
    </Button>
  );
};

export default LoadPlugin;
