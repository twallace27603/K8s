
#Set up connection to cluster
aws eks update-kubeconfig \
  --name k8s

kubectl get svc