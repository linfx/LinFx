﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace LinFx.Extensions.Setting
{
    public interface ISettingStore
    {
        Task<string> GetOrNullAsync(
            [NotNull] string name,
            [CanBeNull] string providerName,
            [CanBeNull] string providerKey
        );

        Task<List<SettingValue>> GetAllAsync(
            [NotNull] string[] names,
            [CanBeNull] string providerName,
            [CanBeNull] string providerKey
        );
    }
}
