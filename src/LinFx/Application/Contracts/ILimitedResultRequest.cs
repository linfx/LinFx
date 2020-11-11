﻿using LinFx;
using LinFx.Application;
using LinFx.Application.Contracts;

namespace LinFx.Application.Contracts
{
    /// <summary>
    /// This interface is defined to standardize to request a limited result.
    /// </summary>
    public interface ILimitedResultRequest
    {
        /// <summary>
        /// Max expected result count.
        /// </summary>
        int Limit { get; set; }
    }
}
