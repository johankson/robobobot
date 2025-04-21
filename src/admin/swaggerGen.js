import { generateApi } from "swagger-typescript-api";
import https from "https";
import { fileURLToPath } from "url";
import path from "path";

process.env["NODE_TLS_REJECT_UNAUTHORIZED"] = 0;

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);

const agent = new https.Agent({
  rejectUnauthorized: false,
});

var url = "https://localhost:7297/swagger/v1/swagger.json";
await generateApi({
  httpClientType: "axios",
  generateClient: true,
  url,
  modular: true,
  output: path.resolve(__dirname, "./apiClient"),
});
