import Sequelize, { Model } from 'sequelize';

class Heritage extends Model {
  static init(sequelize) {
    super.init(
      {
        name: Sequelize.STRING,
        description: Sequelize.STRING,
        code: Sequelize.STRING,
      },
      {
        sequelize,
      }
    );

    return this;
  }

  static associate(models) {
    this.belongsTo(models.File, {
      foreignKey: 'environment_id',
      as: 'environment',
    });
    this.belongsTo(models.File, { foreignKey: 'company_id', as: 'company' });
  }
}

export default Heritage;