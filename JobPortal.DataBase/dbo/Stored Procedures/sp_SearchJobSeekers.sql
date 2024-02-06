
-- =============================================
-- Author:		Sharad  Chaurasia
-- Create date: 21-06-2014
-- Description:	This stored procedure provide search functionality for resume.
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchJobSeekers] 
	-- Add the parameters for the stored procedure here
	@title varchar(50) = NULL, 
	@location Varchar(50) = NULL,
	@categoryId int = NULL,
	@specializationId int = NULL,
	@countryId int = NULL,
	@stateId int = NULL,
	@city varchar(50) = NULL,
	@pageNumber int = 1,
	@pageSize int = 10,
	@totalRecord int = NULL output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;
	declare @minRowId int = ((@pageNumber-1) * @pageSize)+1
	declare @maxRowId int =  @pageNumber * @pageSize
	
    --Insert statements for procedure here
SELECT * from (
SELECT ROW_NUMBER() OVER(ORDER BY FullName ASC) AS RowId, 
					COUNT(FullName) OVER() AS TotalRecord,
					[FullName],
					[Title],
					[UploadedResumeFile],
					City,
					[State]
					FROM [dbo].[vw_SearchResume] 
					WHERE (@title is NULL OR Title like '%' + @title + '%')
					AND (@location is NULL OR City like '%' + @location + '%' OR [State] like '%' + @location + '%' )
					) AS ResultSet
					WHERE RowId BETWEEN @minRowId AND @maxRowId
    
    --exec sp_SearchResumes 
    --@title  = NULL, 
	--@location = 'hyder',
	--@categoryId = NULL,
	--@specializationId  = NULL,
	--@countryId  = NULL,
	--@stateId  = NULL,
	--@city = NULL,
	--@pageNumber = 3,
	--@pageSize = 5
END
