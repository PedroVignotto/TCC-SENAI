import Historic from '../models/Historic';
import Environment from '../models/Environment';
import Heritage from '../models/Heritage';

class HistoricController {
  async index(req, res) {
    const { id } = req.params;

    const historic = await Historic.findAll({
      where: { company_id: id },
      attributes: [
        'id',
        'type_historic',
        'createdAt',
        'heritage_id',
        'company_id',
        'environment_id',
      ],
      include: [
        {
          model: Environment,
          as: 'environment',
          attributes: ['id', 'name'],
        },
        {
          model: Heritage,
          as: 'heritage',
          attributes: ['id', 'code'],
        },
      ],
    });

    return res.json(historic);
  }
}

export default new HistoricController();
