import styled from 'styled-components';
import colors from '~/styles/colors';

export const Container = styled.div`
  width: 100%;
  max-width: 1200px;
  margin: 20px auto;
  padding: 48px;

  section {
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-gap: 24px;

    @media (max-width: 824px) {
      grid-template-columns: repeat(1, 1fr);
    }

    a {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      text-align: center;
      height: 300px;
      width: 100%;
      border: 1px solid ${colors.details};
      border-bottom: 5px solid ${colors.primary};
      border-radius: 8px;
      margin: 0 auto;
      text-transform: uppercase;

      strong {
        font-weight: 500px;
        color: ${colors.primary};
        font-size: 26px;
        margin-top: 32px;
        margin-bottom: 8px;
      }

      span {
        font-weight: 500px;
        color: ${colors.secondary};
        font-size: 18px;
      }

      img {
        width: 300px;
      }

      :hover {
        border: 3px solid ${colors.primary};
        border-bottom: 5px solid ${colors.primary};
        transition: 500ms;
        -webkit-transform: scale(1.02);
        -ms-transform: scale(1.02);
        transform: scale(1.02);
      }
    }
  }
`;
