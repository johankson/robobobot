import { useQuery } from "@tanstack/react-query";
import { useApi } from "../hooks/useApi";

export const useGetBattles = () => {
  const api = useApi();
  const queryFn = () => api.adminListBattlesList().then((d) => d.data);
  return useQuery({
    queryFn,
    queryKey: ["battles"],
  });
};
