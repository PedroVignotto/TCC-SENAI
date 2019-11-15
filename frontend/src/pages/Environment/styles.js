import styled from 'styled-components';
import { darken } from 'polished';
import { Modal } from 'react-bootstrap';

import colors from '~/styles/colors';

export const Container = styled.div`
  width: 100%;
  max-width: 1024px;
  margin: 0 auto;

  ul {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    grid-gap: 15px;
    margin-top: 30px;

    li {
      display: flex;
      align-items: center;
      justify-content: space-between;
      width: 100%;
      height: 100px;
      color: #333;
      border: 1px solid ${colors.details};

      a {
        display: flex;
        flex-direction: column;
        padding: 0 15px;

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

      div {
        display: flex;
        flex-direction: column;
        justify-content: space-between;
        padding: 0 15px;

        button {
          border: 0;
          background: 0;
          color: ${colors.primary};

          + button {
            color: ${colors.red};
          }
        }
      }
    }
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
    padding: 16px 32px;

    input {
      background: none;
      border: 1px solid ${colors.secondary};
      border-radius: 4px;
      width: 100%;
      height: 32px;
      padding: 0 15px;
      color: ${colors.secondary};
      margin: 0 0 10px;

      &::placeholder {
        color: ${colors.secondary};
      }
    }

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
