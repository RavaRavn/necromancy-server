INSERT INTO "account" ("id","name","normal_name","hash","mail","mail_verified","mail_verified_at","mail_token","password_token","state","last_login","created") 
VALUES
(1,'Admin','Admin','$2a$10$WT0s8dLPde7CWOSrEGIM2u4Nk.n/SBaAZe0VQ.FK.V/f/D1sIGqRu','Admin',0,NULL,NULL,NULL,1,NULL,'2019-11-19 15:58:21.1738355'),
 (2,'1234','1234','$2a$10$gHAWndIPBBGuTcsyOL2KFufzvPDI/evj18DygBv/QelpLENzALcBi','1234',0,NULL,NULL,NULL,100,NULL,'2019-11-19 16:03:55.2889936'),
 (3,'ff','ff','$2a$10$GhZE/d8hTdh92URBeP5k6eH9HbCWhoDknupYegGpgdcIOfRUoGodq','ff',0,NULL,NULL,NULL,1,NULL,'2019-11-19 16:13:35.4028315'),
 (4,'wolf','wolf','$2a$10$MoaRYlEbvjm/F2pr0FMrgewdd3TMQzbfOXW5o73C5C1kQPtO2rFeG','wolf',0,NULL,NULL,NULL,1,NULL,'2020-01-22 18:45:35.4028315'),
 (6,'aiko','aiko','$2a$10$HrEue5SiNe7h6cpmkp6.6OIjZB6c583Q.XYBRh2WCy7fnk.4.wnAq','aiko',0,NULL,NULL,NULL,1,NULL,'2020-02-11 14:46:18.1770021'),
 (7,'afflatus','afflatus','$2a$10$xwQkMp90yAKbqTxB2Ph6POkCQQXhcNWAOnOjGN/2nCT4sX0nA/1zm','afflatus',0,NULL,NULL,NULL,1,NULL,'2020-02-11 15:05:03.0611675'),
 (8,'mizuki','mizuki','$2a$10$XF12OScwkzH6hifuVxRI8uZ3TH7vJBsk5auXhFcL0qj/J6QpoysLu','mizuki',0,NULL,NULL,NULL,1,NULL,'2020-02-11 15:15:10.6311294'),
 (9,'hiraeth','hiraeth','$2a$10$JjtJfZvG/cmvVJW22zVrjez529S8GFzuaALB3XayewG/IC2L0tlhO','hiraeth',0,NULL,NULL,NULL,1,NULL,'2020-02-25 21:50:52.729478');

 INSERT INTO "nec_soul" ("id","account_id","name","level","created","password") 
 VALUES
(1,1,'Server',0,'2019-11-19 16:00:24.0076961','0000'),
 (2,2,'Xeno.',100,'2019-11-19 16:04:30.5066857','1234'),
 (3,3,'Unknown',100,'2019-11-19 16:14:12.2002506','0000'),
 (4,4,'Wolfzen',100,'2020-01-22 18:45:35.4028315','0000'),
 (5,6,'Aiko',5,'2020-02-11 14:47:31.179687','0000'),
 (6,7,'Afflatus',3,'2020-02-11 15:05:59.0900745','0000'),
 (7,8,'Mizuki',0,'2020-02-11 15:15:51.1238589','0000'),
 (8,9,'Hiraeth',0,'2020-02-25 21:52:30.3752316','0000');

 INSERT INTO "nec_character" ("id","account_id","soul_id","slot","map_id","x","y","z","name","race_id","sex_id","hair_id","hair_color_id","face_id","alignment_id","strength","vitality","dexterity","agility","intelligence","piety","luck","class_id","level","shortcut_bar0_id","shortcut_bar1_id","shortcut_bar2_id","shortcut_bar3_id","shortcut_bar4_id","created") 
 VALUES 
(1,1,1,1,1001002,23021.0,-224.0,0.0,'Server',1,1,0,0,0,1,7,6,9,6,6,7,4,3,45,1,2,3,4,5,'2019-11-19 16:01:29.30889'),
 (2,2,2,1,2006000,6588.89599609375,-20.5807247161865,0.998966217041016,'Talin',3,0,0,5,0,3,7,6,9,6,6,7,6,1,44,6,7,8,9,10,'2019-11-19 16:05:17.8122517'),
 (3,2,2,2,1001002,23021.0,-224.0,0.0,'Zakura',0,1,0,0,4,1,7,6,9,6,6,7,8,0,43,11,12,13,14,15,'2019-11-19 16:11:13.2318326'),
 (4,2,2,0,1001002,23021.0,-224.0,0.0,'Kadred',0,0,0,0,0,3,7,6,9,6,6,7,3,2,42,16,17,18,19,20,'2019-11-19 16:12:46.8206967'),
 (5,3,3,1,2006000,16179.26953125,3563.39916992188,0.999725341796875,'One',4,1,1,1,0,3,7,6,9,6,6,7,9,3,41,21,22,23,24,25,'2019-11-19 16:15:30.9221983'),
 (6,4,4,0,1001002,23021.0,-224.0,0.0,'Wolf',1,0,0,0,0,2,7,6,9,6,6,7,9,2,20,26,27,28,29,30,'2020-01-22 18:45:35.402831'),
 (7,4,4,1,1001002,23021.0,-224.0,0.0,'Wolfzen',3,0,0,0,0,2,7,6,9,6,6,7,9,1,35,31,32,33,34,35,'2020-01-22 18:45:35.402831'),
 (8,6,5,1,1001902,23209.462890625,-187.910797119141,-9.80994987487793,'Mizuki',0,1,0,0,0,1,0,0,0,0,1,0,0,2,25,36,37,38,39,40,'2020-02-11 14:48:39.8044959'),
 (9,7,6,1,1001007,193.676605224609,1250.80822753906,301.301727294922,'Kadred',3,0,4,5,0,3,0,0,4,0,0,0,10,1,22,41,42,43,44,45,'2020-02-11 15:08:01.1549772'),
 (10,8,7,1,2001105,2226.10009765625,-3628.13110351563,412.150604248047,'Aiko',0,1,0,0,0,3,0,0,3,0,0,0,0,1,33,46,47,48,49,50,'2020-02-11 15:16:57.2783877'),
 (11,9,8,1,2006000,6226.0380859375,-1.98192828975152e-05,1.00094413757324,'Elf',1,1,0,5,1,2,0,0,1,0,0,0,0,1,0,51,52,53,54,55,'2020-02-25 21:55:02.0526297');