import React, { useState, useEffect } from 'react';
import { parseISO, formatDistance } from 'date-fns';
import pt from 'date-fns/locale/pt';
import { useSelector } from 'react-redux';
import { MdSearch, MdHistory } from 'react-icons/md';

import api from '~/services/api';
import Top from '~/components/Top';

import colors from '~/styles/colors';
import { Container, Search } from './styles';

export default function Environment() {
  const [historics, setHistorics] = useState([]);
  const [search, setSearch] = useState('');

  const profile = useSelector(state => state.user.profile);

  async function loadHistorics() {
    const response = await api.get(`${profile.company_id}/historical`, {
      params: { q: search },
    });

    const data = response.data.map(historic => ({
      ...historic,
      createdAt: formatDistance(parseISO(historic.createdAt), new Date(), {
        addSuffix: true,
        locale: pt,
      }),
    }));

    setHistorics(data);
  }

  useEffect(() => {
    loadHistorics();
  }, []); //eslint-disable-line

  return (
    <>
      <Container>
        <Top
          title="Histórico"
          subtitle="Histórico de movimentação e manutenção"
        />

        <Search>
          <div>
            <input
              type="text"
              placeholder="PESQUISAR"
              autoComplete="off"
              onKeyDown={event => event.key === 'Enter' && loadHistorics()}
              onChange={e => setSearch(e.target.value)}
            />
            <button type="button" onClick={() => loadHistorics()}>
              <MdSearch size={28} />
            </button>
          </div>
        </Search>

        {historics.length ? (
          historics.map(historic => (
            <aside key={historic.id}>
              <span>{historic.createdAt}:</span>
              <span>{historic.message}</span>
            </aside>
          ))
        ) : (
          <div>
            <MdHistory size={100} color={colors.primary} />
            <h6>Nenhum histórico para exibir</h6>
          </div>
        )}
      </Container>
    </>
  );
}
