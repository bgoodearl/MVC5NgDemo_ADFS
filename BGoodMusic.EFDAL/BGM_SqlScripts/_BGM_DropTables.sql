--Use BGoodMusicDev;
--USE MVCDemo;
--USE ORCS_rgood3_test;
GO

--********************************************************
-- Drop Tables - start with 
--********************************************************
DECLARE @DropTables bit SET @DropTables = 0;
--DECLARE @DropTables bit SET @DropTables = 1;
IF @DropTables = 1 BEGIN

	DECLARE @TableName NVARCHAR(128), @SqlCmd nvarchar(512)

	SET @TableName = N'bgm_Rehearsals'
	IF (EXISTS (SELECT name FROM sysobjects WHERE (name = @TableName) AND (type = 'U'))) BEGIN
		Set @SqlCmd = N'DROP TABLE ' + @TableName
		EXEC sp_executesql @SqlCmd
		PRINT 'dropped table ' + @TableName
	END ELSE BEGIN
		PRINT 'table ' + @TableName + ' not found for drop.'
	END

	--DECLARE @TableName NVARCHAR(128), @SqlCmd nvarchar(512)
	--SET @TableName = N'bgm_SiteUsers'
	--IF (EXISTS (SELECT name FROM sysobjects WHERE (name = @TableName) AND (type = 'U'))) BEGIN
	--	Set @SqlCmd = N'DROP TABLE ' + @TableName
	--	EXEC sp_executesql @SqlCmd
	--	PRINT 'dropped table ' + @TableName
	--END ELSE BEGIN
	--	PRINT 'table ' + @TableName + ' not found for drop.'
	--END

	--DECLARE @TableName NVARCHAR(128), @SqlCmd nvarchar(512)
	SET @TableName = N'__MigrationHistory'
	IF (EXISTS (SELECT name FROM sysobjects WHERE (name = @TableName) AND (type = 'U'))) BEGIN
		Set @SqlCmd = N'DROP TABLE ' + @TableName
		EXEC sp_executesql @SqlCmd
		PRINT 'dropped table ' + @TableName
	END ELSE BEGIN
		PRINT 'table ' + @TableName + ' not found for drop.'
	END

END;
GO
