param(
  [Parameter(Mandatory=$true)]
  [string[]]$name
)

ng generate component ./components/$name --display-block --skip-tests
