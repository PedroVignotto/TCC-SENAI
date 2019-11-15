import React from 'react';
import PropTypes from 'prop-types';
import { MdSearch, MdAddCircleOutline } from 'react-icons/md';

import { Title, SubTitle, Search } from './styles';

export default function Top({ title, subtitle }) {
  return (
    <>
      <Title>{title}</Title>
      <SubTitle>{subtitle}</SubTitle>
      <Search>
        <div>
          <input type="text" placeholder="PESQUISAR" />
          <button type="button">
            <MdSearch size={28} />
          </button>
        </div>
        <button type="button">
          <MdAddCircleOutline size={28} />
        </button>
      </Search>
    </>
  );
}

Top.propTypes = {
  title: PropTypes.string.isRequired,
  subtitle: PropTypes.string.isRequired,
};
