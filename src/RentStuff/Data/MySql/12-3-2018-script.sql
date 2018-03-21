--
-- Table structure for table `bed`
--

CREATE TABLE `bed` (
  `id` varchar(100) NOT NULL,
  `bed_count` int(11) NOT NULL,
  `bed_type` varchar(50) DEFAULT NULL,
  `hotel_id` varchar(100) DEFAULT NULL,
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `hostelimages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hostel_id` varchar(100) NOT NULL,
  `image_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

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
  PRIMARY KEY (`db_id`),
  UNIQUE KEY `db_id_UNIQUE` (`db_id`),
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `hotelimages` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `hotel_id` varchar(100) NOT NULL,
  `image_id` varchar(200) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `occupants` (
  `id` varchar(100) NOT NULL,
  `adults` int(11) DEFAULT '0',
  `children` int(11) DEFAULT '0',
  `total_occupants` int(11) DEFAULT '0',
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

alter table house add column `landline_number` varchar(25) DEFAULT NULL;
alter table house add column `fax` varchar(25) DEFAULT NULL;