-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/

-- Host: localhost
-- Generation Time: Jan 30, 2024 at 10:00 AM
-- Server version: 8.0.23
-- PHP Version: 7.4.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

-- Drop tables in the reverse order of creation
DROP TABLE IF EXISTS TblTechRoom;
DROP TABLE IF EXISTS TblTechClass;
DROP TABLE IF EXISTS TblClassroom;
DROP TABLE IF EXISTS TblCalendar;
DROP TABLE IF EXISTS TblCourse;
DROP TABLE IF EXISTS TblTechnology;
DROP TABLE IF EXISTS TblEvents;
DROP TABLE IF EXISTS TblProgram;
DROP TABLE IF EXISTS TblUser;



-- Create TblUser table
CREATE TABLE IF NOT EXISTS TblUser (
    Id INT NOT NULL AUTO_INCREMENT,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    SetAsAdmin BOOLEAN NOT NULL,
    UserName VARCHAR(30) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Salt VARCHAR(255) NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;


-- Create TblProgram table
CREATE TABLE IF NOT EXISTS TblProgram (
    Id INT NOT NULL AUTO_INCREMENT,
    Name VARCHAR(60) NOT NULL,
    Year INT NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblEvents table
CREATE TABLE IF NOT EXISTS TblEvents (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    courseCode VARCHAR(255),
    courseName VARCHAR(255),
    daysOfWeek VARCHAR(10),
    startTime DATETIME,
    endTime DATETIME,
    professor INT,
    classroom VARCHAR(100),
    program INT,
    FOREIGN KEY (professor) REFERENCES TblUser(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
    FOREIGN KEY (program) REFERENCES TblProgram(Id) ON DELETE NO ACTION ON UPDATE NO ACTION
);

-- Create TblTechnology table
CREATE TABLE IF NOT EXISTS TblTechnology (
    Id INT NOT NULL AUTO_INCREMENT,
    Description VARCHAR(20) NOT NULL UNIQUE,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblCourse table
CREATE TABLE IF NOT EXISTS TblCourse (
    Id INT NOT NULL AUTO_INCREMENT,
    Code VARCHAR(10) NOT NULL UNIQUE,
    Name VARCHAR(50) NOT NULL,
    Hours INT NOT NULL,
    IdProfessor INT NOT NULL,
    IdProgram INT NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (IdProfessor)
        REFERENCES TblUser(Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION,
    FOREIGN KEY (IdProgram)
        REFERENCES TblProgram(Id)
            ON DELETE NO ACTION
            ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblClassroom table
CREATE TABLE IF NOT EXISTS TblClassroom (
    Id INT NOT NULL AUTO_INCREMENT,
    RoomNumber INT NOT NULL,
    BuildingAcronym CHAR(5) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE (RoomNumber, BuildingAcronym)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblCalendar table
CREATE TABLE IF NOT EXISTS TblCalendar (
    Id INT NOT NULL AUTO_INCREMENT,
    DayWeek VARCHAR(10) NOT NULL,
    StartTime TIME NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblTechClass table
CREATE TABLE IF NOT EXISTS TblTechClass (
    IdCourse INT NOT NULL,
    IdTechnology INT NOT NULL,
    PRIMARY KEY (IdCourse, IdTechnology),
    FOREIGN KEY (IdCourse)
    REFERENCES TblCourse(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (IdTechnology)
    REFERENCES TblTechnology(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Create TblTechRoom table
CREATE TABLE IF NOT EXISTS TblTechRoom (
    IdClassroom INT NOT NULL,
    IdTechnology INT NOT NULL,
    PRIMARY KEY (IdClassroom, IdTechnology),
    FOREIGN KEY (IdClassroom)
    REFERENCES TblClassroom(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (IdTechnology)
    REFERENCES TblTechnology(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Insert data into TblProgram
INSERT INTO TblProgram (Name, Year) VALUES
('IT Web Programming', 1),
('IT Web Programming', 2),
('IT Systems Management and Security', 1),
('IT Systems Management and Security', 2);

-- Insert data into TblUser
INSERT INTO TblUser (FirstName, LastName, SetAsAdmin, UserName, Password, Salt) VALUES
('user', 'admin', TRUE, 'user', 'V3wjYKvYBnCwEn6wIR1HDJbP+P1dz+pgyDbb2UlI95s=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Pablo', 'Santos', TRUE, 'pablo', 'GWftrhCl31KH54HIo5pDKBOZCRDdjxtLxq571rJuycA=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Kayla', 'Gillam', TRUE, 'kayla', 'TS2OQQ7XOGnvq9C/aS4WNiiGAdMz8osde8xBQl6JvsU=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Dylan', 'MacCormack', TRUE, 'dylan', 'HCYHoaYJWGyduzsHuU6+s+dsAofaGClontE/t5blwak=','0w54NUgsGa53PfCmOt9Lhg=='),
('Sean', 'Morrow', FALSE, 'sean', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Ryan', 'McLaren', FALSE, 'ryan', 'AdWTSk6URrGZIonKcPpAecHm5XFGAEcNAT7G32Hf308=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Gordon', 'Larusic', FALSE, 'gordon', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Matthew', 'Redmond', FALSE, 'matthew', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
-- ('Matthew', 'Craig', FALSE, 'matt', 'gljYbbgsHhLCVHk1SHmEubC8GOzp/c4lDZyULueMr3o=', '0w54NUgsGa53PfCmOt9Lhg=='),
-- ('Hyesun', 'Kwon', FALSE, 'hyesun', 'gljYbbgsHhLCVHk1SHmEubC8GOzp/c4lDZyULueMr3o=', '0w54NUgsGa53PfCmOt9Lhg=='),
('test', 'test', FALSE, 'test', 'iBffC45HMekQwUMnYq4aoZulUda+pCpV379Rznrrf1A=', '0w54NUgsGa53PfCmOt9Lhg==');

-- Insert data into TblEvents
-- INSERT INTO TblEvents (courseCode, courseName, daysOfWeek, startTime, endTime, professor, classroom, program) VALUES
-- ('NFT300', 'Math Class', '1', '2024-04-06 08:00:00', '2024-04-06 09:30:00', 5, 'Room A', 1),
-- ('NJF400', 'English Class', '2', '2024-04-06 10:00:00', '2024-04-06 11:30:00', 6, 'Room B', 2),
-- ('NTG800', 'History Class', '3', '2024-04-06 13:00:00', '2024-04-06 14:30:00', 7, 'Room C', 3);


-- Insert data into TblTechnology
INSERT INTO TblTechnology (Description) VALUES
('Projector'),
('TV Touch'),
('Low PCs'),
('Moderate PCs'),
('High PCs');

-- Insert data into TblClassroom
INSERT INTO TblClassroom (RoomNumber, BuildingAcronym) VALUES
(308, 'TRF'),
(312, 'TRF'),
(309, 'TRF'),
(311, 'TRF'),
(307, 'TRM');

-- Insert data into TblCalendar
-- 'DayWeek' is ENUM('Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday')
INSERT INTO TblCalendar (DayWeek, StartTime) VALUES
('Monday', '08:30:00'),
('Monday', '10:30:00'),
('Monday', '13:30:00'),
('Tuesday', '08:30:00'),
('Tuesday', '10:30:00'),
('Tuesday', '14:30:00'),
('Wednesday', '08:30:00'),
('Wednesday', '10:30:00'),
('Wednesday', '13:30:00'),
('Thursday', '08:30:00'),
('Thursday', '10:30:00'),
('Thursday', '13:30:00'),
('Friday', '08:30:00'),
('Friday', '10:30:00'),
('Friday', '13:30:00');

-- x.x.x.x.x.x.x.x --- IT Web Programming --- x.x.x.x.x.x.x.x
-- Second Year IT Web Programming - Courses Fourth term
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
VALUES 
    ('INFT3000', 'Capstone', 60, 5, 2),
    ('WEBD3027', 'Developing for Content Management Systems (CMS)', 60, 6, 2),
    ('ELCT3001', 'Self-directed Study', 60, 5, 2), 
    ('ELCT3002', 'Special Topics', 60, 5, 2), 
    ('WEBD3000', 'Web Application Programming II', 90, 6, 2);

-- -- Second Year IT Web Programming - Courses Third term
-- INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
-- VALUES 
--     ('PROG3017', 'Full Stack Programming', 90, 5, 2),
--     ('NETW2001', 'Introduction to Hardware and Security', 60, 6, 2), -- Assumed Code
--     ('INFT2100', 'Project Management', 60, 6, 1),
--     ('INET2005', 'Web Application Programming I', 60, 5, 2),
--     ('WEBD3100', 'Web Design Fundamentals', 60, 6, 2);

-- First Year IT Web Programming - Courses Second term 
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
VALUES 
    ('PROG2700', 'Client Side Programming', 90, 5, 1),
    ('PROG1400', 'Introduction to Object Oriented Programming', 60, 5, 1),
    ('SAAD1001', 'Introduction to Systems Analysis and Design', 60, 5, 1),
    ('OSYS1000', 'Operating System - Linux', 60, 6, 1),
    ('DESN1700', 'User Interface Design and Development', 60, 6, 1), 
    ('OSYS2040', 'Web Server Fundamentals', 60, 6, 1);

-- -- First Year IT Web Programming - Courses First term
-- INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
-- VALUES 
--     ('DBAS1007', 'Data Fundamentals', 60, 1, 1),
--     ('NETW1027', 'Introduction to Networking and Security', 60, 1, 1),
--     ('OSYS1200', 'Introduction to Windows Administration', 60, 1, 1),
--     ('PROG1700', 'Logic and Programming 1', 60, 1, 1),
--     ('WEBD1000', 'Website Development', 90, 1, 1);


-- x.x.x.x.x.x.x.x --- IT Systems Management and Security --- x.x.x.x.x.x.x.x
-- Second Year IT Systems Management and Security - Fourth Term
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
VALUES 
    ('INFT3001', 'Capstone', 60, 7, 4),
    ('INFT4100', 'Work Experience', 60, 7, 4),
    ('NETW3500', 'Enterprise Management and Automation', 60, 8, 4),
    ('NETW3700', 'Hierarchical Network Infrastructure', 60, 8, 4),
    ('OSYS1200', 'Advanced Windows Administration', 90, 8, 4);

-- -- Second Year IT Systems Management and Security - Third Term
-- INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
-- VALUES 
--     ('INFT2700', 'IT Project Quality Assurance', 90, 8, 4),
--     ('NETW2710', 'Introduction to Cloud Computing and Server Virtualization', 60, 8, 4),
--     ('NETW2500', 'NOS Administration - Windows II', 60, 7, 4),
--     ('OSYS1200', 'Introduction to Windows Administration', 60, 7, 4),
--     ('CSTN4015', 'Help Desk and Customer Support', 60, 7, 4); 

-- First Year IT Systems Management and Security - Second Term
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
VALUES 
    ('PROG1700', 'Logic and Programming', 60, 8, 3),
    ('NETW1500', 'Introduction to NOS Administration', 60, 8, 3),
    ('OSYS3030', 'Network Services Using Linux', 90, 8, 3),
    ('NETW2700', 'Network Infrastructure', 60, 7, 3),
    ('ISEC2700', 'Introduction to Information Security Practices', 60, 7, 3);
    
-- -- First Year IT Systems Management and Security - First Term
-- INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram)
-- VALUES 
--     ('DBAS1007', 'Data Fundamentals', 60, 8, 3),
--     ('HDWR1700', 'Introduction to Hardware and Security', 60, 8, 3),
--     ('NETW1027', 'Introduction to Networking and Security', 60, 7, 3),
--     ('WEBD1000', 'Website Development', 60, 7, 3),
--     ('OSYS1000', 'Operating Systems - Linux', 90, 7, 3);


-- x.x.x.x.x.x.x --- Old insert data into TblCourse ---- x.x.x.x.x.x.x
-- -- Insert data into TblCourse
-- -- sean 5 gord 6 matt 7 ryan 8
-- INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram) VALUES
-- ('WBP001', 'Data Fundamentals', 60, 8, 1),
-- ('WBP002', 'Introduction to Networking and Security', 60, 6, 1),
-- ('WBP003', 'Introduction to Windows Administration', 60, 7, 1),
-- ('WBP004', 'Logic and Programming I', 60, 5, 1),
-- ('WBP006', 'Website Development', 90, 5, 1),
-- ('WBP101', 'Full Stack Programming', 90, 5, 2),
-- ('WBP102', 'Introduction to Hardware and Security', 60, 6, 2),
-- ('WBP104', 'Project Management', 60, 7, 2),
-- ('WBP105', 'Web Application Programming I', 60, 8, 2),
-- ('WEP106', 'Web Design Fundamentals', 60, 10, 2),
-- ('ISM001', 'Fundamentals of Information Systems', 60, 7, 3),
-- ('ISM002', 'Basic Networking Concepts', 90, 6, 3),
-- ('ISM003', 'Introduction to System Administration', 60, 7, 3),
-- ('ISM004', 'IT Security Basics', 60, 6, 3),
-- ('ISM005', 'Computing Essentials', 60, 7, 3),
-- ('ISM006', 'Professional Practices for IT I', 30, 9, 3),
-- ('ISM101', 'Advanced Networking', 60, 6, 4),
-- ('ISM102', 'Advanced System Administration', 60, 7, 4),
-- ('ISM103', 'IT Security Advanced', 60, 7, 4),
-- ('ISM104', 'IT Systems Hardware', 60, 6, 4),
-- ('ISM105', 'Professional Pratices III', 30, 9, 4),
-- ('ISM106', 'Operating Systems', 60, 7, 4);

-- Insert data into TblTechRoom
INSERT INTO TblTechRoom (IdClassroom, IdTechnology) VALUES
    -- For Room 308
    (1, 1), -- Projector
    (1, 2), -- TV Touch
    (1, 4), -- Moderate PCs
    -- For Room 307 
    (2, 1), -- Projector
    (2, 5), -- High PCs
    -- For Room 311
    (3, 1), -- Projector
    (3, 3), -- Low PCs
    -- For Room 309
    (4, 1), -- Projector
    (4, 4), -- Moderate PCs
    -- For Room 312
    (5, 1), -- Projector
    (5, 4); -- Moderate PCs
    
-- Insert data into TblTechClass
-- x.x.x.x.x.x.x.x --- IT Web Programming --- x.x.x.x.x.x.x.x
-- IT Web Programming - Second Year Courses Fourth term
INSERT INTO TblTechClass (IdCourse, IdTechnology) VALUES
    (1, 1), -- Capstone - Projector (Room 308)
    (1, 2), -- Capstone - TV Touch (Room 308)
    (1, 4), -- Capstone - Moderate PCs (Room 308)
    (2, 1), -- Developing for Content Management Systems (CMS) - Projector (Room 311)
    (2, 3), -- Developing for Content Management Systems (CMS) - Low PCs (Room 311)
    (3, 1), -- Self-directed Study - Projector (Room 312)
    (3, 4), -- Self-directed Study - Moderate PCs (Room 312)
    (4, 1), -- Special Topics - Projector (Room 309)
    (4, 4), -- Special Topics - Moderate PCs (Room 309)
    (5, 1), -- Web Application Programming II - Projector (Room 309)
    (5, 4); -- Web Application Programming II - Moderate PCs (Room 309)

-- IT Web Programming - First Year Courses Second term
INSERT INTO TblTechClass (IdCourse, IdTechnology) VALUES
    (6, 1), -- Client Side Programming - Projector (Room 309)
    (6, 4), -- Client Side Programming - Moderate PCs (Room 309)
    (7, 1), -- Introduction to Object Oriented Programming - Projector (Room 311)
    (7, 3), -- Introduction to Object Oriented Programming - Low PCs (Room 311)
    (8, 1), -- Introduction to Systems Analysis and Design - Projector (Room 308)
    (8, 2), -- Introduction to Systems Analysis and Design - TV Touch (Room 308)
    (8, 4), -- Introduction to Systems Analysis and Design - Moderate PCs (Room 308)
    (9, 1), -- Operating System - Linux - Projector (Room 312)
    (9, 4), -- Operating System - Linux - Moderate PCs (Room 312)
    (10, 1), -- User Interface Design and Development - Projector (Room 309)
    (10, 4), -- User Interface Design and Development - Moderate PCs (Room 309)
    (11, 1), -- Web Server Fundamentals - Projector (Room 311)
    (11, 3); -- Web Server Fundamentals - Low PCs (Room 311)

-- IT Systems Management and Security - Second Year Fourth Term
INSERT INTO TblTechClass (IdCourse, IdTechnology) VALUES
    (12, 1), -- Capstone - Projector (Room 312)
    (12, 4), -- Capstone - Moderate PCs (Room 312)
    (13, 1), -- Work Experience - Projector (Room 309)
    (13, 4), -- Work Experience - Moderate PCs (Room 309)
    (14, 1), -- Enterprise Management and Automation - Projector (Room 308)
    (14, 2), -- Enterprise Management and Automation - TV Touch (Room 308)
    (14, 4), -- Enterprise Management and Automation - Moderate PCs (Room 308)
    (15, 1), -- Hierarchical Network Infrastructure - Projector (Room 311)
    (15, 3), -- Hierarchical Network Infrastructure - Low PCs (Room 311)
    (16, 1), -- Advanced Windows Administration - Projector (Room 312)
    (16, 4); -- Advanced Windows Administration - Moderate PCs (Room 312)

-- IT Systems Management and Security - First Year Second Term
INSERT INTO TblTechClass (IdCourse, IdTechnology) VALUES
    (17, 1), -- Logic and Programming - Projector (Room 309)
    (17, 4), -- Logic and Programming - Moderate PCs (Room 309)
    (18, 1), -- Introduction to NOS Administration - Projector (Room 311)
    (18, 3), -- Introduction to NOS Administration - Low PCs (Room 311)
    (19, 1), -- Network Services Using Linux - Projector (Room 312)
    (19, 4), -- Network Services Using Linux - Moderate PCs (Room 312)
    (20, 1), -- Network Infrastructure - Projector (Room 309)
    (20, 4), -- Network Infrastructure - Moderate PCs (Room 309)
    (21, 1), -- Introduction to Information Security Practices - Projector (Room 308)
    (21, 2), -- Introduction to Information Security Practices - TV Touch (Room 308)
    (21, 4); -- Introduction to Information Security Practices - Moderate PCs (Room 308)

-- Capstone (IT Web Programming, 2nd Year) - Room: 308 (TRF) - Tecnologias: Projector, TV Touch, Moderate PCs
-- Developing for Content Management Systems (CMS) - Room: 311 (TRF) - Tecnologias: Projector, Low PCs
-- Self-directed Study - Room: 312 (TRF) - Tecnologias: Projector, Moderate PCs
-- Special Topics - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Web Application Programming II - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Client Side Programming - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Introduction to Object Oriented Programming - Room: 311 (TRF) - Tecnologias: Projector, Low PCs
-- Introduction to Systems Analysis and Design - Room: 308 (TRF) - Tecnologias: Projector, TV Touch, Moderate PCs
-- Operating System - Linux - Room: 312 (TRF) - Tecnologias: Projector, Moderate PCs
-- User Interface Design and Development - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Web Server Fundamentals - Room: 311 (TRF) - Tecnologias: Projector, Low PCs
-- Capstone (IT Systems Management and Security, 2nd Year) - Room: 312 (TRF) - Tecnologias: Projector, Moderate PCs
-- Work Experience - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Enterprise Management and Automation - Room: 308 (TRF) - Tecnologias: Projector, TV Touch, Moderate PCs
-- Hierarchical Network Infrastructure - Room: 311 (TRF) - Tecnologias: Projector, Low PCs
-- Advanced Windows Administration - Room: 312 (TRF) - Tecnologias: Projector, Moderate PCs
-- Logic and Programming - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Introduction to NOS Administration - Room: 311 (TRF) - Tecnologias: Projector, Low PCs
-- Network Services Using Linux - Room: 312 (TRF) - Tecnologias: Projector, Moderate PCs
-- Network Infrastructure - Room: 309 (TRF) - Tecnologias: Projector, Moderate PCs
-- Introduction to Information Security Practices - Room: 308 (TRF) - Tecnologias: Projector, TV Touch, Moderate PCs
