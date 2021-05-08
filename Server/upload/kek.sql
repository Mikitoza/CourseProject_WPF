use PES_UNIVER
Go

--1 point
declare @sub char(20), @result char(300)=''
declare ListSubject cursor for select SUBJECT_ from SUBJECT_ where PULPIT = 'ИСИТ'
open ListSubject;
	fetch ListSubject into @sub;
	print 'Предметы:'
while @@FETCH_STATUS=0
	begin
		set @result=RTRIM(@sub)+', '+ @result;
		fetch ListSubject into @sub;
	end;
	print @result;
close ListSubject
deallocate ListSubject;

--2 point
DECLARE ListOfStudent CURSOR GLOBAL                            
	             for SELECT NAME_, BDAY from STUDENT;
DECLARE @name char(40), @bday varchar(50);      
	OPEN ListOfStudent;	  
	FETCH  ListOfStudent INTO @name, @bday; 	
      PRINT '1. '+@name + @bday;  
GO
 DECLARE @name char(40), @bday varchar(50);     	
	FETCH  ListOfStudent INTO @name, @bday; 	
      PRINT '2. '+@name+@bday;  
GO	  
close ListOfStudent
deallocate ListOfStudent;


DECLARE ListOfStudent CURSOR LOCAL                            
	             for SELECT NAME_, BDAY from STUDENT;
DECLARE @name char(40), @bday varchar(50);      
	OPEN ListOfStudent;	  
	FETCH  ListOfStudent INTO @name, @bday; 
      PRINT '1. '+@name + @bday;  
GO
 DECLARE @name char(40), @bday varchar(50);     	
	FETCH  ListOfStudent INTO @name, @bday; 	
      PRINT '2. '+@name+@bday;  
GO

--3 point
Declare @faculty varchar(20),@name varchar(20);
DECLARE ListF CURSOR LOCAL static
	for select FACULTY, FACULTY_NAME
	from FACULTY;
open ListF;
print 'Количество строк : '+cast(@@CURSOR_ROWS as varchar(5)); 
INSERT FACULTY(FACULTY, FACULTY_NAME) 
	                 values ('ФИТ', 'Информационные технологии'); 

fetch ListF into @faculty,@name
while @@FETCH_STATUS=0
begin
	print @faculty+' - '+@name;
	fetch ListF into @faculty,@name;
end;
close ListF
go

delete AUDITORIUM where AUDITORIUM = '315-2'

--4 point
declare @group int,@faculty char(10);
DECLARE ListG CURSOR LOCAL STATIC 
	for select ROW_NUMBER() over(order by IDGROUP) N,FACULTY
	from GROUPS 
	
	OPEN ListG;
	FETCH  RELATIVE 3 from ListG into  @group, @faculty;                 
		print '3 строка вперед от текущей: ' + cast(@group as varchar(3))+ ' - ' + cast(@faculty as varchar(10));   
	FETCH  NEXT from  ListG into  @group, @faculty;       
		print 'следующая: ' +  cast(@group as varchar(3))+ ' - ' + cast(@faculty as varchar(10));
	FETCH  FIRST from  ListG into  @group, @faculty;       
		print 'первая:' +  cast(@group as varchar(3))+ ' - ' + cast(@faculty as varchar(10));
	FETCH  LAST from  ListG into  @group, @faculty;       
		print 'последняя строка: ' +  cast(@group as varchar(3))+ ' - ' + cast(@faculty as varchar(10));   
	FETCH  PRIOR from  ListG into  @group, @faculty;       
		print 'предыдущая: ' +  cast(@group as varchar(3))+ ' - ' +cast(@faculty as varchar(10));
	FETCH  ABSOLUTE 3 from  ListG into  @group, @faculty;       
		print '3 c начала: ' +  cast(@group as varchar(3))+ ' - ' + cast(@faculty as varchar(10));
	
CLOSE ListG;
DEALLOCATE ListG;
go

--5 point
declare @sub char(10),@stud int,@note int;
DECLARE ListNote CURSOR LOCAL dynamic 
	for select SUBJECT,IDSTUDENT,NOTE
	from PROGRESS for update;
							  			   
OPEN ListNote;
	FETCH  ListNote into @sub,@stud,@note;                 
		DELETE PROGRESS WHERE CURRENT OF ListNote;
	FETCH  ListNote into @sub,@stud,@note;  
		UPDATE PROGRESS SET NOTE=NOTE+1 WHERE CURRENT OF ListNote;
CLOSE ListNote;
DEALLOCATE ListNote;
GO

--6 point
select * from STUDENT as S
inner join PROGRESS as P
on P.IDSTUDENT = S.IDSTUDENT
inner join GROUPS as G
on G.IDGROUP = S.IDGROUP

declare @note varchar(2), @name varchar(100);
declare ListProgress cursor local dynamic
for select NOTE,IDSTUDENT from PROGRESS for update;

open ListProgress;
fetch ListProgress into @note,@name;
while @@FETCH_STATUS = 0
	begin
		print @name + ' - ' + @note
		if cast(@note as int) < 4 
			delete PROGRESS where current of ListProgress
		else
			print @note
			fetch ListProgress into @note, @name;
	end;
fetch last from ListProgress into @note, @name;
update PROGRESS set NOTE = NOTE + 1 where IDSTUDENT=1023;
close ListProgress;
