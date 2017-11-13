/****** Object:  Table [dbo].[AuditEntry]    Script Date: 1/11/2015 3:15:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[EfAuditEntry](
	[Id] [uniqueidentifier] NOT NULL,
	[ExpenseReportId] [uniqueidentifier] NOT NULL,
	[EmployeeId] [uniqueidentifier] NULL,
	[EmployeeName] [nvarchar](200) NULL,
	[Date] [datetime] NULL,
	[BeginStatus] [char](3) NULL,
	[EndStatus] [char](3) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
GO


