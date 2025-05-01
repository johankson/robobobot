import { Api } from "../../apiClient/Api";

export const useApi = () => {
  return new Api({
    baseURL: "https://localhost:7297",
  });
};
