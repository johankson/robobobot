import { useMutation, useQueryClient } from "@tanstack/react-query";
import { useApi } from "../hooks/useApi";

export const useCreateSandboxBattle = () => {
  const api = useApi();
  const queryClient = useQueryClient();
  const mutationFn = () =>
    api
      .battleJoinSandboxCreate({
        name: "Gurra G",
      })
      .then((d) => d.data);
  return useMutation({
    mutationFn,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["battles"] });
    },
  });
};
