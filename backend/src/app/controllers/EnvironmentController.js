import * as Yup from 'yup';

import Environment from '../models/Environment';
import User from '../models/User';

class EnvironmentController {
  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { user_id } = req.body;

    const userExists = await User.findByPk(user_id);

    if (!userExists) {
      return res.status(400).json({ error: 'User does not exist' });
    }

    const { id, name, company_id } = await Environment.create(req.body);

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
}

export default new EnvironmentController();
