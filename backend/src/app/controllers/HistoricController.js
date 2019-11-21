import Historic from '../models/Historic';

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
    });

    return res.json(historic);
  }
}

export default new HistoricController();
