import * as Yup from 'yup';

import Heritage from '../models/Heritage';

class HeritageController {
  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
      description: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { code } = req.body;

    const codeExists = await Heritage.findOne({ where: { code } });

    if (codeExists) {
      return res.status(400).json({ error: 'Code already exist' });
    }

    const {
      id,
      name,
      description,
      company_id,
      environment_id,
    } = await Heritage.create(req.body);

    return res.json({ id, name, description, company_id, environment_id });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
      description: Yup.string(),
      code: Yup.string(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { id } = req.params;
    const { name, code, description } = req.body;

    const heritage = await Heritage.findByPk(id);

    if (code && code !== heritage.code) {
      const codeExists = await Heritage.findOne({ where: { code } });

      if (codeExists) {
        return res.status(400).json({ error: 'Code already exist' });
      }
    }

    await heritage.update(req.body);

    return res.json({ id, name, description, code });
  }
}

export default new HeritageController();
