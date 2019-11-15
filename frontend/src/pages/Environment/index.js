import React, { useState, useEffect } from 'react';
import { useSelector } from 'react-redux';
import { MdDelete, MdCached } from 'react-icons/md';
import { Link } from 'react-router-dom';
import { toast } from 'react-toastify';
import Swal from 'sweetalert2';

import api from '~/services/api';
import Top from '~/components/Top';

import { Container } from './styles';

export default function Environment() {
  const [environments, setEnvironments] = useState([]);

  const profile = useSelector(state => state.user.profile);

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
        confirmButtonColor: '#42cb59',
        cancelButtonColor: '#ee4d64',
        confirmButtonText: 'Yes, delete it!',
      }).then(result => {
        if (result.value) {
          api.delete(`environments/${id}`);
          toast.success('Environment successfully deleted');
        }
      });
    } catch (err) {
      toast.error('Something went wrong try again');
    }
  }

  return (
    <Container>
      <Top title="Ambientes" subtitle="Gerenciamento de ambientes" />

      <ul>
        {environments.map(environment => (
          <li>
            <Link to="dashboard">
              <strong>{environment.name}</strong>
              <span>{environment.user_id}</span>
            </Link>

            <div>
              <button type="button">
                <MdCached size={22} />
              </button>
              <button
                type="button"
                onClick={() => handleDelete(environment.id)}
              >
                <MdDelete size={22} />
              </button>
            </div>
          </li>
        ))}
      </ul>
    </Container>
  );
}
