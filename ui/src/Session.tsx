import React from "react";
import { Box, CircularProgress, Backdrop } from "@mui/material";
import View from "./containers/View";
import { useGetSessionTableViewsQuery } from "./services/application";
import SessionTableView from "./SessionTableView";

interface SessionProps {
  sessionId: string;
}

const Session: React.FC<SessionProps> = (props) => {
  const sessionTablesResponse = useGetSessionTableViewsQuery(props.sessionId);

  return (
    <Box>
      <Backdrop
        open={sessionTablesResponse.isFetching}
        sx={{ position: "absolute" }}
      >
        <CircularProgress color="inherit" />
      </Backdrop>
      {sessionTablesResponse.data?.map((tableId) => (
        <>
          <SessionTableView
            key={tableId.sessionTableViewId}
            sessionId={props.sessionId}
            tableId={tableId.sessionTableId}
            tableViewId={tableId.sessionTableViewId}
          />
          <br />
        </>
      ))}
    </Box>
  );
};

export default Session;
