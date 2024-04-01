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
-- Insert data into TblProgram
INSERT INTO TblProgram (Name, Year) VALUES
('IT Web Programming', 1),
('IT Web Programming', 2),
('IT Systems Management and Security', 1),
('IT Systems Management and Security', 2);

-- Insert data into TblUser
INSERT INTO TblUser (FirstName, LastName, SetAsAdmin, UserName, Password, Salt) VALUES
('user', 'admin', TRUE, 'user', 'jSANqsAHN3BKj2onm6KPStvO5O5taYzxBxDQydzgLTM=', '0w54NUgsGa53PfCmOt9Lhg=='),
('pablo', 'admin', TRUE, 'pablo', 'GWftrhCl31KH54HIo5pDKBOZCRDdjxtLxq571rJuycA=', '0w54NUgsGa53PfCmOt9Lhg=='),
('kayla', 'gillam', TRUE, 'kayla', 'TS2OQQ7XOGnvq9C/aS4WNiiGAdMz8osde8xBQl6JvsU=', '0w54NUgsGa53PfCmOt9Lhg=='),
('sean', 'morrow', TRUE, 'sean', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg=='),
('dylan', 'admin', TRUE, 'dylan', 'HCYHoaYJWGyduzsHuU6+s+dsAofaGClontE/t5blwak=','0w54NUgsGa53PfCmOt9Lhg=='),
('Adam', 'Smith', FALSE, 'adam', 'nNKacirUadrGD9npbzM3FpdqobbNucxXwq9TDPHaG7Q=', '0w54NUgsGa53PfCmOt9Lhg==');

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
INSERT INTO TblCourse (Code, Name, Hours, IdProfessor, IdProgram) VALUES
('WBP101', 'Introduction to Web Technologies', 100, 2, 1),
('WBP102', 'Programming Fundamentals', 100, 3, 1),
('WBP103', 'Web Design Principles', 100, 4, 2),
('WBP104', 'Introduction to Databases', 100, 5, 2),
('WBP105', 'Basic Computing and Networking', 100, 2, 1),
('ISM101', 'Fundamentals of Information Systems', 100, 3, 3),
('ISM102', 'Basic Networking Concepts', 100, 4, 3),
('ISM103', 'Introduction to System Administration', 100, 5, 4),
('ISM104', 'IT Security Basics', 100, 2, 4),
('ISM105', 'Computing Essentials', 100, 3, 3);
