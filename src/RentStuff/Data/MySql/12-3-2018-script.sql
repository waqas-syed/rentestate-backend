alter table rentstuff.hostel add cctv_cameras tinyint(1) DEFAULT NULL;
alter table rentstuff.hostel add backup_electricity tinyint(1) DEFAULT NULL;
alter table rentstuff.hostel add heating tinyint(1) DEFAULT NULL;

alter table rentstuff.hotel add cctv_cameras tinyint(1) DEFAULT NULL;
alter table rentstuff.hotel add backup_electricity tinyint(1) DEFAULT NULL;
alter table rentstuff.hotel add heating tinyint(1) DEFAULT NULL;
alter table rentstuff.hotel add bathtub tinyint(1) DEFAULT NULL;

CREATE TABLE `bed` (
  `id` varchar(100) NOT NULL,
  `bed_count` int NOT NULL,
  `bed_type` varchar(50) DEFAULT NULL,
  `hotel_id` varchar(100) DEFAULT NULL,
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `occupants` (
  `id` varchar(100) NOT NULL,
  `adults` int DEFAULT 0,
  `children` int DEFAULT 0,
  `total_occupants` int DEFAULT 0,
  UNIQUE KEY `id_UNIQUE` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

alter table rentstuff.hotel add column `occupants_id` varchar(100) DEFAULT NULL