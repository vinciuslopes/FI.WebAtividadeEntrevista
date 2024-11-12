﻿CREATE TABLE [dbo].[CLIENTES] (
    [ID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [NOME]          VARCHAR (50)   NOT NULL,
    [SOBRENOME]     VARCHAR (255)  NOT NULL,
    [NACIONALIDADE] VARCHAR (50)   NOT NULL,
    [CEP]           VARCHAR (9)    NOT NULL,
    [ESTADO]        VARCHAR (2)    NOT NULL,
    [CIDADE]        VARCHAR (50)   NOT NULL,
    [LOGRADOURO]    VARCHAR (500)  NOT NULL,
    [EMAIL]         VARCHAR (2079) NULL,
    [TELEFONE]      VARCHAR (15)   NULL,
    [CPF] VARCHAR(14) NOT NULL, 
    PRIMARY KEY CLUSTERED ([ID] ASC)
);

