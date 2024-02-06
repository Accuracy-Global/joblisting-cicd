
-- =============================================
-- Author:		Sharad  Chaurasia
-- Create date: 21-06-2014
-- Description:	This stored procedure provide search functionality for resume.
-- =============================================
Create PROCEDURE [dbo].[sp_SearchJobs] 
	-- Add the parameters for the stored procedure here
	@categoryId int = NULL,
	@specializationId int = NULL,
	@countryId int = NULL,
	@EmploymentTypeId int = null,
	@ZipCode varchar(50) = null,
	@City varchar(50) = null,
	@JobQualificationId int = null,
	@FromDate datetime = null,
	@ToDate datetime = null,
	@pageNumber int = 1,
	@pageSize int = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	declare @minRowId int = ((@pageNumber-1) * @pageSize)+1
	declare @maxRowId int =  @pageNumber * @pageSize
	
	DECLARE @sql nvarchar(MAX)
	
    --Insert statements for procedure here
    SELECT @sql =   
    'SELECT * FROM (
    SELECT   ROW_NUMBER() OVER(ORDER BY jbs.Id ASC) AS RowId,
             COUNT(jbs.Id) OVER() AS TotalRecord,
			 jbs.Id, 
			 jbs.Title, 
			 [State].Text AS StateName, 
			 Country.Text AS CountryName, 
			 jbs.CountryId, 
			 jbs.StateId, 
			 jbs.PermaLink, 
             emp.Company, 
             jbs.Summary, 
             jbs.PublishedDate,
             jbs.CategoryId,
             jbs.SepecializationId,
             jbs.EmploymentTypeId,
             jbs.QualificationId,
             jbs.Zip,
             jbs.IsFeaturedJob
	FROM	 dbo.Jobs as jbs INNER JOIN
			 dbo.Lists AS Country ON jbs.CountryId = Country.Id INNER JOIN
			 dbo.Lists AS [State] ON jbs.StateId = [State].Id INNER JOIN
			 dbo.Employers AS emp ON jbs.EmployerId = emp.Id
			 WHERE  1 = 1'
			 
			 IF @categoryId IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND jbs.CategoryId = '+CAST(@categoryId AS varchar(5))+'' 
			 
			 IF @specializationId IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND jbs.SepecializationId = '+CAST(@specializationId AS varchar(5))+''  
			 
			 IF @countryId IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND jbs.CountryId = '+CAST(@countryId AS varchar(5))+''  
			 
			 IF @EmploymentTypeId  IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND jbs.EmploymentTypeId = '+CAST(@EmploymentTypeId  AS varchar(5))+''  
			 
			 IF @JobQualificationId IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND jbs.QualificationId = '+CAST(@JobQualificationId AS varchar(5))+''  
			 
			 IF @ZipCode  IS NOT NULL AND @zipCode != ''                                            
			 SELECT @sql = @sql + ' AND jbs.Zip like ''%'+@ZipCode+'%'''  
			 
			 IF @City IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND ([State].Text like ''%'+@City+'%'' OR  Country.Text like ''%'+@City+'%'')'
			 
			 IF @City IS NOT NULL                                            
			 SELECT @sql = @sql + ' AND ([State].Text like ''%'+@City+'%'' OR  Country.Text like ''%'+@City+'%'')'
			 
			 IF @FromDate  IS NOT NULL                                          
			 SELECT @sql = @sql + ' AND jbs.PublishedDate >= '''+ CONVERT(VARCHAR(23), @FromDate, 120) + ''''  
			 
			 IF @ToDate  IS NOT NULL                                          
			 SELECT @sql = @sql + ' AND jbs.PublishedDate <= '''+ CONVERT(VARCHAR(23), @ToDate, 120) + ''''  
			 
			 SELECT @sql = @sql + ')  As ResultSet
			 where RowId BETWEEN '+ CAST(@minRowId AS varchar(5))+' AND '+ CAST(@maxRowId AS varchar(5))+''
	
	print @sql
	
    EXEC sp_executesql @sql
    
--exec sp_SearchJobs 
--@EmploymentTypeId=316,
--@categoryId=NULL,
--@specializationId=NULL,
--@countryId=NULL,
--@ZipCode='1234',
--@City='Alabama',
--@JobQualificationId=345,
--@FromDate=NULL,
--@ToDate=NULL,
--@pageNumber=1,
--@pageSize=10

END
