-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: rentstuff
-- ------------------------------------------------------
-- Server version	5.7.18-log

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
-- Dumping data for table `__migrationhistory`
--

LOCK TABLES `__migrationhistory` WRITE;
/*!40000 ALTER TABLE `__migrationhistory` DISABLE KEYS */;
/*!40000 ALTER TABLE `__migrationhistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetroles`
--

LOCK TABLES `aspnetroles` WRITE;
/*!40000 ALTER TABLE `aspnetroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserclaims`
--

LOCK TABLES `aspnetuserclaims` WRITE;
/*!40000 ALTER TABLE `aspnetuserclaims` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserclaims` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserlogins`
--

LOCK TABLES `aspnetuserlogins` WRITE;
/*!40000 ALTER TABLE `aspnetuserlogins` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserlogins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetuserroles`
--

LOCK TABLES `aspnetuserroles` WRITE;
/*!40000 ALTER TABLE `aspnetuserroles` DISABLE KEYS */;
/*!40000 ALTER TABLE `aspnetuserroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('4372711b-c9b9-4e9b-8579-171aea5e13bd','waqas.shah.syed@gmail.com','Syed Waqas',1,'AKOVmJI0jdZN3gQHgHdyxATIAXlcANydsbMVAl4G4kbv7innimbzwQjPhkt/kr6qRg==','c74bf7b2-a35a-4ebe-b955-41e8ad68dd6a',NULL,0,0,NULL,0,0,'waqas.shah.syed@gmail.com',0,NULL);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `dimension`
--

LOCK TABLES `dimension` WRITE;
/*!40000 ALTER TABLE `dimension` DISABLE KEYS */;
INSERT INTO `dimension` VALUES ('050c4ae9-d285-4460-a0fa-f547bf468fd3','0','12',0.0000),('0b81c746-22fe-4bcf-ab62-f5ca0b6a8fa4','1','2',0.0000),('3297a0fc-84b9-4ede-bd0c-1019ca1d6174','0','7',0.0000),('38c08a6f-18ff-46f2-974d-2c7e69617d39','1','2',0.0000),('4297e917-47b1-41bc-836e-61e5f0760e58','0','8',0.0000),('7ce71a87-c3db-4228-890a-f8a963932fab','1','2',0.0000),('8a3e3960-236a-4836-a411-e0bf891e71a6','0','8',0.0000),('9c75cfc6-957f-42b7-ae42-9ca11b52a0aa','0','9',0.0000),('a9c34a1e-1df3-470d-ae0f-ba94c4801186','0','15',0.0000),('e5f62cef-1313-490e-b2f7-0fbf483f5545','0','12',0.0000);
/*!40000 ALTER TABLE `dimension` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `house`
--

LOCK TABLES `house` WRITE;
/*!40000 ALTER TABLE `house` DISABLE KEYS */;
INSERT INTO `house` VALUES ('da7d5b8d-0419-4819-a638-7b6a31f2b8f3',32,'3','3','waqas.shah.syed@gmail.com',33333,0,3,3,0,0,0,0,0,'House','0','03325329974',33.698188000,72.978535300,NULL,NULL,'E-11, Islamabad, Pakistan','38c08a6f-18ff-46f2-974d-2c7e69617d39','Fakhir',1),('7b6fa086-ee9d-4964-ac6e-69c99d6d360f',34,'5','5','waqas.shah.syed@gmail.com',100232,1,2,1,0,1,0,0,1,'House','0','03325329974',31.470679000,74.257522000,NULL,NULL,'Johar Town, Lahore, Pakistan','a9c34a1e-1df3-470d-ae0f-ba94c4801186','Mustansar',0),('dccde092-9efa-4bd7-88aa-1ab016f9b469',35,'6','6','waqas.shah.syed@gmail.com',100,0,0,0,0,0,0,0,0,'Apartment','0','03455138018',31.532209300,74.287191700,NULL,NULL,'Faizan ISF, Dipalpur-Okara Road, Lahore, Pakistan',NULL,'Mustaqeem',0),('3bc2684f-b19b-40f1-94b6-20d9f18edab3',36,'7','7','waqas.shah.syed@gmail.com',77777,7,7,7,1,0,0,1,1,'Apartment','0','03325329974',31.554233200,74.320211100,NULL,NULL,'Queen\'s Road, Lahore, Pakistan','3297a0fc-84b9-4ede-bd0c-1019ca1d6174','Sitara',1),('e886233b-1c31-476a-9d06-1f4f12ff7ac0',37,'8','8','waqas.shah.syed@gmail.com',88888,8,8,8,0,1,1,0,1,'Hotel','0','03455138018',33.359288900,72.946056600,NULL,NULL,'Rawalpindi Tehsil, Pakistan','4297e917-47b1-41bc-836e-61e5f0760e58','Inayat',1),('63726f25-a7a3-4111-9098-2183ffcba544',38,'9','9','waqas.shah.syed@gmail.com',9999,9,9,9,0,1,1,0,1,'Hostel','0','03325329974',25.052900100,66.912910900,NULL,NULL,'Hub, Pakistan','9c75cfc6-957f-42b7-ae42-9ca11b52a0aa','Waqas',1),('2f27b391-c4c8-4f9a-aea4-536f1fd845cf',39,'10','10','waqas.shah.syed@gmail.com',1010,2,1,1,0,0,0,0,0,'Apartment','0','03455138018',30.880034500,73.600330800,NULL,NULL,'Renala Khurd, Pakistan','0b81c746-22fe-4bcf-ab62-f5ca0b6a8fa4','Tahir',0),('6f0102bd-445e-4424-82a5-dc3c205a6180',40,'11','11','waqas.shah.syed@gmail.com',11111,0,0,0,1,1,0,1,1,'Hostel','1','03325329974',33.644012400,73.063809500,NULL,NULL,'Saidpur Rd, Rawalpindi, Pakistan',NULL,'Bala',0),('dc65274c-27d8-46f3-ae87-095c9c2eb955',41,'12','12','waqas.shah.syed@gmail.com',1222,12,12,12,0,1,1,0,1,'Hostel','0','03455138018',33.609320800,73.059731100,NULL,NULL,'Liaquat Rd, Rawalpindi, Pakistan','050c4ae9-d285-4460-a0fa-f547bf468fd3','Liaqat Sir',1),('58de3dc7-b881-49b3-9284-acadabd8a2a7',42,'13','13','waqas.shah.syed@gmail.com',1313,0,0,0,0,0,0,0,0,'House','0','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'waqas',0),('42da6798-d96c-404c-b700-a38b97edc272',43,'14','14','waqas.shah.syed@gmail.com',1414,0,0,0,0,0,0,0,1,'House','0','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'Saad',0),('9df4efd8-cf27-4906-8e59-2616952e4f4c',44,'15','15','waqas.shah.syed@gmail.com',1515,0,0,0,1,1,1,0,1,'House','3','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'Sir Sajjad',0),('55181f00-7346-4055-a484-f5c4bcd94714',45,'16','16','waqas.shah.syed@gmail.com',1616,0,0,0,1,1,0,1,1,'Apartment','0','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'Saif',0),('4eb1172c-b1b3-4e42-983b-24b0ab764e42',46,'17','17','waqas.shah.syed@gmail.com',1717,0,0,0,0,1,0,0,1,'Hostel','0','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'Fareed',0),('9e4f8da9-9882-46d9-bcd5-4fe3b0896b3f',47,'18','18','waqas.shah.syed@gmail.com',1818,0,0,0,0,1,0,0,1,'Hostel','0','03325329974',33.598828200,73.053809700,NULL,NULL,'Saddar, Rawalpindi, Pakistan',NULL,'Inaam',0);
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

-- Dump completed on 2017-07-06 12:08:21
