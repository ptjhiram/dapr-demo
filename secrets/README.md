# Dapr secrets management

### Run with Dapr using Powershell

1. Open a new terminal window and navigate to `my-components` directory: 

```bash
cd F:\DevResources\Dapr\Repos\Tutorials\my-components
```

2. Run the Dotnet service app with Dapr: 
    
```powershell
cd ./order-processor
dapr run --app-id myapp --dapr-http-port 3500 --resources-path ../
```

To stop the dapr instance

```powershell
dapr stop --app-id myapp
```
