import { Router } from 'express';
import multer from 'multer';
import multerConfig from './config/multer';

import UserController from './app/controllers/UserController';
import SessionController from './app/controllers/SessionController';
import FileController from './app/controllers/FileController';
import CompanyController from './app/controllers/CompanyController';
import EnvironmentController from './app/controllers/EnvironmentController';
import HeritageController from './app/controllers/HeritageController';

import authMiddleware from './app/middlewares/auth';

const routes = new Router();
const upload = multer(multerConfig);

routes.post('/users', UserController.store);
routes.post('/sessions', SessionController.store);
routes.post('/companies', CompanyController.store);

routes.use(authMiddleware);

routes.get('/:company_id/users', UserController.index);
routes.get('/:company_id/managers/:email', UserController.show);
routes.put('/company/users/:email', UserController.findAndUpdate);
routes.put('/users', UserController.update);
routes.put('/users/:id', UserController.edit);
routes.delete('/users/:id', UserController.delete);

routes.put('/companies/:id', CompanyController.update);

routes.get('/:company_id/environments/', EnvironmentController.index);
routes.get('/environments', EnvironmentController.show);
routes.post('/environments', EnvironmentController.store);
routes.put('/environments/:id', EnvironmentController.update);
routes.delete('/environments/:id', EnvironmentController.delete);

routes.get('/:company_id/heritages', HeritageController.index);
routes.get(
  '/:company_id/environments/:environment_id/heritages',
  HeritageController.list
);
routes.get('/:company_id/heritages/:id', HeritageController.show);
routes.post('/heritages', HeritageController.store);
routes.put('/heritages/:id', HeritageController.update);
routes.delete('/heritages/:id', HeritageController.delete);

routes.post('/files', upload.single('file'), FileController.store);

export default routes;
