CREATE TABLE IF NOT EXISTS `setting` (
  `key`   VARCHAR(255) NOT NULL,
  `value` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`key`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `account` (
  `id`               INT(11)      NOT NULL AUTO_INCREMENT,
  `name`             VARCHAR(17)  NOT NULL,
  `normal_name`      VARCHAR(17)  NOT NULL,
  `hash`             VARCHAR(255) NOT NULL,
  `mail`             VARCHAR(255) NOT NULL,
  `mail_verified`    TINYINT(1)   NOT NULL,
  `mail_verified_at` DATETIME               DEFAULT NULL,
  `mail_token`       VARCHAR(255)           DEFAULT NULL,
  `password_token`   VARCHAR(255)           DEFAULT NULL,
  `state`            INT(11)      NOT NULL,
  `last_login`       DATETIME               DEFAULT NULL,
  `created`          DATETIME     NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_account_name` (`name`),
  UNIQUE KEY `uq_account_normal_name` (`normal_name`),
  UNIQUE KEY `uq_account_mail` (`mail`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `nec_soul` (
  `id`         INT(11)             NOT NULL,
  `account_id` INT(11)             NOT NULL,
  `name`       VARCHAR(45)         NOT NULL,
  `level`      TINYINT(1) UNSIGNED NOT NULL,
  `created`    DATETIME            NOT NULL,
  `password`   VARCHAR(10)                   DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `uq_nec_soul_name` (`name`),
  KEY `fk_nec_soul_account_id` (`account_id`),
  CONSTRAINT `fk_nec_soul_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
  
CREATE TABLE IF NOT EXISTS `nec_skilltree_item` (
  `id` INT(11)                  NOT NULL,
  `skill_id` INT(11)            NOT NULL,
  `char_id` INT(11)             NOT NULL,
  `level` INT(11) UNSIGNED      NOT NULL,
  PRIMARY KEY (`id`),
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;
 
 CREATE TABLE IF NOT EXISTS `nec_monster_coords` (
  `id`         INT(11)             NOT NULL,
  `monster_id` INT(11)             NOT NULL,
  `map_id` INT(11)                 NOT NULL,
  `coord_dx` INT(11)               NOT NULL,
  `x` FLOAT                        NOT NULL,
  `y` FLOAT                        NOT NULL,
  `z` FLOAT                        NOT NULL,

  PRIMARY KEY (`id`),
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

 CREATE TABLE IF NOT EXISTS `nec_shortcut_bar` (
  `id`    INT(11)             NOT NULL,
  `slot0` INT(11)             NOT NULL,
  `slot1` INT(11)             NOT NULL,
  `slot2` INT(11)             NOT NULL,
  `slot3` INT(11)             NOT NULL,
  `slot4` INT(11)             NOT NULL,
  `slot5` INT(11)             NOT NULL,
  `slot6` INT(11)             NOT NULL,
  `slot7` INT(11)             NOT NULL,
  `slot8` INT(11)             NOT NULL,
  `slot9` INT(11)             NOT NULL,
  `action0` INT(11)             NOT NULL,
  `action1` INT(11)             NOT NULL,
  `action2` INT(11)             NOT NULL,
  `action3` INT(11)             NOT NULL,
  `action4` INT(11)             NOT NULL,
  `action5` INT(11)             NOT NULL,
  `action6` INT(11)             NOT NULL,
  `action7` INT(11)             NOT NULL,
  `action8` INT(11)             NOT NULL,
  `action9` INT(11)             NOT NULL,
   PRIMARY KEY (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;

CREATE TABLE IF NOT EXISTS `nec_character` (
  `id`         INT(11)             NOT NULL AUTO_INCREMENT,
  `account_id` INT(11)             NOT NULL,
  `soul_id`    INT(11)             NOT NULL,
  `character_slot_id` INT(11)             NOT NULL,
 `name`       VARCHAR(45)         NOT NULL,
  `race_id`     BIGINT(11)     UNSIGNED      NOT NULL,
  `sex_id`      BIGINT(11)     UNSIGNED        NOT NULL,
  `hair_id`     SMALLINT(11)             NOT NULL,
  `hair_color_id` SMALLINT(11)             NOT NULL,
  `face_id`     SMALLINT(11)             NOT NULL,
  `alignment_id`      BIGINT(11)  UNSIGNED            NOT NULL,
  `strength`      INT(11)      UNSIGNED        NOT NULL,
  `vitality`      INT(11)    UNSIGNED         NOT NULL,
  `dexterity`      INT(11)    UNSIGNED         NOT NULL,
  `agility`      INT(11)     UNSIGNED        NOT NULL,
  `intelligence`      INT(11)  UNSIGNED           NOT NULL,
  `piety`      INT(11)         UNSIGNED    NOT NULL,
  `luck`      INT(11)       UNSIGNED      NOT NULL,
  `class_id`      INT(11)    UNSIGNED         NOT NULL,
  `level`      INT(11) UNSIGNED NOT NULL,
  `shortcut_bar0_id`      INT(11) NOT NULL,
  `shortcut_bar1_id`      INT(11) NOT NULL,
  `shortcut_bar2_id`      INT(11) NOT NULL,
  `shortcut_bar3_id`      INT(11) NOT NULL,
  `shortcut_bar4_id`      INT(11) NOT NULL,
  `created`    DATETIME            NOT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_nec_character_account_id` (`account_id`),
  KEY `fk_nec_character_soul_id` (`soul_id`),
  CONSTRAINT `fk_nec_soul_account_id` FOREIGN KEY (`account_id`) REFERENCES `account` (`id`),
  CONSTRAINT `fk_nec_soul_soul_id` FOREIGN KEY (`soul_id`) REFERENCES `nec_soul` (`id`)
)
  ENGINE = InnoDB
  DEFAULT CHARSET = utf8;