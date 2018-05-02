if(exists (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'client'))
begin
	drop table [dbo].[client]
end

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[client](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[client] varchar(255) not null unique nonclustered,
	[documentid] varchar(255) not null unique nonclustered,
	[keywords] varchar(max)
	PRIMARY KEY(Id)
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

