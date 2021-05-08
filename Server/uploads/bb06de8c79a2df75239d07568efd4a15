use PES_UNIVER
--1
go
create procedure PSUBJECT
as 
begin
	declare @count int=(select count(*) from SUBJECT_);
	select * from SUBJECT_;
	return @count;
end;

declare @k int=0;
exec @k=PSUBJECT;
print 'количество предметов='+cast(@k as varchar(3));
GO
--drop procedure PSUBJECT
--2
/****** Object:  StoredProcedure [dbo].[PSUBJECT]    Script Date: 17.05.2020 14:33:49 ******/
ALTER procedure [dbo].[PSUBJECT] @p varchar(20)=null,@c int output
as 
begin
	declare @count int=(select count(*) from SUBJECT_);
	select * from SUBJECT_ where PULPIT=@p;
	set @c=@@ROWCOUNT;
	return @count;
end;
GO

declare @k int=0, @r int=0,@p varchar(20)='ИСиТ';
exec @k=PSUBJECT @p, @c=@r output;
print 'количество предмтов всего = '+cast(@k as varchar(3));
print 'количество предметов на кафедре '+@p+'='+cast(@r as varchar(3));

--3
CREATE TABLE #SUBJECT
(
	Код char(10) primary key not null,
	Предмет varchar(100) null,
	Кафедра char(20) null
)
GO
ALTER procedure [dbo].[PSUBJECT] @p varchar(20)=null
as 
begin
	declare @count int=(select count(*) from SUBJECT_);
	select * from SUBJECT_ where PULPIT=@p;
end;

insert #SUBJECT exec PSUBJECT @p = 'ОХ'

select * from #SUBJECT;

--4
go
create procedure PAUDITORIUM_INSERT @a char(20), @n varchar(50), @c int = 0, @t char(10)
as declare @rc int=1
begin
	try
	insert into AUDITORIUM(AUDITORIUM,AUDITORIUM_CAPACITY,AUDITORIUM_NAME,AUDITORIUM_TYPE)
	values (@a,@c,@n,@t)
	return @rc;
	end try
	begin catch
		print 'Номер ошибки: ' + cast(error_number() as varchar(6));
		print 'Сообщение   : ' + error_message();
		print 'Уровень     : ' + cast(error_severity() as varchar(6));
		print 'Метка       : ' + cast(error_state() as varchar(8));
		print 'Номер строки: ' + cast(error_line() as varchar(8));
		if ERROR_PROCEDURE() is not null
		print 'Имя процедуры: '+ error_procedure();
		return -1;
	end catch
go

declare @rc int;
exec @rc = PAUDITORIUM_INSERT @a = '126-1', @c = 100, @n = '126-1', @t = 'ЛК-К';
print 'Код ошибки: ' + cast(@rc as varchar(3));
select * From AUDITORIUM
drop procedure PAUDITORIUM_INSERT
--5

go
create procedure SUBJECT_REPORT @p char(10)
as declare @rc int = 0;                            
begin try
declare @tv char(20), @t char(300) = ' ';  
	declare Subj cursor  for 
      select SUBJECT_ from SUBJECT_ where PULPIT = @p;
      if NOT EXISTS (select SUBJECT_ from SUBJECT_ where PULPIT = @p)
          raiserror('ошибка', 11, 1);
       else 
      open Subj;	  
  fetch Subj into @tv;   
  print   'Предметы: ';   
  while @@fetch_status = 0                                     
   begin 
       set @t = rtrim(@tv) + ', ' + @t;  
       set @rc = @rc + 1;       
       fetch  Subj into @tv; 
    end;   
print @t;        
close  Subj;
    return @rc;
   end try  
   begin catch            
        print 'Ошибка в параметрах' 
        if error_procedure() is not null   
		print 'Имя процедуры : ' + error_procedure();
        return @rc;
   end catch; 

declare @rc2 int;  
exec @rc2 = SUBJECT_REPORT @p  = 'ИСиТ';  
print 'Предметов всего = ' + cast(@rc2 as varchar(3));

--6

drop procedure PAUDITORIUM_INSERT 
go
create procedure PAUDITORIUM_INSERT @a char(20), @c int, @n varchar(50), @t char(10)
as declare @rc int=1
begin
	try
	insert into AUDITORIUM(AUDITORIUM,AUDITORIUM_CAPACITY,AUDITORIUM_NAME,AUDITORIUM_TYPE)
	values (@a,@c,@n,@t)
	return @rc;
	end try
	begin catch
		print 'номер ошибки: ' + cast(error_number() as varchar(6));
		print 'сообщение: ' + error_message();
		print 'уровень: ' + cast(error_severity() as varchar(6));
		print 'метка: ' + cast(error_state() as varchar(8));
		print 'номер строки: ' + cast(error_line() as varchar(8));
		if error_procedure() is not null
		print 'имя процедуры: '+ error_procedure();
		return -1;
	end catch
go

create procedure PAUDITORIUM_INSERTX @a char(20), @n varchar(50), @c int, @t char(10), @tn varchar(50)   
as  declare @rc int=1;                            
begin try 
    set transaction isolation level serializable;          
    begin tran
    insert into AUDITORIUM_TYPE(AUDITORIUM_TYPE, AUDITORIUM_TYPENAME)  
                                               values (@t,@tn)
    exec @rc=PAUDITORIUM_INSERT @a, @c, @n, @t;  
    commit tran; 
    return @rc;           
end try
begin catch 
    print 'номер ошибки  : ' + cast(error_number() as varchar(6));
    print 'сообщение     : ' + error_message();
    print 'уровень       : ' + cast(error_severity()  as varchar(6));
    print 'метка         : ' + cast(error_state()   as varchar(8));
    print 'номер строки  : ' + cast(error_line()  as varchar(8));
    if error_procedure() is not  null print 'имя процедуры : ' + error_procedure();
     if @@trancount > 0 rollback tran ; 
     return -1;	  
end catch;



declare @rc3 int;  
exec @rc3 = PAUDITORIUM_INSERTX @a = '130-1', @c = 40, @n = 'ЛК_И', @t = 'ЛК_И', @tn = 'ЛК_И';
print 'код ошибки=' + cast(@rc3 as varchar(3));  
drop Procedure PAUDITORIUM_INSERTX
