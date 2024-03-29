﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinFx.Extensions.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static bool HasRelationalTransactionManager(this DbContext dbContext) => dbContext.Database.GetService<IDbContextTransactionManager>() is IRelationalTransactionManager;
}
