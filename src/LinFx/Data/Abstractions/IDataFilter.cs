using System;

namespace LinFx.Data.Abstractions
{
    public interface IDataFilter
    {
        IDisposable Enable<TFilter>() where TFilter : class;

        IDisposable Disable<TFilter>() where TFilter : class;

        bool IsEnabled<TFilter>() where TFilter : class;
    }

    public interface IDataFilter<TFilter> where TFilter : class
    {
        IDisposable Enable();

        IDisposable Disable();

        bool IsEnabled { get; }
    }
}
