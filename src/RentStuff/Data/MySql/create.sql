-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               10.0.17-MariaDB - mariadb.org binary distribution
-- Server OS:                    Win64
-- HeidiSQL Version:             9.1.0.4867
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for rentstuff
CREATE DATABASE IF NOT EXISTS `rentstuff` /*!40100 DEFAULT CHARACTER SET latin1 */;
USE `rentstuff`;


-- Dumping structure for table rentstuff.house
CREATE TABLE IF NOT EXISTS `house` (
  `id` bigint(20) NOT NULL AUTO_INCREMENT,
  `owner_email` varchar(50) DEFAULT NULL,
  `price` bigint(20) DEFAULT NULL,
  `for_rent` tinyint(1) DEFAULT NULL,
  `number_of_bedrooms` int(11) DEFAULT NULL,
  `number_of_kitchens` int(11) DEFAULT NULL,
  `number_of_bathrooms` int(11) DEFAULT NULL,
  `families_only` tinyint(1) DEFAULT NULL,
  `girls_only` tinyint(1) DEFAULT NULL,
  `internet_available` tinyint(1) DEFAULT NULL,
  `landline_phone_available` tinyint(1) DEFAULT NULL,
  `cable_tv_available` tinyint(1) DEFAULT NULL,
  `garage_available` tinyint(1) DEFAULT NULL,
  `smoking_allowed` tinyint(1) DEFAULT NULL,
  `property_type` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=13 DEFAULT CHARSET=latin1;

/*!40000 ALTER TABLE `house` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
