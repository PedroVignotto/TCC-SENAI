import Sequelize, { Model } from 'sequelize';

class Historic extends Model {
  static init(sequelize) {
    super.init(
      {
        message: Sequelize.INTEGER,
      },
      {
        sequelize,
      }
    );

    return this;
  }

  static associate(models) {
    this.belongsTo(models.Company, { foreignKey: 'company_id', as: 'company' });
  }
}

export default Historic;
