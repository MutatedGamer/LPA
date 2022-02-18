import React from "react";
import { Box, CircularProgress, Backdrop } from "@mui/material";
import { DataGrid, GridColDef, GridValueGetterParams } from "@mui/x-data-grid";
import View from "./containers/View";
import {
  useGetSessionTableConfigQuery,
  useGetSessionTableDataQuery,
  useGetSessionTablesQuery
} from "./services/application";

interface SessionTableProps {
  sessionId: string;
  tableId: string;
}

const SessionTable: React.FC<SessionTableProps> = (props) => {
  const config = useGetSessionTableConfigQuery({
    sessionId: props.sessionId,
    tableId: props.tableId
  });

  const rowsResponse = useGetSessionTableDataQuery({
    sessionId: props.sessionId,
    tableId: props.tableId
  });

  let configData = config.data;
  if (configData === undefined) {
    configData = [];
  }

  const cols = configData.map((col, index) => {
    return {
      field: `col${index}`,
      headerName: col,
      width: 150,
      resizable: true
    };
  });

  let rowsData = rowsResponse.data;
  if (rowsData === undefined) {
    rowsData = [];
  }

  const rows = rowsData.map((row, index) => {
    const toAdd: any = { id: index + 1 };

    row.forEach((el, index) => {
      toAdd[`col${index}`] = el;
    });
    return toAdd;
  });

  return (
    <Box>
      <Backdrop open={config.isFetching} sx={{ position: "absolute" }}>
        <CircularProgress color="inherit" />
      </Backdrop>
      <DataGrid sx={{ minHeight: "500px" }} columns={cols} rows={rows} />
    </Box>
  );
};

export default SessionTable;
