# Link_Shortener
A URL shortening service built using .NET CORE with a NoSQL Database.

## Functionality
On creating a new shortened link the original link is hashed to 10 characters, this is used as the _id in the database. The original link is also stored along with the last DateTime of it being accessed.

To retrieve a link that has been shortened a get request is sent to the API with the _id of the link as the parameter, it then retrieves the link from the cache or if it's not cached then the DB. The last accessed time is also updated to be used for clearing old unused links.

There is also rate limiting on the endpoint for creating a link to timeout anyone making excessive requests to the API.

## Services
Currently implemented is using a MongoDB and a local in memory cache but I have mocked out the implementation for using distributed Redis cache and a Dynamo Database for deployment to a cloud environment.

## Deployment
Current configuration is for a local deployment but is designed to be able to be deployed into a could environment with ease.\
For localhosting a MongoDB instance will need to be running.\
A service such as AWS ECS would be ideal for this app.\
Deployment to the cloud would include moving to using the Redis cache for distributed caching and moving to an RDS DynamoDB.

## Future Development
Obviously fleshing out the Redis and Dynamo services would be the first step on moving this app to a cloud deployable service.\
After that moving the "frontend" out of the same project as the API and into it's own project and deployment, likely an S3 bucket as it's a simple static site that serves only to call the API.\
Adding an additional worker service to run daily to remove dead links that haven't been accessed in say 30 days would help to keep the DB size to a minimum, and ths costs down.
