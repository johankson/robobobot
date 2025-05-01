import { ImportContacts } from "@mui/icons-material";
import FormatListNumberedIcon from "@mui/icons-material/FormatListNumbered";
import QueryStatsIcon from "@mui/icons-material/QueryStats";
import { Box, ListItem, Stack } from "@mui/material";
import { MenuButton } from "./MenuButton";

export const MainMenu = () => {
  return (
    <Stack padding="1rem">
      <Box component="nav">
        <Box
          component="ul"
          sx={{ display: "flex", flexDirection: "column", padding: 0 }}
        >
          <ListItem disableGutters disablePadding>
            <MenuButton isActive>
              <ImportContacts /> Battles
            </MenuButton>
          </ListItem>
          <ListItem disableGutters disablePadding>
            <MenuButton>
              <QueryStatsIcon /> Stats
            </MenuButton>
          </ListItem>
          <ListItem disableGutters disablePadding>
            <MenuButton>
              <FormatListNumberedIcon /> Logs
            </MenuButton>
          </ListItem>
        </Box>
      </Box>
    </Stack>
  );
};
