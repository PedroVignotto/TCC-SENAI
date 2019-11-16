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
    this.belongsTo(models.User, { foreignKey: 'user_id', as: 'user' });
    this.belongsTo(models.Company, { foreignKey: 'company_id', as: 'company' });
  }
}

export default Environment;
