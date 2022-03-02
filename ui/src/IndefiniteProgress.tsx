import React from "react";
import {
  Box,
  Button,
  Grid,
  LinearProgress,
  linearProgressClasses,
  styled,
  Typography
} from "@mui/material";
import { useParams } from "react-router-dom";
import {
  useCancelProgressMutation,
  useGetAvailableTablesQuery,
  useGetProgressStateQuery
} from "./services/application";
import { FormatColorReset } from "@mui/icons-material";

const BorderLinearProgress = styled(LinearProgress)(({ theme }) => ({
  height: 10,
  borderRadius: 4,
  [`&.${linearProgressClasses.colorPrimary}`]: {
    backgroundColor:
      theme.palette.grey[theme.palette.mode === "light" ? 200 : 800]
  },
  [`& .${linearProgressClasses.bar}`]: {
    borderRadius: 5,
    backgroundColor: theme.palette.mode === "light" ? "#1a90ff" : "#308fe8"
  }
}));

const ProgressBase = (props: { id: string }) => {
  const progressState = useGetProgressStateQuery(props.id);

  const availableTablesResponse = useGetAvailableTablesQuery();

  if (availableTablesResponse.error) {
    alert(availableTablesResponse.error);
  }
  const [cancel] = useCancelProgressMutation();

  if (progressState.data !== undefined) {
    window.document.title = "LPA  - " + progressState.data.label;
  }

  return (
    <Box
      sx={{
        height: "100%",
        padding: "0px 10px",
        display: "flex",
        flexDirection: "column",
        justifyContent: "space-between",
        alignItems: "center"
      }}
    >
      <Grid container rowSpacing={1}>
        <Grid item xs={12}>
          <Typography
            variant="body2"
            color="inherit"
            component="div"
            align="center"
          >
            {progressState.data !== undefined ? progressState.data.label : ""}
          </Typography>
        </Grid>
        <Grid item xs />
        <Grid item xs={12}>
          {progressState.data === undefined ? (
            <BorderLinearProgress variant="determinate" value={0} />
          ) : progressState.data !== undefined &&
            progressState.data.value !== null ? (
            <BorderLinearProgress
              variant="determinate"
              value={
                progressState.data !== undefined ? progressState.data.value : 0
              }
            />
          ) : (
            <BorderLinearProgress />
          )}
        </Grid>
        <Grid
          item
          container
          xs={12}
          direction="row"
          justifyContent="flex-end"
          alignItems="center"
        >
          <Button
            variant="contained"
            size="small"
            disableElevation
            disabled={!progressState.data?.canCancel || false}
            onClick={() => cancel(props.id)}
          >
            {progressState.data !== undefined
              ? progressState.data.cancelText
              : ""}
          </Button>
        </Grid>
      </Grid>
    </Box>
  );
};

const Progress = () => {
  const { id } = useParams();

  return <ProgressBase id={id as string} />;
};

export default Progress;
