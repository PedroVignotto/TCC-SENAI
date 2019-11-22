import React from 'react';
import { Switch } from 'react-router-dom';
import Route from './Route';

import SignIn from '~/pages/SignIn';
import SignUp from '~/pages/SignUp';

import Dashboard from '~/pages/Dashboard';
import Environment from '~/pages/Environment';
import EnvironmentHeritage from '~/pages/Environment/Heritage';
import Profile from '~/pages/Profile';
import User from '~/pages/User';
import Company from '~/pages/Company';
import Heritage from '~/pages/Heritage';
import Historic from '~/pages/Historic';

export default function Routes() {
  return (
    <Switch>
      <Route path="/" exact component={SignIn} />
      <Route path="/register" component={SignUp} />

      <Route path="/dashboard" component={Dashboard} isPrivate />
      <Route path="/environments" component={Environment} isPrivate />
      <Route path="/users" component={User} isPrivate />
      <Route path="/company" component={Company} isPrivate />
      <Route path="/heritages" component={Heritage} isPrivate />
      <Route
        path="/:environment_id/heritages"
        component={EnvironmentHeritage}
        isPrivate
      />
      <Route path="/historical" component={Historic} isPrivate />
      <Route path="/profile" component={Profile} isPrivate />
    </Switch>
  );
}
