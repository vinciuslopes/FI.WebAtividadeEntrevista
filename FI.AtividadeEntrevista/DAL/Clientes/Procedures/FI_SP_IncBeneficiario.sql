﻿CREATE PROC FI_SP_IncBeneficiario
    @CPF		   VARCHAR (14)	,
    @NOME          VARCHAR (50) ,
    @IDCLIENTE     BIGINT		
AS
BEGIN
	INSERT INTO BENEFICIARIOS (CPF, NOME, IDCLIENTE) 
	VALUES (@CPF,@NOME,@IDCLIENTE)

	SELECT SCOPE_IDENTITY()
END