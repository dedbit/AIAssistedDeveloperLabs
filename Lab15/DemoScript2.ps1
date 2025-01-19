# Parameters
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
        $match = $false
        foreach ($pattern in $Excludes) {
            if ($_.Name -match $pattern) { 
                $match = $true
                break
            }
        }
        -not $match
    }

    # Copy files to destination
    foreach ($item in $items) {
        $targetPath = $item.FullName -replace $Source, $Destination 

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