import {
  Box,
  Button,
  CircularProgress,
  List,
  ListItem,
  Stack,
  Typography,
} from "@mui/material";
import { Player } from "../../apiClient/data-contracts";
import { useMovePlayer } from "../api/useMovePlayer";
import { useViewBattle } from "../api/useViewBattle";

type DirectionValue = [number, number];
type DirectionKey = "North" | "South" | "East" | "West";

const directions: Record<DirectionKey, DirectionValue> = {
  North: [0, -1],
  South: [0, 1],
  East: [-1, 0],
  West: [1, 0],
};

interface Props {
  battleId: string;
}
export const BattleView = ({ battleId }: Props) => {
  const { data: battleView, isLoading } = useViewBattle(battleId);
  const { mutate: doMovePlayer } = useMovePlayer();

  const movePlayer = (player: Player, direction: DirectionKey) => {
    if (player.token && player.location) {
      doMovePlayer({
        playerToken: player.token,
        x: player.location.x! + directions[direction][0],
        y: player.location.y! + directions[direction][1],
      });
    }
  };

  return (
    <Stack direction="row">
      {isLoading && <CircularProgress />}
      {!isLoading && (
        <>
          <pre>{battleView?.battleField}</pre>
          <Box>
            <Typography>Players:</Typography>
            <List>
              {battleView?.players?.map((player) => {
                return (
                  <ListItem>
                    {player.name}
                    <Button onClick={() => movePlayer(player, "North")}>
                      Move up
                    </Button>
                    <Button onClick={() => movePlayer(player, "South")}>
                      Move down
                    </Button>
                    <Button onClick={() => movePlayer(player, "East")}>
                      Move left
                    </Button>
                    <Button onClick={() => movePlayer(player, "West")}>
                      Move right
                    </Button>
                  </ListItem>
                );
              })}
            </List>
          </Box>
        </>
      )}
    </Stack>
  );
};
