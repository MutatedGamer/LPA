import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export const applicationApi = createApi({
  reducerPath: 'applicationApi',
  baseQuery: fetchBaseQuery({
    baseUrl: document.referrer !== "" ? document.referrer : window.location.href,
  }),
  endpoints: (builder) => ({
    getCurrentCount: builder.query<number, void>({
      query: () => 'counter',
    }),
    incrementCount: builder.mutation<void, void>({
      query: () => ({ url: 'counter/increment', method: 'POST' }),
    }),
    decrementCount: builder.mutation<void, void>({
      query: () => ({ url: 'counter/decrement', method: 'POST' }),
    })
  }),
})

export const {
  useGetCurrentCountQuery,
  useIncrementCountMutation,
  useDecrementCountMutation
} = applicationApi;