#Build publish content
rm -r publish
rm site.zip
dotnet publish -c RELEASE -r linux-x64 -o publish
cd .\publish
zip -r ..\site.zip *
cd ..

#Upload files to Azure
$context = New-AzureStorageContext -StorageAccountName inedemoassets -StorageAccountKey $key
Set-AzStorageBlobContent -File .\site.zip -Container labfiles  -Blob Cloud-App-Management/site.zip -BlobType Block -Context $context -Force
Set-AzStorageBlobContent -File .\arm-lab-webapp-deployment.json -Container labfiles  -Blob arm-lab-webapp-deployment.json -BlobType Block -Context $context -Force
Set-AzStorageBlobContent -File .\arm-lab-webapp-deployment.sh -Container labfiles  -Blob arm-lab-webapp-deployment.sh -BlobType Block -Context $context -Force

#Manage Docker images
docker image rm twallace27603/k8swebapp:1.0
docker image rm k8s:latest
docker build -t k8s:latest . 
docker image tag k8s:latest twallace27603/k8swebapp:1.0
docker push twallace27603/k8swebapp:1.0
