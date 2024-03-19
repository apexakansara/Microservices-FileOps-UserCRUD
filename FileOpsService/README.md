FileOpsService provides ability to upload csv file.After successful upload, RabbitMQ publisher publishes employee information to a queue named **users** with Type as AddEmployee as tag. Using the tag Consumer can decide how to process the message.
Adding tag helps in adding multiple types of messages in single queue.

Note: A dummy API endpoint is added to generate CSV file with given number of rows.
