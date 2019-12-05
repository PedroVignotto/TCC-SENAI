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
        'state',
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
        'state',
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
      return res.status(400).json({ error: 'Heritage não existe' });
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
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
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
      code: Yup.string().required(),
      environment_id: Yup.number(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { code, company_id } = req.body;

    const codeExists = await Heritage.findOne({
      where: { code, company_id },
    });

    if (codeExists) {
      return res.status(400).json({ error: 'Código já existe' });
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

  async edit(req, res) {
    const schema = Yup.object().shape({
      environment_id: Yup.number(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { environment_id } = req.params;

    const environment = await Environment.findByPk(environment_id);

    if (!environment) {
      return res.status(400).json({ error: 'Ambiente não encontrado' });
    }

    await Heritage.update({ state: false }, { where: { environment_id } });

    return res.json({ success: true });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
      description: Yup.string(),
      code: Yup.string(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { id } = req.params;
    const { name, code, description, environment_id } = req.body;

    const heritage = await Heritage.findByPk(id);

    if (code) {
      return res.status(400).json({ error: 'Código não pode ser alterado' });
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
      return res.status(400).json({ error: 'Heritage não existe' });
    }

    await Heritage.destroy({ where: { id } });

    return res.status(200).json({ success: 'Heritage foi excluído' });
  }
}

export default new HeritageController();
