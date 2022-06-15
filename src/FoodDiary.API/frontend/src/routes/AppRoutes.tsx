import { Navigate, RouteObject, useRoutes } from 'react-router-dom';
import { Auth, RequireAuth } from 'src/features/auth/components';
import { PageContent, Pages } from 'src/features/pages/components';
import { Products } from 'src/features/products/components';
import { Categories } from 'src/features/categories/components';
import NotFound from './NotFound';

const PUBLIC_ROUTES: RouteObject[] = [
  {
    path: '/',
    element: <Navigate to="/pages"></Navigate>,
  },
  {
    path: '/auth',
    element: <Auth></Auth>,
  },
];

const PRIVATE_ROUTES: RouteObject[] = [
  {
    path: '/pages',
    element: <Pages></Pages>,
  },
  {
    path: '/pages/:id',
    element: <PageContent></PageContent>,
  },
  {
    path: '/products',
    element: <Products></Products>,
  },
  {
    path: '/categories',
    element: <Categories></Categories>,
  },
];

export default function AppRoutes() {
  const element = useRoutes([
    ...PUBLIC_ROUTES,
    ...PRIVATE_ROUTES.map(route => ({
      ...route,
      element: <RequireAuth>{route.element}</RequireAuth>,
    })),
    {
      path: '*',
      element: <NotFound></NotFound>,
    },
  ]);

  return element;
}
