-- MySQL dump 10.13  Distrib 5.7.18, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: rentstuff
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
-- Table structure for table `__migrationhistory`
--

DROP TABLE IF EXISTS `__migrationhistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `__migrationhistory` (
  `MigrationId` varchar(100) NOT NULL,
  `ContextKey` varchar(200) NOT NULL,
  `Model` longblob NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`,`ContextKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetroles`
--

DROP TABLE IF EXISTS `aspnetroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetroles` (
  `Id` varchar(128) NOT NULL,
  `Name` varchar(256) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserclaims`
--

DROP TABLE IF EXISTS `aspnetuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserclaims` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `UserId` varchar(128) NOT NULL,
  `ClaimType` longtext,
  `ClaimValue` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Id` (`Id`),
  KEY `UserId` (`UserId`),
  CONSTRAINT `IdentityUser_Claims` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserlogins`
--

DROP TABLE IF EXISTS `aspnetuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserlogins` (
  `LoginProvider` varchar(128) NOT NULL,
  `ProviderKey` varchar(128) NOT NULL,
  `UserId` varchar(128) NOT NULL,
  PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
  KEY `IdentityUser_Logins` (`UserId`),
  CONSTRAINT `IdentityUser_Logins` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetuserroles`
--

DROP TABLE IF EXISTS `aspnetuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetuserroles` (
  `UserId` varchar(128) NOT NULL,
  `RoleId` varchar(128) NOT NULL,
  PRIMARY KEY (`UserId`,`RoleId`),
  KEY `IdentityRole_Users` (`RoleId`),
  CONSTRAINT `IdentityRole_Users` FOREIGN KEY (`RoleId`) REFERENCES `aspnetroles` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `IdentityUser_Roles` FOREIGN KEY (`UserId`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(128) NOT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `FullName` varchar(19) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext,
  `SecurityStamp` longtext,
  `PhoneNumber` longtext,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEndDateUtc` datetime DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  `UserName` varchar(256) NOT NULL,
  `IsPasswordResetRequested` tinyint(1) NOT NULL,
  `PasswordResetExpiryDate` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `dimension`
--

DROP TABLE IF EXISTS `dimension`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `dimension` (
  `id` varchar(100) NOT NULL,
  `dimension_type` varchar(100) NOT NULL,
  `string_value` varchar(100) DEFAULT NULL,
  `decimal_value` decimal(6,4) DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `externalaccesstokenidentifiers`
--

DROP TABLE IF EXISTS `externalaccesstokenidentifiers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `externalaccesstokenidentifiers` (
  `InternalId` varchar(255) NOT NULL,
  `ExternalAccessToken` varchar(255) NOT NULL,
  PRIMARY KEY (`InternalId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hostel`
--

DROP TABLE IF EXISTS `hostel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hostel` (
  `id` varchar(100) NOT NULL,
  `db_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `title` varchar(200) NOT NULL,
  `description` varchar(1500) DEFAULT NULL,
  `owner_email` varchar(100) NOT NULL,
  `rent_price` bigint(20) NOT NULL,
  `internet_available` tinyint(1) DEFAULT NULL,
  `cable_tv_available` tinyint(1) DEFAULT NULL,
  `parking_available` tinyint(1) DEFAULT NULL,
  `property_type` varchar(100) DEFAULT NULL,
  `gender_restriction` varchar(100) DEFAULT NULL,
  `owner_phone_number` varchar(25) DEFAULT NULL,
  `latitude` decimal(13,9) NOT NULL,
  `longitude` decimal(13,9) NOT NULL,
  `house_no` varchar(100) DEFAULT NULL,
  `street_no` varchar(100) DEFAULT NULL,
  `area` varchar(100) DEFAULT NULL,
  `owner_name` varchar(100) NOT NULL,
  `is_shared` tinyint(1) NOT NULL DEFAULT '0',
  `rent_unit` varchar(50) NOT NULL DEFAULT 'Month',
  `landline_number` varchar(25) DEFAULT NULL,
  `fax` varchar(25) DEFAULT NULL,
  `laundry` tinyint(1) DEFAULT NULL,
  `ac` tinyint(1) DEFAULT NULL,
  `geyser` tinyint(1) DEFAULT NULL,
  `fitness_centre` tinyint(1) DEFAULT NULL,
  `attached_bathroom` tinyint(1) DEFAULT NULL,
  `ironing` tinyint(1) DEFAULT NULL,
  `elevator` tinyint(1) DEFAULT NULL,
  `balcony` tinyint(1) DEFAULT NULL,
  `lawn` tinyint(1) DEFAULT NULL,
  `meals` tinyint(1) DEFAULT NULL,
  `pickndrop` tinyint(1) DEFAULT NULL,
  `number_of_seats` int(2) DEFAULT NULL,
  `date_created` datetime DEFAULT NULL,
  `last_modified` datetime DEFAULT NULL,
  `cctv_cameras` tinyint(1) DEFAULT NULL,
  `backup_electricity` tinyint(1) DEFAULT NULL,
  `heating` tinyint(1) DEFAULT NULL,
  PRIMARY KEY (`db_id`),
  UNIQUE KEY `db_id_UNIQUE` (`db_id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=92 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hostelimages`
--

DROP TABLE IF EXISTS `hostelimages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hostelimages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hostel_id` varchar(100) NOT NULL,
  `image_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=155 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hotel`
--

DROP TABLE IF EXISTS `hotel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hotel` (
  `id` varchar(100) NOT NULL,
  `db_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `title` varchar(200) NOT NULL,
  `description` varchar(1500) DEFAULT NULL,
  `owner_email` varchar(100) NOT NULL,
  `rent_price` bigint(20) NOT NULL,
  `internet_available` tinyint(1) DEFAULT NULL,
  `cable_tv_available` tinyint(1) DEFAULT NULL,
  `parking_available` tinyint(1) DEFAULT NULL,
  `property_type` varchar(100) DEFAULT NULL,
  `gender_restriction` varchar(100) DEFAULT NULL,
  `owner_phone_number` varchar(25) DEFAULT NULL,
  `latitude` decimal(13,9) NOT NULL,
  `longitude` decimal(13,9) NOT NULL,
  `house_no` varchar(100) DEFAULT NULL,
  `street_no` varchar(100) DEFAULT NULL,
  `area` varchar(100) DEFAULT NULL,
  `owner_name` varchar(100) NOT NULL,
  `is_shared` tinyint(1) NOT NULL DEFAULT '0',
  `rent_unit` varchar(50) NOT NULL DEFAULT 'Month',
  `landline_number` varchar(25) DEFAULT NULL,
  `fax` varchar(25) DEFAULT NULL,
  `laundry` tinyint(1) DEFAULT NULL,
  `ac` tinyint(1) DEFAULT NULL,
  `geyser` tinyint(1) DEFAULT NULL,
  `fitness_centre` tinyint(1) DEFAULT NULL,
  `attached_bathroom` tinyint(1) DEFAULT NULL,
  `ironing` tinyint(1) DEFAULT NULL,
  `elevator` tinyint(1) DEFAULT NULL,
  `balcony` tinyint(1) DEFAULT NULL,
  `lawn` tinyint(1) DEFAULT NULL,
  `restaurant` tinyint(1) DEFAULT NULL,
  `airport_shuttle` tinyint(1) DEFAULT NULL,
  `breakfast_included` tinyint(1) DEFAULT NULL,
  `sitting_area` tinyint(1) DEFAULT NULL,
  `car_rental` tinyint(1) DEFAULT NULL,
  `spa` tinyint(1) DEFAULT NULL,
  `salon` tinyint(1) DEFAULT NULL,
  `swimming_pool` int(2) DEFAULT NULL,
  `kitchen` int(2) DEFAULT NULL,
  `date_created` datetime DEFAULT NULL,
  `last_modified` datetime DEFAULT NULL,
  `cctv_cameras` tinyint(1) DEFAULT NULL,
  `backup_electricity` tinyint(1) DEFAULT NULL,
  `heating` tinyint(1) DEFAULT NULL,
  `bathtub` tinyint(1) DEFAULT NULL,
  `occupants_id` varchar(100) DEFAULT NULL,
  `number_of_single_beds` int(11) DEFAULT '0',
  `number_of_double_beds` int(11) DEFAULT '0',
  PRIMARY KEY (`db_id`),
  UNIQUE KEY `db_id_UNIQUE` (`db_id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hotelimages`
--

DROP TABLE IF EXISTS `hotelimages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `hotelimages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hotel_id` varchar(100) NOT NULL,
  `image_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `house`
--

DROP TABLE IF EXISTS `house`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `house` (
  `id` varchar(100) NOT NULL,
  `db_id` bigint(20) NOT NULL AUTO_INCREMENT,
  `title` varchar(200) NOT NULL,
  `description` varchar(1500) DEFAULT NULL,
  `owner_email` varchar(100) NOT NULL,
  `rent_price` bigint(20) NOT NULL,
  `number_of_bedrooms` int(11) NOT NULL,
  `number_of_kitchens` int(11) NOT NULL,
  `number_of_bathrooms` int(11) NOT NULL,
  `internet_available` tinyint(1) DEFAULT NULL,
  `landline_phone_available` tinyint(1) DEFAULT NULL,
  `cable_tv_available` tinyint(1) DEFAULT NULL,
  `garage_available` tinyint(1) DEFAULT NULL,
  `smoking_allowed` tinyint(1) DEFAULT NULL,
  `property_type` varchar(100) DEFAULT NULL,
  `gender_restriction` varchar(100) DEFAULT NULL,
  `owner_phone_number` varchar(25) NOT NULL,
  `latitude` decimal(13,9) NOT NULL,
  `longitude` decimal(13,9) NOT NULL,
  `house_no` varchar(100) DEFAULT NULL,
  `street_no` varchar(100) DEFAULT NULL,
  `area` varchar(100) DEFAULT NULL,
  `dimension_id` varchar(100) DEFAULT NULL,
  `owner_name` varchar(100) NOT NULL,
  `is_shared` tinyint(1) NOT NULL DEFAULT '0',
  `rent_unit` varchar(50) NOT NULL DEFAULT 'Month',
  `date_created` datetime DEFAULT NULL,
  `last_modified` datetime DEFAULT NULL,
  `landline_number` varchar(25) DEFAULT NULL,
  `fax` varchar(25) DEFAULT NULL,
  PRIMARY KEY (`db_id`),
  UNIQUE KEY `db_id_UNIQUE` (`db_id`),
  UNIQUE KEY `id_UNIQUE` (`id`),
  KEY `dimension_id_fk_idx` (`dimension_id`),
  CONSTRAINT `dimension_id_fk` FOREIGN KEY (`dimension_id`) REFERENCES `dimension` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=483 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `houseimages`
--

DROP TABLE IF EXISTS `houseimages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `houseimages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `house_id` varchar(100) NOT NULL,
  `image_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=290 DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `occupants`
--

DROP TABLE IF EXISTS `occupants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `occupants` (
  `id` varchar(100) NOT NULL,
  `adults` int(11) DEFAULT '0',
  `children` int(11) DEFAULT '0',
  `total_occupants` int(11) DEFAULT '0',
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-09-10 15:26:21
