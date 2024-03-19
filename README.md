# Microservices-FileOps-UserCRUD
Project follows micro-services architecture using .NET Core to create three microservices - FileOpsService, ConsumerService and DataService.
1. FileOpsService - It deals with file operations where user can upload employee information using csv files. Each employee information is then published to RabbitMQ broker by RabbitMQ publisher.
2. ConsumerService - It consumes any messages available in configured queue and based on the type of message in message body, it uses switch case to call respective message handler. One included message type, read and add employee information to Postgres using EFCore.
3. DataService - It provides endpoints to access employee information using REST Apis.

Integrated swagger with each micro-service to interect with API endpoints. 

for the ease of linking all three services are created as a single repository, in real life all three services should be separate repository to be able to develop, deploy and scale independently.

**System Requirements**
1. .Net Core 7.0
2. Postgres 16
3. RabbitMQ 3.13

