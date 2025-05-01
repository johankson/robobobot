import { Add } from "@mui/icons-material";
import { Box, Button, List, ListItem, Stack, Typography } from "@mui/material";
import { useState } from "react";
import { useCreateSandboxBattle } from "../api/useCreateSandboxBattle";
import { useGetBattles } from "../api/useGetBattles";
import { BattleView } from "./BattleView";

export const BattlesList = () => {
  const [viewBattleId, setViewBattleId] = useState<string | null | undefined>();
  const { data: battles } = useGetBattles();
  const { mutate: createBattle } = useCreateSandboxBattle();

  return (
    <Box>
      {battles && (
        <>
          <Stack direction="row" gap="1rem" alignItems="center">
            <Typography variant="h3">Active battles</Typography>
            <Button onClick={() => createBattle()}>
              <Add />
              Create new
            </Button>
          </Stack>
          {battles.length === 0 && <Typography>No active battles</Typography>}
          {battles.length > 0 && (
            <List>
              {battles.map((battle) => {
                return (
                  <ListItem key={battle.battleToken}>
                    {battle.players?.[0].name}
                    <Button onClick={() => setViewBattleId(battle.battleToken)}>
                      View
                    </Button>
                  </ListItem>
                );
              })}
            </List>
          )}
        </>
      )}
      {viewBattleId && <BattleView battleId={viewBattleId} />}
    </Box>
  );
};
