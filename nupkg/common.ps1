# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of solutions
$solutions = (
    "src/LinFx"
)

# List of projects
$projects = (

    # linfx
    "src/LinFx",
    "src/LinFx.Extensions.Autofac",
    "src/LinFx.Extensions.AspNetCore",
    "src/LinFx.Extensions.EntityFrameworkCore",
    # "src/LinFx.Extensions.EntityFrameworkCore.PostgreSql",
    # "src/LinFx.Extensions.EntityFrameworkCore.Sqlite",

    # modules
    "src/LinFx.Extensions.AuditLogging",
    "src/LinFx.Extensions.PermissionManagement",
    "src/LinFx.Extensions.TenantManagement"
)
