import * as Yup from 'yup';
import { Op } from 'sequelize';

import Heritage from '../models/Heritage';
import Environment from '../models/Environment';
import Historic from '../models/Historic';

class HeritageController {
  async index(req, res) {
    const { company_id } = req.params;
    const { q = null } = req.query;

    const where = q
      ? { company_id, code: { [Op.like]: `%${q}%` } }
      : { company_id };

    const heritage = await Heritage.findAll({
      where,
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

  async show(req, res) {
    const { company_id, environment_id } = req.params;
    const { q = null } = req.query;

    const where = q
      ? { company_id, environment_id, code: { [Op.like]: `%${q}%` } }
      : { company_id, environment_id };

    const heritage = await Heritage.findAll({
      where,
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

  async create(req, res) {
    const schema = Yup.object().shape({
      id: Yup.number().required(),
      code: Yup.string().required(),
      problem: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { company_id } = req.params;
    const { code, problem, id } = req.body;

    await Historic.create({
      company_id,
      message: `Pedido de manutenção solicitado para o patrimônio ${code}`,
    });

    return res.json({ id, code, problem });
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
    const { name, code, description, environment_id } = req.body;

    const heritage = await Heritage.findByPk(id);

    if (code) {
      return res.status(400).json({ error: 'Code cannot be changed' });
    }

    if (environment_id) {
      const oldEnvironment = await Environment.findByPk(
        heritage.environment_id
      );
      const newEnvironment = await Environment.findByPk(environment_id);

      if (!oldEnvironment) {
        await Historic.create({
          company_id: heritage.company_id,
          message: `Patrimônio ${heritage.code} movido para o ambiente ${newEnvironment.name}`,
        });
      } else {
        await Historic.create({
          company_id: heritage.company_id,
          message: `Patrimônio ${heritage.code} movido do ambiente ${oldEnvironment.name} para ${newEnvironment.name}`,
        });
      }
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
