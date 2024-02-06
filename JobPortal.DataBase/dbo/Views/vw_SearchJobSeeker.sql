

CREATE VIEW [dbo].[vw_SearchJobSeeker]
AS
SELECT     ISNULL(dbo.Candidates.FirstName, N'') + ISNULL(dbo.Candidates.MiddleName, N' ') + ISNULL(dbo.Candidates.LastName, N'') AS FullName, dbo.Resumes.Title, 
                      dbo.Resumes.UploadedResumeFile, dbo.Resumes.CategoryId, dbo.Resumes.SpecializationId, dbo.Candidates.CountryId, dbo.Lists.Text AS CountryName, 
                      dbo.Candidates.City, dbo.Candidates.State, dbo.Candidates.StateId
FROM         dbo.Resumes INNER JOIN
                      dbo.candidate_resume ON dbo.Resumes.Id = dbo.candidate_resume.resume_id INNER JOIN
                      dbo.Candidates ON dbo.candidate_resume.candidate_id = dbo.Candidates.Id INNER JOIN
                      dbo.Lists ON dbo.Candidates.CountryId = dbo.Lists.Id AND dbo.Lists.Name = 'Country'

GO




