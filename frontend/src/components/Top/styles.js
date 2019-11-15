import styled from 'styled-components';

import colors from '~/styles/colors';

export const Title = styled.h1`
  margin-top: 50px;
  font-weight: 500;
  color: ${colors.primary};
  text-transform: uppercase;
  text-align: center;
`;

export const SubTitle = styled.h4`
  font-weight: 500;
  color: ${colors.secondary};
  line-height: 30px;
  text-transform: uppercase;
  text-align: center;
`;

export const Search = styled.section`
  display: flex;
  align-items: center;
  padding: 32px 16px;

  div {
    display: flex;
    align-items: center;
    justify-content: center;
    margin: 0 auto;

    input {
      background-color: transparent;
      color: ${colors.secondary};
      padding: 6px 10px;
      width: 200px;
      border: none;
      font-size: 1em;
      font-weight: bold;
      border-bottom: ${colors.details} solid 2px;
    }
  }

  button {
    border: 0;
    background: 0;
  }

  svg {
    color: ${colors.primary};
  }
`;
