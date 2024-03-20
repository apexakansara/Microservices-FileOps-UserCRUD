DataService provides API endpoints to retrieve employees data using company name or email filter 
and with pagination for lazy loading. It uses EF Core ORM to interact with Postgres.

It supports XML, JSON and CSV as supported response format from API. It uses ContentNegotiator middleware to provide data in appropriate format. It supports JSON and XML using available formatter and a custom CSV formatter has been added to support CSV as reponse data format. The default data format is JSON.
