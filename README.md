# Time Series POC


#### Problem Statement
---
We have a time series dataset that contains hundreds of millions of rows of data and its expanding by 100s of thousands of rows daily. The data is coming from multiple sources and is in different formats and is eventually displayed to a dashboard containing multiple visualizations and statistics for different sources. The computations are becomming extremely expensive as the dataset grows, how can this be optimized?

#### My Approach
---
Under the assumptions that the database has been designed in the best possible way, To optimize the computations on a large and growing time series dataset, I took the following system design approach:
- Insert into the table an aggregate sensor that describes the Location ( eg., Room05, Building01, Curtin, etc) and its associated aggregation and time interval (in minutes).
- Create a table (Scheduled_Calcs) to store aggregated calculations for different time intervals (e.g., daily, weekly) for different buildings/clients. 
- Create a second table (Sensor_Calc_Relationships) to map which sensors are involved in generating the aggregated calculations.
- Set up a scheduled Function App that runs at specific intervals (eg., daily at midnight), retrieves the scheduled calculations list, and adds to a Service Bus Queue. This is to make it scalable and fault tolerant.
- Setup another Function App that is triggered by messages in the Service Bus Queue. This Function App will:
  - Retrieve the message from the queue.
  - Fetch the relevant sensors from the Sensor_Calc_Relationships table.
  - Fetch the parent sensor details from the Sensors table to get the aggregation type and time interval.
  - Retrieve the time series data for the specified child sensors from the Sensor_Readings table over the time interval specified by the parent sensor.
  - Perform the necessary aggregate calculations on the data from these sensors over the specified time interval.
  - Store the results into the Sensor_Readings table with the parent sensor ID and the calculated value.
- Create an API to act as a front door to the database, to which both the producer and consumer will use to interact with the database. This will help to abstract the database layer from the producer and consumer and also help to implement security and rate limiting if required.

![Architecture Diagram](./diagrams/TimeSeriesPOCArch.png "Architecture Diagram")

#### Database Schema
---
All table scripts can be located [here](./SQLScripts)
The tables and sample data used in this design are as follows:
- **Sensor_Categories**: This table stores information on the sensors model and its unit of measurement along with an identifier.	
	- | Id | Sensor_Model | Measurement_Unit 
		|----|--------------|------------------|
		| 1  | Thermostat   | C                |
		| 2  | EnergyMeter  | kWh              |

- **Sensor**: This table stores information about each sensor, including its type (FK to Sensor_Categories), its location and a Json object to store its optional metadata (eg., Aggregation, Timespan). The location is string that can be seperated on ':' to location the place (eg., Curtin, UWA, etc), the building (eg., B01, B02, etc) and extended further where required. In a production scenario, the location would be its own table and Sensors would contain an FK to the ID.
	- | Id | Sensor_Model_Id (FK Sensor_Categories [Id]) | Location   | Metadata                              
		|----|---------------------------------------------|------------|---------------------------------------|
		| 1  | 1                                           | Curtin:B01 |                                       |
		| 2  | 1                                           | Curtin:B03 |                                       |
		| 3  | 2                                           | Curtin:B01 |                                       |
		| 4  | 2                                           | Curtin:B03 |                                       |
		| 5  | 2                                           | Curtin:B06 |                                       |
		| 6  | 2                                           | Curtin:B01 | {"Aggregation":"Sum", "TimeSpan":1440}|
		| 7  | 1                                           | Curtin:    | {"Aggregation":"Avg", "TimeSpan":1440}|
		| 8  | 2                                           | Curtin:    | {"Aggregation":"Sum", "TimeSpan":10080}|
		| 9  | 2                                           | Curtin:    | {"Aggregation":"Sum", "TimeSpan":1440}|

