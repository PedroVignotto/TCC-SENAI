import React from 'react';
import { Link } from 'react-router-dom';
import { MdRoom, MdHistory, MdSupervisorAccount } from 'react-icons/md';
import { FaBoxOpen } from 'react-icons/fa';

import colors from '~/styles/colors';

import { Container } from './styles';

export default function Dashboard() {
  return (
    <Container>
      <section>
        <Link to="/environments">
          <MdRoom size={100} color={colors.primary} />
          <strong>Ambientes</strong>
          <span>Gerenciamento de ambientes</span>
        </Link>
        <Link to="/heritages">
          <FaBoxOpen size={100} color={colors.primary} />
          <strong>Patrimônios</strong>
          <span>Gerenciamento de patrimônios</span>
        </Link>
        <Link to="/users">
          <MdSupervisorAccount size={100} color={colors.primary} />
          <strong>Usuários</strong>
          <span>Gerenciamento de usuários</span>
        </Link>
        <Link to="/historical">
          <MdHistory size={100} color={colors.primary} />
          <strong>Histórico</strong>
          <span>Gerenciamento de histórico</span>
        </Link>
      </section>
    </Container>
  );
}
