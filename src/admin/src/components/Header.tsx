import SmartToyIcon from "@mui/icons-material/SmartToy";
import { Box, Stack, styled, Typography } from "@mui/material";

const StyledHeader = styled(Box)`
  height: 3rem;
  padding: 1rem;
  display: flex;
`;

export const Header = () => {
  return (
    <StyledHeader>
      <Stack direction="row" gap={0.5}>
        <SmartToyIcon color="primary" />
        <Typography sx={{ textTransform: "uppercase", fontWeight: 700 }}>
          Robobot admin
        </Typography>
      </Stack>
    </StyledHeader>
  );
};
