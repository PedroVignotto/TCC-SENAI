import jwt from 'jsonwebtoken';
import * as Yup from 'yup';

import authconfig from '../../config/auth';
import User from '../models/User';
import File from '../models/File';

class SessionController {
  async store(req, res) {
    const schema = Yup.object().shape({
      email: Yup.string()
        .email()
        .required(),
      password: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { email, password } = req.body;

    const user = await User.findOne({
      where: { email },
      include: [
        {
          model: File,
          as: 'avatar',
          attributes: ['id', 'path', 'url'],
        },
      ],
    });

    if (!user) {
      return res.status(401).json({ error: 'Usuário não foi encontrado' });
    }

    if (!user.company_id) {
      return res
        .status(401)
        .json({ error: 'Entre em uma empresa para realizar o login' });
    }

    if (!(await user.checkPassword(password))) {
      return res.status(401).json({ error: 'Email ou senha inválidos' });
    }

    const { id, name, avatar, user_level, company_id } = user;

    return res.json({
      user: {
        id,
        name,
        email,
        user_level,
        company_id,
        avatar,
      },
      token: jwt.sign({ id }, authconfig.secret, {
        expiresIn: authconfig.expiresIn,
      }),
    });
  }
}

export default new SessionController();
