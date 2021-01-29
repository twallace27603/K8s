CREATE DATABASE Trivia;
GO
USE Trivia;
Go
CREATE TABLE dbo.Quizzes (
	QuizID uniqueIdentifier Not Null Primary Key Default (NewID()),
	Name nvarchar(100) Not Null Unique,
	Description nvarchar(max) Null
);

CREATE TABLE dbo.Accounts (
	AccountID uniqueIdentifier Not Null Primary Key Default (NewID()),
	Token varchar(4096) Null,
	Email nvarchar(500) Not null unique
);

Create Table dbo.Rounds (
	RoundID uniqueIdentifier Not Null Primary Key Default (NewID()),
	QuizID uniqueIdentifier Not Null ,
	RoundDate DateTime2 Not Null Default(GetDate()),
	CONSTRAINT FKRoundQuiz Foreign Key (QuizID) References Quizzes(QuizID)
);

Create Table dbo.Plays (
	PlayID uniqueIdentifier Not Null Primary Key Default (NewID()),
	RoundID uniqueIdentifier Not Null,
	AccountID uniqueIdentifier Not Null,
	CONSTRAINT FKPlayRound Foreign Key (RoundID) References Rounds(RoundID),
	CONSTRAINT FKPlayAccount Foreign Key (AccountID) References Accounts(AccountID)
);

Create Table dbo.Answers (
	PlayID uniqueIdentifier Not Null,
	QuestionID uniqueIdentifier Not Null,
	Answer int Not null Default (-1),
	Correct bit Not Null Default (0),
	ElapsedTime decimal Not null Default(0),
	CONSTRAINT PKAnswers Primary Key (PlayID, QuestionID),
	CONSTRAINT FKAnswerPlay Foreign Key (PlayID) References Plays(PlayID)
);

INSERT INTO dbo.Accounts(Email) VALUES('twallace@ine.com');

INSERT INTO Quizzes(QuizID,Name,Description) Values (
'efb5d1ba-baea-4859-addc-cadb407e4bc3',
'Core Azure Services',
'Covers core infrastructure and platform services for Azure');

INSERT INTO Quizzes(QuizID,Name,Description) Values (
'60fff66f-ec22-4946-8b91-ab4fcd32c643',
'Azure Cloud Concepts',
'Covers the basic concepts of cloud computing with Microsoft Azure');

GO

CREATE PROC uspGetQuizzes AS
BEGIN
	SELECT * FROM dbo.Quizzes ORDER BY Name;
END

GO
