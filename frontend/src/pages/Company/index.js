import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Form } from '@rocketseat/unform';
import { toast } from 'react-toastify';
import { MdSave } from 'react-icons/md';

import api from '~/services/api';

import Top from '~/components/Top';
import Input from '~/components/Input';

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

  console.tron.log(company);

  async function handleSubmit(data) {
    const { name, cnpj, email, cep, address, district, city, state } = data;

    try {
      await api.put(`companies/${profile.company_id}`, {
        name,
        cnpj,
        email,
        cep,
        address,
        district,
        city,
        state,
      });

      toast.success('Empresa atualizada com sucesso');
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  return (
    <Container>
      <Top title="Empresa" subtitle="Gerenciamento de dados da empresa" />

      <Form initialData={company} onSubmit={handleSubmit}>
        <Input name="name" label="Nome" />
        <Input name="email" label="Email" />
        <div>
          <Input name="cnpj" label="CNPJ" />
          <Input name="cep" label="Cep" />
        </div>
        <div>
          <Input name="address" label="Endereço" />
          <Input name="district" label="Bairro" />
        </div>
        <div>
          <Input name="city" label="Cidade" />
          <Input name="state" label="Estado" />
        </div>

        <button type="submit">
          <MdSave size={22} />
          Atualizar empresa
        </button>
      </Form>
    </Container>
  );
}
