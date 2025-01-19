# Parameters
cd Lab15
$basePath = (pwd).path
$sourcePath = "$basepath\SourceFolder"
$destinationPath = "$basepath\TargetFolder"
$excludePatterns = @("*.tmp", "*.log", "backup*")



# Function to synchronize folders
function Sync-Folders {
    param (
        [string]$Source,
        [string]$Destination,
        [array]$Excludes
    )

    # Ensure destination folder exists
    if (-not (Test-Path -Path $Destination)) {
        Write-Output "Creating destination folder: $Destination"
        New-Item -ItemType Directory -Path $Destination -Force | Out-Null
    }

    # Get all files and subdirectories in the source
    $items = Get-ChildItem -Path $Source -Recurse -Force | Where-Object {
        # Error 1: Exclude logic fails due to incorrect `-match` instead of `-like`
        $match = $false
        foreach ($pattern in $Excludes) {
            if ($_.Name -match $pattern) {  # This should be `-like` instead of `-match`
                $match = $true
                break
            }
        }
        -not $match
    }

    # Copy files to destination
    foreach ($item in $items) {
        # Error 2: Incorrect path replacement logic causes invalid destination paths
        $targetPath = $item.FullName -replace $Source, $Destination  # Should use [regex]::Escape

        if ($item.PSIsContainer) {
            # Create the directory if it doesn't exist
            if (-not (Test-Path -Path $targetPath)) {
                Write-Output "Creating folder: $targetPath"
                New-Item -ItemType Directory -Path $targetPath -Force | Out-Null
            }
        } else {
            # Copy the file
            try {
                Write-Output "Copying file: $($item.FullName) to $targetPath"
                Copy-Item -Path $item.FullName -Destination $targetPath -Force
            } catch {
                Write-Error "Failed to copy file $($item.FullName): $_"
            }
        }
    }
}

# Call the function
Sync-Folders -Source $sourcePath -Destination $destinationPath -Excludes $excludePatterns