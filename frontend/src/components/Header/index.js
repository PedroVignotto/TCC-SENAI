import React from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';

import logo from '~/assets/logo.svg';

import { Container, Content, Profile } from './styles';

export default function Header() {
  const profile = useSelector(state => state.user.profile);

  return (
    <Container>
      <Content>
        <nav>
          <img src={logo} alt="Heritage" width="150" />
          <Link to="/dashboard">HOME</Link>
          <Link to="/dashboard">AMBIENTES</Link>
          <Link to="/dashboard">PATRIMÔNIOS</Link>
          <Link to="/dashboard">USUÁRIOS</Link>
          <Link to="/dashboard">HISTÓRICO</Link>
          {profile.user_level === 1 ? <Link to="/dashboard">EMPRESA</Link> : ''}

          <Link to="/dashboard">COMO USAR</Link>
        </nav>

        <Profile>
          <Link to="/profile">
            <strong>{profile.name}</strong>
            <img
              src={
                (profile.avatar && profile.avatar.url) ||
                'https://api.adorable.io/avatars/50/abott@adorable.png'
              }
              alt="Avatar"
            />
          </Link>
        </Profile>
      </Content>
    </Container>
  );
}
