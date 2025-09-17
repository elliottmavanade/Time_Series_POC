-- Inserts into Sensors for monitoring
INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B01:Temp:Celcius', 'Temperature for Curtin Building 01')

INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B03:Temp:Celcius', 'Temperature for Curtin Building 03')

INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B06:Temp:Celcius', 'Temperature for Curtin Building 06')

INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B01:Delta:kWh', 'Energy delta for Curtin Building 01')

INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B03:Delta:kWh', 'Energy delta for Curtin Building 03')

INSERT INTO Sensors (Sensor_Title, Description)
VALUES ('Curtin:B06:Delta:kWh', 'Energy delta for Curtin Building 06')

-- Aggregation Objects
INSERT INTO Sensors (Sensor_Title, Description, Metadata)
VALUES ('Curtin:All:Total:kWh', 'Energy total for all Curtin buildings over a day', '{"Aggregation":"Sum", "Timespan":1440}')

INSERT INTO Sensors (Sensor_Title, Description, Metadata)
VALUES ('Curtin:All:Avg:Celcius', 'Temperature average for all Curtin buildings over a day', '{"Aggregation":"Avg", "Timespan":1440}')

INSERT INTO Sensors (Sensor_Title, Description, Metadata)
VALUES ('Curtin:All:Total:kWh', 'Energy total for all Curtin buildings over a week', '{"Aggregation":"Sum", "Timespan":10080}')

INSERT INTO Sensors (Sensor_Title, Description, Metadata)
VALUES ('Curtin:B01:Avg:Celcius', 'Temperature average for Curtin building 01 over a day', '{"Aggregation":"Avg", "Timespan":1440}')
