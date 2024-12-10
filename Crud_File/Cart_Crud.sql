CREATE PROCEDURE [dbo].[Cart_Crud]
@Action VARCHAR(10),
@ProductId INT = NULL,
@Quantity INT = NULL,
@UserId INT = NULL
AS
BEGIN
 SET NOCOUNT ON;

 --SELECT
 IF @Action = 'SELECT'
	BEGIN
		SELECT c.ProductId,p.Name,p.ImageUrl,p.Price,c.Quantity, c.Quantity AS Qty,p.Quantity AS PrdQty 
		FROM dbo.carts c
		INNER JOIN dbo.Products p ON p.ProductId = c.ProductId
		WHERE c.UserId = @UserId
	END

--SELECT
 IF @Action = 'INSERT'
	BEGIN
		INSERT INTO dbo.carts(ProductId,Quantity,UserId)
		VALUES (@ProductId, @Quantity, @UserId)
	END

--UPDATE
 IF @Action = 'UPDATE'
	BEGIN
		UPDATE dbo.carts
		SET Quantity = @Quantity 
		WHERE ProductId = @ProductId AND UserId = @UserId
	END

--DELETE
 IF @Action = 'DELETE'
	BEGIN
		DELETE FROM dbo.carts
		WHERE ProductId = @ProductId AND UserId = @UserId
	END

--GET BY ID (PRODUCTID & USERID)
 IF @Action = 'GETBYID'
	BEGIN
		SELECT * FROM dbo.carts
		WHERE ProductId = @ProductId AND UserId = @UserId
	END

END