import Sequelize, { Model } from 'sequelize';

class Environment extends Model {
  static init(sequelize) {
    super.init(
      {
        name: Sequelize.STRING,
      },
      {
        sequelize,
      }
    );

    return this;
  }

  static associate(models) {
    this.belongsTo(models.File, { foreignKey: 'user_id', as: 'user' });
    this.belongsTo(models.File, { foreignKey: 'company_id', as: 'company' });
  }
}

export default Environment;
