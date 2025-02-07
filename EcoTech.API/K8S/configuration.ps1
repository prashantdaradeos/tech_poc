# Get the current Minikube IP
$minikubeIP = minikube ip
$hostname = "minikube"
$hostsFile = "C:\Windows\System32\drivers\etc\hosts"

# Check if Minikube IP is valid
if ($minikubeIP -eq $null) {
    Write-Host "Error: Could not retrieve Minikube IP."
    exit 1
}

# Read the current hosts file content
try {
    $hostsContent = Get-Content -Path $hostsFile -ErrorAction Stop
} catch {
    Write-Host "Error reading hosts file: $($_.Exception.Message)"
    exit 1
}

# Remove existing entry for the hostname
$updatedHostsContent = $hostsContent | Where-Object { $_ -notmatch $hostname }

# Add the new entry for Minikube
$updatedHostsContent += "$minikubeIP`t$hostname"

# Write the updated content back to the hosts file with UTF-8 encoding
try {
    $updatedHostsContent | Set-Content -Path $hostsFile -Encoding UTF8 -ErrorAction Stop
    Write-Host "Updated hosts file with $hostname -> $minikubeIP"
} catch {
    Write-Host "Error updating hosts file: $($_.Exception.Message)"
    exit 1
}

# Flush the DNS cache
Write-Host "Flushing DNS cache..."
ipconfig /flushdns
Write-Host "DNS cache flushed successfully."
