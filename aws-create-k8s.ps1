aws iam attach-role-policy `
  --policy-arn arn:aws:iam::aws:policy/AmazonEKS_CNI_Policy `
  --role-name myAmazonEKSCNIRole

  aws eks update-addon `
  --cluster-name webinar-test `
  --addon-name vpc-cni `
  --service-account-role-arn arn:aws:iam::119286610541:role/myAmazonEKSCNIRole 

  aws iam create-role `
  --role-name myAmazonEKSNodeRole `
  --assume-role-policy-document file://"aws-node-role-trust-policy.json"

aws iam attach-role-policy `
  --policy-arn arn:aws:iam::aws:policy/AmazonEKSWorkerNodePolicy `
  --role-name myAmazonEKSNodeRole
aws iam attach-role-policy `
  --policy-arn arn:aws:iam::aws:policy/AmazonEC2ContainerRegistryReadOnly `
  --role-name myAmazonEKSNodeRole

  aws iam create-role `
  --role-name myAmazonEKSFargatePodExecutionRole `
  --assume-role-policy-document file://"aws-pod-execution-role-trust-policy.json"

  aws iam attach-role-policy `
  --policy-arn arn:aws:iam::aws:policy/AmazonEKSFargatePodExecutionRolePolicy `
  --role-name myAmazonEKSFargatePodExecutionRole