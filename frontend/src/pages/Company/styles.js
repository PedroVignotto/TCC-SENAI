import styled from 'styled-components';
import { darken } from 'polished';

import colors from '~/styles/colors';

export const Container = styled.div`
  max-width: 600px;
  margin: 50px auto;
  padding: 0 30px;

  form {
    display: flex;
    flex-direction: column;
    margin-top: 30px;

    input {
      background: none;
      border: 1px solid ${colors.secondary};
      border-radius: 4px;
      height: 44px;
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
      height: 44px;
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
