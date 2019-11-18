import * as Yup from 'yup';

import Environment from '../models/Environment';
import User from '../models/User';

class EnvironmentController {
  async index(req, res) {
    const { company_id } = req.params;

    const environment = await Environment.findAll({
      where: { company_id },
      attributes: ['id', 'name', 'user_id', 'company_id'],
      order: ['name'],
      include: [
        {
          model: User,
          as: 'user',
          attributes: ['id', 'email'],
        },
      ],
    });

    return res.json(environment);
  }

  async show(req, res) {
    const environment = await Environment.findAll({
      where: { user_id: req.userId },
      attributes: ['id', 'name', 'user_id', 'company_id'],
      order: ['name'],
      include: [
        {
          model: User,
          as: 'user',
          attributes: ['id', 'email'],
        },
      ],
    });

    return res.json(environment);
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { user_id, name } = req.body;

    if (user_id) {
      const userExists = await User.findByPk(user_id);

      if (!userExists) {
        return res.status(400).json({ error: 'User does not exist' });
      }
    }

    const nameExist = await Environment.findOne({ where: { name } });

    if (nameExist) {
      return res.status(400).json({ error: 'Environment already exist' });
    }

    const { id, company_id } = await Environment.create(req.body);

    return res.json({ id, name, company_id, user_id });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { id } = req.params;
    const { name } = req.body;

    const environment = await Environment.findByPk(id);

    if (name && name !== environment.name) {
      const nameExists = await Environment.findOne({ where: { name } });

      if (nameExists) {
        return res.status(400).json({ error: 'Name not available' });
      }
    }

    await environment.update(req.body);

    return res.json({ id, name });
  }

  async delete(req, res) {
    const { id } = req.params;

    const environment = await Environment.findByPk(id);

    if (!environment) {
      return res.status(400).json({ error: 'Environment does not exist' });
    }

    await Environment.destroy({ where: { id } });

    return res.status(200).json({ success: 'Environment has been deleted' });
  }
}

export default new EnvironmentController();
