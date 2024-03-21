# Valmar
[![part of Typo ecosystem](https://img.shields.io/badge/Typo%20ecosystem-Valmar-blue?style=flat&logo=data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIAAAACACAYAAADDPmHLAAACV0lEQVR4nO3dPUrDYByA8UQ8g15AI+gsOOnmrufoIBT0DAUFB+/R3bFTobOCwQvoJSouNcObhHyZ9n2eHwiirW3Th79J2iaJJEmSJEmSJIC06iGu1+vgz9M0Df9CY6t8PkP2fMrYDADOAOAMAM4A4OrWGl3bj0Pp8+wEgDMAuP2uD//w7I6+DEf19fbc6eadAHAGAGcAcAYAZwBwnbcCTrIj+jL8Fx/55yA34wSAMwA4A4AzADgDgDMAOAOAMwC4zjuCzi+uN9+fZgeNrvuefw+69FfL10H/fgycAHAGAGcAcAYAZwBwnbcCioZeq2+quIVS5NbBHycAnAHARffRsOksr71Ml38Bi/mk9XVH5EfDFGYAcHVbAWWjw08NbyePEaRmDADOAOAMAM4A4Fq9FjCd5cG1zaeHrPeleXnzsvl+MZ802vooe4fSatn9ftUILp/iYxlCm51UTgA4A4Dr9eXgsv3wtJdfhx71fXICwBkAXGUAv+cLCH0pHk4AOAOAMwA4A4AzALhedwRpXBVneSu9X04AOAOAMwA4A4AzADgDgDMAOAOAMwA4A4AzADgDgDMAOAOAMwA4A4AzALio3xG0bUcu3UZOADgDgDMAOAOAMwC4qLcCRjxG0M5wAsAZAJwBwBkAnAHAGQCcAcAZAJwBwBkAnAHA+Y4gOCcAnAHAGQCcAcAZAFyrrYDH++NGl7+6ZZ0yZpc4AeAMAC66HUFDnLwyZk4AOAOAKz+QfMXx58dScdz7se5o8A7t0HJzAtAZAJwBwBkAnAFIkiRJkiRJUtySJPkBweNXgRaWkYQAAAAASUVORK5CYII=)](https://github.com/topics/skribbl-typo)
> Valmar, the capital of the god-like Valar, held the key to Middle-earth's destiny.  

Similar to its role in the LOTR lore, Valmar aims to be the centralized domain service for all skribbl-typo services.  
Valmar implements the persistance and domain-service layer, and exposes all functions with a gRPC API.  
Other services can scaffold clients and types easily based on the proto definitions.   
This way, there is only one place where logic of new features needs to be implemented.

## Brief ecosystem overview
Following graphic illustrates the current state of the typo ecosystem:
  
![typo ecosystem](https://i.imgur.com/JN5AoEM.jpg)  
  
It can be seen that domain/persistance layer is implemented multiple times; this is unnecessarily duplicated code resulting in unmaintainable code, higher difficulty to implement new features uniformly, and makes it incredibly easy to let errors slip in one of the many implementations.  
Valmar will act in future as the only implementation of domain & persistance layer, like it is currently done in Tirith. More in the Roadmap section.

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
Domain services should be designed to enable the application layer to re-use fetched data to avoid fetching the same data multiple times.  
Therefore, adding parameters - primitives or domain objects - to the service functions is encouraged, instead fetching an object from another service in the service method.
- Persistence Layer:  
The persistence layer is scaffolded by efcore tools. 

All layers use dependency injection of mappers, database, loggers and other service dependencies.

## Design guidelines

### Performance
Each service should have the goal to create granular access to its entities and not waste resources to calculate/fetch data that might not be needed by the majority of its consumers.  
Generally speaking, Valmar should not cache any data.
An exception are drops, which are the main performance bottleneck of the whole application. 

Following concept should be implemented to handle drops:
### Drop Chunks with Octree access
The drop table is a huge table with a lot of data (a couple millions of drops).
To handle this, the table should be split into chunks.
The chunks should be stored in an octree structure, where each chunk is a leaf node in the tree.
Each node in the octree is either an abstraction of its child nodes, or a leaf node which is assigned to a range of drop IDs in the database.   

The abstraction nodes calculate the stats of their children and store (cache) that, to save performance cost.  
Initially, abstraction nodes are dirty. When a read request is made, the node calculates the stats (recursively) from its child nodes, saves them, marks itself as clean, and returns the value.  
For the next read operation, the node can directly return the stats without recalculating them.  
When a node encounters a writing access, it marks the stats as dirty, which is propagated downwards to the chunks/nodes that are requried for the write access.
At the next read access, the dirty nodes will have to recalculate the stats from its child nodes.

The leaf nodes are assigned to a range of drop IDs and calculate their stats directly from the database.
During the initialization of the octree, the initializer creates a parent abstraction node and starts loading chunks of size n.
It adds the chunks to the current head node; when the head node is filled up with eight chunks it moves the chunks to a new child node and therefore has seven new chunks available. 

This uses the temporal locality of drop accesses, especially the write accesses - new drops and redeeming league drops. 
Most chunks will remain clean and when frequent write accesses occur, it is likely that they are affecting a number of chunks in the same area of the octree due to drop events.
The chunksize should be chosen to be as large as possible to minimize the amount of nodes in the octree, but as small as necessary to keep the amount of dirty nodes low.

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
