import React from "react";
import {
  MenuItem,
  Select,
  InputLabel,
  FormControl,
  SelectChangeEvent,
  Typography,
  CircularProgress,
  Box
} from "@mui/material";
import {
  useGetSessionTableConfigsQuery,
  useGetSessionTableViewConfigQuery,
  useSetSessionTableViewConfigMutation
} from "./services/application";

interface SessionTableConfigDropdownProps {
  sessionId: string;
  tableId: string;
  tableViewId: string;
}

const SessionTableConfigDropdown: React.FC<SessionTableConfigDropdownProps> = (
  props
) => {
  const args = { sessionId: props.sessionId, tableId: props.tableId };

  const configs = useGetSessionTableConfigsQuery(args);

  const currentConfig = useGetSessionTableViewConfigQuery({
    ...args,
    viewId: props.tableViewId
  });

  const [setConfig] = useSetSessionTableViewConfigMutation();

  const handleChange = (event: SelectChangeEvent) => {
    var newConfig = event.target.value;
    setConfig({ ...args, viewId: props.tableViewId, configId: newConfig });
  };

  return (
    <Select
      sx={{
        mt: "-2px",
        ml: 1,
        height: "2em",
        overflowX: "hidden"
      }}
      variant="standard"
      renderValue={(value) => {
        return (
          <Typography
            noWrap
            variant="caption"
            color="inherit"
            component="span"
            sx={{ ml: 0, pl: 0 }}
          >
            {configs.data?.find((val) => val.id == value)?.name || "ERROR"}
          </Typography>
        );
      }}
      autoWidth={true}
      value={currentConfig.data || ""}
      onChange={handleChange}
    >
      {configs.isFetching ? (
        <Box sx={{ paddingLeft: "25px", paddingRight: "25px" }}>
          <CircularProgress size={"20px"} />
        </Box>
      ) : configs.data === undefined ? (
        <Box sx={{ paddingLeft: "25px", paddingRight: "25px" }}>
          {"Error loading configs."}
        </Box>
      ) : configs.data.length === 0 ? (
        <Box sx={{ paddingLeft: "25px", paddingRight: "25px" }}>
          {"No configs found."}
        </Box>
      ) : (
        (configs.data || []).map((config) => {
          return <MenuItem value={config.id}>{config.name}</MenuItem>;
        })
      )}
    </Select>
  );
};

export default SessionTableConfigDropdown;
