import { Box, Grid } from "@mui/material";
import { BattlesList } from "./battles/BattlesList";
import { BattleStats } from "./battles/BattlesStats";
import { Header } from "./components/Header";
import { MainMenu } from "./components/MainMenu";

const App = () => {
  return (
    <Box>
      <Grid container>
        <Grid size={12}>
          <Box component="header">
            <Header />
          </Box>
        </Grid>
        <Grid size={2}>
          <MainMenu />
        </Grid>
        <Grid size={10}>
          <BattleStats />
          <BattlesList />
        </Grid>
      </Grid>
    </Box>
  );
};

export default App;
