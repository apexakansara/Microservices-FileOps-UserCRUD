ConsumerService works as message consumer for RabbitMQ broker. IT consumes messages from **users** queue and based on the tag message will be processed differently.
Using prefetch count as 1, it processes messages one by one and once a message is acknowledge then only broker will send a new message to consumer.
Using EFCore it will save employee information to PostgreSQL. For replacing EFCore with any other database handler like micro-ORM dapper or ADO.NET, it needs to implement IRepository.
If it encounters any issue during message processing, message will be negatively acknowledged and queued to dead letter queue named as **dead-queue**. Dead letter messages will be consumed and logged for the analysis purpose.
