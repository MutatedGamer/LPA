import React from "react";
import {
  useGetAvailableTablesQuery,
  AvailableTable,
  useGetTableEnablementQuery,
  useToggleTableEnablementMutation,
  useDisableAllTablesMutation
} from "./services/application";
import View from "./containers/View";
import {
  Checkbox,
  Collapse,
  Divider,
  Box,
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Typography,
  Button
} from "@mui/material";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { groupBy } from "./utils/arrayUtils";
import LoadPlugin from "./LoadPlugin";

const NoTablesLoaded = () => {
  return (
    <Box
      display="flex"
      flexDirection={"column"}
      justifyContent="center"
      alignItems="center"
      height="100%"
    >
      <Typography variant="h4" color="inherit" component="div" align="center">
        {"No tables loaded."}
      </Typography>
      <br />
      <LoadPlugin />
    </Box>
  );
};

const AvailableTablesGroupItem: React.FC<{ table: AvailableTable }> = (
  props
) => {
  const isEnabledResponse = useGetTableEnablementQuery(props.table.guid);
  const [toggleEnablement] = useToggleTableEnablementMutation();

  return (
    <ListItemButton
      sx={{
        pl: 4,
        overflowX: "hidden",
        wordWrap: "normal",
        wordBreak: "normal"
      }}
      onClick={() => toggleEnablement(props.table.guid)}
    >
      <ListItemIcon>
        <Checkbox
          edge="start"
          tabIndex={-1}
          disableRipple
          checked={isEnabledResponse.data || false}
        />
      </ListItemIcon>
      <ListItemText
        primary={props.table.name}
        secondary={props.table.description}
      />
    </ListItemButton>
  );
};

const AvailableTablesGroup: React.FC<{
  group: string;
  tables: AvailableTable[];
}> = (props) => {
  const [open, setOpen] = React.useState(true);

  const handleClick = () => {
    setOpen(!open);
  };

  return (
    <>
      <ListItemButton onClick={handleClick}>
        <ListItemText primary={props.group} />
        {open ? <ExpandLess /> : <ExpandMore />}
      </ListItemButton>
      <Divider variant="fullWidth" component="li" />
      <Collapse in={open} timeout="auto">
        <List component="div" disablePadding>
          {props.tables.map((table, index) => {
            return <AvailableTablesGroupItem key={table.guid} table={table} />;
          })}
        </List>
      </Collapse>
    </>
  );
};

export const AvailableTables = () => {
  const availableTablesResponse = useGetAvailableTablesQuery();

  const [disableAll] = useDisableAllTablesMutation();

  if (availableTablesResponse.data === undefined) {
    return <div>"Error"</div>;
  }

  const tablesByCategory = groupBy(
    availableTablesResponse.data,
    (table) => table.category
  );

  return (
    <View
      title="Available Tables"
      loading={
        availableTablesResponse.isLoading || availableTablesResponse.isFetching
      }
    >
      {availableTablesResponse.data.length === 0 ? (
        <NoTablesLoaded />
      ) : (
        <>
          <Button onClick={() => disableAll()}>Disable All</Button>
          <List
            sx={{
              width: "100%",
              pb: 0,
              pt: 0,
              wordWrap: "break-word",
              bgcolor: "background.paper"
            }}
          >
            {Object.entries(tablesByCategory).map((kvp) => {
              return (
                <AvailableTablesGroup
                  key={kvp[0]}
                  group={kvp[0]}
                  tables={kvp[1]}
                />
              );
            })}
          </List>
        </>
      )}
    </View>
  );
};
