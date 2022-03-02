import React from "react";
import {
  Box,
  CircularProgress,
  Backdrop,
  Paper,
  AppBar,
  Toolbar,
  Typography,
  Button,
  IconButton,
  Popover,
  List,
  ListItem,
  ListItemButton,
  ListItemText
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import MoreHorizIcon from "@mui/icons-material/MoreHoriz";
import { DataGrid, GridColDef, GridValueGetterParams } from "@mui/x-data-grid";
import SessionTableConfigDropdown from "./SessionTableConfigDropdown";
import {
  useCloseSessionTableViewMutation,
  useExportSessionTableViewCsvMutation,
  useGetSessionTableInfoQuery,
  useGetSessionTableViewColumnsQuery,
  useGetSessionTableViewRowCountQuery,
  useLazyGetSessionTableViewRowsQuery
} from "./services/application";
import Resizable from "react-resizable-box";
import { height } from "@mui/system";

interface RowsState {
  page: number;
  pageSize: number;
  rows: any[];
  loading: boolean;
}

interface SessionTableViewProps {
  sessionId: string;
  tableId: string;
  tableViewId: string;
}

const TableViewPopover: React.FC<SessionTableViewProps> = (props) => {
  const args = {
    sessionId: props.sessionId,
    tableId: props.tableId,
    viewId: props.tableViewId
  };

  const [exportCsv] = useExportSessionTableViewCsvMutation();

  const [anchorEl, setAnchorEl] = React.useState<HTMLButtonElement | null>(
    null
  );

  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleExportClick = () => {
    handleClose();
    exportCsv(args);
  };

  const open = Boolean(anchorEl);
  const id = open ? "simple-popover" : undefined;

  return (
    <div>
      <IconButton edge="end" size="small" color="inherit" onClick={handleClick}>
        <MoreHorizIcon />
      </IconButton>
      <Popover
        id={id}
        open={open}
        anchorEl={anchorEl}
        onClose={handleClose}
        anchorOrigin={{
          vertical: "bottom",
          horizontal: "left"
        }}
      >
        <List>
          <ListItem disablePadding>
            <ListItemButton onClick={handleExportClick}>
              <ListItemText primary="Export to CSV" />
            </ListItemButton>
          </ListItem>
        </List>
      </Popover>
    </div>
  );
};

const SessionTableView: React.FC<SessionTableViewProps> = (props) => {
  const args = {
    sessionId: props.sessionId,
    tableId: props.tableId,
    viewId: props.tableViewId
  };

  const [closeView] = useCloseSessionTableViewMutation();

  const info = useGetSessionTableInfoQuery(args);

  const rowCount = useGetSessionTableViewRowCountQuery(args);

  const columns = useGetSessionTableViewColumnsQuery(args);

  let columnsData = columns.data;
  if (columnsData === undefined) {
    columnsData = [];
  }

  const cols = columnsData.map((col, index) => {
    return {
      field: `col${index}`,
      headerName: col.name,
      width: 150
    };
  });

  const [rowsState, setRowsState] = React.useState<RowsState>({
    page: 0,
    pageSize: 100,
    rows: [],
    loading: false
  });

  const [fetchRows, results, lastResult] =
    useLazyGetSessionTableViewRowsQuery();

  React.useEffect(() => {
    const rows = results.data || [];

    const newRows = rows.map((row, index) => {
      const toAdd: any = { id: index + 1 };

      row.forEach((el, index) => {
        toAdd[`col${index}`] = el;
      });
      return toAdd;
    });

    setRowsState((prev) => ({ ...prev, loading: false, rows: newRows }));
  }, [results.data]);

  React.useEffect(() => {
    (async () => {
      setRowsState((prev) => ({ ...prev, loading: true }));

      fetchRows({
        ...args,
        start: rowsState.page * rowsState.pageSize,
        count: rowsState.pageSize
      });
    })();
  }, [rowsState.page, rowsState.pageSize]);

  return (
    <Resizable
      className="session-view-container"
      height={400}
      style={{ width: "100%" }}
      bounds={"parent"}
      enable={{
        top: false,
        right: false,
        bottom: true,
        left: false,
        topLeft: false,
        topRight: false,
        bottomLeft: false,
        bottomRight: false
      }}
    >
      <Box sx={{ width: "100%", height: "100%" }}>
        <Paper
          elevation={6}
          sx={{ height: "100%", display: "flex", flexFlow: "column" }}
        >
          <Backdrop
            open={info.isFetching || rowCount.isFetching}
            sx={{ position: "absolute" }}
          >
            <CircularProgress color="inherit" />
          </Backdrop>
          <AppBar position="relative" elevation={0}>
            <Toolbar variant="dense">
              <Typography
                noWrap
                variant="caption"
                color="inherit"
                component="div"
                sx={{ ml: 0, pl: 0 }}
              >
                {info.data?.name || ""}
              </Typography>
              <SessionTableConfigDropdown
                sessionId={props.sessionId}
                tableId={props.tableId}
                tableViewId={props.tableViewId}
              />
              <TableViewPopover {...props} />
              <Box sx={{ flexGrow: 1 }}></Box>
              <IconButton
                edge="end"
                size="small"
                color="inherit"
                onClick={() => closeView(args)}
              >
                <CloseIcon />
              </IconButton>
            </Toolbar>
          </AppBar>
          <Box
            sx={{
              overflowY: "auto",
              width: "100%",
              height: "100%"
            }}
          >
            <Box
              sx={{
                overflowY: "auto",
                width: "100%",
                height: "100%"
              }}
            >
              <DataGrid
                columns={cols}
                pagination
                rowBuffer={25}
                density="compact"
                rowCount={rowCount.data}
                {...rowsState}
                paginationMode="server"
                onPageChange={(page) =>
                  setRowsState((prev) => ({ ...prev, page }))
                }
                onPageSizeChange={(pageSize) =>
                  setRowsState((prev) => ({ ...prev, pageSize }))
                }
              />
            </Box>
          </Box>
        </Paper>
      </Box>
    </Resizable>
  );
};

export default SessionTableView;
