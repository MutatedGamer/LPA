import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react'

export interface AvailableTable
{
  guid: string,
  name: string,
  description: string,
  category: string,
}

export interface ProgressState
{
  label: string,
  cancelText: string,
  value: number,
  canCancel: boolean,
}

export interface SessionTableInfo
{
  name: string,
  category: string,
}

export interface SessionTableViewIdentifier
{
  sessionTableId: string,
  sessionTableViewId: string,
}

export interface SessionTableConfig
{
  id: string,
  name: string,
}

export interface Column
{
  isGraph: boolean,
  isPivot: boolean,
  isLeftFrozen: boolean,
  isRightFrozen: boolean,
  name: string,
  sessionTableColumnGuid: string,
}

if (document.referrer !== "" && !document.referrer.endsWith("3000/"))
{
    localStorage.setItem("referrer", document.referrer);
}

const referrer = localStorage.getItem("referrer");

export const applicationApi = createApi({
  reducerPath: 'applicationApi',
  baseQuery: fetchBaseQuery({
    baseUrl: referrer !== null ? referrer : window.location.origin,
  }),
  tagTypes: [
    'AvailableTables',
    'TableEnablement',
    'Sessions',
    'SessionTables',
    'SessionTable',
    'SessionTableConfig',
    'SessionTableViews',
    'SessionTableViewColumns',
    'SessionTableViewData',
    'SessionTableViewConfig',
    'Progress'
  ],
  endpoints: (builder) => ({
    // Load Plugin
    loadPlugin: builder.mutation<void, void>({
      query: () => ({ url: 'pluginLoader', method: 'POST' })
    }),

    // Available Tables
    getAvailableTables: builder.query <AvailableTable[], void>({
      query: () => 'availableTables',
      providesTags: ['AvailableTables']
    }),

    // Table Enablement
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

    // Sessions
    getSessions: builder.query <string[], void>({
      query: () => 'sessions',
      providesTags: ['Sessions']
    }),

    // Session Tables
    getSessionTables: builder.query <string[], string>({
      query: (id) => `sessions/${id}/sessionTables`,
      providesTags: (result, error, arg) => [{ type: 'SessionTables', id: arg }]
    }),
    getSessionTableInfo: builder.query <SessionTableInfo, {sessionId: string, tableId: string}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/info`
    }),

    // Session Table Configs
    getSessionTableConfigs: builder.query <SessionTableConfig[], {sessionId: string, tableId: string}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/configs`,
    }),
    
    // Session Table Views
    getSessionTableViews: builder.query <SessionTableViewIdentifier[], string>({
      query: (id) => `sessions/${id}/sessionTableViews`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableViews', id: arg }]
    }),

    closeSessionTableView: builder.mutation <void, {sessionId: string, tableId: string, viewId: string}>({
      query: (args) => ({
        url: `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/close`, method: 'POST'
      })
    }),

    getSessionTableViewConfig: builder.query <string, {sessionId: string, tableId: string, viewId: string}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/config`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableViewConfig', id: arg.viewId }]
    }),

    setSessionTableViewConfig: builder.mutation <void, {sessionId: string, tableId: string, viewId: string, configId: string}>({
      query: (args) => ({ url: `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/config/${args.configId}`, method: 'POST' }),
      invalidatesTags: (result, error, arg) => [{ type: 'SessionTableViewConfig', id: arg.viewId }]
    }),

    getSessionTableViewColumns: builder.query <Column[], {sessionId: string, tableId: string, viewId: string}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/columns`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableViewColumns', id: arg.viewId }]
    }),

    getSessionTableViewRowCount: builder.query <number, {sessionId: string, tableId: string, viewId: string}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/rowCount`,
    }),

    getSessionTableViewRows: builder.query <string[][], {sessionId: string, tableId: string, viewId: string, start: number, count: number}>({
      query: (args) => `sessions/${args.sessionId}/sessionTables/${args.tableId}/views/${args.viewId}/rows/${args.start}/${args.count}`,
      providesTags: (result, error, arg) => [{ type: 'SessionTableViewData', id: arg.viewId }]
    }),

    // Progress
    getProgressState: builder.query<ProgressState, string>({
      query: (id) => `progress/${id}`,
      providesTags: (result, error, arg) => [{ type: 'Progress', id: arg }]
    }),
    cancelProgress: builder.mutation<void, string>({
      query: (id) => ({ url: `progress/${id}`, method: 'POST' }),
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
  useGetSessionTableViewsQuery,
  useDisableAllTablesMutation,
  useCloseSessionTableViewMutation,
  useGetSessionTableInfoQuery,
  useGetSessionTableConfigsQuery,
  useSetSessionTableViewConfigMutation,
  useGetSessionTableViewConfigQuery,
  useGetSessionTableViewColumnsQuery,
  useGetSessionTableViewRowCountQuery,
  useLazyGetSessionTableViewRowsQuery,
  useGetProgressStateQuery,
  useCancelProgressMutation
} = applicationApi;