import storage from 'redux-persist/lib/storage';
import { persistReducer } from 'redux-persist';

export default reducers => {
  const persistedReducer = persistReducer(
    {
      key: 'Heritage',
      storage,
      whiteList: ['auth', 'user'],
    },
    reducers
  );

  return persistedReducer;
};
