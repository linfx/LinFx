# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of solutions
$solutions = (
    "src/LinFx"
    # "src/modules/audit-logging"
)

# List of projects
$projects = (

    # linfx
    "src/LinFx"

    # modules/audit-logging
    # "src/modules/audit-logging/LinFx.Extensions.Auditing"
)
