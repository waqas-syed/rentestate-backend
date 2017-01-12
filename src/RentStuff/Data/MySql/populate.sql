-- MySQL dump 10.13  Distrib 5.7.9, for Win32 (AMD64)
--
-- Host: localhost    Database: rentstuff
-- ------------------------------------------------------
-- Server version	5.7.10-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Dumping data for table `dimension`
--

LOCK TABLES `dimension` WRITE;
/*!40000 ALTER TABLE `dimension` DISABLE KEYS */;
/*!40000 ALTER TABLE `dimension` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `geo_location`
--

LOCK TABLES `geo_location` WRITE;
/*!40000 ALTER TABLE `geo_location` DISABLE KEYS */;
/*!40000 ALTER TABLE `geo_location` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `house`
--

LOCK TABLES `house` WRITE;
/*!40000 ALTER TABLE `house` DISABLE KEYS */;
INSERT INTO `house` VALUES ('ee54b9b4-508b-4bc4-a0c6-8c39c5a76977',10,'dummy@dumdum1234560.com',50000,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.295000,73.415000,'1230','130','Harley Street0',NULL),('201be931-0bc0-4e0c-ae7d-242ede3bfaf1',11,'dummy@dumdum1234561.com',50001,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.300000,73.420000,'1231','131','Harley Street1',NULL),('98359195-a433-468a-8bc5-cccc99c13fc5',12,'dummy@dumdum1234562.com',50002,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.305000,73.425000,'1232','132','Harley Street2',NULL),('895c9325-1190-4cc2-b62d-75fe49f42e19',13,'dummy@dumdum1234563.com',50003,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.310000,73.430000,'1233','133','Harley Street3',NULL),('153f2268-b6cd-4dcf-9c4b-e93c48503859',14,'dummy@dumdum1234564.com',50004,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.315000,73.435000,'1234','134','Harley Street4',NULL),('acfb4f4a-25e9-45b7-9273-44066b54bf95',15,'dummy@dumdum1234565.com',50005,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.320000,73.440000,'1235','135','Harley Street5',NULL),('c4f8b878-c891-402d-9482-26e3f386840e',16,'dummy@dumdum1234566.com',50006,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.325000,73.445000,'1236','136','Harley Street6',NULL),('d55f461b-1e09-4a2f-92a0-455ddba977cb',17,'dummy@dumdum1234567.com',50007,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.330000,73.450000,'1237','137','Harley Street7',NULL),('cfdc1e59-5bcf-44cc-870a-43984dcff564',18,'dummy@dumdum1234568.com',50008,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.335000,73.455000,'1238','138','Harley Street8',NULL),('130621fe-b6b0-4eef-a2eb-a4fb4a6c21ac',19,'dummy@dumdum1234569.com',50009,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.340000,73.460000,'1239','139','Harley Street9',NULL),('43e2b160-aa98-428b-bd38-24a0f98cceec',20,'dummy@dumdum12345610.com',50010,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.345000,73.465000,'12310','1310','Harley Street10',NULL),('b63e7e41-e14d-4e3b-b929-1d11da08ec90',21,'dummy@dumdum12345611.com',50011,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.350000,73.470000,'12311','1311','Harley Street11',NULL),('5fc48397-62de-4bd6-b682-5fff28cca5d4',22,'dummy@dumdum12345612.com',50012,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.355000,73.475000,'12312','1312','Harley Street12',NULL),('5777427f-7f5e-4cde-b1b8-837ced85712b',23,'dummy@dumdum12345613.com',50013,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.360000,73.480000,'12313','1313','Harley Street13',NULL),('83d4a068-ad4a-4448-b28b-2393cb0a26d3',24,'dummy@dumdum12345614.com',50014,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.365000,73.485000,'12314','1314','Harley Street14',NULL),('bcd1dcaf-acb5-4cad-bd0c-d3be0b872395',25,'dummy@dumdum12345615.com',50015,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.370000,73.490000,'12315','1315','Harley Street15',NULL),('f9b7dbbb-1d7e-4799-96bb-e4b7f15b9a69',26,'dummy@dumdum12345616.com',50016,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.375000,73.495000,'12316','1316','Harley Street16',NULL),('205c690e-e45b-47b1-a4ca-e3aec5142019',27,'dummy@dumdum12345617.com',50017,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.380000,73.500000,'12317','1317','Harley Street17',NULL),('263d89ca-adb9-41c9-80e9-3c5ed36a97a7',28,'dummy@dumdum12345618.com',50018,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.385000,73.505000,'12318','1318','Harley Street18',NULL),('de4d5073-ac32-48b8-a8c9-97131bdd122b',29,'dummy@dumdum12345619.com',50019,1,1,1,1,0,1,1,1,1,0,'1',0,NULL,33.390000,73.510000,'12319','1319','Harley Street19',NULL);
/*!40000 ALTER TABLE `house` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `houseimages`
--

LOCK TABLES `houseimages` WRITE;
/*!40000 ALTER TABLE `houseimages` DISABLE KEYS */;
/*!40000 ALTER TABLE `houseimages` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-01-12 15:31:18
