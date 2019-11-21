import * as Yup from 'yup';

import Heritage from '../models/Heritage';
import Environment from '../models/Environment';

class HeritageController {
  async index(req, res) {
    const { company_id } = req.params;

    const heritage = await Heritage.findAll({
      where: { company_id },
      attributes: [
        'id',
        'name',
        'description',
        'code',
        'company_id',
        'environment_id',
      ],
      order: ['name'],
      include: [
        {
          model: Environment,
          as: 'environment',
          attributes: ['id', 'name'],
        },
      ],
    });

    return res.json(heritage);
  }

  async list(req, res) {
    const { company_id, environment_id } = req.params;

    const heritage = await Heritage.findAll({
      where: { company_id, environment_id },
      attributes: [
        'id',
        'name',
        'description',
        'code',
        'company_id',
        'environment_id',
      ],
      include: [
        {
          model: Environment,
          as: 'environment',
          attributes: ['id', 'name'],
        },
      ],
    });

    if (!heritage) {
      return res.status(400).json({ error: 'Heritage does not exist' });
    }

    return res.json(heritage);
  }

  async show(req, res) {
    const { company_id, id } = req.params;

    const heritage = await Heritage.findOne({
      where: { company_id, id },
      include: [
        {
          model: Environment,
          as: 'environment',
          attributes: ['id', 'name'],
        },
      ],
    });

    if (!heritage) {
      return res.status(400).json({ error: 'Heritage does not exist' });
    }

    const { name, description, code, environment_id } = heritage;

    return res.json({
      id,
      name,
      description,
      code,
      company_id,
      environment_id,
    });
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
      description: Yup.string(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { code, company_id } = req.body;

    const codeExists = await Heritage.findOne({
      where: { code, company_id },
    });

    if (codeExists) {
      return res.status(400).json({ error: 'Code already exist' });
    }

    const { id, name, description, environment_id } = await Heritage.create(
      req.body
    );

    return res.json({
      id,
      name,
      description,
      code,
      company_id,
      environment_id,
    });
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

    if (code) {
      return res.status(400).json({ error: 'Code cannot be changed' });
    }

    await heritage.update(req.body);

    return res.json({ id, name, description });
  }

  async delete(req, res) {
    const { id } = req.params;

    const heritage = await Heritage.findByPk(id);

    if (!heritage) {
      return res.status(400).json({ error: 'Heritage does not exist' });
    }

    await Heritage.destroy({ where: { id } });

    return res.status(200).json({ success: 'Heritage has been deleted' });
  }
}

export default new HeritageController();
