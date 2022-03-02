import React from "react";
import { Box, Tab, Tabs, Typography } from "@mui/material";
import View from "./containers/View";
import { useGetSessionsQuery } from "./services/application";
import Session from "./Session";

const NoSessions = () => {
  return (
    <Box
      display="flex"
      flexDirection={"column"}
      justifyContent="center"
      alignItems="center"
      height="100%"
    >
      <Typography variant="h4" color="inherit" component="div" align="center">
        {"No sessions loaded."}
      </Typography>
      <br />
      <Typography
        variant="body1"
        color="inherit"
        component="div"
        align="center"
      >
        {"To get started, first load a plugin and then process a file."}
      </Typography>
    </Box>
  );
};

function a11yProps(index: number) {
  return {
    id: `simple-tab-${index}`,
    "aria-controls": `simple-tabpanel-${index}`
  };
}

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      <Box sx={{ p: 3 }}>
        <Typography>{children}</Typography>
      </Box>
    </div>
  );
}

const SessionTabs: React.FC<{ sessions: string[] }> = (props) => {
  const [value, setValue] = React.useState(0);

  const handleChange = (_event: React.SyntheticEvent, newValue: number) => {
    setValue(newValue);
  };

  return (
    <Box sx={{ width: "100%" }}>
      <Box sx={{ borderBottom: 1, borderColor: "divider" }}>
        <Tabs
          value={value}
          onChange={handleChange}
          aria-label="basic tabs example"
          variant="scrollable"
          scrollButtons
          allowScrollButtonsMobile
        >
          {props.sessions.map((sessionId, index) => (
            <Tab
              key={sessionId}
              label={`Session ${index + 1}`}
              {...a11yProps(index)}
            />
          ))}
        </Tabs>
      </Box>
      {props.sessions.map((sessionId, index) => (
        <TabPanel key={sessionId} value={value} index={index}>
          <Session sessionId={sessionId} />
        </TabPanel>
      ))}
    </Box>
  );
};

const Sessions = () => {
  const sessionsResponse = useGetSessionsQuery();

  if (
    !sessionsResponse.isFetching &&
    !sessionsResponse.isLoading &&
    sessionsResponse.data === undefined
  ) {
    return <div>"Error"</div>;
  }

  return (
    <View title="Sessions" loading={sessionsResponse.isFetching}>
      {sessionsResponse.data === undefined ? (
        <></>
      ) : sessionsResponse.data.length == 0 ? (
        <NoSessions />
      ) : (
        <SessionTabs sessions={sessionsResponse.data as string[]} />
      )}
    </View>
  );
};

export default Sessions;
