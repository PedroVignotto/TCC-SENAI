import Sequelize, { Model } from 'sequelize';

class Historic extends Model {
  static init(sequelize) {
    super.init(
      {
        type_historic: Sequelize.INTEGER,
      },
      {
        sequelize,
      }
    );

    return this;
  }

  static associate(models) {
    this.belongsTo(models.Environment, {
      foreignKey: 'environment_id',
      as: 'environment',
    });
    this.belongsTo(models.Company, { foreignKey: 'company_id', as: 'company' });
    this.belongsTo(models.Heritage, {
      foreignKey: 'heritage_id',
      as: 'heritage',
    });
  }
}

export default Historic;
