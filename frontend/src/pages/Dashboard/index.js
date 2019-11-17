import React from 'react';

import heritage from '~/assets/heritage.png';

import { Container } from './styles';

export default function Dashboard() {
  return (
    <Container>
      <img src={heritage} alt="Heritage" />
    </Container>
  );
}
