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

routes.put('/users', UserController.update);

routes.put('/companies/:id', CompanyController.update);

routes.get('/:company_id/environments/', EnvironmentController.index);
routes.get('/:company_id/environments/:id', EnvironmentController.show);
routes.post('/environments', EnvironmentController.store);
routes.put('/environments/:id', EnvironmentController.update);
routes.delete('/environments/:id', EnvironmentController.delete);

routes.post('/heritages', HeritageController.store);

routes.post('/files', upload.single('file'), FileController.store);

export default routes;
