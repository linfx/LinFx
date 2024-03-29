﻿using LinFx.Extensions.Setting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LinFx.Extensions.SettingManagement
{
    [Service]
    public class SettingStore : ISettingStore
    {
        protected ISettingManagementStore ManagementStore { get; }

        public SettingStore(ISettingManagementStore managementStore)
        {
            ManagementStore = managementStore;
        }

        public virtual Task<string> GetOrNullAsync(string name, string providerName, string providerKey)
        {
            return ManagementStore.GetOrNullAsync(name, providerName, providerKey);
        }

        public virtual Task<List<SettingValue>> GetAllAsync(string[] names, string providerName, string providerKey)
        {
            return ManagementStore.GetListAsync(names, providerName, providerKey);
        }
    }
}
