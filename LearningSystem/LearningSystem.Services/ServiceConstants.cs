namespace LearningSystem.Services
{
    public static class ServiceConstants
    {
        public const int BlogArticlesPageSize = 20;
        public const int AdminCoursesPageSize = 20;
        public const int AdminUsersPageSize = 20;

        public const string PdfCertificateFormat = @"
        <div style='text-align:center'>
        <br/>
        <br/>
        <h1>Certificate</h1>
        <br/>
        <h2>of</h2>
        <br/>
        <h2>{3} - Grade {4}</h2>
        <br/>
        <br/>
        <h2>for</h2>
        <br/>
        <h2>{0} Course</h2>
        <br/>
        <br/>
        <h3>{1}  -  {2}</h3>
        <br/>
        <h4>Signed By {5}</h4>
        <br/>
        <br/>
        <br/>
        </div>
        <h4>SoftUni, Sofia</h4>
        <h4>Date: {6}</h4>
        ";

    }
}