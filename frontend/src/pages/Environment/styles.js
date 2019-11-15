import styled from 'styled-components';

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
