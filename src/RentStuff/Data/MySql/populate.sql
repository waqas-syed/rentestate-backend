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
INSERT INTO `dimension` VALUES ('009a2607-7c7f-4406-9c7b-bf5d253dda0e','1','16',0.0000),('0a8f34fd-2ba6-463b-8a17-b0ca38d026aa','0','9',0.0000),('1e4a18b2-1bb6-4011-b728-e6cf13e6695b','1','12',0.0000),('21122744-0e73-4762-af39-e33b5a5d8110','0','7',0.0000),('3a4b715e-2452-4130-a4b6-59b1737ae4b0','1','14',0.0000),('4a74323d-93eb-47ae-935a-0b1953552450','0','5',0.0000),('6f156515-7a52-464e-b0a9-c9676ddb4ad5','0','1',0.0000),('7cd71e93-7641-470a-b5a6-e640bfb76ce5','0','11',0.0000),('89ebfa44-7632-4d84-9b3d-b84d49da72dd','1','2',0.0000),('a0635bf1-235c-4596-9a51-e22ff58a5ab0','1','10',0.0000),('acbd7022-aa2f-4aca-9689-c98dd713ba67','0','13',0.0000),('ba98c890-e69c-46aa-8231-fb8b74ebef75','1','6',0.0000),('c6dcf5df-d3fa-412d-be0d-347176004f51','1','0',0.0000),('d1995160-540a-4991-8d43-9d0cc9a98c95','0','15',0.0000),('d4d7c9ec-5286-4ec1-8ee8-3ab198315d1f','1','8',0.0000),('d8fc315e-f2c9-4d82-98b8-38292c5feeb0','1','4',0.0000),('f5606b06-0323-4efd-b9b0-9457cf306f94','0','3',0.0000);
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
INSERT INTO `house` VALUES ('b4973d0a-409a-4605-9c3e-eba0ef12a42b',30,'house@1234567-0.com',100000,0,0,0,1,0,0,0,0,0,0,'0',0,'+925001000000',33.649793700,73.068566500,'House # 0','0','Pindora, Rawalpindi, Pakistan','c6dcf5df-d3fa-412d-be0d-347176004f51','Title # 0','Owner Name 0'),('d01385df-ed7a-445e-b389-3262e8f08fda',31,'house@1234567-1.com',100001,1,1,1,0,0,1,1,1,1,1,'0',1,'+925001000001',33.635478000,73.072993400,'House # 1','1','Satellite Town, Rawalpindi, Pakistan','6f156515-7a52-464e-b0a9-c9676ddb4ad5','Title # 1','Owner Name 1'),('35f58509-55f0-45a7-9f06-da51aeb17253',32,'house@1234567-2.com',100002,2,2,2,1,0,0,0,0,0,0,'0',0,'+925001000002',33.598828200,73.053809700,'House # 2','2','Saddar, Rawalpindi, Pakistan','89ebfa44-7632-4d84-9b3d-b84d49da72dd','Title # 2','Owner Name 2'),('fd341068-1fcb-4494-af4a-17449e02e7a8',33,'house@1234567-3.com',100003,3,3,3,0,0,1,1,1,1,1,'0',1,'+925001000003',33.642762800,73.070754600,'House # 3','3','6th Rd, Rawalpindi, Pakistan','f5606b06-0323-4efd-b9b0-9457cf306f94','Title # 3','Owner Name 3'),('8967bc21-6cfa-4321-9502-981878fe64ce',34,'house@1234567-4.com',100004,4,4,4,1,0,0,0,0,0,0,'0',0,'+925001000004',33.660147300,73.055285400,'House # 4','4','I-9, Islamabad, Pakistan','d8fc315e-f2c9-4d82-98b8-38292c5feeb0','Title # 4','Owner Name 4'),('d6576c8d-96b2-4f55-8d8f-d2eb48bf64b8',35,'house@1234567-5.com',100005,5,5,5,0,0,1,1,1,1,1,'0',1,'+925001000005',33.668065400,73.072993400,'House # 5','5','I-8, Islamabad, Pakistan','4a74323d-93eb-47ae-935a-0b1953552450','Title # 5','Owner Name 5'),('991a986d-82ea-4ca3-b159-3b1fda95f6d0',36,'house@1234567-6.com',100006,6,6,6,1,0,0,0,0,0,0,'0',0,'+925001000006',33.719863300,73.055285400,'House # 6','6','F-7, Islamabad, Pakistan','ba98c890-e69c-46aa-8231-fb8b74ebef75','Title # 6','Owner Name 6'),('bee3f0a4-3d21-4ef2-a858-70130ed5901a',37,'house@1234567-7.com',100007,7,7,7,0,0,1,1,1,1,1,'0',1,'+925001000007',33.637708800,73.069568300,'House # 7','7','Commercial Market Rd, Rawalpindi, Pakistan','21122744-0e73-4762-af39-e33b5a5d8110','Title # 7','Owner Name 7'),('9a36af0d-9cf3-44dc-aa28-8d54c2d7482d',38,'house@1234567-8.com',100008,8,8,8,1,0,0,0,0,0,0,'0',0,'+925001000008',33.707829900,73.049954400,'House # 8','8','The Centaurus Mall, Jinnah Avenue, Islamabad, Pakistan','d4d7c9ec-5286-4ec1-8ee8-3ab198315d1f','Title # 8','Owner Name 8'),('325a37db-9e3d-4674-acb2-36bf8ada7f02',39,'house@1234567-9.com',100009,9,9,9,0,0,1,1,1,1,1,'0',1,'+925001000009',33.581881600,73.560885200,'House # 9','9','Beor, Pakistan','0a8f34fd-2ba6-463b-8a17-b0ca38d026aa','Title # 9','Owner Name 9'),('0d9eff35-ada9-422c-b457-77ff85fa4942',40,'house@1234567-10.com',100010,10,10,10,1,0,0,0,0,0,0,'0',0,'+925001000010',33.495896000,73.105630100,'House # 10','10','Bahria Town, Rawalpindi, Pakistan','a0635bf1-235c-4596-9a51-e22ff58a5ab0','Title # 10','Owner Name 10'),('4dbe98c6-8775-49b7-a577-9ba2ec28ea08',41,'house@1234567-11.com',100011,11,11,11,0,0,1,1,1,1,1,'0',1,'+925001000011',33.528264800,73.161512700,'House # 11','11','DHA Phase II, Pakistan','7cd71e93-7641-470a-b5a6-e640bfb76ce5','Title # 11','Owner Name 11'),('c1784eea-6623-4056-8dcd-1db3f572b61e',42,'house@1234567-12.com',100012,12,12,12,1,0,0,0,0,0,0,'0',0,'+925001000012',33.614512800,73.055469900,'House # 12','12','Raja Bazar, Rawalpindi, Pakistan','1e4a18b2-1bb6-4011-b728-e6cf13e6695b','Title # 12','Owner Name 12'),('d2cb0286-db13-4848-afd9-8afae5bfe36d',43,'house@1234567-13.com',100013,13,13,13,0,0,1,1,1,1,1,'0',1,'+925001000013',33.650794800,73.074110500,'House # 13','13','Stadium Rd, Rawalpindi, Pakistan','acbd7022-aa2f-4aca-9689-c98dd713ba67','Title # 13','Owner Name 13'),('263ccfad-9fc8-4079-84da-de000d59b798',44,'house@1234567-14.com',100014,14,14,14,1,0,0,0,0,0,0,'0',0,'+925001000014',33.698188000,72.978535300,'House # 14','14','E-11, Islamabad, Pakistan','3a4b715e-2452-4130-a4b6-59b1737ae4b0','Title # 14','Owner Name 14'),('eea7a366-ce1c-43f6-b20a-7b62d1578392',45,'house@1234567-15.com',100015,15,15,15,0,0,1,1,1,1,1,'0',1,'+925001000015',33.582470600,73.092175500,'House # 15','15','Chaklala Scheme 3, Rawalpindi, Pakistan','d1995160-540a-4991-8d43-9d0cc9a98c95','Title # 15','Owner Name 15'),('96e31bff-df86-403d-8b05-71f72987edd9',46,'house@1234567-16.com',100016,16,16,16,1,0,0,0,0,0,0,'0',0,'+925001000016',33.713334800,73.061926100,'House # 16','16','Blue Area, Islamabad, Pakistan','009a2607-7c7f-4406-9c7b-bf5d253dda0e','Title # 16','Owner Name 16');
/*!40000 ALTER TABLE `house` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping data for table `houseimages`
--

LOCK TABLES `houseimages` WRITE;
/*!40000 ALTER TABLE `houseimages` DISABLE KEYS */;
INSERT INTO `houseimages` VALUES ('b4973d0a-409a-4605-9c3e-eba0ef12a42b','345d0e82-fdc0-499c-aea7-adc498c6f0d4.jpg'),('d01385df-ed7a-445e-b389-3262e8f08fda','c88ddd0c-7e02-4345-830b-0bed2d7b66c3.jpg'),('35f58509-55f0-45a7-9f06-da51aeb17253','06bcf548-d544-4195-bb06-3d57ada22492.jpg'),('fd341068-1fcb-4494-af4a-17449e02e7a8','73e9d290-4c9c-49b0-95d6-b53a91ac21a7.jpg'),('8967bc21-6cfa-4321-9502-981878fe64ce','0546441b-effe-4140-bf14-5af755d37c62.jpg'),('d6576c8d-96b2-4f55-8d8f-d2eb48bf64b8','af514a05-13d3-46d9-be78-30a4cc5acfc6.jpg'),('991a986d-82ea-4ca3-b159-3b1fda95f6d0','0a20aafc-6afb-4969-9e87-48da64f2f4b5.jpg'),('bee3f0a4-3d21-4ef2-a858-70130ed5901a','e678f5e0-9c4b-4706-99d3-08edb9fe9e35.jpg'),('9a36af0d-9cf3-44dc-aa28-8d54c2d7482d','49b6925c-49da-42c1-b7c9-ee9ee59497df.jpg'),('325a37db-9e3d-4674-acb2-36bf8ada7f02','57d4b71f-1a80-4abe-8e09-3bf680d90dbc.jpg'),('0d9eff35-ada9-422c-b457-77ff85fa4942','c9744be2-427e-4da3-aa3b-ebac93f9c772.jpg'),('4dbe98c6-8775-49b7-a577-9ba2ec28ea08','175f47c7-b9e0-4497-ac6c-75641934e1f4.jpg'),('c1784eea-6623-4056-8dcd-1db3f572b61e','60fdfd81-db73-486c-90cc-4d1323aaf8b9.jpg'),('d2cb0286-db13-4848-afd9-8afae5bfe36d','d13b7ec5-9b45-463e-a348-dc5058bc043a.jpg'),('263ccfad-9fc8-4079-84da-de000d59b798','a31e3df8-d111-4360-a8db-ff1051956df1.jpg'),('eea7a366-ce1c-43f6-b20a-7b62d1578392','1aff26e3-cb0f-429e-9bef-1a9c785da807.jpg'),('96e31bff-df86-403d-8b05-71f72987edd9','1433484c-fae9-43d7-a64a-f7b85c74ee2f.jpg');
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

-- Dump completed on 2017-02-08 12:47:06
