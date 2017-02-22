-- http://stackoverflow.com/a/23635319
    SELECT 
        t.TABLE_CATALOG,
        t.TABLE_SCHEMA,
        t.TABLE_NAME,
        'create table '+QuoteName(t.TABLE_SCHEMA)+'.' + QuoteName(so.name) + ' (' + LEFT(o.List, Len(o.List)-1) + ');  ' 
            + CASE WHEN tc.Constraint_Name IS NULL THEN '' 
              ELSE 
                'ALTER TABLE ' + QuoteName(t.TABLE_SCHEMA)+'.' + QuoteName(so.name) 
                + ' ADD CONSTRAINT ' + tc.Constraint_Name  + ' PRIMARY KEY ' + ' (' + LEFT(j.List, Len(j.List)-1) + ');  ' 
              END as 'SQL_CREATE_TABLE'
    FROM sysobjects so

    CROSS APPLY (
        SELECT 
              '  ['+column_name+'] ' 
              +  data_type 
              + case data_type
                    when 'sql_variant' then ''
                    when 'text' then ''
                    when 'ntext' then ''
                    when 'decimal' then '(' + cast(numeric_precision as varchar) + ', ' + cast(numeric_scale as varchar) + ')'
                  else 
                  coalesce(
                    '('+ case when character_maximum_length = -1 
                        then 'MAX' 
                        else cast(character_maximum_length as varchar) end 
                    + ')','') 
                end 
            + ' ' 
            + case when exists ( 
                SELECT id 
                FROM syscolumns
                WHERE 
                    object_name(id) = so.name
                    and name = column_name
                    and columnproperty(id,name,'IsIdentity') = 1 
              ) then
                'IDENTITY(' + 
                cast(ident_seed(so.name) as varchar) + ',' + 
                cast(ident_incr(so.name) as varchar) + ')'
              else ''
              end 
            + ' ' 
            + (case when IS_NULLABLE = 'No' then 'NOT ' else '' end) 
            + 'NULL ' 
            + case when information_schema.columns.COLUMN_DEFAULT IS NOT NULL THEN 'DEFAULT '+ information_schema.columns.COLUMN_DEFAULT 
              ELSE '' 
              END 
            + ','  -- can't have a field name or we'll end up with XML

        FROM information_schema.columns 
        WHERE table_name = so.name
        ORDER BY ordinal_position
        FOR XML PATH('')
    ) o (list)

    LEFT JOIN information_schema.table_constraints tc on  
        tc.Table_name = so.Name
        AND tc.Constraint_Type  = 'PRIMARY KEY'

    LEFT JOIN information_schema.tables t on  
        t.Table_name = so.Name

    CROSS APPLY (
        SELECT QuoteName(Column_Name) + ', '
        FROM information_schema.key_column_usage kcu
        WHERE kcu.Constraint_Name = tc.Constraint_Name
        ORDER BY ORDINAL_POSITION
        FOR XML PATH('')
    ) j (list)

    WHERE
        xtype = 'V'
        AND name NOT IN ('dtproperties')
        -- AND so.name = 'ASPStateTempSessions'
    ;