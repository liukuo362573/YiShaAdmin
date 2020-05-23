USE yisha_admin
IF OBJECT_ID (N'dbo.FUN_GetNewName', N'FN') IS NOT NULL  
	DROP FUNCTION FUN_GetNewName;
GO
CREATE FUNCTION dbo.FUN_GetNewName(@name VARCHAR(50))  
RETURNS VARCHAR(50) 
AS 
BEGIN
    DECLARE @tempName VARCHAR(50);
	SET @tempName = @name;
	SET @tempName = CONCAT(UPPER(LEFT(@tempName, 1)),SUBSTRING(@tempName, 2,LEN(@tempName)-1));
    SET @tempName = REPLACE(@tempName,'_a','A');
	SET @tempName = REPLACE(@tempName,'_b','B');
	SET @tempName = REPLACE(@tempName,'_c','C');
	SET @tempName = REPLACE(@tempName,'_d','D');
	SET @tempName = REPLACE(@tempName,'_e','E');
	SET @tempName = REPLACE(@tempName,'_f','F');
	SET @tempName = REPLACE(@tempName,'_g','G');
	SET @tempName = REPLACE(@tempName,'_h','H');
	SET @tempName = REPLACE(@tempName,'_i','I');
	SET @tempName = REPLACE(@tempName,'_j','J');
	SET @tempName = REPLACE(@tempName,'_k','K');
	SET @tempName = REPLACE(@tempName,'_l','L');
	SET @tempName = REPLACE(@tempName,'_m','M');
	SET @tempName = REPLACE(@tempName,'_n','N');
	SET @tempName = REPLACE(@tempName,'_o','O');
	SET @tempName = REPLACE(@tempName,'_p','P');
	SET @tempName = REPLACE(@tempName,'_q','Q');
	SET @tempName = REPLACE(@tempName,'_r','R');
	SET @tempName = REPLACE(@tempName,'_s','S');
	SET @tempName = REPLACE(@tempName,'_t','T');
	SET @tempName = REPLACE(@tempName,'_u','U');
	SET @tempName = REPLACE(@tempName,'_v','V');
	SET @tempName = REPLACE(@tempName,'_w','W');
	SET @tempName = REPLACE(@tempName,'_x','X');
	SET @tempName = REPLACE(@tempName,'_y','Y');
	SET @tempName = REPLACE(@tempName,'_z','Z');
	RETURN @tempName;
END;
GO

DECLARE @tableName           VARCHAR(50)
DECLARE @fieldName           VARCHAR(50)
DECLARE @newTableName        VARCHAR(50)
DECLARE @newFieldName        VARCHAR(50)
DECLARE @oldTableColumnName  VARCHAR(50)
DECLARE curTable CURSOR FOR 
	SELECT name FROM sysobjects WHERE xtype = 'u' and name in('sys_area','sys_auto_job','sys_auto_job_log','sys_data_dict','sys_data_dict_detail','sys_department',
															  'sys_log_api','sys_log_login','sys_log_operate','sys_menu','sys_menu_authorize','sys_news',
															  'sys_position','sys_role','sys_user','sys_user_belong')
OPEN curTable								 
FETCH NEXT FROM curTable INTO @tableName     
WHILE (@@fetch_status = 0 )            
    BEGIN
		DECLARE curField CURSOR FOR 
			SELECT name FROM SysColumns Where id = Object_Id(@tableName)
		OPEN curField
		FETCH NEXT FROM curField INTO @fieldName  
		WHILE ( @@fetch_status = 0 )     
		BEGIN
			SET @newFieldName = dbo.FUN_GetNewName(@fieldName)
			SET @oldTableColumnName = CONCAT(@tableName,'.',@fieldName)
				
			print @oldTableColumnName + '*****' + @newFieldName
			EXEC [sp_rename] @objname = @oldTableColumnName , 
					            @newname = @newFieldName, 
							    @objtype = 'COLUMN'
			FETCH NEXT FROM curField INTO @fieldName
		END
		CLOSE curField								
		DEALLOCATE curField
		SET @newTableName = dbo.FUN_GetNewName(@tableName)
		print @tableName + '-------------------------' + @newTableName	
		EXEC sp_rename @tableName, @newTableName;  
        FETCH NEXT FROM curTable INTO @tableName	
    END
CLOSE curTable
DEALLOCATE curTable
DROP FUNCTION dbo.FUN_GetNewName