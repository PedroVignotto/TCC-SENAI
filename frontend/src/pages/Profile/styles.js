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

    hr {
      border: 0;
      height: 1px;
      background: ${colors.details};
      margin: 10px 0 20px;
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

  > button {
    display: flex;
    align-items: center;
    justify-content: center;
    width: 100%;
    margin: 10px 0 0;
    height: 44px;
    background: ${colors.red};
    color: #fff;
    border: 0;
    border-radius: 4px;
    font-size: 16px;
    transition: background 0.2s;

    svg {
      margin-right: 5px;
    }

    &:hover {
      background: ${darken(0.05, colors.red)};
    }
  }
`;
