-- Sensory Categories table
CREATE TABLE Sensor_Categories (
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Sensor_Model NVARCHAR(255) NOT NULL,
    Measurement_Unit NVARCHAR(255) NOT NULL
);

-- Sensors table
CREATE TABLE Sensors (
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Sensor_Model_Id INT NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Metadata NVARCHAR(MAX) NULL
        CONSTRAINT CK_Sensors_Metadata_JSON CHECK (ISJSON(Metadata) > 0)  -- enforce valid JSON
    FOREIGN KEY (Sensor_Model_Id) REFERENCES Sensor_Categories(Id)
);

-- Sensor Readings table
CREATE TABLE Sensor_Readings (
    Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    Sensor_Id INT NOT NULL,
    Sensor_Value INT NOT NULL,
    Date_Created DATETIME NOT NULL,
    FOREIGN KEY (Sensor_Id) REFERENCES Sensors(Id) ON DELETE CASCADE
);

-- Sensor Relationships table (self-referencing many-to-many)
CREATE TABLE Sensor_Calc_Relationships (
    Sensor_Parent_Id INT NOT NULL,
    Sensor_Child_Id INT NOT NULL,
    PRIMARY KEY (Sensor_Parent_Id, Sensor_Child_Id),
    FOREIGN KEY (Sensor_Parent_Id) REFERENCES Sensors(Id),
    FOREIGN KEY (Sensor_Child_Id) REFERENCES Sensors(Id) 
);

-- Scheduled Runs table
CREATE TABLE Scheduled_Calcs ( -- Does this need an Id?
    Id INT NOT NULL IDENTITY (1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Sensor_Id INT NOT NULL,
    FOREIGN KEY (Sensor_Id) REFERENCES Sensors(Id)
);


CREATE NONCLUSTERED INDEX IDX_Sensor_Readings_SensorDate
ON Sensor_Readings (Sensor_Id, Date_Created);