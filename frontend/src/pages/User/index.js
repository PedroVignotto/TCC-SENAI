import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';
import Swal from 'sweetalert2';
import { Input, Form, Select } from '@rocketseat/unform';
import {
  MdDeleteForever,
  MdCached,
  MdSave,
  MdSearch,
  MdAddCircleOutline,
  MdInfo,
} from 'react-icons/md';

import api from '~/services/api';
import Top from '~/components/Top';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

export default function User() {
  const [users, setUsers] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showInfo, setShowInfo] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(false);

  const options = [
    { id: 1, title: 'Administrador' },
    { id: 2, title: 'Gerenciador' },
    { id: 3, title: 'Suporte' },
  ];

  const profile = useSelector(state => state.user.profile);

  function handleShowEdit(user) {
    setEdit(user);
    setShowEdit(true);
  }

  function handleShowInfo(user) {
    setEdit(user);
    setShowInfo(true);
  }

  useEffect(() => {
    async function loadUsers() {
      const response = await api.get(`${profile.company_id}/users`);

      setUsers(response.data);
    }

    loadUsers();
  }, []); //eslint-disable-line

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
          api.delete(`users/${id}`);
          toast.success('User successfully deleted');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleEdit({ user_level, id }) {
    console.tron.log(user_level, id);

    try {
      await api.put(`users/${id}`, {
        user_level,
      });

      toast.success('User level updated successfully');
      setShowEdit(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ user_level, email }) {
    try {
      await api.put(`company/users/${email}`, {
        user_level,
        company_id: profile.company_id,
      });

      toast.success('User successfully added');
      setShowAdd(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }
  return (
    <>
      <Container>
        <Top
          title="Usuários"
          subtitle="Veja as informações dos usuários e mude seu nivel de permissão"
        />

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
          {users.map(user => (
            <li key={user.id}>
              <Link to="dashboard">
                <img
                  src={
                    (user.avatar && user.avatar.url) ||
                    `https://api.adorable.io/avatar/50/${user.name}.pnc`
                  }
                  alt={user.name}
                />
                <div>
                  <strong>{user.name}</strong>
                  <span>{user.user_level === 1 ? 'Administrador' : ''}</span>
                  <span>{user.user_level === 2 ? 'Gerenciador' : ''}</span>
                  <span>{user.user_level === 3 ? 'Suporte' : ''}</span>
                </div>
              </Link>

              <div>
                <button type="button" onClick={() => handleShowInfo(user)}>
                  <MdInfo size={22} />
                </button>
                <button type="button" onClick={() => handleShowEdit(user)}>
                  <MdCached size={22} />
                </button>
                <button type="button" onClick={() => handleDelete(user.id)}>
                  <MdDeleteForever size={22} />
                </button>
              </div>
            </li>
          ))}
        </ul>
      </Container>

      <Modals show={showInfo} onHide={() => setShowInfo(false)} animation>
        <Modals.Header>
          <h4>Informações do usuário</h4>
          <button type="button" onClick={() => setShowInfo(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form initialData={edit} onSubmit={handleEdit}>
            <img
              src={
                (edit.avatar && edit.avatar.url) ||
                `https://api.adorable.io/avatar/50/${edit.name}.pnc`
              }
              alt={edit.name}
            />

            <Input name="name" readOnly />
            <Input name="email" readOnly />
          </Form>
        </Modals.Body>
      </Modals>

      <Modals show={showEdit} onHide={() => setShowEdit(false)} animation>
        <Modals.Header>
          <h4>Alterar permissões do usuário</h4>
          <button type="button" onClick={() => setShowEdit(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form initialData={edit} onSubmit={handleEdit}>
            <Input name="id" type="hidden" />
            <Select name="user_level" options={options} />
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
            <Input name="email" placeholder="Email" required />
            <Select
              name="user_level"
              options={options}
              placeholder="Nível de permissão"
              required
            />
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
