import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';
import Swal from 'sweetalert2';
import { Input, Form } from '@rocketseat/unform';
import {
  MdDeleteForever,
  MdCached,
  MdSave,
  MdSearch,
  MdAddCircleOutline,
} from 'react-icons/md';

import api from '~/services/api';
import Top from '~/components/Top';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

export default function Environment() {
  const [environments, setEnvironments] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(false);

  const profile = useSelector(state => state.user.profile);

  function handleShowEdit(environment) {
    setEdit(environment);
    setShowEdit(true);
  }

  useEffect(() => {
    async function loadEnvironments() {
      const response = await api.get(`${profile.company_id}/environments`);

      setEnvironments(response.data);
    }

    loadEnvironments();
  }, [environments, profile.company_id]);

  function handleDelete(id) {
    try {
      Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: `${colors.green}`,
        cancelButtonColor: `${colors.red}`,
        confirmButtonText: 'Yes, delete it!',
      }).then(result => {
        if (result.value) {
          api.delete(`environments/${id}`);
          toast.success('Environment successfully deleted');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleEdit({ id, name, user_id }) {
    console.tron.log(id, name, user_id);

    try {
      await api.put(`environments/${id}`, {
        name,
        user_id,
      });

      toast.success('Data updated successfully');
      setShowEdit(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ name, user_id }) {
    try {
      await api.post('environments', {
        name,
        user_id,
        company_id: profile.company_id,
      });

      toast.success('Environment successfully added');
      setShowAdd(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  return (
    <>
      <Container>
        <Top title="Ambientes" subtitle="Gerenciamento de ambientes" />

        <Search>
          <div>
            <input type="text" placeholder="PESQUISAR" />
            <button type="button">
              <MdSearch size={28} />
            </button>
          </div>
          <button type="button">
            <MdAddCircleOutline size={28} onClick={() => setShowAdd(true)} />
          </button>
        </Search>

        <ul>
          {environments.map(environment => (
            <li>
              <Link to="dashboard">
                <strong>{environment.name}</strong>
                <span>{environment.user_id}</span>
              </Link>

              <div>
                <button
                  type="button"
                  onClick={() => handleShowEdit(environment)}
                >
                  <MdCached size={22} />
                </button>
                <button
                  type="button"
                  onClick={() => handleDelete(environment.id)}
                >
                  <MdDeleteForever size={22} />
                </button>
              </div>
            </li>
          ))}
        </ul>
      </Container>

      <Modals show={showEdit} onHide={() => setShowEdit(false)} animation>
        <Modals.Header>
          <h4>Editar ambiente</h4>
          <button type="button" onClick={() => setShowEdit(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form initialData={edit} onSubmit={handleEdit}>
            <Input name="id" type="hidden" />
            <Input name="name" />
            <Input name="user_id" />
            <button type="submit">
              <MdSave size={22} />
              Salvar
            </button>
          </Form>
        </Modals.Body>
      </Modals>

      <Modals show={showAdd} onHide={() => setShowAdd(false)} animation>
        <Modals.Header>
          <h4>Adicionar ambiente</h4>
          <button type="button" onClick={() => setShowAdd(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form onSubmit={handleAdd}>
            <Input name="id" type="hidden" />
            <Input name="name" placeholder="Nome" />
            <Input name="user_id" placeholder="Gerenciador" />
            <button type="submit">
              <MdSave size={22} />
              Salvar
            </button>
          </Form>
        </Modals.Body>
      </Modals>
    </>
  );
}
