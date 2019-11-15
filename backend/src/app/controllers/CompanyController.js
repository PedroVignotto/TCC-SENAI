import * as Yup from 'yup';
import { Op } from 'sequelize';

import Company from '../models/Company';

class CompanyController {
  async store(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string().required(),
      cnpj: Yup.string().required(),
      email: Yup.string()
        .email()
        .required(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { email, cnpj } = req.body;

    const companyExists = await Company.findOne({
      where: {
        [Op.or]: [
          {
            email,
          },
          {
            cnpj,
          },
        ],
      },
    });

    if (companyExists) {
      return res.status(400).json({ error: 'Company already exists' });
    }

    const { id, name } = await Company.create(req.body);

    return res.json({ id, name, email, cnpj });
  }
}

export default new CompanyController();
