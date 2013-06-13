if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spb_Task_ Forum_PerStage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop proc [dbo].[spb_Task_ Forum_PerStage]

DECLARE fks cursor for
select 'alter table ['+ object_name(parent_obj) + '] drop constraint ['+name+']; '
from sysobjects where (xtype = 'D' or xtype = 'F') and name like '%spb_%' or name like '%tn_%'
open fks
declare @dfk varchar(8000)
fetch next from fks into @dfk
while(@@fetch_status=0)
begin
	exec(@dfk)
	fetch next from fks into @dfk
end
close fks
deallocate fks
GO

DECLARE fks cursor for
select 'alter table ['+ object_name(parent_obj) + '] drop constraint ['+name+']; '
from sysobjects where xtype = 'PK' and name like '%spb_%' or name like '%tn_%'
open fks
declare @dfk varchar(8000)
fetch next from fks into @dfk
while(@@fetch_status=0)
begin
	exec(@dfk)
	fetch next from fks into @dfk
end
close fks
deallocate fks
GO

declare @sc_Item varchar(500)
declare @xtype varchar(10)
declare schame cursor
for select [name],[xtype] from sysobjects where xtype = 'P' or xtype = 'V' or (xtype = 'U' and name like 'spb_%')
open schame
fetch next from schame into @sc_Item,@xtype
while @@fetch_status = 0
begin
	if @xtype = 'U' and EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('old_' + @sc_Item))
	begin
		break;
	end

	if @xtype = 'U' and NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('old_' + @sc_Item))
		exec('exec sp_rename ''' + @sc_Item+''',''old_'+@sc_Item + '''')
	else if @xtype = 'P'
		exec('drop procedure ' + @sc_Item)
	else if @xtype = 'V'
		exec('drop View ' + @sc_Item)
	fetch next from schame into @sc_Item,@xtype
end
close schame
deallocate schame
