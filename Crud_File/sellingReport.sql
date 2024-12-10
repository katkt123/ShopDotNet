CREATE PROCEDURE SellingReport
    @FromDate DATE = NULL,
    @ToDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ROW_NUMBER() OVER(ORDER BY (SELECT 1)) AS [SrNo],u.Name, u.Email, 
        SUM(o.Quantity) AS TotalOrders, SUM(o.Quantity * p.Price) AS TotalPrice
    FROM Orders o
    INNER JOIN Products p ON p.ProductId = o.ProductId
    INNER JOIN Users u ON u.UserId = o.UserId
    WHERE CAST(o.OrderDate AS DATE) BETWEEN @FromDate AND @ToDate
    GROUP BY u.Name, u.Email;
END