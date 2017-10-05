rename table `houseimages` to `propertyimages`;
alter table `propertyimages` change `house_id` `property_id` VARCHAR(100) NOT NULL;