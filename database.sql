CREATE DATABASE MusicPlayer

USE MusicPlayer
GO

CREATE TABLE DBPlayList
	(	Id int identity(1,1) primary key, 
		Name varchar(100) 
	)
	
CREATE TABLE DBFiles
	(	Id int identity(1,1) primary key, 
		Number int,
		Path varchar(100),
		PLId int foreign key references DBPlayList(Id),
	)

CREATE TABLE ForSdachaLaba
	(	DBPlayList_Id int foreign key, 
		DBFiles_Id int foreign key
	)