CREATE DATABASE MoIA;
USE MoIA;
/*DROP DATABASE MoIA;*/

/* Не принимают */

CREATE TABLE Departament
(
	ID_Departament INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Departament VARCHAR(100) NOT NULL,
    Departament_Creation_Date DATE NOT NULL,
    Departament_Employees_Number INT NOT NULL
);

insert into Departament(Name_Departament, Departament_Creation_Date, Departament_Employees_Number) 
values ('Отдел МВД России по Пресненскому району','2001-12-05','150');

select * from Departament;

CREATE TABLE Code
(
	ID_Code INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Code VARCHAR(30) NOT NULL
);

insert into Code(Name_Code) 
values ('Уголовный');

select * from Code;

CREATE TABLE Category
(
	ID_Category INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Category VARCHAR(30) NOT NULL
);

insert into Category(Name_Category) 
values ('Заявление');

select * from Category;

CREATE TABLE Rankk
(
	ID_Rank INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Rank VARCHAR(30) NOT NULL
);

insert into Rankk(Name_Rank) 
values ('Младший лейтенант');

select * from Rankk;

CREATE TABLE Positionn
(
	ID_Position INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Position VARCHAR(30) NOT NULL
);

insert into Positionn(Name_Position) 
values ('Следователь');

select * from Positionn;

CREATE TABLE Status
(
	ID_Status INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Status VARCHAR(30) NOT NULL
);

insert into Status(Name_Status) 
values ('Открыто');

select * from Status;

CREATE TABLE Investigation_Act
(
	ID_Investigation_Act INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Investigation_Act_Number INT NOT NULL CHECK(length(Investigation_Act_Number)=3),
    Beginning_Date DATE NOT NULL,
    Revealed_Facts VARCHAR(1000) NOT NULL,
    Completion_Date DATE NOT NULL
);

insert into Investigation_Act(Investigation_Act_Number, Beginning_Date, Revealed_Facts, Completion_Date) 
values (597, '2016-12-06', 'Вор', '2017-02-25');

select * from Investigation_Act;

CREATE TABLE Citizen
(
	ID_Citizen INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    First_Name_Citizen VARCHAR(30) NOT NULL,
    Name_Citizen VARCHAR(30) NOT NULL,
    Middle_Name_Citizen VARCHAR(30) DEFAULT '-',
    Login_Citizen VARCHAR(32) NOT NULL CHECK(length(Login_Citizen)>=8),
    Password_Citizen VARCHAR(32) NOT NULL CHECK(regexp_like(Password_Citizen, '[a-zA-Z0-9!@#$%^&*()]')),
    Passport_Series VARCHAR(4) NOT NULL CHECK(regexp_like(Passport_Series, '[0-9][0-9][0-9][0-9]')),
    Passport_Number VARCHAR(6) NOT NULL CHECK(regexp_like(Passport_Number, '[0-9][0-9][0-9][0-9][0-9][0-9]')),
    Citizen_Number VARCHAR(17) UNIQUE CHECK(regexp_like(Citizen_Number, '\\+7\\([0-9]{3}\\)[0-9]{3}\\-[0-9]{2}\\-[0-9]{2}')),
    Citizen_Address VARCHAR(200) NOT NULL,
    Citizen_E_Mail VARCHAR(200) UNIQUE CHECK(Citizen_E_Mail like '%@%.%')
);

insert into Citizen(First_Name_Citizen, Name_Citizen, Middle_Name_Citizen, Login_Citizen, Password_Citizen,
 Passport_Series, Passport_Number, Citizen_Number, Citizen_Address, Citizen_E_Mail) 
values ('Черный', 'Владимир', 'Михайлович', 'Pa$$w0rd', 'Pa$$w0rd', '4468', '515295', '+7(925)511-95-93', 'г. Лотошино, бульвар Славы, 66', 'blackvmih@gmail.com');

select * from Citizen;

/* Принимают */

CREATE TABLE Office
(
	ID_Office INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Office VARCHAR(30) NOT NULL,
    Office_Creation_Date DATE NOT NULL,
    Office_Employees_Number INT NOT NULL,
    Departament_ID INT NOT NULL,
	FOREIGN KEY (Departament_ID) REFERENCES Departament (ID_Departament)
);

insert into Office(Name_Office, Office_Creation_Date, Office_Employees_Number, Departament_ID) 
values ('Транспортная безопасноть', '2006-08-03', 20, 1);

select * from Office;

CREATE TABLE Employee
(
	ID_Employee INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    First_Name_Employee VARCHAR(30) NOT NULL,
    Name_Employee VARCHAR(30) NOT NULL,
    Middle_Name_Employee VARCHAR(30) DEFAULT '-',
    Login_Employee VARCHAR(32) NOT NULL CHECK(length(Login_Employee)>=8),
    Password_Employee VARCHAR(32) NOT NULL CHECK(regexp_like(Password_Employee, '[a-zA-Z0-9!@#$%^&*()]')),
    Rank_ID INT NOT NULL,
    Position_ID INT NOT NULL,
    Office_ID INT NOT NULL,
    FOREIGN KEY (Rank_ID) REFERENCES Rankk (ID_Rank),
    FOREIGN KEY (Position_ID) REFERENCES Positionn (ID_Position),
    FOREIGN KEY (Office_ID) REFERENCES Office (ID_Office)
);

insert into Employee(First_Name_Employee, Name_Employee, Middle_Name_Employee, Login_Employee, Password_Employee,
Rank_ID, Position_ID, Office_ID) 
values ('Бочаров', 'Альберт', 'Львович', 'Pa$$w0rd', 'Pa$$w0rd', 1, 1, 1);

select * from Employee;

