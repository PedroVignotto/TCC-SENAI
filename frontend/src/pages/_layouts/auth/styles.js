import styled from 'styled-components';
import { darken } from 'polished';

import colors from '~/styles/colors';

export const Wrapper = styled.div`
  height: 100%;
  background: none;
  display: flex;
  justify-content: center;
  align-items: center;
`;

export const Content = styled.div`
  width: 100%;
  max-width: 315px;
  text-align: center;

  img {
    width: 150px;
  }

  form {
    display: flex;
    flex-direction: column;
    text-align: start;
    margin-top: 30px;

    button {
      margin: 5px 0 0;
      height: 44px;
      background: ${colors.primary};
      color: ${colors.white};
      border: 0;
      border-radius: 4px;
      font-size: 16px;
      transition: background 0.2s;

      &:hover {
        background: ${darken(0.05, colors.primary)};
      }
    }

    a {
      align-self: center;
      color: ${darken(0.03, colors.secondary)};
      margin-top: 15px;
      font-size: 16px;
      opacity: 0.8;

      &:hover {
        opacity: 1;
      }
    }
  }
`;
