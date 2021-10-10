CREATE TABLE CITY (
City_ID				int IDENTITY(1,1) NOT NULL PRIMARY KEY,
City_Name			varchar(35)
);

CREATE TABLE USER_TYPE (
User_Type			tinyint IDENTITY(1,1) NOT NULL PRIMARY KEY,
Type_Description		varchar(20)
);

CREATE TABLE ANIMAL_TYPE (
Type_Number			int IDENTITY(1,1) NOT NULL PRIMARY KEY,
Type_Name			varchar(35)
);

CREATE TABLE USERS (
User_Number			int IDENTITY(1,1) NOT NULL,
User_Type			tinyint,
User_Name			varchar(35),
User_Surname			varchar(35),
CONSTRAINT PK_USERS PRIMARY KEY (User_Number),
CONSTRAINT FK_USERS_USER_TYPE FOREIGN KEY (User_Type) REFERENCES USER_TYPE (User_Type)
);

CREATE TABLE PASSWORDS (
User_Number			int IDENTITY(1,1) NOT NULL,
Hash				varchar(50),
Salt				varchar(50),
CONSTRAINT PK_PASSWORDS PRIMARY KEY (User_Number),
CONSTRAINT FK_PASSWORDS_USER FOREIGN KEY (User_Number) REFERENCES USERS (User_Number)
);

CREATE TABLE ADOPTER (
Adopter_Number		int IDENTITY(1,1) NOT NULL,
Adopter_Surname		varchar(35),
Adopter_Name			varchar(35),
Adopter_Contact_Number	char(10),
Adopter_Street_Number	varchar(6),
Adopter_Street_Name		varchar(50),
City_ID				int,
CONSTRAINT PK_ADOPTER PRIMARY KEY (Adopter_Number),
CONSTRAINT FK_ADOPTER_CITY FOREIGN KEY (City_ID) REFERENCES CITY (City_ID)
);

CREATE TABLE ANIMAL (
Animal_Number		int IDENTITY(1,1) NOT NULL,
Animal_Name			varchar(35),
Type_Number			int,
CONSTRAINT PK_ANIMAL PRIMARY KEY (Animal_Number),
CONSTRAINT FK_ANIMAL_ANIMAL_TYPE FOREIGN KEY (Type_Number) REFERENCES ANIMAL_TYPE (Type_Number)
);

CREATE TABLE ADOPTION (
Adoption_Number		int IDENTITY(1,1) NOT NULL,
Adopter_Number		int,
Animal_Number		int,
User_Number			int,
Result_Description		varchar(10),
Adoption_Date			date,
CONSTRAINT PK_ADOPTION PRIMARY KEY (Adoption_Number),
CONSTRAINT FK_ADOPTION_ADOPTER FOREIGN KEY (Adopter_Number) REFERENCES ADOPTER (Adopter_Number),
CONSTRAINT FK_ADOPTION_ANIMAL FOREIGN KEY (Animal_Number) REFERENCES ANIMAL (Animal_Number),
CONSTRAINT FK_ADOPTION_USERS FOREIGN KEY (User_Number) REFERENCES USERS (User_Number)
);
