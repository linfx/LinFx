﻿namespace LinFx.Extensions.MultiTenancy;

public interface ITenantResolveContributor
{
    string Name { get; }

    void Resolve(ITenantResolveContext context);
}