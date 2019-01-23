using LinFx.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinFx.Application.Models
{
    /// <summary>
    /// Simply implements <see cref="IPagedResultRequest"/>.
    /// </summary>
    public class PagedResultRequestDto : LimitedResultRequest, IPagedResultRequest
    {
        [Range(0, int.MaxValue)]
        public virtual int SkipCount { get; set; }
        public int MaxResultCount { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