- **Sensor_Readings**: This is the table from the problem statement and is responsible for storing all time series data. It is composed of an Id, Sensor_Id (FK to Sensors), Sensor_Value, and Date_Created. Note: The sample data here doesn't show any aggregate calculation entries.
	- | Id | Sensor_Id (FK Sensors [Id]) | Sensor_Value | Date_Created        
		|----|------------------------------|--------------|---------------------|
		| 1  | 1                            | 21           | 19/09/2025 12:00 am |
		| 2  | 2                            | 20           | 19/09/2025 1:00 am  |
		| 3  | 3                            | 2            | 19/09/2025 2:00 am  |
		| 4  | 4                            | 1            | 19/09/2025 2:30 am  |
		| 5  | 5                            | 2            | 19/09/2025 3:00 am  |
		| 6  | 5                            | 1            | 19/09/2025 5:00 am  |
		| 7  | 4                            | 2            | 19/09/2025 10:00 am |
		| 8  | 3                            | 1            | 19/09/2025 4:00 pm  |
		| 9  | 2                            | 20           | 19/09/2025 12:00 am |
		| 10 | 1                            | 21           | 19/09/2025 12:00 am |

- **Scheduled_Calcs**: This table represents scheduled jobs that occur to solve the problem statement, it stores the name of the job and the sensor_id (FK to Sensors) for the aggregate function.
	- | Id | Name | Sensor_Id (FK Sensors [Id]) 
		|----|--------------------------------|----------------------------|
		| 1  | Calc_Curtin: Avg_Temp_Day      | 7                          |
		| 2  | Calc_Curtin:B01_Sum_Energy_Day | 6                          |
		| 3  | Calc_Curtin: Sum_Energy_Day    | 9                          |
		| 4  | Calc_Curtin: Sum_Energy_Week   | 8                          |

- **Sensor_Calc_Relationships**: This table describes all the child sensors that are required to create the aggregate sensor. It is composed of a Sensor_Parent_Id (FK to Sensors) and Sensor_Child_Id (FK to Sensors).
	- | Parent_Sensor_Id (FK Sensor [Id])| Child_Sensor_Id (FK Sensor [Id]) 
		|--------------------------------|--------------------------------|
		| 6                              | 3                              |
		| 7                              | 1                              |
		| 7                              | 3                              |
		| 9                              | 3                              |
		| 9                              | 4                              |
		| 9                              | 5                              |
		| 8                              | 9                              |
		
Below is a DB Design:
![DB Design Diagram](./diagrams/TimeSeriesPOCDbDesign.png "DB Design Diagram")

#### Decisions Made
---
- **API Layer**:
	- I chose to implement an API layer to abstract the database layer from the producer and consumer. This was done for multiple reasons:
		1. Separation of Concerns:
			- Function Apps only focus on orchestration and processing, not on how data is stored or retrieved.
			- Removal of duplicate code in multiple Function Apps to query the database.
		2. Consistent Centralized Business Logic:
			- The API ensures all consumers see the same view of the data.
			- Any changes to the database schema or business logic only need to be made in one place.
		3. Security:
			- Reduce surface area of attack by limiting direct database access.
		4. Scalability:
			- The API abstracts the database, allowing for changes to be made without requiring changes in consumers.
	- This has also been split up into different controllers and services to ensure single responsibility and separation of concerns.
		- Some controllers and services have not been implemented as they are out of scope, these include:
			- ClientController/ClientService: This would perform CRUD operations for clients.
			- DashboardController/DashboardService: This would retrieve data for the dashboard.
	- **Improvements**:
		- Below are some improvements that were out of scope for this POC but should be included in a production scenario:
			1. Implement caching to reduce database load for frequently accessed data.
			2. Add rate limiting to prevent abuse and ensure fair usage.
			3. Implement logging and monitoring to track API usage and performance.
			4. Add authentication and authorization to secure the API endpoints.
			5. Implement pagination for endpoints that return large datasets.
			6. Unit Tests and Integration Tests to ensure the API works as expected.
