# Valmar
> Valmar, the capital of the god-like Valar, held the key to Middle-earth's destiny.  

Similar to its role in the LOTR lore, Valmar aims to be the centralized business service for all skribbl-typo services.  
Valmar implements the persistance and business-service layer, and exposes all functions with an gRPC API.  
Other services can scaffold clients based on the proto definitions.   
This way, there is only one place where logic of new features has to be implemented.

## Status & Roadmap
Valmar is under development and not production-ready.  
The Nest API in toobeeh/Tirith will be refactored along with the progress of Valmar on a development branch.  
As soon as the Tirith-API has fully switched from native Database access to gRPC, Valmar will be deployed.  

Further steps involve integrating more heavy business logic from Palantir into Valmar, and begin a separation of the 
different Palantir components into separate services,  
each independent of another and receiving data from Valmar.

## Architecture
Valmar aims to improve the Domain Driven Design architecture in the typo ecosystem.  
While as a whole, it implements the Domain/Business and Persistence layer in the ecosystem, Valmar itself is structured in individual layers.  

- gRPC Services / Application Layer:  
The services in /Grpc implement the services in the proto definitions.  
They must not include domain-specific functions; just retrieve data from underlying layers and map them to response objects.
- Domain Services / Service Layer:  
The services in /Domain implement the domain logic and access data from the persistence layer.  
- Persistence Layer:  
The persistence layer is scaffolded by efcore tools. 

There are currently no interface definitions for a clean separation of the layers and interchangeability,   
since this application is an "interchangeable" part on its own and components most likely wont need replacement in any scenario.

All layers use dependency injection of mappers, database, loggers and other service dependencies.

## Database
Valmar accesses the MariaDb Palantir Database. 
### Database client scaffolding
` dotnet ef dbcontext scaffold "Name=ConnectionStrings:Palantir" Pomelo.EntityFrameworkCore.MySql -o Database`  
It might be possible that it is requried to add [Key] attributes to some primary keys since the get omitted by naming conventions (?).
