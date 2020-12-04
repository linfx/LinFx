# Paths
$packFolder = (Get-Item -Path "./" -Verbose).FullName
$rootFolder = Join-Path $packFolder "../"

# List of solutions
$solutions = (
    "src/LinFx",
    "src/modules/account"
)

# List of projects
$projects = (

    # linfx
    "src/LinFx",

    # modules/account
    "src/modules/account/LinFx.Extensions.Account",
    "src/modules/account/LinFx.Extensions.Account.HttpApi",

    # # modules/account
    "src/modules/identity/LinFx.Extensions.Identity"
)
