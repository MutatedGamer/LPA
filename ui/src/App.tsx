import "./App.css";

import { AvailableTables } from "./AvailableTables";
import Sessions from "./Sessions";
import {
  Box,
  Container,
  createTheme,
  CssBaseline,
  Grid,
  Paper,
  ThemeProvider
} from "@mui/material";

import Splitter, { SplitDirection } from "@devbookhq/splitter";

const mdTheme = createTheme();

function App() {
  return (
    <ThemeProvider theme={mdTheme}>
      <Box sx={{ display: "flex", height: "100vh" }}>
        <CssBaseline />
        <Box
          component="main"
          sx={{
            backgroundColor: (theme) =>
              theme.palette.mode === "light"
                ? theme.palette.grey[100]
                : theme.palette.grey[900],
            flexGrow: 1,
            height: "100vh",
            overflow: "auto"
          }}
        >
          <Splitter direction={SplitDirection.Horizontal}>
            <AvailableTables />
            <Sessions />
          </Splitter>
        </Box>
      </Box>
    </ThemeProvider>
  );
}

export default App;
