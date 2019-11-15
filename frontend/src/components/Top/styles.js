import styled from 'styled-components';

import colors from '~/styles/colors';

export const Title = styled.h1`
  margin-top: 50px;
  font-weight: 500;
  color: ${colors.primary};
  text-transform: uppercase;
  text-align: center;
`;

export const SubTitle = styled.h4`
  font-weight: 500;
  color: ${colors.secondary};
  line-height: 30px;
  text-transform: uppercase;
  text-align: center;
`;
