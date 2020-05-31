USE yisha_admin;
DROP FUNCTION IF EXISTS `Fun_GetNewName`;
DELIMITER $$
CREATE FUNCTION Fun_GetNewName(name varchar(50)) RETURNS varchar(50) DETERMINISTIC
BEGIN
	 SET name = CONCAT(UCASE(LEFT(name, 1)),SUBSTRING(name, 2));	
	 SET name = REPLACE(name,'_a','A');
	 SET name = REPLACE(name,'_b','B');
	 SET name = REPLACE(name,'_c','C');
	 SET name = REPLACE(name,'_d','D');
	 SET name = REPLACE(name,'_e','E');
	 SET name = REPLACE(name,'_f','F');
	 SET name = REPLACE(name,'_g','G');
	 SET name = REPLACE(name,'_h','H');
	 SET name = REPLACE(name,'_i','I');
	 SET name = REPLACE(name,'_j','J');
	 SET name = REPLACE(name,'_k','K');
	 SET name = REPLACE(name,'_l','L');
	 SET name = REPLACE(name,'_m','M');
	 SET name = REPLACE(name,'_n','N');
	 SET name = REPLACE(name,'_o','O');
	 SET name = REPLACE(name,'_p','P');
	 SET name = REPLACE(name,'_q','Q');
	 SET name = REPLACE(name,'_r','R');
	 SET name = REPLACE(name,'_s','S');
	 SET name = REPLACE(name,'_t','T');
	 SET name = REPLACE(name,'_u','U');
	 SET name = REPLACE(name,'_v','V');
	 SET name = REPLACE(name,'_w','W');
	 SET name = REPLACE(name,'_x','X');
	 SET name = REPLACE(name,'_y','Y');
	 SET name = REPLACE(name,'_z','Z');	
   RETURN name;
END 
$$
DROP PROCEDURE IF EXISTS `SP_UpdateColumnName`;
DELIMITER $$
CREATE PROCEDURE `SP_UpdateColumnName`()
    BEGIN
        DECLARE tableName       VARCHAR(50);
        DECLARE fieldName       VARCHAR(50);
        DECLARE fieldType       VARCHAR(50);
        DECLARE tableDone BOOLEAN DEFAULT false;
        DECLARE fieldDone BOOLEAN DEFAULT false;
        
        DECLARE curTable CURSOR FOR SELECT table_name FROM information_schema.tables WHERE table_schema = DATABASE() AND 
																						   (table_type='base table' or table_type='BASE TABLE') AND 
																						   table_name in('sys_area','sys_auto_job','sys_auto_job_log','sys_data_dict','sys_data_dict_detail','sys_department',
																										 'sys_log_api','sys_log_login','sys_log_operate','sys_menu','sys_menu_authorize','sys_news',
																										 'sys_position','sys_role','sys_user','sys_user_belong');
        DECLARE CONTINUE HANDLER FOR NOT FOUND SET tableDone = TRUE;   
        OPEN curTable; 
            curTableLoop: LOOP
                 FETCH curTable INTO tableName;
                 IF tableDone THEN
                        LEAVE curTableLoop;
                 END IF;
					  BEGIN
					  		DECLARE curField CURSOR FOR SELECT column_name,column_type FROM information_schema.columns WHERE table_schema = DATABASE() AND table_name = tableName;
				         DECLARE CONTINUE HANDLER FOR NOT FOUND SET fieldDone = TRUE;				          
				         OPEN curField;				 
				            curFieldLoop: LOOP
				                 FETCH curField INTO fieldName,fieldType;
				                 IF fieldDone THEN
				                        LEAVE curFieldLoop;
				                 END IF;
									  BEGIN 
									  		SET @newFieldName = Fun_GetNewName(fieldName);
									      SET @tempSql = CONCAT('ALTER TABLE ', tableName ,' CHANGE COLUMN ',fieldName,' ',@newFieldName ,' ',fieldType);
									      SELECT @tempSql;
									      PREPARE stmt FROM @tempSql;
										   EXECUTE stmt;
										   DEALLOCATE PREPARE stmt;									      
									  END;
				            END LOOP curFieldLoop;				          
				         CLOSE curField;
				         SET @newTableName = Fun_GetNewName(tableName);
					      SET @tempSql = CONCAT('RENAME TABLE ', tableName ,' TO ',@newTableName);
							SELECT @tempSql;
							PREPARE stmt FROM @tempSql;
							EXECUTE stmt;
							DEALLOCATE PREPARE stmt;
							SELECT tableName;
							SET fieldDone = FALSE;
					  END;
            END LOOP curTableLoop;          
        CLOSE curTable;
    END;
$$
call SP_UpdateColumnName();
DROP FUNCTION IF EXISTS `Fun_GetNewName`;
DROP PROCEDURE IF EXISTS `SP_UpdateColumnName`;