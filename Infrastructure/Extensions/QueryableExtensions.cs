﻿using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Infrastructure.Extensions;
public static class QueryableExtensions
{
    public static IQueryable<T> Sort<T>(this IQueryable<T> source, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return source;

        var orderParams = orderByQueryString.Trim().Split(',');
        var propertyInfos = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var orderQueryBuilder = new StringBuilder();

        foreach (var param in orderParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            var propertyFromQueryName = param.Split(" ")[0];
            var objectProperty = propertyInfos.FirstOrDefault(pi =>
                pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

            if (objectProperty == null)
                continue;

            var direction = param.EndsWith(" desc") ? "descending" : "ascending";
            orderQueryBuilder.Append($"{objectProperty.Name} {direction}, ");
        }

        var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
        if (string.IsNullOrWhiteSpace(orderQuery))
            return source;

        return source.OrderBy(orderQuery);
    }
}
