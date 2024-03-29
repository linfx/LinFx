﻿using JetBrains.Annotations;
using LinFx.Domain.Entities;

namespace LinFx.Extensions.SettingManagement
{
    public class Setting : Entity<string>, IAggregateRoot<string>
    {
        [NotNull]
        public virtual string Name { get; protected set; }

        [NotNull]
        public virtual string Value { get; internal set; }

        [CanBeNull]
        public virtual string ProviderName { get; protected set; }

        [CanBeNull]
        public virtual string ProviderKey { get; protected set; }

        protected Setting() { }

        public Setting(
            string id,
            [NotNull] string name,
            [NotNull] string value,
            [CanBeNull] string providerName = null,
            [CanBeNull] string providerKey = null)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(value, nameof(value));

            Id = id;
            Name = name;
            Value = value;
            ProviderName = providerName;
            ProviderKey = providerKey;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, Name = {Name}, Value = {Value}, ProviderName = {ProviderName}, ProviderKey = {ProviderKey}";
        }
    }
}