import { DragHandleRounded } from "@mui/icons-material";
import {
  AppBar,
  Backdrop,
  Box,
  CircularProgress,
  Toolbar,
  Typography
} from "@mui/material";
import { border, borderRadius, height, maxHeight } from "@mui/system";
import React, { FC, useRef } from "react";

const View: FC<{ title: string; loading: boolean }> = (props) => {
  return (
    <Box
      sx={{
        height: "100%",
        width: "100%",
        boxSizing: "border-box",
        display: "flex",
        flexDirection: "column",
        overflowX: "hidden",
        position: "relative"
      }}
    >
      <Backdrop open={props.loading} sx={{ position: "absolute" }}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <AppBar position="relative">
        <Toolbar variant="dense" style={{ padding: "0px" }}>
          <DragHandleRounded style={{ margin: "0px 15px", cursor: "grab" }} />
          <Typography
            variant="caption"
            color="inherit"
            component="div"
            sx={{ ml: 0, pl: 0 }}
          >
            {props.title.toUpperCase()}
          </Typography>
        </Toolbar>
      </AppBar>
      <Box
        sx={{
          overflowY: "auto",
          height: "100%",
          width: "100%"
        }}
      >
        {props.children}
      </Box>
    </Box>
  );
};

export default View;
