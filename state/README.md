# Dapr secrets management

### Run with Dapr using Powershell

1. Open a new terminal window and navigate to `state` directory: 

```powershell
cd F:\DevResources\Dapr\Repos\Tutorials\state
```

2. Run the Dotnet service app with Dapr: 
    
```powershell
cd state-min-api
dapr run --app-id state-app --dapr-http-port 3500 --resources-path ../ -- dotnet run
```

To stop the dapr instance

```powershell
dapr stop --app-id state-app
```
