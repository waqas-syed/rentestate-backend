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
INSERT INTO `dimension` VALUES ('02010e6f-ccba-4126-b027-af871e199652','0','3',0.0000),('0dea2eef-96af-440a-a492-2b6d456ef968','0','15',0.0000),('0e4ec18a-7d8d-4e48-a5c7-efdcda9afae5','1','4',0.0000),('1abb7f1d-4fb7-4351-9992-4f477c666210','1','2',0.0000),('1b58db66-ad31-4db2-b27f-afaf55a655c3','1','8',0.0000),('3590db25-a03d-45bb-b8b8-e69e75d95b77','1','10',0.0000),('4f1702da-d19c-417c-b7e5-c2eaacb148da','0','11',0.0000),('6c6546ab-e22d-4530-84bc-c32d74931719','1','14',0.0000),('7f4ddf56-b5cd-4e85-9384-9449224f4ea9','1','6',0.0000),('83784a1c-8803-4261-9377-bdf660c980cf','1','16',0.0000),('9d26a2b6-d3f8-4231-8e80-9df5394a21e2','0','1',0.0000),('a83cc140-e5a7-4275-a8c3-b7dda14bb65c','0','13',0.0000),('b58c6fca-e0ee-4536-ae8a-03ef01f938e1','1','12',0.0000),('c80e6423-51b3-450d-8cf6-7bf3ff41a9a5','0','7',0.0000),('ccdc836a-b814-40a1-b5d7-37a4c8e97ec6','1','0',0.0000),('cd13db31-0f36-44ab-9f1e-8f621d0b0fe0','0','9',0.0000),('fbce7372-75dc-411f-bf02-9fc13749fafb','0','5',0.0000);
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
INSERT INTO `house` VALUES ('26cae14e-f10d-4bf2-8242-2f8a421c0a50',30,'house@1234567-0.com',100000,0,0,0,1,0,0,0,0,0,0,'0',0,'+925001000000',33.649793700,73.068566500,'House # 0','0','Pindora, Rawalpindi, Pakistan','ccdc836a-b814-40a1-b5d7-37a4c8e97ec6','Title # 0'),('14775a64-1ca2-41af-b68a-a07f85d0cf23',31,'house@1234567-1.com',100001,1,1,1,0,0,1,1,1,1,1,'0',1,'+925001000001',33.635478000,73.072993400,'House # 1','1','Satellite Town, Rawalpindi, Pakistan','9d26a2b6-d3f8-4231-8e80-9df5394a21e2','Title # 1'),('f158fea6-5bf7-4a9d-a06e-ef820e2e09fb',32,'house@1234567-2.com',100002,2,2,2,1,0,0,0,0,0,0,'0',0,'+925001000002',33.598828200,73.053809700,'House # 2','2','Saddar, Rawalpindi, Pakistan','1abb7f1d-4fb7-4351-9992-4f477c666210','Title # 2'),('a12a3578-c333-4c04-857c-583c3cd7aa43',33,'house@1234567-3.com',100003,3,3,3,0,0,1,1,1,1,1,'0',1,'+925001000003',33.642762800,73.070754600,'House # 3','3','6th Rd, Rawalpindi, Pakistan','02010e6f-ccba-4126-b027-af871e199652','Title # 3'),('7cdc7cc0-0dba-46d7-a66e-d622eb8fa23a',34,'house@1234567-4.com',100004,4,4,4,1,0,0,0,0,0,0,'0',0,'+925001000004',33.660147300,73.055285400,'House # 4','4','I-9, Islamabad, Pakistan','0e4ec18a-7d8d-4e48-a5c7-efdcda9afae5','Title # 4'),('3d1105e1-f363-47cc-bae4-d975fc64b679',35,'house@1234567-5.com',100005,5,5,5,0,0,1,1,1,1,1,'0',1,'+925001000005',33.668065400,73.072993400,'House # 5','5','I-8, Islamabad, Pakistan','fbce7372-75dc-411f-bf02-9fc13749fafb','Title # 5'),('a5e1cf9c-a7a8-439b-ba47-ad1680a8d56d',36,'house@1234567-6.com',100006,6,6,6,1,0,0,0,0,0,0,'0',0,'+925001000006',33.719863300,73.055285400,'House # 6','6','F-7, Islamabad, Pakistan','7f4ddf56-b5cd-4e85-9384-9449224f4ea9','Title # 6'),('19b21fb9-58f9-4097-8a3c-83f22a77e4d5',37,'house@1234567-7.com',100007,7,7,7,0,0,1,1,1,1,1,'0',1,'+925001000007',33.637708800,73.069568300,'House # 7','7','Commercial Market Rd, Rawalpindi, Pakistan','c80e6423-51b3-450d-8cf6-7bf3ff41a9a5','Title # 7'),('6abeaea6-56cb-45ef-8881-f210737e7ca0',38,'house@1234567-8.com',100008,8,8,8,1,0,0,0,0,0,0,'0',0,'+925001000008',33.707829900,73.049954400,'House # 8','8','The Centaurus Mall, Jinnah Avenue, Islamabad, Pakistan','1b58db66-ad31-4db2-b27f-afaf55a655c3','Title # 8'),('61703ab5-9853-498c-8551-529378b9bd79',39,'house@1234567-9.com',100009,9,9,9,0,0,1,1,1,1,1,'0',1,'+925001000009',33.581881600,73.560885200,'House # 9','9','Beor, Pakistan','cd13db31-0f36-44ab-9f1e-8f621d0b0fe0','Title # 9'),('81f65d3d-5f64-4867-a0e0-72ecfc1f7493',40,'house@1234567-10.com',100010,10,10,10,1,0,0,0,0,0,0,'0',0,'+925001000010',33.495896000,73.105630100,'House # 10','10','Bahria Town, Rawalpindi, Pakistan','3590db25-a03d-45bb-b8b8-e69e75d95b77','Title # 10'),('c6b637b7-7691-4079-9419-12ef6d86200e',41,'house@1234567-11.com',100011,11,11,11,0,0,1,1,1,1,1,'0',1,'+925001000011',33.528264800,73.161512700,'House # 11','11','DHA Phase II, Pakistan','4f1702da-d19c-417c-b7e5-c2eaacb148da','Title # 11'),('3c982cfe-2634-4fe7-a0b1-2a186bac82bd',42,'house@1234567-12.com',100012,12,12,12,1,0,0,0,0,0,0,'0',0,'+925001000012',33.614512800,73.055469900,'House # 12','12','Raja Bazar, Rawalpindi, Pakistan','b58c6fca-e0ee-4536-ae8a-03ef01f938e1','Title # 12'),('4e9e7980-a5f5-4739-b712-c64b4c62ce0a',43,'house@1234567-13.com',100013,13,13,13,0,0,1,1,1,1,1,'0',1,'+925001000013',33.650794800,73.074110500,'House # 13','13','Stadium Rd, Rawalpindi, Pakistan','a83cc140-e5a7-4275-a8c3-b7dda14bb65c','Title # 13'),('b6073e6d-9d67-47a7-8b14-f7df51fc3890',44,'house@1234567-14.com',100014,14,14,14,1,0,0,0,0,0,0,'0',0,'+925001000014',33.698188000,72.978535300,'House # 14','14','E-11, Islamabad, Pakistan','6c6546ab-e22d-4530-84bc-c32d74931719','Title # 14'),('598748bd-3056-475b-b5c1-bc3ac273354d',45,'house@1234567-15.com',100015,15,15,15,0,0,1,1,1,1,1,'0',1,'+925001000015',33.582470600,73.092175500,'House # 15','15','Chaklala Scheme 3, Rawalpindi, Pakistan','0dea2eef-96af-440a-a492-2b6d456ef968','Title # 15'),('529aeeab-e8c6-4029-97ff-abbaf8050751',46,'house@1234567-16.com',100016,16,16,16,1,0,0,0,0,0,0,'0',0,'+925001000016',33.713334800,73.061926100,'House # 16','16','Blue Area, Islamabad, Pakistan','83784a1c-8803-4261-9377-bdf660c980cf','Title # 16');
/*!40000 ALTER TABLE `house` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `houseimages`
--

LOCK TABLES `houseimages` WRITE;
/*!40000 ALTER TABLE `houseimages` DISABLE KEYS */;
INSERT INTO `houseimages` VALUES ('26cae14e-f10d-4bf2-8242-2f8a421c0a50','0045f341-786d-4232-96b1-930433bc243f.jpg'),('14775a64-1ca2-41af-b68a-a07f85d0cf23','f5975a9b-4cba-4e96-a6f6-ea3c9b697a1e.jpg'),('f158fea6-5bf7-4a9d-a06e-ef820e2e09fb','55177c4b-7541-4471-92f8-ee61c511bdd9.jpg'),('a12a3578-c333-4c04-857c-583c3cd7aa43','5b33c295-db85-4ff2-8755-48bc5971823f.jpg'),('7cdc7cc0-0dba-46d7-a66e-d622eb8fa23a','74511951-79d7-4e0f-90cf-959370d7201b.jpg'),('3d1105e1-f363-47cc-bae4-d975fc64b679','eb14b008-41e7-44d0-8ee3-27a0ad803a4a.jpg'),('a5e1cf9c-a7a8-439b-ba47-ad1680a8d56d','d72dc5c4-1908-4a8e-9b4d-a02f72b801fa.jpg'),('19b21fb9-58f9-4097-8a3c-83f22a77e4d5','dfc48e24-cae3-42c1-ab89-70f040411580.jpg'),('6abeaea6-56cb-45ef-8881-f210737e7ca0','f5061618-bd9f-47f7-89cb-2babeeef38cf.jpg'),('61703ab5-9853-498c-8551-529378b9bd79','902b8280-1406-49c1-baf7-518b6fa2d3b9.jpg'),('81f65d3d-5f64-4867-a0e0-72ecfc1f7493','abb11bfe-9313-43ac-a291-3a20d3950119.jpg'),('c6b637b7-7691-4079-9419-12ef6d86200e','4bddb4af-836c-417c-8760-2b96451b5963.jpg'),('3c982cfe-2634-4fe7-a0b1-2a186bac82bd','606e4883-1e79-4ce7-ba57-cfac019b9ddb.jpg'),('4e9e7980-a5f5-4739-b712-c64b4c62ce0a','07d2a64b-27d7-49dd-abed-fae90511bf80.jpg'),('b6073e6d-9d67-47a7-8b14-f7df51fc3890','4f5aeed4-145a-4d99-82b8-9b8dbc1fe101.jpg'),('598748bd-3056-475b-b5c1-bc3ac273354d','7063a38d-9c14-424f-9151-7a616feda69d.jpg');
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

-- Dump completed on 2017-02-05 15:22:58
