# 安装 dotnet-ef
dotnet tool install -g dotnet-ef

# 更新 dotnet-ef
dotnet tool update -g dotnet-ef

# 数据迁移
dotnet ef migrations add -c TenantManagementMigrationsDbContext Init
dotnet ef migrations add -c AuditLoggingDbContext Init
dotnet ef migrations add -c PermissionManagementDbContext Init

dotnet ef database update -c TenantManagementMigrationsDbContext
dotnet ef database update -c AuditLoggingDbContext
