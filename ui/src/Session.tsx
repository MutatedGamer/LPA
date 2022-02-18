import React from "react";
import { Box, CircularProgress, Backdrop } from "@mui/material";
import View from "./containers/View";
import { useGetSessionTablesQuery } from "./services/application";
import SessionTable from "./SessionTable";

interface SessionProps {
  sessionId: string;
}

const Session: React.FC<SessionProps> = (props) => {
  const sessionTablesResponse = useGetSessionTablesQuery(props.sessionId);

  return (
    <Box>
      <Backdrop
        open={sessionTablesResponse.isFetching}
        sx={{ position: "absolute" }}
      >
        <CircularProgress color="inherit" />
      </Backdrop>
      {sessionTablesResponse.data?.map((tableId) => (
        <Box key={tableId} sx={{ margin: "5px" }}>
          <SessionTable sessionId={props.sessionId} tableId={tableId} />
        </Box>
      ))}
    </Box>
  );
};

export default Session;
