import Sequelize from 'sequelize';

import User from '../app/models/User';
import File from '../app/models/File';
import Company from '../app/models/Company';
import Environment from '../app/models/Environment';
import Heritage from '../app/models/Heritage';
import Historic from '../app/models/Historic';

import databaseConfig from '../config/database';

const models = [User, File, Company, Environment, Heritage, Historic];

class Database {
  constructor() {
    this.init();
  }

  init() {
    this.connection = new Sequelize(databaseConfig);

    models
      .map(model => model.init(this.connection))
      .map(model => model.associate && model.associate(this.connection.models));
  }
}

export default new Database();
