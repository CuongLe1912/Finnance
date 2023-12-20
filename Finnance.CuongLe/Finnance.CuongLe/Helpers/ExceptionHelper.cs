using Microsoft.AspNetCore.Mvc;

namespace Meey.Admin.Helper
{
    public class ExceptionHelper
    {
        public static ContentResult HandleException(Exception ex, int? userId = null)
        {
            if (ex is UnauthorizedAccessException)
            {
                return new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Invalid login or access token",
                };
            }
            else
            {
                CreateLogException(ex, userId);
                throw ex;
            }
        }

        private static void CreateLogException(Exception ex, int? userId = null)
        {
            try
            {
                //using (var db = new MeeyAdminContext())
                //{
                //    var log = new LogException
                //    {
                //        UserId = userId,
                //        Exception = ex.Message,
                //        DateTime = DateTime.Now,
                //        StackTrace = ex.StackTrace,
                //        InnerException = ex.InnerException?.Message,
                //    };
                //    db.LogExceptions.Add(log);
                //    db.SaveChanges();
                //}
            }
            catch { }
        }
    }
}
