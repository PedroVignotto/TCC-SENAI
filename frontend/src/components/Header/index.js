import React, { useState } from 'react';
import { useSelector } from 'react-redux';
import { MdMenu, MdClear } from 'react-icons/md';

import logo from '~/assets/logo.svg';
import avatar from '~/assets/avatar.png';

import { Container, Content, Profile, Link } from './styles';

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
          {visible ? <MdClear size={36} /> : <MdMenu size={36} />}
        </button>

        <nav className="header">
          <Link to="/dashboard" onClick={handleTogleVisible}>
            HOME
          </Link>
          <Link to="/environments" onClick={handleTogleVisible}>
            AMBIENTES
          </Link>
          <Link to="/heritages" onClick={handleTogleVisible}>
            PATRIMÔNIOS
          </Link>
          <Link to="/users" onClick={handleTogleVisible}>
            USUÁRIOS
          </Link>
          <Link to="/historical" onClick={handleTogleVisible}>
            HISTÓRICO
          </Link>
          {profile && profile.user_level === 1 ? (
            <Link to="/company" onClick={handleTogleVisible}>
              EMPRESA
            </Link>
          ) : (
            ''
          )}
        </nav>

        <Profile>
          <Link to="/profile">
            <strong>{profile.name}</strong>
            <img
              src={(profile.avatar && profile.avatar.url) || avatar}
              alt="Avatar"
            />
          </Link>
        </Profile>
      </Content>
    </Container>
  );
}
