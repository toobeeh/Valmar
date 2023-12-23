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
different Palantir components into separate services, each independent of another and receiving data from Valmar.

## Database
Valmar accesses the MariaDb Palantir Database. 
### Database client scaffolding
` dotnet ef dbcontext scaffold "Name=ConnectionStrings:Palantir" Pomelo.EntityFrameworkCore.MySql -o Database`