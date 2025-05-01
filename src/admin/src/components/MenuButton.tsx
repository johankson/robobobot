import { ButtonBaseOwnProps, ListItemButton, styled } from "@mui/material";
import { varAlpha } from "minimal-shared";
import { PropsWithChildren } from "react";

const StyledMenuButton = styled(ListItemButton)`
  border-radius: 0.75;
  font-weight: "fontWeightMedium";
  color: ${({ theme }) => theme.vars.palette.text.secondary};
  min-height: 44;
`;
interface Props extends PropsWithChildren<ButtonBaseOwnProps> {
  isActive?: boolean;
}

export const MenuButton = ({ children, isActive, ...rest }: Props) => {
  return (
    <StyledMenuButton
      disableGutters
      sx={[
        (theme) => ({
          pl: 2,
          py: 1,
          gap: 2,
          pr: 1.5,
          typography: "body2",
          ...(isActive && {
            fontWeight: "fontWeightSemiBold",
            color: theme.vars.palette.primary.main,
            bgcolor: varAlpha(theme.vars.palette.primary.mainChannel, 0.08),
            "&:hover": {
              bgcolor: varAlpha(theme.vars.palette.primary.mainChannel, 0.16),
            },
          }),
        }),
      ]}
      {...rest}
    >
      {children}
    </StyledMenuButton>
  );
};
