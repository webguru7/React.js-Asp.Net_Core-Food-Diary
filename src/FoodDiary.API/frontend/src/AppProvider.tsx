import { CssBaseline, StyledEngineProvider, Theme, ThemeProvider } from '@mui/material';
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFns';
import React from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { Store } from 'redux';
import AuthProvider from './features/auth/AuthProvider';
import theme from './theme';

declare module '@mui/styles/defaultTheme' {
  // eslint-disable-next-line @typescript-eslint/no-empty-interface
  interface DefaultTheme extends Theme {}
}

type AppProviderProps = {
  store: Store;
  withAuthentication?: boolean;
  useFakeAuth?: boolean;
};

const AppProvider: React.FC<React.PropsWithChildren<AppProviderProps>> = ({
  children,
  store,
  withAuthentication,
  useFakeAuth,
}) => {
  return (
    <StyledEngineProvider injectFirst>
      <ThemeProvider theme={theme}>
        <LocalizationProvider dateAdapter={AdapterDateFns}>
          <Provider store={store}>
            <AuthProvider isAuthenticated={withAuthentication} useFakeAuth={useFakeAuth}>
              <CssBaseline />
              <BrowserRouter>{children}</BrowserRouter>
            </AuthProvider>
          </Provider>
        </LocalizationProvider>
      </ThemeProvider>
    </StyledEngineProvider>
  );
};

export default AppProvider;
