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
}

export default new EnvironmentController();
