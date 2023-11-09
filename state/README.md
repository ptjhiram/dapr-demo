# Dapr secrets management

### Run with Dapr using Powershell

1. Open a new terminal window and navigate to `state` directory: 

    ```powershell
    cd F:\DevResources\Dapr\Repos\Tutorials\state
    ```

2. Initialize state stores.

    * In-Memory -

        * Replace contents in `statestore.yaml` with:
            ```yaml
            apiVersion: dapr.io/v1alpha1
            kind: Component
            metadata:
                name: statestore
            spec:
                type: state.in-memory
                version: v1
                metadata: []
            ```

    * Mongo - 

        * Pull docker image:
            ```yaml
            docker pull mongo
            ```

        * Start container
            ```powershell
            docker run --name dapr-mongo -p 27017:27017 -d mongo
            ```
            Replace contents in `statestore.yaml` with:
            ```yaml
            apiVersion: dapr.io/v1alpha1
            kind: Component
            metadata:
                name: statestore
            spec:
                type: state.mongodb
                version: v1
                metadata:
                - name: host
                  value: 127.0.0.1:27017
            ```
    * MySQL - 
        * Pull docker image:
            ```yaml
            docker pull mysql
            ```
        * Start container
            ```powershell
            docker run --env=MYSQL_ROOT_PASSWORD=dapr-demo -p 3306:3306 -p 33060:33060 -d mysql
            ```
            Replace contents in `statestore.yaml` with:
            ```yaml
            apiVersion: dapr.io/v1alpha1
            kind: Component
            metadata:
                name: statestore
            spec:
                type: state.mysql
                version: v1
                metadata:
                - name: connectionString
                  value: "root:dapr-demo@tcp(localhost:3306)/?allowNativePasswords=true"
            ```

2. Run the Dotnet service app with Dapr: 
    
    ```powershell
    cd state-min-api
    dapr run --app-id state-api --dapr-http-port 3500 --resources-path ../ -- dotnet run
    ```

To stop the dapr instance

    ```powershell
    dapr stop --app-id state-api
    ```


# Inspecting the State Store Containers

### MySql
Docker container reference can be found here: [MySql](https://hub.docker.com/_/mysql)

Open bash shell in container:
```powershell
docker exec -it dapr-mysql bash
```

When bash termminal opens (i.e. `bash-4.4#`), execute `mysql` command
```bash
mysql -h localhost -u root -p
```
You will be prompted to enter the password used when starting the container (i.e. `dapr-demo`)

Bash prompt should be replaced with `mysql>` for you to begin executing queries. [MySql Documentation](https://dev.mysql.com/doc/refman/8.2/en/introduction.html)

To show a list of databases,
```
SHOW DATABASES;
```

The default Dapr database `dapr_state_store` should be listed. Open the database
```
USE dapr_state_store
```

Dapr will create 2 tables in that database: `dapr_metadata` and `state`. To show a list of tables in that database
```
SHOW TABLES;
```

To list all the records in the `state` table
```
SELECT * FROM state
```

To confirm the key is in the database
```
SELECT * FROM state WHERE id = 'state-api||key1';
```
The "`id`" is based on a combination of the app id used when starting the Dapr instance (i.e. `dapr run --app-id state-api ...`) and the key name sperated by double pipes (`||`).

To end mysql session
```
QUIT
```

Then to exit container bash shell
```
exit
```

### MongoDB
Docker container reference can be found here: [Mongo](https://hub.docker.com/_/mongo)

Open bash shell in container:
```powershell
docker exec -it dapr-mongo bash
```

When bash termminal opens (i.e. `root@<CONTAINER ID>:/#`), open the mongo shell
```bash
mongosh
```

Bash prompt should be replaced with the default database `test` for you to begin executing queries. [Mongo Documentation](https://www.mongodb.com/docs/manual/)

To show a list of databases,
```
show dbs
```

The default Dapr database `daprStore` should be listed. Open the database
```
use daprStore
```

To show a list of collections (tables) in that database
```
show collections
```

To list all the records in the `state` table
```
db.daprCollection.find()
```

To confirm the key is in the database
```
db.daprCollection.find({"_id": "state-api||key1"})
```
The "`_id`" is based on a combination of the app id used when starting the Dapr instance (i.e. `dapr run --app-id state-api ...`) and the key name sperated by double pipes (`||`).

To end mysql session
```
quit
```

Then to exit container bash shell
```
exit
```
