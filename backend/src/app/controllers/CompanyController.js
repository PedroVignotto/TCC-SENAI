import * as Yup from 'yup';
import { Op } from 'sequelize';

import Company from '../models/Company';

class CompanyController {
  async show(req, res) {
    const { company_id } = req.params;

    const company = await Company.findOne({
      where: { id: company_id },
      attributes: [
        'name',
        'cnpj',
        'email',
        'cep',
        'address',
        'district',
        'city',
        'state',
      ],
    });

    if (!company) {
      return res.status(400).json({ error: 'Company not found' });
    }

    return res.json(company);
  }

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

    const {
      id,
      name,
      cep,
      address,
      district,
      city,
      state,
    } = await Company.create(req.body);

    return res.json({
      id,
      name,
      email,
      cnpj,
      cep,
      address,
      district,
      city,
      state,
    });
  }

  async update(req, res) {
    const schema = Yup.object().shape({
      name: Yup.string(),
      cnpj: Yup.string(),
      email: Yup.string().email(),
    });

    if (!(await schema.isValid(req.body))) {
      return res.status(400).json({ error: 'Validation fails' });
    }

    const { id } = req.params;

    const { email, cnpj } = req.body;

    const company = await Company.findByPk(id);

    if (email && email !== company.email) {
      const emailExists = await Company.findOne({ where: { email } });

      if (emailExists) {
        return res.status(400).json({ error: 'Email not available' });
      }
    }

    if (cnpj && cnpj !== company.cnpj) {
      const cnpjExists = await Company.findOne({ where: { cnpj } });

      if (cnpjExists) {
        return res.status(400).json({ error: 'CNPJ not available' });
      }
    }

    const { name } = await company.update(req.body);

    return res.json({ id, name, cnpj, email });
  }
}

export default new CompanyController();
