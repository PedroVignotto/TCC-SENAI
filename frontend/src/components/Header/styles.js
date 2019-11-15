import styled from 'styled-components';

import colors from '~/styles/colors';

export const Container = styled.div`
  background: #fff;
  padding: 0 30px;

  img {
    margin-top: 8px;
    width: 200px;
    height: 65px;
  }
`;

export const Content = styled.div`
  height: 80px;
  display: flex;
  justify-content: space-between;
  align-items: center;

  nav {
    display: flex;
    align-items: center;

    a {
      font-weight: 500;
      font-size: 18px;
      color: ${colors.secondary};
      margin: 0 16px;

      :hover {
        color: ${colors.primary};
      }

      :after {
        background: ${colors.primary};
        content: '';
        display: block;
        height: 3px;
        width: 20px;
        transition: 0.5s;
      }

      :hover::after {
        width: 30px;
      }
    }
  }
`;

export const Profile = styled.div`
  display: flex;

  a {
    display: flex;
    flex-direction: row;
    align-items: center;
    justify-content: center;
  }

  strong {
    font-weight: 500;
    font-size: 18px;
    color: ${colors.secondary};
    margin-right: 8px;
  }

  img {
    width: 40px;
    height: 40px;
    border-radius: 50%;
    margin-top: -5px;
  }
`;
