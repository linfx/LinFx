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

    # modules
    "src/modules/audit-logging/LinFx.Extensions.Auditing",
    "src/modules/permission-management/LinFx.Extensions.PermissionManagement",
    "src/modules/tenant-management/LinFx.Extensions.TenantManagement"
)
