import { Op } from 'sequelize';

import Historic from '../models/Historic';

class HistoricController {
  async index(req, res) {
    const { id } = req.params;
    const { q = null } = req.query;

    const where = q
      ? { company_id: id, message: { [Op.like]: `%${q}%` } }
      : { company_id: id };

    const historic = await Historic.findAll({
      where,
      attributes: ['id', 'message', 'company_id', 'createdAt'],
    });

    return res.json(historic);
  }
}

export default new HistoricController();
