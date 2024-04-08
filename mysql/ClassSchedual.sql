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

-- Create dbClassSchedule
CREATE DATABASE IF NOT EXISTS dbClassSchedule;

-- Selecting DB
USE dbClassSchedule;

-- Drop tables in the reverse order of creation
DROP TABLE IF EXISTS TblTechRoom;
DROP TABLE IF EXISTS TblTechClass;
DROP TABLE IF EXISTS TblSchedule;
DROP TABLE IF EXISTS TblClassroom;
DROP TABLE IF EXISTS TblCalendar;
DROP TABLE IF EXISTS TblCourse;
DROP TABLE IF EXISTS TblTechnology;
DROP TABLE IF EXISTS TblProgram;
DROP TABLE IF EXISTS TblUser;
-- added this to test schedule modle and data fetching.
DROP TABLE IF EXISTS TblEvents;



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
    title VARCHAR(255),
    description VARCHAR(255),
    daysOfWeek VARCHAR(10),
    StartTime DATETIME,
    EndTime DATETIME,
    teacher INT,
    Classroom VARCHAR(100),
    program INT,
    FOREIGN KEY (teacher) REFERENCES TblUser(Id) ON DELETE NO ACTION ON UPDATE NO ACTION,
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

-- Create TblSchedule table
CREATE TABLE IF NOT EXISTS TblSchedule (
    IdCalendar INT NOT NULL,
    IdCourse INT NOT NULL,
    IdClassroom INT NOT NULL,
    PRIMARY KEY (IdCalendar, IdCourse, IdClassroom),
    FOREIGN KEY (IdCalendar)
    REFERENCES TblCalendar(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (IdCourse)
    REFERENCES TblCourse(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (IdClassroom)
    REFERENCES TblClassroom(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
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
    IdRoom INT NOT NULL,
    IdTechnology INT NOT NULL,
    PRIMARY KEY (IdRoom, IdTechnology),
    FOREIGN KEY (IdRoom)
    REFERENCES TblClassroom(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION,
    FOREIGN KEY (IdTechnology)
    REFERENCES TblTechnology(Id)
        ON DELETE NO ACTION
        ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- INSERTS
-- Insert data into TblEvents


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
('Gordon', 'Larusic', FALSE, 'gordon', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Matthew', 'Redmond', FALSE, 'matthew', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Ryan', 'McLaren', FALSE, 'ryan', 'AdWTSk6URrGZIonKcPpAecHm5XFGAEcNAT7G32Hf308=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Matthew', 'Craig', FALSE, 'matt', 'gljYbbgsHhLCVHk1SHmEubC8GOzp/c4lDZyULueMr3o=', '0w54NUgsGa53PfCmOt9Lhg=='),
('Hyesun', 'Kwon', FALSE, 'hyesun', 'gljYbbgsHhLCVHk1SHmEubC8GOzp/c4lDZyULueMr3o=', '0w54NUgsGa53PfCmOt9Lhg=='),
('test', 'test', FALSE, 'test', 'iBffC45HMekQwUMnYq4aoZulUda+pCpV379Rznrrf1A=', '0w54NUgsGa53PfCmOt9Lhg==');

INSERT INTO TblEvents (courseCode, courseName, daysOfWeek, startTime, endTime, professor, classroom, program) VALUES
('NFT300', 'Math Class', '1', '2024-04-06 08:00:00', '2024-04-06 09:30:00', 5, 'Room A', 1),
('NJF400', 'English Class', '2', '2024-04-06 10:00:00', '2024-04-06 11:30:00', 6, 'Room B', 2),
('NTG800', 'History Class', '3', '2024-04-06 13:00:00', '2024-04-06 14:30:00', 7, 'Room C', 3);


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
(112, 'TRM');

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

-- Insert data into TblCourse
-- sean 5 gord 6 matt 7 ryan 8
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram) VALUES
('WBP001', 'Data Fundamentals', 60, 8, 1),
('WBP002', 'Introduction to Networking and Security', 60, 6, 1),
('WBP003', 'Introduction to Windows Administration', 60, 7, 1),
('WBP004', 'Logic and Programming I', 60, 5, 1),
('WBP005', 'Professional Practices for IT I', 30, 9, 1),
('WBP006', 'Website Development', 90, 5, 1),
('WBP101', 'Full Stack Programming', 90, 5, 2),
('WBP102', 'Introduction to Hardware and Security', 60, 6, 2),
('WBP103', 'Professional Pratices III', 30, 9, 2),
('WBP104', 'Project Management', 60, 7, 2),
('WBP105', 'Web Application Programming I', 60, 8, 2),
('WEP106', 'Web Design Fundamentals', 60, 10, 2),
('ISM001', 'Fundamentals of Information Systems', 60, 7, 3),
('ISM002', 'Basic Networking Concepts', 90, 6, 3),
('ISM003', 'Introduction to System Administration', 60, 7, 3),
('ISM004', 'IT Security Basics', 60, 6, 3),
('ISM005', 'Computing Essentials', 60, 7, 3),
('ISM006', 'Professional Practices for IT I', 30, 9, 3),
('ISM101', 'Advanced Networking', 60, 6, 4),
('ISM102', 'Advanced System Administration', 60, 7, 4),
('ISM103', 'IT Security Advanced', 60, 7, 4),
('ISM104', 'IT Systems Hardware', 60, 6, 4),
('ISM105', 'Professional Pratices III', 30, 9, 4),
('ISM106', 'Operating Systems', 60, 7, 4);