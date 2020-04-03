import { Colors } from '@blueprintjs/core';

const themeSetting: "dark" | "light" = "dark";

export const theme = {
  BACKGROUND1: (themeSetting == "dark") ? Colors.DARK_GRAY1 : Colors.LIGHT_GRAY1,
  BACKGROUND2: (themeSetting == "dark") ? Colors.DARK_GRAY2 : Colors.LIGHT_GRAY2,
  BACKGROUND3: (themeSetting == "dark") ? Colors.DARK_GRAY3 : Colors.LIGHT_GRAY3,
  BACKGROUND4: (themeSetting == "dark") ? Colors.DARK_GRAY4 : Colors.LIGHT_GRAY4,
  BACKGROUND5: (themeSetting == "dark") ? Colors.DARK_GRAY5 : Colors.LIGHT_GRAY5,

  spacing: 8,
};