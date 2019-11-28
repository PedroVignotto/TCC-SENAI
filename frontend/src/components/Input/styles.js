import styled from 'styled-components';
import colors from '~/styles/colors';

export const Label = styled.label`
  display: flex;
  flex-direction: column-reverse;
  position: relative;
  margin-bottom: 16px;

  > span:first-child {
    color: ${colors.red};
    font-size: 12px;
    font-weight: 500;
    line-height: 1.3;
    width: 100%;
    margin: 8px 0 4px;
    border-radius: 4px;
    transform: none;
    animation: fadeIn 350ms ease-in-out 1;

    @keyframes fadeIn {
      from {
        transform: translateY(-20px);
        opacity: 0;
      }
      to {
        transform: translateY(0);
        opacity: 1;
      }
    }

    + input {
      border-color: ${colors.red};
    }
  }

  > input,
  textarea {
    border: 1px solid ${colors.secondary};
    border-radius: 4px;
    height: 40px;
    color: ${colors.secondary};
    background: none;
    padding: 0 15px;
    transition: 180ms ease-in-out;

    :focus {
      box-shadow: none;
    }

    + span {
      color: ${colors.secondary};
      font-weight: 500;
      margin-bottom: 8px;
      text-transform: uppercase;
    }
  }

  > textarea {
    resize: none;
    width: 100%;
    height: 80px;
    padding: 8px 16px;
  }
`;
