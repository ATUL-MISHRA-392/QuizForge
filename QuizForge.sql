-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: quizforge
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `attemptanswers`
--

DROP TABLE IF EXISTS `attemptanswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `attemptanswers` (
  `AttemptAnswerId` int NOT NULL AUTO_INCREMENT,
  `AttemptId` int NOT NULL,
  `QuestionId` int NOT NULL,
  `SelectedOptionId` int DEFAULT NULL,
  `IsCorrect` tinyint(1) DEFAULT '0',
  `TimeTaken` int DEFAULT '0',
  PRIMARY KEY (`AttemptAnswerId`),
  KEY `AttemptId` (`AttemptId`),
  KEY `QuestionId` (`QuestionId`),
  KEY `SelectedOptionId` (`SelectedOptionId`),
  CONSTRAINT `attemptanswers_ibfk_1` FOREIGN KEY (`AttemptId`) REFERENCES `quizattempts` (`AttemptId`),
  CONSTRAINT `attemptanswers_ibfk_2` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`),
  CONSTRAINT `attemptanswers_ibfk_3` FOREIGN KEY (`SelectedOptionId`) REFERENCES `quizoptions` (`OptionId`)
) ENGINE=InnoDB AUTO_INCREMENT=31 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `categories`
--

DROP TABLE IF EXISTS `categories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categories` (
  `CategoryId` int NOT NULL AUTO_INCREMENT,
  `CategoryName` varchar(100) NOT NULL,
  PRIMARY KEY (`CategoryId`),
  UNIQUE KEY `CategoryName` (`CategoryName`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questions` (
  `QuestionId` int NOT NULL AUTO_INCREMENT,
  `CategoryId` int NOT NULL,
  `QuestionText` text NOT NULL,
  `ImagePath` varchar(500) DEFAULT NULL,
  `Difficulty` varchar(20) DEFAULT 'Medium',
  `TimeLimit` int DEFAULT '30',
  PRIMARY KEY (`QuestionId`),
  KEY `CategoryId` (`CategoryId`),
  CONSTRAINT `questions_ibfk_1` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quizattempts`
--

DROP TABLE IF EXISTS `quizattempts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quizattempts` (
  `AttemptId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `CategoryId` int NOT NULL,
  `Score` int DEFAULT '0',
  `TotalQuestions` int DEFAULT '0',
  `CorrectAnswers` int DEFAULT '0',
  `WrongAnswers` int DEFAULT '0',
  `SkippedAnswers` int DEFAULT '0',
  `StartedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  `CompletedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`AttemptId`),
  KEY `UserId` (`UserId`),
  KEY `CategoryId` (`CategoryId`),
  CONSTRAINT `quizattempts_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`),
  CONSTRAINT `quizattempts_ibfk_2` FOREIGN KEY (`CategoryId`) REFERENCES `categories` (`CategoryId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quizhistory`
--

DROP TABLE IF EXISTS `quizhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quizhistory` (
  `HistoryId` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `AttemptId` int NOT NULL,
  `Score` int DEFAULT '0',
  `Percentage` decimal(5,2) DEFAULT '0.00',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`HistoryId`),
  KEY `UserId` (`UserId`),
  KEY `AttemptId` (`AttemptId`),
  CONSTRAINT `quizhistory_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`),
  CONSTRAINT `quizhistory_ibfk_2` FOREIGN KEY (`AttemptId`) REFERENCES `quizattempts` (`AttemptId`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quizoptions`
--

DROP TABLE IF EXISTS `quizoptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `quizoptions` (
  `OptionId` int NOT NULL AUTO_INCREMENT,
  `QuestionId` int NOT NULL,
  `OptionText` varchar(500) NOT NULL,
  `IsCorrect` tinyint(1) DEFAULT '0',
  PRIMARY KEY (`OptionId`),
  KEY `QuestionId` (`QuestionId`),
  CONSTRAINT `quizoptions_ibfk_1` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`)
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `FullName` varchar(100) NOT NULL,
  `Email` varchar(150) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Role` varchar(20) DEFAULT 'Student',
  `CreatedAt` datetime DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-27 18:43:53
