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

  @media (min-width: 1024px) {
    button {
      display: none;
    }
  }

  @media (max-width: 1024px) {
    flex-direction: row-reverse;

    nav.header {
      display: ${props => (props.visible ? 'flex' : 'none')};
      flex-direction: column;
      align-items: center;
      justify-content: center;
      position: fixed;
      width: 100%;
      height: 100%;
      top: 0;
      left: 0;
      background: rgba(255, 255, 255, 0.9);

      a {
        font-size: 18px;
        line-height: 32px;

        :after {
          content: none;
        }
      }
    }

    img:first-child {
      display: none;
    }
  }

  nav {
    display: flex;
    align-items: center;

    a {
      font-weight: 500;
      font-size: 16px;
      color: ${colors.secondary};
      margin: 0 16px;
      white-space: nowrap;

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

export const Button = styled.button`
  border: 0;
  background: none;
  z-index: 2;
`;

export const Profile = styled.div`
  display: flex;
  margin-top: -5px;

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
  }
`;
