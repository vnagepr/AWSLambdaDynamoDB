using CourseAPI.Domains.Models;

namespace CourseAPI.Domains.Interfaces
{
    public interface ICourseService
    {
        public Task<List<Course>> GetAllCourses();
        public Task<Course> GetCourseById(string id);
        public Task<Boolean> SaveCourse(CourseParam course);
        public Task<Course> UpdateCourse(CourseParam course);
        public Task<Boolean> DeleteCourseById(string id);
    }
}
