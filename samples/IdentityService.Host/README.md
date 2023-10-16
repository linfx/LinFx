# 指定变量环境
export ASPNETCORE_ENVIRONMENT=Production

# 安装 dotnet-ef
dotnet tool install -g dotnet-ef

# 更新 dotnet-ef
dotnet tool update -g dotnet-ef

# 数据迁移
dotnet ef migrations add -c ApplicationDbContext Init
dotnet ef database update -c ApplicationDbContext

dotnet ef migrations add -c PermissionManagementDbContext Init
dotnet ef database update -c PermissionManagementDbContext