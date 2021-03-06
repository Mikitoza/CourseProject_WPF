-- План:
-- Время проведения олимпиады с 9:50 по 13:00. 
-- Чистое время - 3 часа. 
-- 0. Создать базу данных под именем OlympUser_XX, где XX - ваш номер.
-- 1. Выполнить скрипт создания таблиц в своей БД.
-- 2. Создать скрипт с именем OlympUser_XX, в который вы будете помещать решения задач.
-- 3. В комментариях обязательно указать номер задачи.
-- 4. Скрипт должен быть передан преподавателю до 13:00.

-- Некоторые заметки:
-- предполагается, что в качестве решения будет использован SQL запрос или предложение SQL;
-- задачи можно делать в любом порядке;
-- по сложности задачи расположены в произвольном порядке;
-- количество баллов за каждую задачу - величина переменная.

-- Скрипт создания таблиц
-- drop table dept;
-- drop table emp;

/* Таблица подразделений */
use OlympUser_37;
create table DEPT
(
  DEPTNO int not null,
  DNAME  VARCHAR(14) not null,
  LOC    VARCHAR(13)
)
;

alter table DEPT
  add constraint DEPT_PK primary key (DEPTNO);
alter table DEPT
  add constraint DEPT_UK unique (DNAME);

/* Таблица сотрудников */
create table EMP
(
  EMPNO    int not null,
  ENAME    VARCHAR(10) not null,
  JOB      VARCHAR(9),
  MGR      INT,
  HIREDATE DATE,
  TERMDATE DATE,	
  SAL      decimal(7,2),
  COMM     decimal(7,2),
  DEPTNO   int,
  PHONE    VARCHAR(13)
)
;

alter table EMP
  add constraint EMP_PK primary key (EMPNO);
alter table EMP
  add constraint EMP_UK unique (ENAME);
alter table EMP
  add constraint EMP_DEPT_FK foreign key (DEPTNO)
  references DEPT (DEPTNO);
alter table EMP
  add constraint EMP_MGR_FK foreign key (MGR)
  references EMP (EMPNO);


/* Данные по подразделениям */
INSERT INTO DEPT VALUES (10,  'ACCOUNTING', 'NEW YORK');
INSERT INTO DEPT VALUES (20,  'RESEARCH',   'DALLAS');
INSERT INTO DEPT VALUES (30,  'SALES',      'CHICAGO');
INSERT INTO DEPT VALUES (40,  'OPERATIONS', 'BOSTON');
INSERT INTO DEPT VALUES (50,  'LOGISTICS',  'MIAMI');

/* Данные по сотрудникам */
INSERT INTO EMP VALUES (7839, 'KING',   'PRESIDENT',  NULL, '17/11/81',  NULL,         5000, NULL, 10, '+375176003034');
INSERT INTO EMP VALUES (7698, 'BLAKE',  'MANAGER',    7839, '01/05/82',   NULL,         2850, NULL, 30, '+375176003035');
INSERT INTO EMP VALUES (7782, 'CLARK',  'MANAGER',    7839, '9/06/91',   NULL,         2450, NULL, 10, '+375176003036');
INSERT INTO EMP VALUES (7566, 'JONES',  'MANAGER',    7839, '2/04/81',   '1/08/99',   2975, NULL, 20, '+375176003037');
INSERT INTO EMP VALUES (7654, 'MARTIN', 'SALESMAN',   7698, '28/09/83',  '3/08/85',   1250, 400,  30, '+375176003038');
INSERT INTO EMP VALUES (7499, 'ALLEN',  'SALESMAN',   7698, '20/02/84',  '24/01/89',  1600, 300,  30, '+375176003039');
INSERT INTO EMP VALUES (7844, 'TURNER', 'SALESMAN',   7698, '8/09/81',   '10/02/91',  1500, 0,    30, '+375176003023');
INSERT INTO EMP VALUES (7900, 'JAMES',  'CLERK',      7698, '3/12/81',   NULL,         750,  NULL, 30, '+375176003024');
INSERT INTO EMP VALUES (7521, 'WARD',   'SALESMAN',   7698, '22/02/89',  NULL,         1250, 500,  30, '+375176003025');
INSERT INTO EMP VALUES (7902, 'FORD',   'ANALYST',    7566, '3/12/81',   NULL,         3000, NULL, 20, '+375176003026');
INSERT INTO EMP VALUES (7369, 'SMITH',  'CLERK',      7902, '17/12/96',  NULL,         800,  NULL, 20, '+375176003027');
INSERT INTO EMP VALUES (7788, 'SCOTT',  'ANALYST',    7566, '09/12/82',  '3/11/83',   3000, NULL, 20, '+375176003028');
INSERT INTO EMP VALUES (7876, 'ADAMS',  'CLERK',      7788, '12/01/92',  NULL,         1100, NULL, 20, '+375176003029');
INSERT INTO EMP VALUES (7934, 'MILLER', 'CLERK',      7782, '23/01/82',  NULL,         800,  NULL, 10, '+375176003050');

-- Задачи:
-- 1. Найти самое значительное повышение зарплаты CLERK. 
-- Если в 81 году клерку устанавливали зарплату в 750, 
-- в 82 - 800, а в 83 - 1100, то ответ '82-83'.


-- 2. Найти среднее значение зарплаты без максимального и минимального значений.


-- 3. В какие годы не принимали сотрудников на работу? 
-- первый год - дата приема первого сотрудника, последний - 2021.


-- 4. Подсчитать, в какие годы сколько работало сотрудников в отделах в виде сводного отчета:

-- Год 	ACCOUNTING	RESEARCH	SALES	OPERATIONS	LOGISTICS
-- 1981		5	4		3		2	0
-- 1982		6	2		5		3	0
-- Если сотрудник проработал хотя бы один день, то он работал в этом году.


-- 5.Вывести в одну строку все имена и зарплаты в формате ename: sal. 
-- Выводить через запятую, в конце - точка.


-- 6. Какому менеджеру (MANAGER) подчиняется больше всего сотрудников?


-- 7. Отсортировать сотрудников по возрастанию зарплаты (SAL),
-- причем вначале должны идти сотрудники с назначенными комиссионными (COMM),
-- а только после них те, у кого комиссионные не назначены.


-- 8. Написать SELECT запрос, в результате которого будет возвращаться 
-- средняя зарплата по коду отдела или фраза 'EMPTY DEPT' в случае пустого отдела.


-- 9. Известно, что в год создания компании сотрудники работали и в выходные: 
-- в субботу 8 часов и 6 часов в воскресенье. 
-- Сколько часов они отработали? 


-- 10. Сколько человеко-часов они отработали?

