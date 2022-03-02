using LPA.Application.Sessions.Tables.Columns;

namespace LPA.Application.Sessions.Tables.TableConfigs
{
    public interface ITableConfiguration
    {
        Guid Id { get; }

        int LeftFreezeColumnIndex { get; }
        int RightFreezeColumnIndex { get; }
        int PivotColumnIndex { get; }
        int GraphColumnIndex { get; }

        IColumn[] Columns { get; }

        string Name { get; }
    }
}