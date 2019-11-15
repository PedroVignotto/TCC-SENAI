import React from 'react';
import PropTypes from 'prop-types';

import { Title, SubTitle } from './styles';

export default function Top({ title, subtitle }) {
  return (
    <>
      <Title>{title}</Title>
      <SubTitle>{subtitle}</SubTitle>
    </>
  );
}

Top.propTypes = {
  title: PropTypes.string.isRequired,
  subtitle: PropTypes.string.isRequired,
};
