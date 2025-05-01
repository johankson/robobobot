import type { ContainerProps } from "@mui/material/Container";
import Container from "@mui/material/Container";
import type { Breakpoint } from "@mui/material/styles";

// ----------------------------------------------------------------------

export type DashboardContentProps = ContainerProps & {
  layoutQuery?: Breakpoint;
  disablePadding?: boolean;
};

export function DashboardContent({
  sx,
  children,
  disablePadding,
  maxWidth = "lg",
  layoutQuery = "lg",
  ...other
}: DashboardContentProps) {
  return (
    <Container
      maxWidth={maxWidth}
      sx={[
        (theme) => ({
          display: "flex",
          flex: "1 1 auto",
          flexDirection: "column",
          pt: "var(--layout-dashboard-content-pt)",
          pb: "var(--layout-dashboard-content-pb)",
          [theme.breakpoints.up(layoutQuery)]: {
            px: "var(--layout-dashboard-content-px)",
          },
          ...(disablePadding && {
            p: {
              xs: 0,
              sm: 0,
              md: 0,
              lg: 0,
              xl: 0,
            },
          }),
        }),
        ...(Array.isArray(sx) ? sx : [sx]),
      ]}
      {...other}
    >
      {children}
    </Container>
  );
}
