
-- =============================================
-- Author:		Sharad Chaurasia
-- Create date: 21-06-2014
-- Description:	This function is used to check whether value is null or empty.
-- =============================================
CREATE FUNCTION [dbo].[GetMaximumExperience] 
(
	-- Add the parameters for the function here
	@val varchar(10)
)
RETURNS tinyint
AS
BEGIN

	-- Declare the return variable here
	DECLARE @Result tinyint = NULL

	-- Add the T-SQL statements to compute the return value here
	IF @val IS NOT NULL AND LEN(@val) > 0
	BEGIN
			IF(CHARINDEX('-', @val) > 0)
			BEGIN
			SET @Result = SUBSTRING(@val, CHARINDEX('-', @val)+1,LEN(@val))
			END
			ELSE IF(@val = 'Fresh')
			BEGIN
			SET @Result = 0
			END
			ELSE IF(@val = '10+')
			BEGIN
			SET @Result = NULL
			END
    END
    ELSE
    BEGIN
    SET @Result = NULL
    END


	-- Return the result of the function
	RETURN @Result

END