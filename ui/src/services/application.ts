import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export const applicationApi = createApi({
  reducerPath: 'applicationApi',
  baseQuery: fetchBaseQuery({
    baseUrl: document.referrer !== "" ? document.referrer : window.location.href,
  }),
  tagTypes: ['Count'],
  endpoints: (builder) => ({
    getCurrentCount: builder.query<number, void>({
      query: () => 'counter',
      providesTags: ['Count']
    }),
    incrementCount: builder.mutation<void, void>({
      query: () => ({ url: 'counter/increment', method: 'POST' }),
      invalidatesTags: ['Count']
    }),
    decrementCount: builder.mutation<void, void>({
      query: () => ({ url: 'counter/decrement', method: 'POST' }),
      invalidatesTags: ['Count']
    })
  }),
})

export const {
  useGetCurrentCountQuery,
  useIncrementCountMutation,
  useDecrementCountMutation
} = applicationApi;