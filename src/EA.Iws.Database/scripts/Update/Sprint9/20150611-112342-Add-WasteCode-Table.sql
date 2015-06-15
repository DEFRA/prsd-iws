GO
PRINT N'CREATE [Lookup].[WasteCode]...';


CREATE TABLE [Lookup].[WasteCode](
	[Id] [uniqueidentifier] NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CodeType] [int] NOT NULL
 CONSTRAINT [PK_WasteCode] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
PRINT N'Update complete.';



