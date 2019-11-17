import React from 'react';
import { Switch } from 'react-router-dom';
import Route from './Route';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';

import Dashboard from '~/pages/Dashboard';
import Environment from '~/pages/Environment';
import Profile from '~/pages/Profile';
import User from '~/pages/User';

export default function Routes() {
  return (
    <Switch>
      <Route path="/" exact component={SignIn} />
      <Route path="/register" component={SignUp} />

      <Route path="/dashboard" component={Dashboard} isPrivate />
      <Route path="/environments" component={Environment} isPrivate />
      <Route path="/users" component={User} isPrivate />
      <Route path="/profile" component={Profile} isPrivate />
    </Switch>
  );
}
