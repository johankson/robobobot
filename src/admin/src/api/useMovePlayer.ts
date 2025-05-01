import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useApi } from "../hooks/useApi";

interface MoveRequest {
  playerToken?: string;
  x?: number;
  y?: number;
}
export const useMovePlayer = () => {
  const api = useApi();
  const queryClient = useQueryClient();
  const mutationFn = (move: MoveRequest) =>
    api.adminMovePlayerList(move).then((d) => d.data);
  return useMutation({
    mutationFn,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["battles", "view"] });
    },
  });
};
