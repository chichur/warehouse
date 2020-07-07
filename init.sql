-- create user test with password 'test';
-- alter role test set client_encoding to 'utf8';
-- alter role test set default_transaction_isolation to 'read committed';
-- alter role test set timezone to 'UTC';

-- create database warehousedb owner test;

CREATE TABLE platforms(
  id_platform SERIAL,
  cargo INT,
  PRIMARY KEY (id_platform)
);

CREATE TABLE stocks(
 	id_stock SERIAL,
	name_stock VARCHAR(20),
	id_platform INT,
	picket INT,
	PRIMARY KEY (id_stock),
	FOREIGN KEY (id_platform) REFERENCES platforms(id_platform)
);

CREATE TABLE platforms_history(
    stamp timestamp NOT NULL,
    id_stock INT NOT NULL,
	name_stock VARCHAR(20) NOT NULL,
	id_platform INT NOT NULL,
	picket INT NOT NULL
);

CREATE TABLE cargo_history(
	stamp timestamp NOT NULL,
    id_platform INT NOT NULL,
	cargo INT NOT NULL
);

CREATE OR REPLACE FUNCTION process_platforms() RETURNS TRIGGER AS $process_platforms$
    BEGIN
		EXECUTE format('INSERT INTO %I SELECT now(), * FROM %I', TG_NAME, TG_TABLE_NAME);
		RETURN NULL;
    END;
$process_platforms$ LANGUAGE plpgsql;

CREATE TRIGGER platforms_history
AFTER INSERT OR UPDATE OR DELETE ON stocks
    FOR EACH STATEMENT EXECUTE PROCEDURE process_platforms();
	
CREATE TRIGGER cargo_history
AFTER INSERT OR UPDATE OR DELETE ON platforms
    FOR EACH STATEMENT EXECUTE PROCEDURE process_platforms();
	
