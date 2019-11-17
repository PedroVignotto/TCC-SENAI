import * as Yup from 'yup';
import User from '../models/User';
import File from '../models/File';

class UserController {
  async index(req, res) {
    const { company_id } = req.params;

    const heritage = await User.findAll({
      where: { company_id },
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
      return res.status(400).json({ error: 'Email not found' });
    }

    const { user_level } = user;

    if (user_level !== 2) {
      return res.status(400).json({ error: 'Please enter a valid manager' });
    }

    return res.json(user);
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
      email: Yup.string()
        .email()
        .required(),
      password: Yup.string()
        .required()
        .min(6),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const userExists = await User.findOne({ where: { email: req.body.email } });

    if (userExists) {
      return res.status(400).json({ error: 'User already exists' });
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
      return res
        .status(400)
        .json({ error: 'Email not found or user joins another company' });
    }

    await user.update(req.body);

    return res.json(user);
  }

  async edit(req, res) {
    const { id } = req.params;

    const user = await User.findByPk(id);

    if (!user) {
      return res.status(400).json({ error: 'User does not exist' });
    }

    const { user_level } = await user.update(req.body);

    return res.json({ user_level });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
      email: Yup.string().email(),
      oldPassword: Yup.string().min(6),
      password: Yup.string()
        .min(6)
        .when('oldPassword', (oldPassword, field) =>
          oldPassword ? field.required() : field
        ),
      confirmPassword: Yup.string().when('password', (password, field) =>
        password ? field.required().oneOf([Yup.ref('password')]) : field
      ),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { email, oldPassword } = req.body;

    const user = await User.findByPk(req.userId);

    if (email && email !== user.email) {
      const userExists = await User.findOne({ where: { email } });

      if (userExists) {
        return res.status(400).json({ error: 'User already exists' });
      }
    }

    if (oldPassword && !(await user.checkPassword(oldPassword))) {
      return res.status(401).json({ error: 'Password does not match' });
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

  async delete(req, res) {
    const { id } = req.params;

    const user = await User.findByPk(id);

    if (!user) {
      return res.status(400).json({ error: 'User does not exist' });
    }

    await User.destroy({ where: { id } });

    return res.status(200).json({ success: 'User has been deleted' });
  }
}

export default new UserController();
