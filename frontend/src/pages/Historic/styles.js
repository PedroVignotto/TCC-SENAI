import styled from 'styled-components';

import colors from '~/styles/colors';

export const Container = styled.div`
  width: 100%;
  max-width: 1024px;
  margin: 0 auto;
  padding: 0 30px;

  aside {
    display: flex;
    flex-direction: row;
    align-items: baseline;
    justify-content: flex-start;
    padding: 0 20px;

    @media (max-width: 750px) {
      display: flex;
      flex-direction: column;
      border-bottom: 1px solid ${colors.details};
      padding: 8px;
    }

    span {
      color: ${colors.secondary};
      font-size: 18px;
      line-height: 26px;
      padding: 4px 0;

      @media (max-width: 1366px) {
        font-size: 16px;
      }

      :first-of-type {
        color: ${colors.primary};
        font-weight: bold;
        text-transform: capitalize;
      }

      @media (min-width: 750px) {
        :last-of-type {
          padding-left: 10px;
        }
      }
    }
  }
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
