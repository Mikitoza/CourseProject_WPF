create database MusicService;
use MusicService;
create table GENRES(
	genre_id int identity(1,1) constraint GENRES_PK primary key,
	genre nvarchar(255) not null
)

create table USERS(
	id_user int identity(1,1) constraint USERS_PK primary key ,
	user_login nvarchar(255) not null,
	user_nickname nvarchar(255),
	user_password nvarchar(255) not null,
	user_role int default 0
);

create table PLAYLISTS(
	id_user int constraint PLAYLISTS_USERS_FK foreign key references USERS(id_user),
	playlist_id int identity(1,1) constraint PLAYLIST_PK primary key,
	playlist_name nvarchar(255) not null
)

create table ALBUMS(
	id_user int constraint ALBUMS_USERS_FK foreign key references USERS(id_user),
	genre_id int constraint ALBUMS_GENRE_FK foreign key references GENRES(genre_id),
	album_id int identity(1,1) constraint ALBUMS_PK primary key,
	albums_name nvarchar(255) not null
)

create table TRACKS(
	id_user int constraint TRACKS_USERS_FK foreign key references USERS(id_user),
	genre_id int constraint TRACKS_GENRE_FK foreign key references GENRES(genre_id),
	album_id int constraint TRACKS_ALBUMS_FK foreign key references ALBUMS(album_id),
	track_id int identity(1,1) constraint TRACKS_PK primary key,
	track_name nvarchar(255) not null
)

create table USERTRACKS_ALL(
	id_user int constraint USERTRACKS_ALL_USERTRACKS_FK foreign key references USERS(id_user),
	tracks_id int constraint USERTRACKS_ALL_TRACKS_FK foreign key references TRACKS(track_id),
	primary key(id_user,tracks_id)
)

create table PLAYLIST_ALL(
	playlists_id int constraint PLAYLISTS_ALL_PLAYLISTS_FK foreign key references PLAYLISTS(playlist_id),
	tracks_id int constraint  PLAYLISTS_ALL_TRACKS_FK foreign key references TRACKS(track_id)
	primary key(playlists_id,tracks_id)
)
