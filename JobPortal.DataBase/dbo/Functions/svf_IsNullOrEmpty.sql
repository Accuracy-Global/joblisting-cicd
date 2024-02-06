
-- =============================================
-- Author:		Sharad Chaurasia
-- Create date: 21-06-2014
-- Description:	This function is used to check whether value is null or empty.
-- =============================================
CREATE FUNCTION [dbo].[IsNullOrEmpty] 
(
	-- Add the parameters for the function here
	@val varchar(max)
)
RETURNS bit
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Result bit = NULL

	-- Add the T-SQL statements to compute the return value here
	IF @val IS NOT NULL AND LEN(@val) > 0
	BEGIN
    SET @Result = 0
    END
    ELSE
    BEGIN
    SET @Result = 1
    END


	-- Return the result of the function
	RETURN @Result

END

GO


