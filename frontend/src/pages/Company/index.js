import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Form, Input } from '@rocketseat/unform';
import { MdSave } from 'react-icons/md';

import api from '~/services/api';

import { Container } from './styles';

export default function Profile() {
  const [company, setCompany] = useState([]);

  const profile = useSelector(state => state.user.profile);

  useEffect(() => {
    async function loadCompany() {
      const response = await api.get(`/companies/${profile.company_id}`);

      setCompany(response.data);
    }

    loadCompany();
  }, []); //eslint-disable-line

  function handleSubmit() {}

  return (
    <Container>
      <Form initialData={company} onSubmit={handleSubmit}>
        <Input name="name" placeholder="Nome da empresa" />
        <Input name="cnpj" type="email" placeholder="CNPJ da empresa" />
        <Input name="email" placeholder="Email da empresa" />

        <button type="submit">
          <MdSave size={22} />
          Atualizar empresa
        </button>
      </Form>
    </Container>
  );
}
