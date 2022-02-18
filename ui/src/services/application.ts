import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export interface AvailableTable
{
  guid: string,
  name: string,
  description: string,
  category: string,
}

if (document.referrer !== "" && !document.referrer.endsWith("3000/"))
{
    localStorage.setItem("referrer", document.referrer);
}

const referrer = localStorage.getItem("referrer");

export const applicationApi = createApi({
  reducerPath: 'applicationApi',
  baseQuery: fetchBaseQuery({
    baseUrl: referrer !== null ? referrer : window.location.href,
  }),
  tagTypes: [
    'AvailableTables',
    'TableEnablement',
    'Sessions',
    'SessionTables',
    'SessionTable',
    'SessionTableConfig',
    'SessionTableData'
  ],
  endpoints: (builder) => ({
    loadPlugin: builder.mutation<void, void>({
      query: () => ({ url: 'pluginLoader', method: 'POST' })
    }),
    getAvailableTables: builder.query <AvailableTable[], void>({
      query: () => 'availableTables',
      providesTags: ['AvailableTables']
    }),
    getTableEnablement: builder.query<boolean, string>({
      query: (id) => `enabledTables/${id}`,
      providesTags: (result, error, arg) => [{ type: 'TableEnablement', id: arg }]
    }),
    disableAllTables: builder.mutation<void, void>({
      query: () => ({ url: `enabledTables/disableAll`, method: 'POST' }),
      invalidatesTags:['TableEnablement']
    }),
    toggleTableEnablement: builder.mutation<void, string>({
      query: (id) => ({ url: `enabledTables/${id}`, method: 'POST' }),
      invalidatesTags: (result, error, arg) => [{ type: 'TableEnablement', id: arg }]
    }),
    getSessions: builder.query <string[], void>({
      query: () => 'sessions',
      providesTags: ['Sessions']
    }),
    getSessionTables: builder.query <string[], string>({
      query: (id) => `session/${id}/tables`,
      providesTags: (result, error, arg) => [{ type: 'SessionTables', id: arg }]
    }),
    getSessionTable: builder.query <string[], {sessionId: string, tableId: string}>({
      query: (args) => `session/${args.sessionId}/tables/${args.tableId}/all`,
      providesTags: (result, error, arg) => [{ type: 'SessionTable', id: arg.tableId }]
    }),
    getSessionTableConfig: builder.query <string[], {sessionId: string, tableId: string}>({
      query: (args) => `session/${args.sessionId}/tables/${args.tableId}/config`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableConfig', id: arg.tableId }]
    }),
    getSessionTableData: builder.query <string[][], {sessionId: string, tableId: string}>({
      query: (args) => `session/${args.sessionId}/tables/${args.tableId}/rows`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableData', id: arg.tableId }]
    }),
  }),
})

export const {
  useGetAvailableTablesQuery,
  useLoadPluginMutation,
  useGetTableEnablementQuery,
  useToggleTableEnablementMutation,
  useGetSessionsQuery,
  useGetSessionTablesQuery,
  useGetSessionTableQuery,
  useDisableAllTablesMutation,
  useGetSessionTableConfigQuery,
  useGetSessionTableDataQuery
} = applicationApi;