USE [FoodieDB]
GO
/****** Object:  StoredProcedure [dbo].[User_Crud]    Script Date: 12/10/2024 12:28:21 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[User_Crud]  
    @Action VARCHAR(20),  
    @UserId INT = NULL,  
    @Name varchar(50) = NULL,  
    @Username varchar(50) = NULL,  
    @Mobile varchar(50) = NULL,  
    @Email varchar(50) = NULL,  
    @Address varchar(max) = NULL,  
    @PostCode varchar(50) = NULL,  
    @Password varchar(50) = NULL,  
    @ImageUrl varchar(max) = NULL,
    @Role BIT = NULL  -- Thêm tham số Role
AS  
BEGIN  
    SET NOCOUNT ON;  

    --SELECT FOR LOGIN  
    IF @Action = 'SELECTLOGIN'  
		BEGIN  
			SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password  
		END  

    --SELECT FOR USER PROFILE  
    IF @Action = 'SELECT4PROFILE'  
		BEGIN  
			SELECT * FROM dbo.Users WHERE UserId = @UserId  
		END  

	
    -- Insert (REGISTRATION)  
    IF @Action = 'INSERT'  
		BEGIN  
			INSERT INTO dbo.Users(Name, Username, Mobile, Email, Address, PostCode, Password, ImageUrl, Role, CreatedDate)  
			VALUES (@Name, @Username, @Mobile, @Email, @Address, @PostCode, @Password, @ImageUrl, @Role, GETDATE())  
		END  

	--UPDATE USER PROFILE  
	IF @Action = 'UPDATE'  
		BEGIN  
			DECLARE @UPDATE_IMAGE VARCHAR(20)  
			SELECT @UPDATE_IMAGE = (CASE WHEN @ImageUrl IS NULL THEN 'NO' ELSE 'YES' END)  
			IF @UPDATE_IMAGE = 'NO'  
			BEGIN  
				UPDATE dbo.Users  
				SET Name = @Name, Username = @Username, Mobile = @Mobile, Email = @Email, PostCode = @PostCode, Role = @Role, Password = @Password
				WHERE UserId = @UserId  
			END  
			ELSE  
			BEGIN  
				UPDATE dbo.Users  
				SET Name = @Name, Username = @Username, Mobile = @Mobile, Email = @Email, Address = @Address, PostCode = @PostCode, ImageUrl = @ImageUrl, Role = @Role, Password = @Password  
				WHERE UserId = @UserId  
			END  
		END  

	--SELECT FOR ADMIN  
	IF @Action = 'SELECT4ADMIN'  
		BEGIN  
			SELECT ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS [SrNo], UserId, Name,  
				   Username, Email, Role, CreatedDate , Password  
			FROM Users  
		END  

	--DELETE BY ADMIN  
	IF @Action = 'DELETE'  
		BEGIN  
			DELETE FROM dbo.Users WHERE UserId = @UserId  
		END  
END