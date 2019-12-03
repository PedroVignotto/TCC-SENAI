import React from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Form } from '@rocketseat/unform';
import { MdExitToApp, MdCached } from 'react-icons/md';
import * as Yup from 'yup';

import { signOut } from '~/store/modules/auth/actions';
import { updateProfileRequest } from '~/store/modules/user/actions';

import Input from '~/components/Input';
import AvatarInput from './AvatarInput';

import { Container } from './styles';

const schema = Yup.object().shape({
  avatar_id: Yup.number(),
  name: Yup.string().required('O nome é obrigatório'),
  email: Yup.string()
    .email('Insira um e-mail válido')
    .required('O e-mail é obrigatório'),
  oldPassword: Yup.string(),
  password: Yup.string().when('oldPassword', (oldPassword, field) =>
    oldPassword ? field.required('A nova senha é obrigatória') : field
  ),
  confirmPassword: Yup.string().when('password', (password, field) =>
    password
      ? field
          .required('A confirmação da nova senha é obrigatória')
          .oneOf([Yup.ref('password')], 'Senha não corresponde')
      : field
  ),
});

export default function Profile() {
  const dispatch = useDispatch();
  const profile = useSelector(state => state.user.profile);

  function handleSubmit(data) {
    dispatch(updateProfileRequest(data));
  }

  function handleSignOut() {
    dispatch(signOut());
  }

  return (
    <Container>
      <Form initialData={profile} onSubmit={handleSubmit} schema={schema}>
        <AvatarInput name="avatar_id" />
        <Input name="name" placeholder="Nome completo" />
        <Input name="email" type="email" placeholder="Seu endereço de e-mail" />

        <hr />

        <Input name="oldPassword" type="password" placeholder="Senha atual" />
        <Input name="password" type="password" placeholder="Nova senha" />
        <Input
          name="confirmPassword"
          type="password"
          placeholder="Confirmar senha"
        />

        <button type="submit">
          <MdCached size={22} />
          Atualizar perfil
        </button>
      </Form>

      <button type="button" onClick={handleSignOut}>
        <MdExitToApp size={22} />
        Sair
      </button>
    </Container>
  );
}
