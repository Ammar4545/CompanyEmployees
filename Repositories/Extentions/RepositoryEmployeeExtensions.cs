using Entities.Models;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Repositories.Extensions
{
    public static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge) =>
            employees.Where(e => (e.Age >= minAge && e.Age <= maxAge));
        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm) 
        { 
            if (string.IsNullOrWhiteSpace(searchTerm)) 
                return employees; 
            
            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        //public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string OrderByQueryString)
        //{
        //    if (string.IsNullOrEmpty(OrderByQueryString))
        //        return employees.OrderBy(e => e.Name);

        //    var orderParam = OrderByQueryString.Trim().Split(",");

        //    var propertyInfos = typeof(Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        //    var orderQueryBuilder = new StringBuilder();
        //}
    }
}
