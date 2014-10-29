USE MVCDemo
GO

/****** Object:  DatabaseRole [di_EFAccess]    Script Date: 9/12/2014 1:26:31 PM ******/
CREATE ROLE [bgm_EFAccess]
GO
GRANT INSERT, SELECT, UPDATE on bgm_Rehearsals to bgm_EFAccess;
