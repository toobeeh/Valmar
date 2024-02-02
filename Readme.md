# Valmar
> Valmar, the capital of the god-like Valar, held the key to Middle-earth's destiny.  

Similar to its role in the LOTR lore, Valmar aims to be the centralized business service for all skribbl-typo services.  
Valmar implements the persistance and business-service layer, and exposes all functions with an gRPC API.  
Other services can scaffold clients based on the proto definitions.   
This way, there is only one place where logic of new features has to be implemented.

## Status & Roadmap
Valmar is has just left beta testing and is already used in production!  
The Nest API in toobeeh/Tirith has been refactored and solely uses Valmar instead of native database access.  
Valmar is currently deployed on the same server as the remaining typo ecosystem and not publicy exposed, since it has no (and will never have any) sort of authentication. 

Further steps involve integrating more heavy business logic from Palantir into Valmar, and begin a separation of the 
different Palantir components into separate services, each independent of another and receiving data from Valmar.  
Another goal is to get rid of the domain code from Ithil-Rebirth as well and fully switch to Valmar.

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

### Performance
Each service should have the goal to create granular access to its entities and not waste resources to calculate/fetch data that might not be needed by the majority of its consumers.  


### Entities / DTOs
Following layers should expose following entity types:
- Application layer: GRPC response classes
- Domain Layer: Entities, JSON Models for partial entities or "Domain Data Objects"/Ddos.

gRPC requests/responses have to be only used in the application layer.  
If suitable JSON Models exists, they may be used instead of creating dedicated Ddo classes.
Ddo classes should be used to combine data from multiple domains to a single response.

The application layer should only use Automapper to convert Domain layer entities to Application layer entities.   
This engages that the domain layer produces ready-to-use entities and decouples it from the application layer.  
The primary objective of the auto-mapping should be stripping properties and mapping to slightly different structure.

The domain layer should not use Automapper to deal more explicitly with attribute name/type conversion.
Automapper is useful for converting objects with similar signature, but the domain layer should rather produce full-featured objects than "stripped" objects.

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
