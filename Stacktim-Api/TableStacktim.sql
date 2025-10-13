DROP TABLE IF EXISTS dbo.Players;
DROP TABLE IF EXISTS dbo.Teams;
DROP TABLE IF EXISTS dbo.TeamPlayers;

CREATE TABLE Players(
   Id_Players INT IDENTITY(1,1) PRIMARY KEY,
   Name VARCHAR(50) NOT NULL UNIQUE,
   Email VARCHAR(100) NOT NULL UNIQUE,
   RankPlayer VARCHAR(20),
   TotalScore INT NOT NULL DEFAULT 0,
   RegistrationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
   CONSTRAINT CHK_Player_Rank CHECK (RankPlayer IN ('Bronze', 'Silver', 'Gold', 'Platinum', 'Diamond', 'Master')),
   CONSTRAINT CHK_Player_TotalScore CHECK (TotalScore >= 0)
);
 
INSERT INTO Players (Name, Email, RankPlayer, TotalScore) VALUES
('Diana', 'diana@email.com', 'Gold', 1500),
('Logy', 'logy@email.com', 'Platinum', 2200),
('Polm', 'polm@email.com', 'Gold', 1800),
('Sefi', 'sefi@email.com', 'Diamond', 3100),
('Kiko', 'kiko@email.com', 'Silver', 900),
('Azerty', 'azerty@email.com', 'Master', 4500);


CREATE TABLE Teams(
   Id_Teams INT IDENTITY(1,1) PRIMARY KEY,
   Name VARCHAR(50) NOT NULL UNIQUE,
   Tag CHAR(3) NOT NULL UNIQUE,
   CaptainId INT NOT NULL,
   CreationDate DATETIME2 NOT NULL DEFAULT GETDATE(),
   CONSTRAINT CHK_Teams_Tag CHECK (Tag LIKE '[A-Z][A-Z][A-Z]'),
   CONSTRAINT FK_Teams_Captain FOREIGN KEY (CaptainId) REFERENCES Players(Id_Players)
);

INSERT INTO Teams(Name, Tag, CaptainId) VALUES
('Fortnite', 'FTR', 1),
('Minecraft', 'MIN', 2),
('Call of Duty', 'COD', 3);

CREATE TABLE TeamPlayers(
   PlayerId INT NOT NULL,
   TeamId INT NOT NULL,
   Role INT NOT NULL,
   JoinDate DATETIME NOT NULL DEFAULT GETDATE(),
   PRIMARY KEY(PlayerId, TeamId),
   FOREIGN KEY(PlayerId) REFERENCES Players(Id_Players) ON DELETE CASCADE,
   FOREIGN KEY(TeamId) REFERENCES Teams(Id_Teams) ON DELETE CASCADE,
   CONSTRAINT CHK_TeamPlayers_Role CHECK (Role IN (0, 1, 2))
);

INSERT INTO TeamPlayers(PlayerId, TeamId, Role) VALUES
(1, 1, 0),
(2, 1, 1),
(3, 2, 0),
(4, 2, 1),
(5, 3, 0),
(6, 3, 1); 