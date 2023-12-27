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

All layers use dependency injection of mappers, database, loggers and other service dependencies.

## Design guidelines

### Entities / DTOs
gRPC requests/responses have to be only used in the application layer.  
In the domain/persistence layer, the efcore entities and additional classes for data encapsulation are used. 
To map entities from domain to application layer, the IMapper interface has to be used.

### Exception handling 
gRPC exceptions should be only thrown in the application layer.  
Exceptions that occur during domain logic should use exception classes located in /Domain/Exceptions.  
They will be mapped to gRPC exceptions with appropriate status code in the exception interceptor.  

In most of the cases, it is not needed to throw exceptions in the application layer at all.  
Domain exceptions are handled in the domain layer, and static validation is performed via the validator configuration.  

### Interfaces for development  
Since writing tests (and TDD) for this project is out of scope for a one-man-team, interfaces are used to guideline feature implementation.  
- Proto definitions should be the first step to start a new feature. This ensures consideration of the client's needs.  
- Considering the proto definitions, an interface for a new domain service (or to extend an existing) should be created to satisfy needed functions to make the service work.
- The gRPC service class can be created, depending on the new domain service interface. If the interface is insufficient, extend it.
- The domain service can now be implemented and added to the dependency injection. 
- Mappers and validators should be written and added to dependency injection.

This workflow ensures a design process where the domain logic follows last - preventing possibly lacking implementations - and a clean implementation chain where the project is able to compile at any time.

## Database
Valmar accesses the MariaDb Palantir Database. 
### Database client scaffolding

Using the entity framework and its cli tools, the persistence layer can be easily scaffolded:  
`dotnet ef dbcontext scaffold "Name=ConnectionStrings:Palantir" Pomelo.EntityFrameworkCore.MySql -o Database`  
It might be possible that it is required to add [Key] attributes to some primary keys since they get omitted due to naming conventions (might be an early .net8 bug?).
