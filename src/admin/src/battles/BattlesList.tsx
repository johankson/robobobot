import { Box, List, ListItem, Typography } from "@mui/material";
import { useGetBattles } from "../api/useGetBattles";

export const BattlesList = () => {
  const { data: battles } = useGetBattles();

  return (
    <Box>
      {battles && (
        <>
          <Typography variant="h3">Active battles</Typography>
          {battles.length === 0 && <Typography>No active battles</Typography>}
          {battles.length > 0 && (
            <List>
              {battles.map((battle) => {
                return (
                  <ListItem key={battle.battleToken}>
                    {battle.players?.[0].name}
                  </ListItem>
                );
              })}
            </List>
          )}
        </>
      )}
    </Box>
  );
};
