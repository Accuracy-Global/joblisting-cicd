-- =============================================
-- Author:		Sharad Kumar Chaurasia
-- Create date: 09-07-2014
-- Description:	
-- =============================================
Create PROCEDURE [dbo].[sp_GetBookMarkedJobsByUserName] 
	-- Add the parameters for the stored procedure here
	@Username varchar(50) = NULL,
	@pageNumber int = 1,
	@pageSize int = 10
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	
	SET NOCOUNT ON;
	declare @minRowId int = ((@pageNumber-1) * @pageSize)+1
	declare @maxRowId int =  @pageNumber * @pageSize

    -- Insert statements for procedure here
	SELECT * from ( SELECT ROW_NUMBER() OVER(ORDER BY jbs.Id ASC) AS RowId,
				   COUNT(jbs.Id) OVER() AS TotalRecord,
				   jbs.Id as JobId,
				   jbs.Title as Title, 
				   cmp.Company as Company, 
				   jbm.DateCreated as DateCreated, 
				   st.Text as StateName,
				   cnt.Text as CountryName, 
				   cnd.Username as UserName
				   FROM   dbo.Jobs as jbs
				   INNER JOIN dbo.JobBookmarks jbm ON jbs.Id = jbm.JobId 
				   INNER JOIN dbo.Employers cmp ON jbs.EmployerId = cmp.Id  
				   INNER JOIN dbo.Lists as cnt ON jbs.CountryId = cnt.Id
				   INNER JOIN dbo.Lists as st ON jbs.StateId = st.Id
				   Inner join dbo.Candidates as cnd ON jbm.CandidateId = cnd.Id
				   where @Username is NULL OR cnd.Username = @UserName
				   ) AS ResultSet
				   WHERE RowId BETWEEN @minRowId AND @maxRowId
END
GO