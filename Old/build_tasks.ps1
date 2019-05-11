properties {
    $build_config = $config

    $base_dir = Resolve-Path . # Not currently used but nearly always useful
    $nunit_runner = ".\packages\NUnit.ConsoleRunner.3.5.0\tools\nunit3-console.exe"

    $solution = "CSharpChess.sln"
    $unit_tests = ".\CSharpChess.UnitTests\bin\debug\CSharpChess.UnitTests.dll"

    $msbuild_options = "/p:Configuration=$build_config /p:Platform='x64'"
}
# FormatTaskName "-------- {0} --------"
FormatTaskName {
   param($taskName)
   $padding = "-" * $taskName.length;
   write-host "------------------$padding"  -foregroundcolor magenta
   write-host "-------- $taskName --------" -foregroundcolor magenta
   write-host "------------------$padding" -foregroundcolor magenta
}
Framework "4.6"

task default -depends test

task clean { 
    # Exec { msbuild $solution /t:Clean $msbuild_options }
    Exec { del */bin/$build_config/* }
}

task compile -depends clean, compile-only { 
}

task compile-only { 
    'TODO: Ensure nuget restore has run, *DO NOT* rely on MSBuild to do it.'
    Exec { msbuild $solution /t:Build $msbuild_options }
}

task test -depends compile, test-only { 
}

task test-only { 
    Exec { & $nunit_runner $unit_tests --output test-results.xml }
}
task ? -Description "Helper to display task info" {
	invoke-psake .\build_tasks.ps1 -docs
}

task show-version {
    msbuild /version
}
