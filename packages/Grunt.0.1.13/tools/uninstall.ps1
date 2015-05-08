param($installPath, $toolsPath, $package, $project)

. (Join-Path $toolsPath 'bin_tools.ps1')

$cmd = '.bin\grunt.cmd'
Delete-Bin $cmd