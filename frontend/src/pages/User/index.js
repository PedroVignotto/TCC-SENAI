import React, { useState, useEffect, useMemo } from 'react';
import { useSelector } from 'react-redux';
import { toast } from 'react-toastify';
import Swal from 'sweetalert2';
import { Form, Select } from '@rocketseat/unform';
import {
  MdDeleteForever,
  MdCached,
  MdSave,
  MdSearch,
  MdAddCircleOutline,
  MdSupervisorAccount,
} from 'react-icons/md';

import api from '~/services/api';
import Top from '~/components/Top';
import Loading from '~/components/Loading';
import Input from '~/components/Input';

import avatarProfile from '~/assets/profile.png';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

export default function User() {
  const [users, setUsers] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showInfo, setShowInfo] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(false);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(false);

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

  async function loadUsers() {
    try {
      setLoading(true);
      const response = await api.get(`${profile.company_id}/users`, {
        params: { q: search },
      });

      setUsers(response.data);
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadUsers();
  }, []); //eslint-disable-line

  const userSize = useMemo(() => users.length, [users]);

  function handleDelete(id) {
    try {
      Swal.fire({
        title: 'Você tem certeza?',
        text: 'Você não poderá desfazer isso!',
        showCancelButton: true,
        confirmButtonColor: `${colors.green}`,
        cancelButtonColor: `${colors.red}`,
        confirmButtonText: 'Sim, excluir!',
        cancelButtonText: 'Cancelar',
      }).then(result => {
        if (result.value) {
          api.put(`users/${id}`, {
            user_level: null,
            company_id: null,
          });

          setUsers(users.filter(user => user.id !== id));
          toast.success('Usuário excluído da empresa com sucesso');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleEdit({ user_level, id }) {
    try {
      await api.put(`users/${id}`, {
        user_level,
      });

      toast.success('Nível de usuário atualizado com sucesso');
      setShowEdit(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ user_level, email }) {
    try {
      const response = await api.put(`company/users/${email}`, {
        user_level,
        company_id: profile.company_id,
      });

      setUsers([...users, response.data]);

      toast.success('Usuário adicionado com sucesso');
      setShowAdd(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  function handleUserLevel(user_level) {
    if (user_level === 1) {
      return 'Administrador';
    }
    if (user_level === 2) {
      return 'Gerenciador';
    }
    return 'Suporte';
  }

  return (
    <>
      <Container>
        <Top
          title="Usuários"
          subtitle="Veja as informações dos usuários e mude seu nível de permissão"
        />

        <Search>
          <div>
            <input
              type="text"
              placeholder="PESQUISAR"
              autoComplete="off"
              onKeyDown={event => event.key === 'Enter' && loadUsers()}
              onChange={e => setSearch(e.target.value)}
            />
            <button type="button" onClick={() => loadUsers()}>
              <MdSearch size={28} />
            </button>
          </div>
          {profile.user_level === 1 ? (
            <button type="button">
              <MdAddCircleOutline size={28} onClick={() => setShowAdd(true)} />
            </button>
          ) : (
            ''
          )}
        </Search>

        {loading ? (
          <Loading />
        ) : (
          <>
            {userSize ? (
              <ul>
                {users.map(user => (
                  <li key={user.id}>
                    <section>
                      <img
                        src={(user.avatar && user.avatar.url) || avatarProfile}
                        alt={user.name}
                      />
                      <button
                        type="button"
                        onClick={() => handleShowInfo(user)}
                      >
                        <strong>{user.name.split(' ', 1)}</strong>
                        <span>{handleUserLevel(user.user_level)}</span>
                      </button>
                    </section>

                    <div>
                      {profile.user_level === 1 ? (
                        <>
                          <button
                            type="button"
                            onClick={() => handleShowEdit(user)}
                          >
                            <MdCached size={22} color={colors.primary} />
                          </button>
                          <button
                            type="button"
                            onClick={() => handleDelete(user.id)}
                          >
                            <MdDeleteForever size={22} color={colors.red} />
                          </button>
                        </>
                      ) : (
                        ''
                      )}
                    </div>
                  </li>
                ))}
              </ul>
            ) : (
              <aside>
                <MdSupervisorAccount size={100} color={colors.primary} />
                <h6>Nenhum usuário foi encontrado</h6>
              </aside>
            )}
          </>
        )}
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
              src={(edit.avatar && edit.avatar.url) || avatarProfile}
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
          <h4>Adicionar usuário</h4>
          <button type="button" onClick={() => setShowAdd(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form onSubmit={handleAdd}>
            <Input name="email" placeholder="Email" type="email" required />
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
