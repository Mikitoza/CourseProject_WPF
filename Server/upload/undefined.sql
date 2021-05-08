use CH_UNIVERS;
declare @a char(1) = 'h', 
@b varchar(5) = 'world', 
@c datetime, 
@d int,
@e smallint, 
@f numeric(12,5),
@g time,
@h tinyint;
set @d = 15;
set @f = 25.05;
set @h =5;
set @c =  1994-08-01;
Select @d = 4 , @e = 5;-- присвоение через select
Select  @d,@e,@c,@g
Print 'a = ' + cast(@a as varchar(10));
Print 'b = ' + cast(@b as varchar(10));
Print 'f = ' + cast(@f as varchar(10));
Print 'h = ' + cast(@h as varchar(10));
---------------------------------------------
Declare @capacity numeric(8,3) = (select cast(sum(AUDITORIUM_CAPACITY)
as  numeric(8,3)) from AUDITORIUM), @avgcap real, @kol int, @kolvip numeric(8,3), @proc real;
if @capacity > 200
begin
SELECT @kol = (select cast(count(*) as int) from AUDITORIUM),
       @avgcap = (select cast(avg(AUDITORIUM_CAPACITY) AS REAL) from AUDITORIUM)
Set @kolvip = (select cast(count(*) as numeric(8,3)) from AUDITORIUM where AUDITORIUM_CAPACITY < @avgcap)
SET @proc = @kolvip/@kol * 100;
Select @capacity 'Вместимость', @kol 'Количество', @avgcap 'Средняя', @kolvip '< средней', @proc 'Процент';
end
else if @capacity <200
Print 'Общая вместимость = ' + cast(@capacity as varchar(10));
----------------------------------------------------------------------
Print 'число обработанных строк = ' + cast(@@ROWCOUNT  as varchar(10));
Print 'версия SQL Server ' + cast(@@VERSION  as varchar(10));
Print 'возвращает системный идентификатор процесса, назначенный сервером те-кущему подключению ' + cast(@@SPID as varchar(10));
Print 'код последней ошибки ' + cast(@@ERROR as varchar(10));
Print 'имя сервера ' + cast(@@SERVERNAME as varchar(10));
Print 'возвращает уровень вложенности транзакции ' + cast(@@TRANCOUNT as varchar(10));
Print 'проверка результата считывания строк результирующего набора ' + cast(@@FETCH_STATUS  as varchar(10));
Print 'уровень вложенности текущей процедуры ' + cast(@@NESTLEVEL  as varchar(10));
-------------------------------------------------------------------------------------------------------------------------------------------
declare @t real = 5, @x real = 4, @z real;
IF (@t > @x) SET @z = POWER(SIN(@t), 2);
ELSE IF (@t < @x) SET @z = 4*(@t+@x);
ELSE IF (@t = @x) SET @z = 1 - exp(@x-2);
Print 'z =  ' + cast(@z as varchar(10));
--
declare @fio varchar(70) = 'Чиков Георгий Олегович'
Print @fio
set @fio = left(@fio, CHARINDEX(' ', @fio)) +
left (right (@fio, LEN(@fio) - CHARINDEX(' ', @fio)),1) + '.' +
left (right (@fio, LEN(@fio) - CHARINDEX(' ', @fio, CHARINDEX(' ',@fio)+1)),1) + '.'
Print @fio

--
select NAME, BDAY, year(getdate())-year(BDAY)-1[Age]
From STUDENT
where month(BDAY) = month(getdate())+1

--
select s.Name, p.Subject, s.Idgroup, DATEPART(weekday,p.Pdate)[WeekDay]
from STUDENT s JOIN Progress p
on s.Idstudent = p.Idstudent
where s.Idgroup = 4 and p.Subject = 'СУБД'
------------------------------------------------------------------
declare @xi int = (select COUNT(*) from STUDENT) 
IF (@xi > 90) 
begin
Print 'Студентов много...'; 
end
else 
Print 'Студентов ' + cast(@xi as varchar(10));
------------------------------------------------------------------
select case
            when NOTE between 1 and 3 then 'BAD'
			when NOTE between 4 and 6 then 'NORM' 
			when NOTE between 7 and 10 then 'WELL' 
			else 'ERROR' 
			end Result, count(*) [Sum]
			From dbo.PROGRESS
			Group by case 
			when NOTE between 1 and 3 then 'BAD'
			when NOTE between 4 and 6 then 'NORM' 
			when NOTE between 7 and 10 then 'WELL'
			else 'ERROR' 
            end
------------------------------------------------------------------
/*Create table #temptable
( a int,
  b varchar(100),
  c int
)*/
set nocount on; 
declare @i int = 0;
while @i < 10 
begin 
insert #temptable(a,b,c) 
values(floor(200*rand()), 'курс банка / курс страны', floor(200*rand()))
set @i = @i + 1;
end
select * from #temptable;
drop table #temptable
--------------------------------------------------------------------------------------------
Declare @te int = 1
print @te + 1 
print @te + 2
RETURN
print @te + 3 
--------------------------------------------------------------------------------------------
begin TRY 
UPDATE STUDENT set BDAY = 'Ура'
where BDAY = '1994-08-01' 
end try 
begin CATCH 
PRINT ERROR_NUMBER()
PRINT ERROR_MESSAGE()
PRINT ERROR_LINE()
PRINT ERROR_PROCEDURE()
PRINT ERROR_SEVERITY()
PRINT ERROR_STATE()
end CATCH