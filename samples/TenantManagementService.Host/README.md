# 安装 dotnet-ef
dotnet tool install -g dotnet-ef

# 更新 dotnet-ef
dotnet tool update -g dotnet-ef

# 数据迁移
dotnet ef migrations add -c TenantManagementDbContext Initial
dotnet ef database update -c TenantManagementDbContext


dotnet ef migrations add -c AuditLoggingDbContext Initial