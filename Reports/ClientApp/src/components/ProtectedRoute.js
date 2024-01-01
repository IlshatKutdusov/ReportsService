import React from 'react';
import { Route, Redirect } from "react-router-dom";

const ProtectedRoute = ({ component: Component, isLoggedIn, path, ...props  }) => {
  return (
    <Route path={path}>
      {
        () => isLoggedIn ? <Component { ...props } /> : <Redirect to="/signin" />
      }
    </Route>
)}

export default ProtectedRoute;