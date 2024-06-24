CREATE DATABASE PRN221_Asm1;
USE PRN221_Asm1;
-- Tạo bảng Employees với cột Id tự động tăng
CREATE TABLE Employees (
    Id INT IDENTITY(1,1),
    Name NVARCHAR(100),
    Gender NVARCHAR(10),
    Dob DATE,
    Phone VARCHAR(15),
    IDNumber VARCHAR(12),
    PRIMARY KEY(Id)
);

-- Tạo bảng Rooms
CREATE TABLE Rooms (
    Title VARCHAR(10),
    Square TINYINT,
    Floor TINYINT,
    Description NTEXT,
    Comment NTEXT,
    PRIMARY KEY(Title)
);

-- Tạo bảng Services với khóa ngoại
CREATE TABLE Services (
    Id INT IDENTITY(1,1),
    RoomTitle VARCHAR(10) FOREIGN KEY REFERENCES Rooms(Title),
    Month TINYINT,
    Year INT,
    FeeType NVARCHAR(50),
    Amount MONEY,
    PaymentDate DATE,
    Employee INT FOREIGN KEY REFERENCES Employees(Id),
    PRIMARY KEY(Id)
);

insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Nguyen Hong Son','Male', '2003/01/02','0974182365','1');
insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Le Anh Dung','Male', '2003/01/02','0986123123','2');
insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Vuong Cong Minh','Male', '2003/02/03','0947456456','3');
insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Dao The Anh','Male', '2003/05/04','0989789789','4');
insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Ha Anh Dung','Male', '2003/08/06','0353565656','5');
insert into Employees(Name,Gender,Dob,Phone,IDNumber) values('Nguyen Tuyet Nhu','Female', '2003/10/11','0974182365','6');
