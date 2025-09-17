/* Inserting Sensor-Aggregate relationships */
-- Sum of all kWh Curtin Buildings over a day
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (7,4)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (7,5)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (7,6)

-- Sum of all kWh Curtin Buildings over a week
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,4)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,5)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,6)

-- Average temp of all Curtin Buildings over a day
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (8,1)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (8,2)
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (8,3)

-- Average temp of Curtin Building B01 over a day
INSERT INTO Sensor_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (10,1)
