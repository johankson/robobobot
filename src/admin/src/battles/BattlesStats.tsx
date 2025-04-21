import BarChartIcon from "@mui/icons-material/BarChart";
import { Card, Grid, styled, Typography } from "@mui/material";
import { useGetBattles } from "../api/useGetBattles";

const Widget = styled(Card)`
  padding: 1rem;
`;

export const BattleStats = () => {
  const { data: battles } = useGetBattles();

  const numPlayers = battles?.reduce(
    (_acc, battle) => battle.players?.length ?? 0,
    0
  );

  return (
    <Grid container columnSpacing="1rem">
      <Grid size={3}>
        <Widget>
          <BarChartIcon />
          <Typography variant="h4">Active battles</Typography>
          <Typography variant="h1">{battles?.length ?? 0}</Typography>
        </Widget>
      </Grid>
      <Grid size={3}>
        <Widget>
          <BarChartIcon />
          <Typography variant="h4">Number of players</Typography>
          <Typography variant="h1">{numPlayers}</Typography>
        </Widget>
      </Grid>
    </Grid>
  );
};
