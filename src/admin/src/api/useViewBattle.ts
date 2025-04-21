import { useQuery } from "@tanstack/react-query";
import { useApi } from "../hooks/useApi";

export const useViewBattle = (battleId?: string | null) => {
  const api = useApi();
  const queryFn = () =>
    api
      .battleViewBattleList({
        battleId: battleId ?? "",
      })
      .then((d) => d.data);
  return useQuery({
    queryFn,
    queryKey: ["battles", "view", battleId],
    enabled: Boolean(battleId),
  });
};
