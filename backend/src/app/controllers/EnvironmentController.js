import * as Yup from 'yup';
import { Op } from 'sequelize';

import Environment from '../models/Environment';
import User from '../models/User';
import Historic from '../models/Historic';

class EnvironmentController {
  async index(req, res) {
    const { company_id } = req.params;
    const { q = null } = req.query;

    const where = q
      ? { company_id, name: { [Op.like]: `%${q}%` } }
      : { company_id };

    const environment = await Environment.findAll({
      where,
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
    const { q = null } = req.query;

    const where = q
      ? { user_id: req.userId, name: { [Op.like]: `%${q}%` } }
      : { user_id: req.userId };

    const environment = await Environment.findAll({
      where,
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

  async findOne(req, res) {
    const { company_id, name } = req.params;

    const environment = await Environment.findOne({
      where: { company_id, name },
    });

    if (!environment) {
      return res.status(400).json({ error: 'Ambiente não existe' });
    }

    return res.json(environment);
  }

  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { user_id, name } = req.body;

    if (user_id) {
      const userExists = await User.findByPk(user_id);

      if (!userExists) {
        return res.status(400).json({ error: 'Usuário não existe' });
      }
    }

    const nameExist = await Environment.findOne({ where: { name } });

    if (nameExist) {
      return res.status(400).json({ error: 'Ambiente já existe' });
    }

    const { id, company_id } = await Environment.create(req.body);

    return res.json({ id, name, company_id, user_id });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
    });

    if (!(await schema.isValid(req.body))) {
      return res
        .status(400)
        .json({ error: 'Algo deu errado, tente novamente' });
    }

    const { id } = req.params;
    const { name } = req.body;

    const environment = await Environment.findByPk(id);

    if (name && name !== environment.name) {
      const nameExists = await Environment.findOne({ where: { name } });

      if (nameExists) {
        return res
          .status(400)
          .json({ error: 'Esse nome já está sendo utilizado' });
      }

      const user = await User.findByPk(req.userId);

      await Historic.create({
        company_id: environment.company_id,
        message: `${user.email} renomeou o ambiente ${environment.name} para ${name}`,
      });
    }

    await environment.update(req.body);

    return res.json({ id, name });
  }

  async delete(req, res) {
    const { id } = req.params;

    const environment = await Environment.findByPk(id);

    if (!environment) {
      return res.status(400).json({ error: 'Ambiente não existe' });
    }

    const user = await User.findByPk(req.userId);

    await Historic.create({
      company_id: environment.company_id,
      message: `${user.email} excluiu o ambiente ${environment.name}`,
    });

    await Environment.destroy({ where: { id } });

    return res.status(200).json({ success: 'Ambiente foi excluído' });
  }
}

export default new EnvironmentController();
