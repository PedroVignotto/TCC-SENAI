import React, { useState, useEffect, useMemo } from 'react';
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
  MdCheckCircle,
} from 'react-icons/md';
import { FaTools, FaBoxOpen } from 'react-icons/fa';

import api from '~/services/api';
import Top from '~/components/Top';
import Loading from '~/components/Loading';
import Input from '~/components/Input';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

const schema = Yup.object().shape({
  name: Yup.string().required('Nome é obrigatório'),
  description: Yup.string(),
  code: Yup.string().required('Código é obrigatório'),
  environment_name: Yup.string(),
});

const schemaProblem = Yup.object().shape({
  id: Yup.string().required(),
  name: Yup.string().required('Nome é obrigatório'),
  description: Yup.string(),
  code: Yup.string().required('Código é obrigatório'),
  environment_name: Yup.string(),
  problem: Yup.string().required('Descrição do problema é obrigatório'),
});

export default function Heritage() {
  const [heritages, setHeritages] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showInfo, setShowInfo] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showMaintenance, setShowMaintenance] = useState(false);
  const [showAdd, setShowAdd] = useState(false);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(false);

  const profile = useSelector(state => state.user.profile);

  function handleShowEdit(heritage) {
    setEdit(heritage);
    setShowEdit(true);
  }

  function handleShowInfo(heritage) {
    setEdit(heritage);
    setShowInfo(true);
  }

  function handleShowMaintenance(heritage) {
    setEdit(heritage);
    setShowMaintenance(true);
  }

  async function loadHeritages() {
    setLoading(true);
    const response = await api.get(`${profile.company_id}/heritages`, {
      params: { q: search },
    });

    setHeritages(response.data);
    setLoading(false);
  }

  useEffect(() => {
    loadHeritages();
  }, []); //eslint-disable-line

  const heritageSize = useMemo(() => heritages.length, [heritages]);

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
          api.delete(`heritages/${id}`);

          setHeritages(heritages.filter(heritage => heritage.id !== id));
          toast.success('Patrimônio excluído com sucesso');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleMaintenance({ id, code, problem }) {
    try {
      await api.post(`maintenance/${profile.company_id}`, {
        id,
        code,
        problem,
      });

      toast.success('A informação do chamado foi enviado para seu email');
      setShowMaintenance(false);
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleEdit({ id, name, description, ...rest }) {
    try {
      if (rest.environment.name) {
        const response = await api.get(
          `${profile.company_id}/environments/${rest.environment.name}`
        );
        await api.put(`heritages/${id}`, {
          name,
          description,
          environment_id: response.data.id,
        });

        toast.success('Patrimônio atualizado com sucesso');
        setShowEdit(false);
      } else {
        await api.put(`heritages/${id}`, {
          name,
          description,
        });

        toast.success('Patrimônio atualizado com sucesso');
        setShowEdit(false);
      }
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ environment_name, name, description, code }) {
    try {
      if (environment_name) {
        const select = await api.get(
          `${profile.company_id}/environments/${environment_name}`
        );

        const response = await api.post('heritages', {
          name,
          description,
          code,
          company_id: profile.company_id,
          environment_id: select.data.id,
        });

        setHeritages([...heritages, response.data]);
        toast.success('Patrimônio adicionado com sucesso');
        setShowAdd(false);
      } else {
        const response = await api.post('heritages', {
          name,
          description,
          code,
          company_id: profile.company_id,
        });

        setHeritages([...heritages, response.data]);
        toast.success('Patrimônio adicionado com sucesso');
        setShowAdd(false);
      }
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  return (
    <>
      <Container>
        <Top title="Patrimônios" subtitle="Gerenciamento de patrimônios" />

        <Search>
          <div>
            <input
              type="text"
              placeholder="PESQUISAR"
              autoComplete="off"
              onKeyDown={event => event.key === 'Enter' && loadHeritages()}
              onChange={e => setSearch(e.target.value)}
            />
            <button type="button" onClick={() => loadHeritages()}>
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
            {heritageSize ? (
              <ul>
                {heritages.map(heritage => (
                  <li key={heritage.id}>
                    <button
                      type="button"
                      onClick={() => handleShowInfo(heritage)}
                    >
                      <MdCheckCircle
                        size={20}
                        color={heritage.state ? colors.green : colors.details}
                      />
                      <div>
                        <strong>{heritage.code}</strong>
                        <span>
                          {(heritage.environment &&
                            heritage.environment.name) ||
                            ''}
                        </span>
                      </div>
                    </button>

                    <div>
                      {profile.user_level === 1 ? (
                        <>
                          <button
                            type="button"
                            onClick={() => handleShowMaintenance(heritage)}
                          >
                            <FaTools size={18} color={colors.secondary} />
                          </button>
                          <button
                            type="button"
                            onClick={() => handleShowEdit(heritage)}
                          >
                            <MdCached size={20} color={colors.primary} />
                          </button>
                          <button
                            type="button"
                            onClick={() => handleDelete(heritage.id)}
                          >
                            <MdDeleteForever size={20} color={colors.red} />
                          </button>
                        </>
                      ) : (
                        <>
                          <button
                            type="button"
                            onClick={() => handleShowMaintenance(heritage)}
                          >
                            <FaTools size={20} color={colors.secondary} />
                          </button>
                        </>
                      )}
                    </div>
                  </li>
                ))}
              </ul>
            ) : (
              <aside>
                <FaBoxOpen size={100} color={colors.primary} />
                <h6>Nenhum patrimônio foi encontrado</h6>
              </aside>
            )}
          </>
        )}
      </Container>

      <Modals show={showInfo} onHide={() => setShowInfo(false)} animation>
        <Modals.Header>
          <h4>Informações do patrimônio</h4>
          <button type="button" onClick={() => setShowInfo(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form initialData={edit}>
            <Input name="id" type="hidden" />
            <Input label="Código" name="code" readOnly />
            <Input label="Nome" name="name" readOnly />
            <Input label="Descrição" name="description" readOnly multiline />
            <Input label="Ambiente" name="environment.name" readOnly />
          </Form>
        </Modals.Body>
      </Modals>

      <Modals
        show={showMaintenance}
        onHide={() => setShowMaintenance(false)}
        animation
      >
        <Modals.Header>
          <h4>Abrir chamado de manutenção</h4>
          <button type="button" onClick={() => setShowMaintenance(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form
            initialData={edit}
            onSubmit={handleMaintenance}
            schema={schemaProblem}
          >
            <Input name="id" type="hidden" />
            <Input label="Código" name="code" readOnly />
            <Input label="Nome" name="name" readOnly />
            <Input label="Descrição" name="description" multiline readOnly />
            <Input label="Ambiente" name="environment.name" readOnly />
            <Input label="Descrição do problema *" name="problem" multiline />
            <button type="submit">
              <MdSave size={22} />
              Salvar
            </button>
          </Form>
        </Modals.Body>
      </Modals>

      <Modals show={showEdit} onHide={() => setShowEdit(false)} animation>
        <Modals.Header>
          <h4>Editar patrimônio</h4>
          <button type="button" onClick={() => setShowEdit(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form initialData={edit} onSubmit={handleEdit}>
            <Input name="id" type="hidden" />
            <Input label="Código" name="code" readOnly />
            <Input label="Nome" name="name" />
            <Input label="Descrição" name="description" multiline />
            <Input label="Ambiente" name="environment.name" />
            <button type="submit">
              <MdSave size={22} />
              Salvar
            </button>
          </Form>
        </Modals.Body>
      </Modals>

      <Modals show={showAdd} onHide={() => setShowAdd(false)} animation>
        <Modals.Header>
          <h4>Adicionar patrimônio</h4>
          <button type="button" onClick={() => setShowAdd(false)}>
            x
          </button>
        </Modals.Header>
        <Modals.Body>
          <Form onSubmit={handleAdd} schema={schema}>
            <Input label="Código *" name="code" autoComplete="off" />
            <Input label="Nome *" name="name" />
            <Input label="Descrição" name="description" multiline />
            <Input label="Ambiente" name="environment_name" />
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
