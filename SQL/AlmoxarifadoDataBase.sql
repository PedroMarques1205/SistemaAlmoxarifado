CREATE DATABASE DB_EQUIPMENTS;

USE DB_EQUIPMENTS;
GO

CREATE TABLE Equipmentos
(
	Equipmento_ID int PRIMARY KEY,
	Equipmento_Nome varchar(40) NOT NULL,
	Quantidade int NOT NULL
);

CREATE TABLE Movimentacao
(
	Equipamento_ID int NOT NULL,
	Nome_Movimentador varchar(40) NOT NULL,
	Quantidade int NOT NULL,
	Tipo_Movimentacao varchar NOT NULL,
	Estado_conservacao varchar NOT NULL,
	Data_movimentacao date
);
