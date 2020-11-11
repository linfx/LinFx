﻿using LinFx;
using LinFx.Application;
using LinFx.Application.Contracts;

namespace LinFx.Application.Contracts
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// Skip count (beginning of the page).
        /// </summary>
        int Page { get; set; }
    }
}