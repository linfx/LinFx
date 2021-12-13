# 安装 dotnet-ef
dotnet tool install -g dotnet-ef

# 更新 dotnet-ef
dotnet tool update -g dotnet-ef

# 数据迁移
dotnet ef migrations add Initial
dotnet ef database update


dotnet ef migrations add -c AuditLoggingDbContext Initial