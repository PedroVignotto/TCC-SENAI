import * as Yup from 'yup';
import { Op } from 'sequelize';
import User from '../models/User';
import File from '../models/File';

class UserController {
  async index(req, res) {
    const { company_id } = req.params;
    const { q = null } = req.query;

    const where = q
      ? { company_id, name: { [Op.like]: `%${q}%` } }
      : { company_id };

    const heritage = await User.findAll({
      where,
      attributes: ['id', 'name', 'email', 'user_level', 'company_id'],
      order: ['name'],
      include: [
        {
          model: File,
          as: 'avatar',
          attributes: ['id', 'path', 'url'],
        },
      ],
    });

    return res.json(heritage);
  }

  async show(req, res) {
    const { company_id, email } = req.params;

    const user = await User.findOne({
      where: { company_id, email },
      attributes: ['id', 'name', 'email', 'user_level'],
    });

    if (!user) {
      return res.status(400).json({ error: 'Email não foi encontrado' });
    }

    const { user_level } = user;

    if (user_level !== 2) {
      return res
        .status(400)
        .json({ error: 'Por favor insira um gerenciador válido' });
    }

    return res.json(user);
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
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

    const userExists = await User.findOne({ where: { email: req.body.email } });

    if (userExists) {
      return res.status(400).json({ error: 'Usuário já existe' });
    }

    const { id, name, email } = await User.create(req.body);

    return res.json({ id, name, email });
  }

  async findAndUpdate(req, res) {
    const { email } = req.params;

    const user = await User.findOne({
      where: { email, company_id: null },
      attributes: ['id', 'name', 'email', 'user_level', 'company_id'],
    });

    if (!user) {
      return res.status(400).json({
        error: 'Email não encontrado ou o usuário pertence a outra empresa',
      });
    }

    await user.update(req.body);

    return res.json(user);
  }

  async edit(req, res) {
    const { id } = req.params;

    const user = await User.findByPk(id);

    if (!user) {
      return res.status(400).json({ error: 'Usuário não existe' });
    }

    const { user_level } = await user.update(req.body);

    return res.json({ user_level });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
      email: Yup.string().email(),
      oldPassword: Yup.string(),
      password: Yup.string().when('oldPassword', (oldPassword, field) =>
        oldPassword ? field.required() : field
      ),
      confirmPassword: Yup.string().when('password', (password, field) =>
        password ? field.required().oneOf([Yup.ref('password')]) : field
      ),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { email, oldPassword } = req.body;

    const user = await User.findByPk(req.userId);

    if (email && email !== user.email) {
      const userExists = await User.findOne({ where: { email } });

      if (userExists) {
        return res.status(400).json({ error: 'Usuário já existe' });
      }
    }

    if (oldPassword && !(await user.checkPassword(oldPassword))) {
      return res.status(401).json({ error: 'Senha inválida' });
    }

    await user.update(req.body);

    const { id, name, avatar, company_id, user_level } = await User.findByPk(
      req.userId,
      {
        include: [
          {
            model: File,
            as: 'avatar',
            attributes: ['id', 'path', 'url'],
          },
        ],
      }
    );

    return res.json({ id, name, email, avatar, company_id, user_level });
  }
}

export default new UserController();
