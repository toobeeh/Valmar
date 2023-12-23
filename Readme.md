# Valmar
> Valmar, the capital of the god-like Valar, held the key to Middle-earth's destiny.  

Similar to its role in the LOTR lore, Valmar aims to be the centralized business service for all skribbl-typo services.  
Valmar implements the persistance and business-service layer, and exposes all functions with an gRPC API.  
Other services can scaffold clients based on the proto definitions.   
This way, there is only one place where logic of new features has to be implemented.

## Database
Valmar accesses the MariaDb palantir Database. 
### Database client scaffolding
` dotnet ef dbcontext scaffold "Name=ConnectionStrings:Palantir" Pomelo.EntityFrameworkCore.MySql -o Database`