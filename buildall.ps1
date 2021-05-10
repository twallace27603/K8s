rm -r publish
rm site.zip
dotnet publish -c RELEASE -r linux-x64 -o publish
cd .\publish
zip -r ..\site.zip *
cd ..
$context = New-AzureStorageContext -StorageAccountName inedemoassets -StorageAccountKey $key
Set-AzStorageBlobContent -File .\site.zip -Container labfiles  -Blob Cloud-App-Management/site.zip -BlobType Block -Context $context