-- Inserts into Sensors for monitoring
INSERT INTO Sensors (Sensor_Model_Id, Location)
VALUES (1, 'Curtin:B01')

INSERT INTO Sensors (Sensor_Model_Id, Location)
VALUES (1, 'Curtin:B03')

INSERT INTO Sensors (Sensor_Model_Id, Location)
VALUES (2, 'Curtin:B01')

INSERT INTO Sensors (Sensor_Model_Id, Location)
VALUES (2, 'Curtin:B03')

INSERT INTO Sensors (Sensor_Model_Id, Location)
VALUES (2, 'Curtin:B06')

-- Aggregation Objects
INSERT INTO Sensors (Sensor_Model_Id, Location, Metadata)
VALUES (2, 'Curtin:B01', '{"Aggregation":"Sum", "Timespan":1440}')

INSERT INTO Sensors (Sensor_Model_Id, Location, Metadata)
VALUES (1, 'Curtin:', '{"Aggregation":"Avg", "Timespan":1440}')

INSERT INTO Sensors (Sensor_Model_Id, Location, Metadata)
VALUES (2, 'Curtin:', '{"Aggregation":"Sum", "Timespan":10080}')

INSERT INTO Sensors (Sensor_Model_Id, Location, Metadata)
VALUES (2, 'Curtin:', '{"Aggregation":"Sum", "Timespan":1440}')