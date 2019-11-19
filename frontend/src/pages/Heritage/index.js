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
  MdInfo,
} from 'react-icons/md';

import api from '~/services/api';
import Top from '~/components/Top';

import colors from '~/styles/colors';
import { Container, Modals, Search } from './styles';

export default function Heritage() {
  const [heritages, setHeritages] = useState([]);
  const [edit, setEdit] = useState([]);
  const [showInfo, setShowInfo] = useState(false);
  const [showEdit, setShowEdit] = useState(false);
  const [showAdd, setShowAdd] = useState(false);

  const profile = useSelector(state => state.user.profile);

  function handleShowEdit(heritage) {
    setEdit(heritage);
    setShowEdit(true);
  }

  function handleShowInfo(heritage) {
    setEdit(heritage);
    setShowInfo(true);
  }

  async function loadHeritages() {
    const response = await api.get(`${profile.company_id}/heritages`);

    setHeritages(response.data);
  }

  useEffect(() => {
    loadHeritages();
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
          api.delete(`heritages/${id}`);
          toast.success('Heritage successfully deleted');
        }
      });
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }
  async function handleEdit({ id, name, description, ...rest }) {
    console.tron.log(rest.environment.name);
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

        toast.success('Heritage updated successfully');
        setShowEdit(false);
      } else {
        await api.put(`heritages/${id}`, {
          name,
          description,
        });

        toast.success('Heritage updated successfully');
        setShowEdit(false);
      }
    } catch (err) {
      toast.error(err.response.data.error);
    }
  }

  async function handleAdd({ environment_name, name, description, code }) {
    try {
      if (environment_name) {
        const response = await api.get(
          `${profile.company_id}/environments/${environment_name}`
        );
        await api.post('heritages', {
          name,
          description,
          code,
          company_id: profile.company_id,
          environment_id: response.data.id,
        });
        toast.success('Heritage successfully added');
        setShowAdd(false);
      } else {
        await api.post('heritages', {
          name,
          description,
          code,
          company_id: profile.company_id,
        });
        toast.success('Heritage successfully added');
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
            <input type="text" placeholder="PESQUISAR" />
            <button type="button">
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

        <ul>
          {heritages.map(heritage => (
            <li key={heritage.id}>
              <Link to="dashboard">
                <strong>{heritage.code}</strong>
                <span>
                  {(heritage.environment && heritage.environment.name) || ''}
                </span>
              </Link>

              <div>
                {profile.user_level === 1 ? (
                  <>
                    <button
                      type="button"
                      onClick={() => handleShowInfo(heritage)}
                    >
                      <MdInfo size={22} />
                    </button>
                    <button
                      type="button"
                      onClick={() => handleShowEdit(heritage)}
                    >
                      <MdCached size={22} />
                    </button>
                    <button
                      type="button"
                      onClick={() => handleDelete(heritage.id)}
                    >
                      <MdDeleteForever size={22} />
                    </button>
                  </>
                ) : (
                  <button
                    type="button"
                    onClick={() => handleShowInfo(heritage)}
                  >
                    <MdInfo size={22} />
                  </button>
                )}
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
            <Input name="id" type="hidden" />
            <Input name="name" placeholder="Nome do patrimônio" readOnly />
            <Input
              name="description"
              placeholder="Descrição do patrimônio"
              readOnly
            />
            <Input
              name="environment.name"
              placeholder="Não possui ambiente"
              readOnly
            />
            <Input name="code" placeholder="Código do patrimônio" readOnly />
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
            <Input name="name" placeholder="Nome do patrimônio" />
            <Input name="description" placeholder="Descrição do patrimônio" />
            <Input name="environment.name" placeholder="Nome do ambiente" />
            <Input name="code" placeholder="Código do patrimônio" readOnly />
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
          <Form onSubmit={handleAdd}>
            <Input name="name" placeholder="Nome do patrimônio" />
            <Input name="description" placeholder="Descrição" />
            <Input name="code" placeholder="Código do patrimônio" />
            <Input name="environment_name" placeholder="Nome do ambiente" />
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
