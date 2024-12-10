CREATE TYPE [dbo].[OrderDetails] AS TABLE (
    [OrderNo] [varchar](max) NULL,
    [ProductId] [int] NULL,
    [Quantity] [int] NULL,
    [UserId] [int] NULL,
    [Status] [varchar](50) NULL,
    [PaymentId] [int] NULL,
    [OrderDate] [datetime] NULL
)
