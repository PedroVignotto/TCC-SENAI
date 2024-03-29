import styled from 'styled-components';
import { darken } from 'polished';
import { Modal } from 'react-bootstrap';

import colors from '~/styles/colors';

export const Container = styled.div`
  width: 100%;
  max-width: 1024px;
  margin: 0 auto;
  padding: 0 30px;

  aside {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    margin: 100px auto;

    h6 {
      font-size: 28px;
      font-weight: 500;
      color: ${colors.secondary};
      margin-top: 16px;

      @media (max-width: 650px) {
        font-size: 24px;
      }

      @media (max-width: 450px) {
        font-size: 16px;
      }
    }
  }

  ul {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    margin-top: 30px;

    @media (max-width: 950px) {
      grid-template-columns: repeat(2, 1fr);
    }

    @media (max-width: 630px) {
      grid-template-columns: repeat(1, 1fr);
    }

    li {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
      height: 100px;
      color: #333;
      border: 1px solid ${colors.details};

      button {
        display: flex;
        justify-content: flex-start;
        align-items: center;
        padding: 0 0 0 15px;
        border: 0;
        background: 0;

        div {
          display: flex;
          flex-direction: column;
          align-items: flex-start;
          justify-content: center;

          strong {
            font-weight: 500;
            font-size: 18px;
            color: ${colors.primary};
            margin-bottom: 10px;
          }

          span {
            font-weight: 500;
            font-size: 16px;
            color: ${colors.secondary};
          }
        }
      }

      div {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding: 0 15px;

        button {
          border: 0;
          background: 0;
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

export const Modals = styled(Modal)`
  div.modal-header {
    display: flex;
    align-items: center;

    h4 {
      font-weight: 500;
      color: ${colors.primary};
      text-transform: uppercase;
    }

    button {
      border: 0;
      background: none;
      font-weight: bold;
      font-size: 24px;
      color: ${colors.primary};
    }
  }

  form {
    padding: 0 32px;

    button {
      display: flex;
      align-items: center;
      justify-content: center;
      margin: 5px 0 0;
      width: 100%;
      height: 40px;
      background: ${colors.primary};
      color: #fff;
      border: 0;
      border-radius: 4px;
      font-size: 16px;
      transition: background 0.2s;

      svg {
        margin-right: 5px;
      }

      &:hover {
        background: ${darken(0.05, colors.primary)};
      }
    }
  }
`;
