/* Inserting Sensor-Aggregate relationships */
-- Sum of Curtin:B01 kWh over a day
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (6,3)

-- Sum of Curtin: Celcius over a day
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (7,1)
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (7,2)

-- Sum of all kWh Curtin Buildings over a day
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,3)
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,4)
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (9,5)

-- Sum of all kWh Curtin Buildings over a week
INSERT INTO Sensor_Calc_Relationships (Sensor_Parent_Id, Sensor_Child_Id)
VALUES (8,9)
