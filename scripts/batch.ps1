# PowerShell Core Script for processing multiple files with shared parameters

# Define the path to the executable
$exePath = "ConsoleApp.exe"

# Define the shared parameters
$sharedParams = @{
    VertexCount = 262144
    Scale = -6
    TotalHeight = 2.5
    RightDirection = "PositiveX"
    UpDirection = "NegativeY"
    ForwardDirection = "NegativeZ"
    RotateX = 0
    RotateY = 0
    RotateZ = 0
    TranslateX = 0
    TranslateY = 0
    TranslateZ = 0
}

# List of files with their unique parameters
$fileParams = @(
    # Table 1
    @{
        Input = "posters/AI×VR 24 Poster 0A68821.png"
        Output = "0A68821@T1.ply"
        RotateY = -0.7853981634
        TranslateX = -4.65612
        TranslateY = 0.07
        TranslateZ = -5.28861
    },
    # Table 2
    @{
        Input = "posters/AI×VR 24 Poster B2B4F20.png"
        Output = "B2B4F20@T2.ply"
        RotateY = -1.570796327
        TranslateX = -10.5426
        TranslateY = 0.07
        TranslateZ = -1.57908
    },
    # Table 3
    @{
        Input = "posters/AI×VR 24 Poster B7AA04E.png"
        Output = "B7AA04E@T3.ply"
        RotateY = -0.7853981634
        TranslateX = -10.29
        TranslateY = 0.07
        TranslateZ = 6.78238
    },
    # Table 4
    @{
        Input = "posters/AI×VR 24 Poster C1418C8.png"
        Output = "C1418C8@T4.ply"
        RotateY = 2.617993878
        TranslateX = -1.47207
        TranslateY = 0.07
        TranslateZ = 6.56429
    },
    # Table 5
    @{
        Input = "posters/AI×VR 24 Poster CFA55E4.png"
        Output = "CFA55E4@T5.ply"
        RotateY = 3.141592654
        TranslateX = 6.25742
        TranslateY = 0.07
        TranslateZ = 5.96545
    },
    # Table 6
    @{
        Input = "posters/AI×VR 24 Poster E2FB61F.png"
        Output = "E2FB61F@T6.ply"
        RotateY = 3.141592654
        TranslateX = 12.0473
        TranslateY = 0.07
        TranslateZ = 4.90353
    }
)

# Function to convert a hashtable to a command line argument string
function ConvertTo-CommandLineArgs($params) {
    $arguments = @()
    foreach ($key in $params.Keys) {
        $arguments += "--$key"
        $value = $params[$key]

        # Check if the value is a string and, if so, enclose it in quotes
        # This is necessary for correctly handling paths with spaces
        if ($value -is [string]) {
            $value = "`"$value`""  # Using backticks (`) to escape double quotes
        }

        $arguments += $value
    }
    return $arguments -join ' '  # Joining all arguments into a single string
}

# Process each file
foreach ($fileParam in $fileParams) {
    # Combine shared and file-specific parameters
    $fullParams = $sharedParams.Clone()
    $fileParam.GetEnumerator() | ForEach-Object { $fullParams[$_.Key] = $_.Value }

    # Convert parameters to command line arguments
    $cmdArgs = ConvertTo-CommandLineArgs -params $fullParams

    # Construct the full command
    $fullCommand = "& `"$exePath`" $cmdArgs"

    # Execute the command
    Invoke-Expression $fullCommand
}
