import React from 'react';
import { FaSpinner } from 'react-icons/fa';

import colors from '~/styles/colors';
import { Container } from './styles';

export default function Loading() {
  return (
    <Container>
      <FaSpinner size={50} color={colors.primary} />
    </Container>
  );
}