CREATE TABLE Candidate
(
	ID_Candidate INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    First_Name_Candidate VARCHAR(30) NOT NULL,
    Name_Candidate VARCHAR(30) NOT NULL,
    Middle_Name_Candidate VARCHAR(30) DEFAULT '-',
    Login_Candidate VARCHAR(32) NOT NULL CHECK(length(Login_Candidate)>=8),
    Password_Candidate VARCHAR(32) NOT NULL CHECK(regexp_like(Password_Candidate, '[a-zA-Z0-9!@#$%^&*()]')),
    Private_Dossier_Number VARCHAR(30) NOT NULL,
    Passport_Series VARCHAR(4) NOT NULL CHECK(regexp_like(Passport_Series, '[0-9][0-9][0-9][0-9]')),
    Passport_Number VARCHAR(6) NOT NULL CHECK(regexp_like(Passport_Number, '[0-9][0-9][0-9][0-9][0-9][0-9]')),
    SNILS VARCHAR(11) UNIQUE NOT NULL CHECK(regexp_like(SNILS, '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')),
    TIN VARCHAR(12) UNIQUE NOT NULL CHECK(regexp_like(TIN, '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')),
    Policy VARCHAR(16) UNIQUE NOT NULL CHECK(regexp_like(Policy, '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')),
    Military_ID_Series VARCHAR(2) NOT NULL CHECK(regexp_like(Military_ID_Series, '[А-Я][А-Я]')),
    Military_ID_Number VARCHAR(7) NOT NULL CHECK(regexp_like(Military_ID_Number, '[0-9][0-9][0-9][0-9][0-9][0-9][0-9]')),
    VPO_Ending_Diploma VARCHAR(30) UNIQUE NOT NULL,
    Service_Weapon_Number VARCHAR(5) DEFAULT '-',
    Service_Weapon_Sort VARCHAR(200) DEFAULT '-',
    Schedule VARCHAR(5) DEFAULT '-' CHECK(regexp_like(Schedule, '[А-Я][А-Я]-[А-Я][А-Я]')),
    Rank_ID INT NOT NULL,
    Position_ID INT NOT NULL,
    Office_ID INT NOT NULL,
    FOREIGN KEY (Rank_ID) REFERENCES Rankk (ID_Rank),
    FOREIGN KEY (Position_ID) REFERENCES Positionn (ID_Position),
    FOREIGN KEY (Office_ID) REFERENCES Office (ID_Office)
);

insert into Candidate(First_Name_Candidate, Name_Candidate, Middle_Name_Candidate, Login_Candidate,
Password_Candidate, Private_Dossier_Number, Passport_Series, Passport_Number, SNILS, TIN, Policy,
Military_ID_Series, Military_ID_Number, VPO_Ending_Diploma, Service_Weapon_Number, Service_Weapon_Sort,
Schedule, Rank_ID, Position_ID, Office_ID) 
values ('Игнатьев', 'Кирилл', 'Максимович', 'Pa$$w0rd', 'Pa$$w0rd', '43243', '4485', '602613', '16658944815',
 '632510802603', '4566010832000109', 'АС', '0937229', '107777 0253595', '10224', ' Пистолет Макарова', 'Пн-Пт', 1, 1, 1);

select * from Candidate;



CREATE TABLE Article
(
	ID_Article INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Name_Article VARCHAR(30) NOT NULL,
    Article_Number INT NOT NULL,
    Code_ID INT NOT NULL,
    FOREIGN KEY (Code_ID) REFERENCES Code (ID_Code)
);

insert into Article(Name_Article, Article_Number, Code_ID) 
values ('Кража', '158', 1);

select * from Article;

CREATE TABLE Appeal
(
	ID_Appeal INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Appeal_Number VARCHAR(13) UNIQUE NOT NULL CHECK(regexp_like(Appeal_Number, '[0-9][0-9][0-9][0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][0-9]')),
    Formation_Date DATE NOT NULL,
    Appeal_Description VARCHAR(1000) NOT NULL,
    Citizen_ID INT NOT NULL,
    Article_ID INT NOT NULL,
    Category_ID INT NOT NULL,
    Employee_ID INT NOT NULL,
    FOREIGN KEY (Citizen_ID) REFERENCES Citizen (ID_Citizen),
    FOREIGN KEY (Article_ID) REFERENCES Article (ID_Article),
    FOREIGN KEY (Category_ID) REFERENCES Category (ID_Category),
    FOREIGN KEY (Employee_ID) REFERENCES Employee (ID_Employee)
);

insert into Appeal(Appeal_Number, Formation_Date, Appeal_Description, Citizen_ID, Article_ID,
Category_ID, Employee_ID) 
values ('6564476-56457', '2016-12-03', 'Кража наручных часов', 1, 1, 1, 1);

select * from Appeal;

CREATE TABLE Dossier
(
	ID_Dossier INT NOT NULL AUTO_INCREMENT PRIMARY KEY,
    Dossier_Number VARCHAR(5) UNIQUE NOT NULL CHECK(regexp_like(Dossier_Number, '[0-9][0-9][0-9][0-9][0-9]')),
	Employee_ID INT NOT NULL,
	Investigation_Act_ID INT NOT NULL,
	Status_ID INT NOT NULL,
	Appeal_ID INT NOT NULL,
    FOREIGN KEY (Employee_ID) REFERENCES Employee (ID_Employee),
    FOREIGN KEY (Investigation_Act_ID) REFERENCES Investigation_Act (ID_Investigation_Act),
    FOREIGN KEY (Status_ID) REFERENCES Status (ID_Status),
    FOREIGN KEY (Appeal_ID) REFERENCES Appeal (ID_Appeal)
);

insert into Dossier(Dossier_Number, Employee_ID, Investigation_Act_ID, Status_ID, Appeal_ID) 
values (10075, 1, 1, 1, 1);

select * from Dossier;






