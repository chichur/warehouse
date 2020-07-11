-- команды инициализации и создание структуры бд
-- create user test with password 'test';
-- alter user test with superuser;
-- alter role test set client_encoding to 'utf8';
-- alter role test set default_transaction_isolation to 'read committed';
-- alter role test set timezone to 'UTC';
-- create database warehousedb owner test;

-- таблица хранения платформ и значения их грузов
CREATE TABLE platforms(
  id_platform SERIAL,
  cargo INT,
  PRIMARY KEY (id_platform)
);

-- таблица хранящая структуру склада
CREATE TABLE stocks(
 	id_stock SERIAL,
	name_stock VARCHAR(20),
	id_platform INT,
	picket INT,
	PRIMARY KEY (id_stock),
	FOREIGN KEY (id_platform) REFERENCES platforms(id_platform)
		ON DELETE CASCADE;
);

-- таблица в которую записывается история
-- изменения структуры площадок и склада
CREATE TABLE platforms_history(
	operation char(1) NOT NULL,
    stamp timestamptz NOT NULL,
    id_stock INT NOT NULL,
	name_stock VARCHAR(20) NOT NULL,
	id_platform INT NOT NULL,
	picket INT NOT NULL
);

-- таблица в которую записывается
-- история изменения грузов площадок
CREATE TABLE cargo_history(
	operation char(1) NOT NULL,
	stamp timestamptz NOT NULL,
    id_platform INT NOT NULL,
	cargo INT NOT NULL
);

-- триггерная функция записи изменений структуры склада
CREATE OR REPLACE FUNCTION process_platforms() RETURNS TRIGGER AS $process_platforms$
    BEGIN
		IF (TG_OP = 'INSERT') THEN
			INSERT INTO platforms_history SELECT 'I', now(), NEW.*;
			RETURN NEW;
		ELSIF (TG_OP = 'DELETE') THEN
			INSERT INTO platforms_history SELECT 'D', now(), OLD.*;
			RETURN NEW;
		END IF;
		RETURN NULL;
	END;
$process_platforms$ LANGUAGE plpgsql;

-- триггерная функция ведения учета груза на складе
CREATE OR REPLACE FUNCTION process_cargo() RETURNS TRIGGER AS $process_cargo$
    BEGIN
		IF (TG_OP = 'INSERT') THEN
			INSERT INTO cargo_history SELECT 'I', now(), NEW.*;
			RETURN NEW;
		ELSIF (TG_OP = 'UPDATE') THEN
			INSERT INTO cargo_history SELECT 'U', now(), NEW.*;
			RETURN NEW;
		END IF;
		RETURN NULL;
	END;
$process_cargo$ LANGUAGE plpgsql;

-- создание триггера на таблице структуры склада
CREATE TRIGGER platforms_history
AFTER INSERT OR UPDATE OR DELETE ON stocks
    FOR EACH ROW EXECUTE FUNCTION process_platforms();
	
-- создание триггера на таблице грузов площадок
CREATE TRIGGER process_cargo
AFTER INSERT OR UPDATE OR DELETE ON platforms
    FOR EACH ROW EXECUTE FUNCTION process_cargo();
