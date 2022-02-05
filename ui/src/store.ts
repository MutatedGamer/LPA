import { configureStore } from '@reduxjs/toolkit'
import { setupListeners } from '@reduxjs/toolkit/query'
import { applicationApi } from './services/application'

export const store = configureStore({
  reducer: {
    [applicationApi.reducerPath]: applicationApi.reducer,
  },
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(applicationApi.middleware),
})

setupListeners(store.dispatch)