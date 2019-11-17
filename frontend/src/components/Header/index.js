import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { MdReorder } from 'react-icons/md';

import logo from '~/assets/logo.svg';

import { Container, Content, Profile } from './styles';

export default function Header() {
  const [visible, setVisible] = useState(false);

  const profile = useSelector(state => state.user.profile);

  function handleTogleVisible() {
    setVisible(!visible);
  }

  return (
    <Container>
      <Content visible={visible}>
        <img src={logo} alt="Heritage" />

        <button type="button" onClick={handleTogleVisible}>
          <MdReorder size={36} />
        </button>

        <nav className="header">
          <Link to="/dashboard" onClick={handleTogleVisible}>
            HOME
          </Link>
          <Link to="/environments" onClick={handleTogleVisible}>
            AMBIENTES
          </Link>
          <Link to="/dashboard" onClick={handleTogleVisible}>
            PATRIMÔNIOS
          </Link>
          <Link to="/dashboard" onClick={handleTogleVisible}>
            USUÁRIOS
          </Link>
          <Link to="/dashboard" onClick={handleTogleVisible}>
            HISTÓRICO
          </Link>
          {profile.user_level === 1 ? (
            <Link to="/dashboard" onClick={handleTogleVisible}>
              EMPRESA
            </Link>
          ) : (
            ''
          )}

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
