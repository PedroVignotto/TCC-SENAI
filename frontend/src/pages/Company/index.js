import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Form, Input } from '@rocketseat/unform';
import { toast } from 'react-toastify';
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

  async function handleSubmit({ name, cnpj, email }) {
    try {
      await api.put(`companies/${profile.company_id}`, {
        name,
        cnpj,
        email,
      });

      toast.success('Empresa atualizada com sucesso');
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  return (
    <Container>
      <Form initialData={company} onSubmit={handleSubmit}>
        <Input name="name" placeholder="Nome da empresa" />
        <Input name="cnpj" placeholder="CNPJ da empresa" />
        <Input name="email" placeholder="Email da empresa" />

        <button type="submit">
          <MdSave size={22} />
          Atualizar empresa
        </button>
      </Form>
    </Container>
  );
}
