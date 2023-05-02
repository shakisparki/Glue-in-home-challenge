# GlueHome - Platform Technical Task

---

### Instructions on How To Run and Notes at end of Read ME

---

## Background

Your company has decided to create a delivery system to connect users from the consumer market to partners from the logistics business sector.

You are responsible for building a Web API that will be used by partners and users to create, manage and execute deliveries.

### Business Requirements

* The API should support all CRUD operations (create, read, update, delete).
* A delivery must handle 5 different states: `created`, `approved`, `completed`, `cancelled`, `expired`
* Users may `approve` a delivery before it starts.
* Partner may `complete` a delivery, that is already in `approved` state.
* If a delivery is not `completed` during the access window period, then it should expire. 
* Either the partner or the user should be able to cancel a pending delivery (in state `created` or `approved`).

A delivery should respect the following structrure:

```json
{
    "state": "created",
    "accessWindow": {
        "startTime": "2019-12-13T09:00:00Z",
        "endTime": "2019-12-13T11:00:00Z"
    },
    "recipient": {
        "name": "John Doe",
        "address": "Merchant Road, London",
        "email": "john.doe@mail.com",
        "phoneNumber": "+44123123123"
    },
    "order": {
        "orderNumber": "12209667",
        "sender": "Ikea"
    }
}
```

### Technical Requirements

* All code should be written in C# and target the .NET core, or .NET framework library version 4.5+.
* Please check all code into a public accessible repository on GitHub and send us a link to your repository.

Feel free to make any assumptions whenever you are not certain about the requirements, but make sure your assumptions are made clear either through the design or additional documentation.

### Bonus
* Application logging
* Documentation
* Containerization
* Authentication
* Testing
* Data storage
* Partner facing Pub/Sub API for receiving state updates.
* Anything else you feel may benefit your solution from a technical perspective.

### How To Run

* Source code only provided. Written on Visual Studio 2019, .NET 5.0.
To test recommended to Run on VS2019. Open Solution, Select TT.Deliveries.Web.Api as Startup Project.
Ideally Run with Kestrel, Should pop up swagger on localhost:5001 TLS.
Will need to deploy/publish to run in some prod type environment.

* State expiration job is a seperate hosted service, that can be run on some serverless PaaS e.g. Lambda, AzureFunctions, WebJobs etc. or as a separate process/service. 
Uses CRON to specify frequency of execution. Currently set to every 1 min. 
To test recommended to Run on VS2019. Open Solution, Select TT.Deliveries.CronJobs as Startup Project.
Click Run.

### Notes
* DB is SQLlite as lightweight for test, ideally will connect to some cloud DB. EFCore will provision and migrate db on dev environment at startup, this will also create the db if non-existent. Connectionstrings, secrets etc are provided in apseetings.Dev. In prod will ideally be passed from a KeyVault etc.

* Can use a DbConnectionFactory to support databases of multiple Providers e.g. CosmosDB, and pass not only connection string but dataProvider name as argument.

* Data Repository / Unit of Work pattern for EFCore is not encouraged, so avoided using this.

* to run update: dotnet ef migrations add "AddDeliveryTable" -s ..\Web\TT.Deliveries.Web.Api\TT.Deliveries.Web.Api.csproj


* should we validate the order number on POST operations? - should multiple deliveries of the same order be possible?  Maybe, if e.g. order has to be redelivered?

* can add support for a Json Query language e.g. JMESPath

* separate api state controller created to apply different authorization, as to exposing an endpoint such as {Id}/{State}

* to avoid localization/globalization/culture/region issue, we assume all times are in UTC. Alternatives are using TimeZoneInfo to convert times between regions

* logging logs to console only, can be modified to log to file or other sinks

*  also possible for State Expiration job to expire delivery states via Delivery API rather than direct db access

* Using JWT bearer token authentication is probably the right way to go, but basic authentication demonstrates the concept due to time constraints

* More Unit and Integration tests should definitely be written. Time contraints didnt permit this.

* Api documentation can be automatically generated from OpenAPI specification and XML comments.

* Would love to have added some dockerfiles but had only Sunday and Monday (Thank God for Bank Holiday) to work on the entire thing. 

* Please, Please, Please let it Pass the techincal task !! :)
