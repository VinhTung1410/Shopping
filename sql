--------------------------------------------------------
--  DDL for Table CUSTOMERS
--------------------------------------------------------

  CREATE TABLE "TUNG"."CUSTOMERS" 
   (	"CUSTOMERID" VARCHAR2(255 BYTE), 
	"COMPANYNAME" VARCHAR2(255 BYTE), 
	"CONTACTNAME" VARCHAR2(255 BYTE), 
	"CONTACTTITLE" VARCHAR2(255 BYTE), 
	"ADDRESS" VARCHAR2(255 BYTE), 
	"CITY" VARCHAR2(255 BYTE), 
	"REGION" VARCHAR2(255 BYTE), 
	"POSTALCODE" VARCHAR2(255 BYTE), 
	"COUNTRY" VARCHAR2(255 BYTE), 
	"PHONE" VARCHAR2(255 BYTE), 
	"FAX" VARCHAR2(255 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table CATEGORIES
--------------------------------------------------------

  CREATE TABLE "TUNG"."CATEGORIES" 
   (	"CATEGORYID" NUMBER, 
	"CATEGORYNAME" VARCHAR2(255 BYTE), 
	"DESCRIPTION" CLOB
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" 
 LOB ("DESCRIPTION") STORE AS SECUREFILE (
  TABLESPACE "MYTABLESPACE" ENABLE STORAGE IN ROW 4000 CHUNK 8192
  NOCACHE LOGGING  NOCOMPRESS  KEEP_DUPLICATES ) ;
--------------------------------------------------------
--  DDL for Table ORDERDETAILS
--------------------------------------------------------

  CREATE TABLE "TUNG"."ORDERDETAILS" 
   (	"ORDERID" NUMBER, 
	"PRODUCTID" NUMBER, 
	"UNITPRICE" NUMBER(18,2), 
	"QUANTITY" NUMBER, 
	"DISCOUNT" FLOAT(126), 
	"NOTE" CLOB
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" 
 LOB ("NOTE") STORE AS SECUREFILE (
  TABLESPACE "MYTABLESPACE" ENABLE STORAGE IN ROW 4000 CHUNK 8192
  NOCACHE LOGGING  NOCOMPRESS  KEEP_DUPLICATES 
  STORAGE(INITIAL 262144 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)) ;
--------------------------------------------------------
--  DDL for Table EMPLOYEES
--------------------------------------------------------

  CREATE TABLE "TUNG"."EMPLOYEES" 
   (	"EMPLOYEEID" NUMBER, 
	"LASTNAME" VARCHAR2(255 BYTE), 
	"FIRSTNAME" VARCHAR2(255 BYTE), 
	"TITLE" VARCHAR2(255 BYTE), 
	"TITLEOFCOURTESY" VARCHAR2(255 BYTE), 
	"BIRTHDATE" DATE, 
	"HIREDATE" DATE, 
	"ADDRESS" VARCHAR2(255 BYTE), 
	"CITY" VARCHAR2(255 BYTE), 
	"REGION" VARCHAR2(255 BYTE), 
	"POSTALCODE" VARCHAR2(255 BYTE), 
	"COUNTRY" VARCHAR2(255 BYTE), 
	"HOMEPHONE" VARCHAR2(255 BYTE), 
	"EXTENSION" VARCHAR2(255 BYTE), 
	"REPORTSTO" NUMBER
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table ORDERS
--------------------------------------------------------

  CREATE TABLE "TUNG"."ORDERS" 
   (	"ORDERID" NUMBER, 
	"CUSTOMERID" VARCHAR2(255 BYTE), 
	"EMPLOYEEID" NUMBER, 
	"ORDERDATE" DATE, 
	"REQUIREDDATE" DATE, 
	"SHIPPEDDATE" DATE, 
	"SHIPVIA" NUMBER, 
	"FREIGHT" NUMBER(18,2), 
	"SHIPNAME" VARCHAR2(255 BYTE), 
	"SHIPADDRESS" VARCHAR2(255 BYTE), 
	"SHIPCITY" VARCHAR2(255 BYTE), 
	"SHIPREGION" VARCHAR2(255 BYTE), 
	"SHIPPOSTALCODE" VARCHAR2(255 BYTE), 
	"SHIPCOUNTRY" VARCHAR2(255 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table PERMISSIONS
--------------------------------------------------------

  CREATE TABLE "TUNG"."PERMISSIONS" 
   (	"PERMISSIONID" NUMBER, 
	"NAMEPERMISSION" VARCHAR2(255 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table PRODUCT_IMAGES
--------------------------------------------------------

  CREATE TABLE "TUNG"."PRODUCT_IMAGES" 
   (	"PRODUCTIMAGEID" NUMBER, 
	"PRODUCTID" NUMBER, 
	"IMAGEURL" VARCHAR2(255 BYTE), 
	"ORIGINALFILENAME" VARCHAR2(255 BYTE), 
	"FILESIZE" NUMBER, 
	"IMAGEWIDTH" NUMBER, 
	"IMAGEHEIGHT" NUMBER
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table PRODUCTS
--------------------------------------------------------

  CREATE TABLE "TUNG"."PRODUCTS" 
   (	"PRODUCTID" NUMBER, 
	"PRODUCTNAME" VARCHAR2(255 BYTE), 
	"SUPPLIERID" NUMBER, 
	"CATEGORYID" NUMBER, 
	"QUANTITYPERUNIT" VARCHAR2(255 BYTE), 
	"UNITPRICE" NUMBER(18,2), 
	"UNITSINSTOCK" NUMBER, 
	"UNITSONORDER" NUMBER, 
	"REORDERLEVEL" NUMBER, 
	"DISCONTINUED" NUMBER(1,0), 
	"DESCRIPTION" CLOB
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" 
 LOB ("DESCRIPTION") STORE AS SECUREFILE (
  TABLESPACE "MYTABLESPACE" ENABLE STORAGE IN ROW 4000 CHUNK 8192
  NOCACHE LOGGING  NOCOMPRESS  KEEP_DUPLICATES 
  STORAGE(INITIAL 262144 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)) ;
--------------------------------------------------------
--  DDL for Table ROLEPERMISSIONS
--------------------------------------------------------

  CREATE TABLE "TUNG"."ROLEPERMISSIONS" 
   (	"ROLEID" NUMBER, 
	"PERMISSIONID" NUMBER
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table ROLES
--------------------------------------------------------

  CREATE TABLE "TUNG"."ROLES" 
   (	"ROLEID" NUMBER, 
	"ROLENAME" VARCHAR2(255 BYTE)
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table SHIPPERS
--------------------------------------------------------

  CREATE TABLE "TUNG"."SHIPPERS" 
   (	"SHIPPERID" NUMBER, 
	"COMPANYNAME" VARCHAR2(255 BYTE), 
	"PHONE" VARCHAR2(255 BYTE)
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Table SUPPLIERS
--------------------------------------------------------

  CREATE TABLE "TUNG"."SUPPLIERS" 
   (	"SUPPLIERID" NUMBER, 
	"COMPANYNAME" VARCHAR2(255 BYTE), 
	"CONTACTNAME" VARCHAR2(255 BYTE), 
	"CONTACTTITLE" VARCHAR2(255 BYTE), 
	"ADDRESS" VARCHAR2(255 BYTE), 
	"CITY" VARCHAR2(255 BYTE), 
	"REGION" VARCHAR2(255 BYTE), 
	"POSTALCODE" VARCHAR2(255 BYTE), 
	"COUNTRY" VARCHAR2(255 BYTE), 
	"PHONE" VARCHAR2(255 BYTE), 
	"FAX" VARCHAR2(255 BYTE), 
	"HOMEPAGE" CLOB
   ) SEGMENT CREATION DEFERRED 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  TABLESPACE "MYTABLESPACE" 
 LOB ("HOMEPAGE") STORE AS SECUREFILE (
  TABLESPACE "MYTABLESPACE" ENABLE STORAGE IN ROW 4000 CHUNK 8192
  NOCACHE LOGGING  NOCOMPRESS  KEEP_DUPLICATES ) ;
--------------------------------------------------------
--  DDL for Table USERS
--------------------------------------------------------

  CREATE TABLE "TUNG"."USERS" 
   (	"USERID" NUMBER, 
	"ROLEID" NUMBER, 
	"EMPLOYEEID" NUMBER, 
	"USERNAME" VARCHAR2(255 BYTE), 
	"PASSWORDHASH" VARCHAR2(255 BYTE), 
	"EMAIL" VARCHAR2(255 BYTE), 
	"ISACTIVE" NUMBER(1,0), 
	"CREATEDAT" DATE, 
	"UPDATEDAT" DATE
   ) SEGMENT CREATION IMMEDIATE 
  PCTFREE 10 PCTUSED 40 INITRANS 1 MAXTRANS 255 
 NOCOMPRESS LOGGING
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
REM INSERTING into TUNG.CUSTOMERS
SET DEFINE OFF;
REM INSERTING into TUNG.CATEGORIES
SET DEFINE OFF;
REM INSERTING into TUNG.ORDERDETAILS
SET DEFINE OFF;
Insert into TUNG.ORDERDETAILS (ORDERID,PRODUCTID,UNITPRICE,QUANTITY,DISCOUNT) values (11020,1,20,13,0);
Insert into TUNG.ORDERDETAILS (ORDERID,PRODUCTID,UNITPRICE,QUANTITY,DISCOUNT) values (11018,20,30,5,0);
REM INSERTING into TUNG.EMPLOYEES
SET DEFINE OFF;
Insert into TUNG.EMPLOYEES (EMPLOYEEID,LASTNAME,FIRSTNAME,TITLE,TITLEOFCOURTESY,BIRTHDATE,HIREDATE,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,HOMEPHONE,EXTENSION,REPORTSTO) values (1,'Tung','Nguyen','Software Engineer',null,null,null,null,null,null,null,null,null,null,null);
Insert into TUNG.EMPLOYEES (EMPLOYEEID,LASTNAME,FIRSTNAME,TITLE,TITLEOFCOURTESY,BIRTHDATE,HIREDATE,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,HOMEPHONE,EXTENSION,REPORTSTO) values (18,'Lala','Haha',null,null,null,null,'12 Wall St',null,null,null,null,null,null,null);
Insert into TUNG.EMPLOYEES (EMPLOYEEID,LASTNAME,FIRSTNAME,TITLE,TITLEOFCOURTESY,BIRTHDATE,HIREDATE,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,HOMEPHONE,EXTENSION,REPORTSTO) values (19,'Lju','Hehe',null,null,null,null,'12 Wall St',null,null,null,null,null,null,null);
Insert into TUNG.EMPLOYEES (EMPLOYEEID,LASTNAME,FIRSTNAME,TITLE,TITLEOFCOURTESY,BIRTHDATE,HIREDATE,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,HOMEPHONE,EXTENSION,REPORTSTO) values (20,'haha','LALa','Software Engineer',null,null,null,null,null,null,null,null,null,null,null);
Insert into TUNG.EMPLOYEES (EMPLOYEEID,LASTNAME,FIRSTNAME,TITLE,TITLEOFCOURTESY,BIRTHDATE,HIREDATE,ADDRESS,CITY,REGION,POSTALCODE,COUNTRY,HOMEPHONE,EXTENSION,REPORTSTO) values (17,'Lala','Haha',null,null,null,null,'12 Wall St',null,null,null,null,null,null,null);
REM INSERTING into TUNG.ORDERS
SET DEFINE OFF;
Insert into TUNG.ORDERS (ORDERID,CUSTOMERID,EMPLOYEEID,ORDERDATE,REQUIREDDATE,SHIPPEDDATE,SHIPVIA,FREIGHT,SHIPNAME,SHIPADDRESS,SHIPCITY,SHIPREGION,SHIPPOSTALCODE,SHIPCOUNTRY) values (11020,null,18,to_date('07-JUN-25','DD-MON-RR'),to_date('14-JUN-25','DD-MON-RR'),null,null,null,'N/A','N/A',null,null,null,null);
Insert into TUNG.ORDERS (ORDERID,CUSTOMERID,EMPLOYEEID,ORDERDATE,REQUIREDDATE,SHIPPEDDATE,SHIPVIA,FREIGHT,SHIPNAME,SHIPADDRESS,SHIPCITY,SHIPREGION,SHIPPOSTALCODE,SHIPCOUNTRY) values (11018,null,1,to_date('07-JUN-25','DD-MON-RR'),to_date('14-JUN-25','DD-MON-RR'),null,null,null,'N/A','N/A',null,null,null,null);
REM INSERTING into TUNG.PERMISSIONS
SET DEFINE OFF;
REM INSERTING into TUNG.PRODUCT_IMAGES
SET DEFINE OFF;
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (6,3,'3_20250601173848.jpg','IMG_5712.jpg',19241,572,572);
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (2,1,'1_20250601165807.jpg','465128080_407021305800704_1255899771286892569_n.jpg',188739,1350,1080);
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (3,20,'20_20250601170724.jpg','462692116_1104354258359692_3965275112673535440_n.jpg',93282,1024,1024);
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (4,20,'20_20250601171228.jpg','IMG_4666.jpg',114041,419,419);
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (7,4,'4_20250601173859.jpg','hinh-anh-cute-nu-1.jpg',36447,700,700);
Insert into TUNG.PRODUCT_IMAGES (PRODUCTIMAGEID,PRODUCTID,IMAGEURL,ORIGINALFILENAME,FILESIZE,IMAGEWIDTH,IMAGEHEIGHT) values (5,2,'2_20250601173837.jpg','vegeta-dragon-ball-z-dragon-ball-super-saiyan-wallpaper-preview.jpg',202787,728,1122);
REM INSERTING into TUNG.PRODUCTS
SET DEFINE OFF;
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (20,'Lala',null,null,'20',30,0,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (1,'Game',null,null,'300',20,20,20,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (2,'Nono',null,null,'200',20,30,20,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (3,'Khash',null,null,'200',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (4,'Hehe',null,null,'100',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (5,'Hele',null,null,'100',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (6,'Lile',null,null,'100',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (7,'Hepe',null,null,'100',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (8,'Heye',null,null,'100',20,30,30,null,null);
Insert into TUNG.PRODUCTS (PRODUCTID,PRODUCTNAME,SUPPLIERID,CATEGORYID,QUANTITYPERUNIT,UNITPRICE,UNITSINSTOCK,UNITSONORDER,REORDERLEVEL,DISCONTINUED) values (9,'Heie',null,null,'100',20,30,30,null,null);
REM INSERTING into TUNG.ROLEPERMISSIONS
SET DEFINE OFF;
REM INSERTING into TUNG.ROLES
SET DEFINE OFF;
Insert into TUNG.ROLES (ROLEID,ROLENAME) values (1,'Admin');
Insert into TUNG.ROLES (ROLEID,ROLENAME) values (2,'User');
REM INSERTING into TUNG.SHIPPERS
SET DEFINE OFF;
REM INSERTING into TUNG.SUPPLIERS
SET DEFINE OFF;
REM INSERTING into TUNG.USERS
SET DEFINE OFF;
Insert into TUNG.USERS (USERID,ROLEID,EMPLOYEEID,USERNAME,PASSWORDHASH,EMAIL,ISACTIVE,CREATEDAT,UPDATEDAT) values (1,1,1,'vinhtung','Aimabit14','tung@gmail.com',1,to_date('30-MAY-25','DD-MON-RR'),null);
Insert into TUNG.USERS (USERID,ROLEID,EMPLOYEEID,USERNAME,PASSWORDHASH,EMAIL,ISACTIVE,CREATEDAT,UPDATEDAT) values (11023,2,18,'halahala','Aimabit14','hala@gmail.com',1,to_date('31-MAY-25','DD-MON-RR'),to_date('07-JUN-25','DD-MON-RR'));
Insert into TUNG.USERS (USERID,ROLEID,EMPLOYEEID,USERNAME,PASSWORDHASH,EMAIL,ISACTIVE,CREATEDAT,UPDATEDAT) values (11024,2,19,'hehe','Aimabit14','ha@gmail.com',1,to_date('31-MAY-25','DD-MON-RR'),to_date('02-JUN-25','DD-MON-RR'));
Insert into TUNG.USERS (USERID,ROLEID,EMPLOYEEID,USERNAME,PASSWORDHASH,EMAIL,ISACTIVE,CREATEDAT,UPDATEDAT) values (11025,1,20,'lele','Aimabit14','haha@gmail.com',1,to_date('31-MAY-25','DD-MON-RR'),to_date('03-JUN-25','DD-MON-RR'));
Insert into TUNG.USERS (USERID,ROLEID,EMPLOYEEID,USERNAME,PASSWORDHASH,EMAIL,ISACTIVE,CREATEDAT,UPDATEDAT) values (11022,2,17,'halahala','Aimabit14','hala@gmail.com',1,to_date('30-MAY-25','DD-MON-RR'),null);
--------------------------------------------------------
--  DDL for Index SYS_C008339
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008339" ON "TUNG"."CUSTOMERS" ("CUSTOMERID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008342
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008342" ON "TUNG"."CATEGORIES" ("CATEGORYID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008345
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008345" ON "TUNG"."ORDERDETAILS" ("ORDERID", "PRODUCTID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008340
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008340" ON "TUNG"."EMPLOYEES" ("EMPLOYEEID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008344
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008344" ON "TUNG"."ORDERS" ("ORDERID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008350
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008350" ON "TUNG"."PERMISSIONS" ("PERMISSIONID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008366
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008366" ON "TUNG"."PRODUCT_IMAGES" ("PRODUCTIMAGEID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008343
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008343" ON "TUNG"."PRODUCTS" ("PRODUCTID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008351
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008351" ON "TUNG"."ROLEPERMISSIONS" ("ROLEID", "PERMISSIONID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008347
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008347" ON "TUNG"."ROLES" ("ROLEID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008346
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008346" ON "TUNG"."SHIPPERS" ("SHIPPERID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008341
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008341" ON "TUNG"."SUPPLIERS" ("SUPPLIERID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008348
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008348" ON "TUNG"."USERS" ("USERID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  DDL for Index SYS_C008349
--------------------------------------------------------

  CREATE UNIQUE INDEX "TUNG"."SYS_C008349" ON "TUNG"."USERS" ("EMPLOYEEID") 
  PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE" ;
--------------------------------------------------------
--  Constraints for Table CUSTOMERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."CUSTOMERS" ADD PRIMARY KEY ("CUSTOMERID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table CATEGORIES
--------------------------------------------------------

  ALTER TABLE "TUNG"."CATEGORIES" ADD PRIMARY KEY ("CATEGORYID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ORDERDETAILS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ORDERDETAILS" ADD CONSTRAINT "SYS_C008345" PRIMARY KEY ("ORDERID", "PRODUCTID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table EMPLOYEES
--------------------------------------------------------

  ALTER TABLE "TUNG"."EMPLOYEES" ADD PRIMARY KEY ("EMPLOYEEID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ORDERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ORDERS" ADD PRIMARY KEY ("ORDERID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table PERMISSIONS
--------------------------------------------------------

  ALTER TABLE "TUNG"."PERMISSIONS" ADD PRIMARY KEY ("PERMISSIONID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table PRODUCT_IMAGES
--------------------------------------------------------

  ALTER TABLE "TUNG"."PRODUCT_IMAGES" MODIFY ("PRODUCTID" NOT NULL ENABLE);
  ALTER TABLE "TUNG"."PRODUCT_IMAGES" MODIFY ("IMAGEURL" NOT NULL ENABLE);
  ALTER TABLE "TUNG"."PRODUCT_IMAGES" ADD PRIMARY KEY ("PRODUCTIMAGEID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table PRODUCTS
--------------------------------------------------------

  ALTER TABLE "TUNG"."PRODUCTS" ADD PRIMARY KEY ("PRODUCTID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ROLEPERMISSIONS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ROLEPERMISSIONS" ADD PRIMARY KEY ("ROLEID", "PERMISSIONID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table ROLES
--------------------------------------------------------

  ALTER TABLE "TUNG"."ROLES" ADD PRIMARY KEY ("ROLEID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table SHIPPERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."SHIPPERS" ADD PRIMARY KEY ("SHIPPERID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table SUPPLIERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."SUPPLIERS" ADD PRIMARY KEY ("SUPPLIERID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Constraints for Table USERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."USERS" ADD PRIMARY KEY ("USERID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
  ALTER TABLE "TUNG"."USERS" ADD UNIQUE ("EMPLOYEEID")
  USING INDEX PCTFREE 10 INITRANS 2 MAXTRANS 255 COMPUTE STATISTICS 
  STORAGE(INITIAL 65536 NEXT 1048576 MINEXTENTS 1 MAXEXTENTS 2147483645
  PCTINCREASE 0 FREELISTS 1 FREELIST GROUPS 1
  BUFFER_POOL DEFAULT FLASH_CACHE DEFAULT CELL_FLASH_CACHE DEFAULT)
  TABLESPACE "MYTABLESPACE"  ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table ORDERDETAILS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ORDERDETAILS" ADD CONSTRAINT "FK_ORDERDETAILS_ORDER" FOREIGN KEY ("ORDERID")
	  REFERENCES "TUNG"."ORDERS" ("ORDERID") ENABLE;
  ALTER TABLE "TUNG"."ORDERDETAILS" ADD CONSTRAINT "FK_ORDERDETAILS_PRODUCT" FOREIGN KEY ("PRODUCTID")
	  REFERENCES "TUNG"."PRODUCTS" ("PRODUCTID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table EMPLOYEES
--------------------------------------------------------

  ALTER TABLE "TUNG"."EMPLOYEES" ADD CONSTRAINT "FK_EMPLOYEE_MANAGER" FOREIGN KEY ("REPORTSTO")
	  REFERENCES "TUNG"."EMPLOYEES" ("EMPLOYEEID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table ORDERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ORDERS" ADD CONSTRAINT "FK_ORDER_CUSTOMER" FOREIGN KEY ("CUSTOMERID")
	  REFERENCES "TUNG"."CUSTOMERS" ("CUSTOMERID") ENABLE;
  ALTER TABLE "TUNG"."ORDERS" ADD CONSTRAINT "FK_ORDER_EMPLOYEE" FOREIGN KEY ("EMPLOYEEID")
	  REFERENCES "TUNG"."EMPLOYEES" ("EMPLOYEEID") ENABLE;
  ALTER TABLE "TUNG"."ORDERS" ADD CONSTRAINT "FK_ORDER_SHIPPER" FOREIGN KEY ("SHIPVIA")
	  REFERENCES "TUNG"."SHIPPERS" ("SHIPPERID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table PRODUCT_IMAGES
--------------------------------------------------------

  ALTER TABLE "TUNG"."PRODUCT_IMAGES" ADD CONSTRAINT "FK_PRODUCT_IMAGES_PRODUCT" FOREIGN KEY ("PRODUCTID")
	  REFERENCES "TUNG"."PRODUCTS" ("PRODUCTID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table PRODUCTS
--------------------------------------------------------

  ALTER TABLE "TUNG"."PRODUCTS" ADD CONSTRAINT "FK_PRODUCT_SUPPLIER" FOREIGN KEY ("SUPPLIERID")
	  REFERENCES "TUNG"."SUPPLIERS" ("SUPPLIERID") ENABLE;
  ALTER TABLE "TUNG"."PRODUCTS" ADD CONSTRAINT "FK_PRODUCT_CATEGORY" FOREIGN KEY ("CATEGORYID")
	  REFERENCES "TUNG"."CATEGORIES" ("CATEGORYID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table ROLEPERMISSIONS
--------------------------------------------------------

  ALTER TABLE "TUNG"."ROLEPERMISSIONS" ADD CONSTRAINT "FK_ROLEPERM_ROLE" FOREIGN KEY ("ROLEID")
	  REFERENCES "TUNG"."ROLES" ("ROLEID") ENABLE;
  ALTER TABLE "TUNG"."ROLEPERMISSIONS" ADD CONSTRAINT "FK_ROLEPERM_PERM" FOREIGN KEY ("PERMISSIONID")
	  REFERENCES "TUNG"."PERMISSIONS" ("PERMISSIONID") ENABLE;
--------------------------------------------------------
--  Ref Constraints for Table USERS
--------------------------------------------------------

  ALTER TABLE "TUNG"."USERS" ADD CONSTRAINT "FK_USER_ROLE" FOREIGN KEY ("ROLEID")
	  REFERENCES "TUNG"."ROLES" ("ROLEID") ENABLE;
  ALTER TABLE "TUNG"."USERS" ADD CONSTRAINT "FK_USER_EMPLOYEE" FOREIGN KEY ("EMPLOYEEID")
	  REFERENCES "TUNG"."EMPLOYEES" ("EMPLOYEEID") ENABLE;
