param(
  [Parameter(Mandatory=$true)]
  [string[]]$name
)

ng generate component ./pages/$name --display-block --skip-tests --type page
