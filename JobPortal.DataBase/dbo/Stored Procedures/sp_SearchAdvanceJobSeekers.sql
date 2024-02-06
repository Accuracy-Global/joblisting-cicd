
-- =============================================
-- Author:		Sharad  Chaurasia
-- Create date: 21-06-2014
-- Description:	This stored procedure provide search functionality for resume.
-- =============================================
CREATE PROCEDURE [dbo].[sp_SearchAdvanceJobSeekers] 
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
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @minRowId int = ((@pageNumber-1) * @pageSize)+1
	declare @maxRowId int =  @pageNumber * @pageSize
	
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
					AND (@categoryId is NULL OR CategoryId = @CategoryId)
					AND (@specializationId is NULL OR SpecializationId = @specializationId)
					AND (@countryId is NULL OR CountryId = @countryId)
					AND (@stateId is NULL OR StateId = @stateId)) AS ResultSet
					WHERE RowId BETWEEN @minRowId AND @maxRowId
    
    --exec sp_SearchAdvanceResumes @title=N'network'
				--			,@location=N''
				--			,@categoryId=1
				--			,@specializationId=25
				--			,@countryId=NULL
				--			,@stateId=NULL
				--			,@city=N''
				--			,@pageNumber=1
				--			,@pageSize=100
END
