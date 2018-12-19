# Azure Humio observability sample app

The purpose of this repository is to evaluate Humio's ability to ingest logs from APIs deployed in
Azure Kubernetes Service. 

## Prerequisites

All
 * .NET Core SDK 2.1
 * An Azure account

Mac
  
  * Download and install [Docker for Mac](https://docs.docker.com/docker-for-mac/install/)
  * Preferences -> Kubernetes -> Enable Kubernetes

Windows
 * You're on your own. Probably Docker and Minikube
 
# Setup steps

## Create a resource group

Note, you may want to alter the names of all the Azure resources created.

```
az login
az group create --name=humio-observability-sample-group --location=ukwest
```

## Create an Azure Kubernetes Cluster, deploy a web API and install the humio agent

Create the cluster.

```
az aks create --resource-group humio-observability-sample-group --name humio-observability-aks-cluster --node-count 1
```

Install the Azure Kubernetes Service Command Line Interface if you have not already. 
```
az aks install-cli
```

Configure kubectl to connect to your cluster. 
```
az aks get-credentials --resource-group humio-observability-sample-group --name humio-observability-aks-cluster --overwrite-existing
```

[Install Helm](https://docs.helm.sh/using_helm/#installing-helm) and [give it access](https://docs.helm.sh/using_helm/#example-service-account-with-cluster-admin-role) to the default namespace.
```
brew install kubernetes-helm
kubectl create -f rbac-config.yml
helm init --service-account tiller
```

Deploy the Humio agent. Add your ingest token into the command.
```
helm install stable/fluent-bit --name=humio-agent --set backend.es.http_user=[YourIngestToken] -f humio-agent.yml
```

Deploy the API.

```
kubectl apply -f deployment.yml
# or in the WebApi folder
./kubectl-apply.sh
```

Wait for public IP.
```
kubectl get service observability-sample-service --watch
```

Once the public IP appears you can access the API in your browser. https://[[PublicIP]]:4000/api/dog



## References

- [Humio Kubernetes setup](https://docs.humio.com/integrations/platform-integrations/kubernetes/)
- [Installing Helm](https://docs.helm.sh/using_helm/#installing-helm)
- [Enabling Helm to access the default namespace](https://docs.helm.sh/using_helm/#example-service-account-with-cluster-admin-role)