- **Producer Function App**:
	- I chose to use a Function App to act as a producer to add messages to the Service Bus Queue. This was done for multiple reasons:
		1. Scalability:
			- Function Apps can scale out automatically based on demand, making them suitable for handling varying workloads. With this being a predicted workload, we can define the autoscaling rules.
		2. Integration with Azure Services:
			- Function Apps have built-in integrations with various Azure services, including Service Bus, making it easier to implement the producer-consumer pattern.
	- **Improvements**:
		- Below are some improvements that were out of scope for this POC but should be included in a production scenario:
			1. When querying for scheduled calculations, implement pagination to start producing messages to the queue as soon as possible.
			2. Use an Orchestrator Function to create a timer and trigger the producer. This could allow for different aggregation intervals for different jobs and making the system more flexible.
			3. Implement logging and monitoring to track job execution and performance.
			4. Add error handling and retry logic to handle transient failures.
			5. Implement unit tests and integration tests to ensure the Function App works as expected.
- **Consumer Function App**:
	- I chose to use a Function App to act as a consumer to process messages from the Service Bus Queue. This was done for multiple reasons:
		1. Scalability:
			- Function Apps can scale out automatically based on demand, making them suitable for handling varying workloads. With this being a predicted workload, we can define the autoscaling rules.
		2. Integration with Azure Services:
			- Function Apps have built-in integrations with various Azure services, including Service Bus, making it easier to implement the producer-consumer pattern.
		3. Event-Driven Architecture:
			- Function Apps are designed for event-driven architectures, allowing them to respond to events (like new messages in a queue) efficiently.
	- **Improvements**:
		- Below are some improvements that were out of scope for this POC but should be included in a production scenario:
			1. There is an argument to be made that there is a lot of back and fourth communication between the API and consumer and that this could contribute to overhead. For a production scenario I would run tests to see the overheads and decide if there is value to merging the requests.
			2. Implement logging and monitoring to track message processing and performance.
			3. Add error handling and retry logic to handle transient failures.
			4. Implement unit tests and integration tests to ensure the Function App works as expected.

- **Database Design**:
	- The tables described are a very simplified version over what a production database woud look like. There is a lot of context and sample data missing that could pave ways for a better design.
	- **Improvements**:
		1. The Location column in the Sensors table should be its own table and the Sensors table would just hold a FK to the new Locations table.
		2. This POC only used the client Curtin and 3 of its buildings for the Sensor table, in reality there would be much more sensor models, units of measurement and locations.
		3. The metadata stored in Sensor table could be done differently given more context.


### How to Run Locally
---
##### Azure Service Bus Emulator
This solution uses the Azure Service Bus Emulator [(Azure Service Bus Emulator Overview)](https://learn.microsoft.com/en-us/azure/service-bus-messaging/overview-emulator) and is required for local testing. Navigate to [here](./emulator) and run ```docker compose up```. This will start the Service Bus Emulator. An error I faced after setting up the emulator is my Function Apps wouldn't work without running a Storage Account Emulator, to fix this I ran this command: ```docker run -p 10000:10000 -p 10001:10001 -p 10002:10002 mcr.microsoft.com/azure-storage/azurite```.

##### The Database
This solution uses the localDB on SQLServer, go to your SQL Server Management Studio and create a database called ```TimeSeriesPoc```. From here locate the [SQL scripts](./SQLScripts) to create tables and hydrate the database with sample data.

#### Startup Projects
Configure the startup project profile to include the Api, ProducerFA, and ConsumerFA. From here you can start the solution. The producer will run every 5 minutes and runs on initial startup. From here the producer will communicate with the API for a list of jobs and place these items on the Service Bus Queue. This will trigger the consumer to from the API the relationships, the sensor details and all the time series data for the query. It will then calculate the aggregation and write that to the database.

To validate this has worked, select from the Sensor_Readings table and see that the aggregation sensor_ids have been inserted and are correct.