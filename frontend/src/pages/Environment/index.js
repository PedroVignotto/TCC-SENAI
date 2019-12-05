import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { toast } from 'react-toastify';
import Swal from 'sweetalert2';
import { Form } from '@rocketseat/unform';
import * as Yup from 'yup';
import {
  MdDeleteForever,
  MdCached,
  MdSave,
  MdSearch,
  MdAddCircleOutline,
  MdRoom,
} from 'react-icons/md';

import api from '~/services/api';
import history from '~/services/history';
import Top from '~/components/Top';
import Loading from '~/components/Loading';
import Input from '~/components/Input';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

const schema = Yup.object().shape({
  email: Yup.string().email('Endereço de email inválido'),
  name: Yup.string().required('Nome é obrigatório'),
});

export default function Environment() {
  const [environments, setEnvironments] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(false);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(false);

  const profile = useSelector(state => state.user.profile);

  function handleShowEdit(environment) {
    setEdit(environment);
    setShowEdit(true);
  }

  async function loadEnvironments() {
    if (profile.user_level === 1) {
      setLoading(true);
      const response = await api.get(`${profile.company_id}/environments`, {
        params: { q: search },
      });

      setEnvironments(response.data);
      setLoading(false);
    } else {
      setLoading(true);
      const response = await api.get('/environments', {
        params: { q: search },
      });

      setEnvironments(response.data);
      setLoading(false);
    }
  }

  useEffect(() => {
    loadEnvironments();
  }, []); //eslint-disable-line

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
          api.delete(`environments/${id}`);

          setEnvironments(
            environments.filter(environment => environment.id !== id)
          );
          toast.success('Ambiente excluído com sucesso');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleEdit({ id, name, ...rest }) {
    const { email } = rest.user;

    try {
      if (email) {
        const response = await api.get(
          `${profile.company_id}/managers/${email}`
        );

        await api.put(`environments/${id}`, {
          name,
          user_id: response.data.id,
        });

        toast.success('Ambiente atualizado com sucesso');
        setShowEdit(false);
      } else {
        await api.put(`environments/${id}`, {
          name,
        });

        toast.success('Ambiente atualizado com sucesso');
        setShowEdit(false);
      }
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ name, email }) {
    try {
      if (email) {
        const select = await api.get(`${profile.company_id}/managers/${email}`);

        const response = await api.post('environments', {
          name,
          user_id: select.data.id,
          company_id: profile.company_id,
        });

        setEnvironments([...environments, response.data]);

        toast.success('Ambiente adicionado com sucesso');
        setShowAdd(false);
      } else {
        const response = await api.post('environments', {
          name,
          company_id: profile.company_id,
        });

        setEnvironments([...environments, response.data]);

        toast.success('Ambiente adicionado com sucesso');
        setShowAdd(false);
      }
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
            <input
              type="text"
              placeholder="PESQUISAR"
              autoComplete="off"
              onKeyDown={event => event.key === 'Enter' && loadEnvironments()}
              onChange={e => setSearch(e.target.value)}
            />
            <button type="button" onClick={() => loadEnvironments()}>
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
            {environments.length ? (
              <ul>
                {environments.map(environment => (
                  <li key={environment.id}>
                    <button
                      type="button"
                      onClick={() =>
                        history.push(`/${environment.id}/heritages`)
                      }
                    >
                      <strong>{environment.name}</strong>
                      <span>
                        {(environment.user && environment.user.email) || ''}
                      </span>
                    </button>

                    <div>
                      {profile.user_level === 1 ? (
                        <>
                          {' '}
                          <button
                            type="button"
                            onClick={() => handleShowEdit(environment)}
                          >
                            <MdCached size={22} color={colors.primary} />
                          </button>
                          <button
                            type="button"
                            onClick={() => handleDelete(environment.id)}
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
                <MdRoom size={100} color={colors.primary} />
                <h6>Nenhum ambiente foi encontrado</h6>
              </aside>
            )}
          </>
        )}
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
            <Input label="Nome do ambiente" name="name" />
            <Input
              label="Email do gerenciador"
              type="email"
              name="user.email"
            />
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
          <Form onSubmit={handleAdd} schema={schema}>
            <Input label="Nome do ambiente *" name="name" />
            <Input label="Email do gerenciador" type="email" name="email" />
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
