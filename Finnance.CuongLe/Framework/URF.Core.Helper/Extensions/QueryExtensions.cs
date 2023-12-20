using FastMember;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using URF.Core.EF.Trackable;
using URF.Core.EF.Trackable.Enums;
using URF.Core.EF.Trackable.Models;

namespace URF.Core.Helper.Extensions
{
    public static class QueryExtensions
    {
        private static readonly List<string> PROPERTIES = new List<string> { "Id", "MeeyId", "MeeyUserId", "Url", "Code", "Name", "ShortName", "FullName", "Title", "Email", "Phone", "Subject", "UserName", "NameShort", "NameUnsigned", "FileName", "FilePath", "CustomerName", "CustomerEmail", "CustomerPhone", "TitleVn", "TitleEn", "NameVn", "NameEn", "SystemName", "Controller", "Action" };

        public static IQueryable<T> FilterQuery<T>(this IQueryable<T> db) where T : ISqlExEntity
        {
            return db
                .Where(c => !c.IsDelete.HasValue || !c.IsDelete.Value)
                .Where(c => !c.IsActive.HasValue || c.IsActive.Value);
        }

        public static IQueryable<T> FilterQueryNoTraking<T>(this IQueryable<T> db) where T : SqlExEntity
        {
            return db
                .AsNoTracking()
                .Where(c => !c.IsDelete.HasValue || !c.IsDelete.Value)
                .Where(c => !c.IsActive.HasValue || c.IsActive.Value);
        }

        public static IQueryable ToSelect<T>(this IQueryable<T> db, string propertyName)
        {
            return db.Select("new (" + propertyName + ")", null);
        }

        public static ResultApi ToQuery<T>(this IQueryable<T> db, TableData model = null)
        {
            if (db == null)
            {
                return new ResultApi
                {
                    Object = null,
                    ObjectExtra = model,
                };
            }
            var items = db.ToFilter(model).ToOrder(model).ToPaging(model).ToList();
            var count = db.ToFilter(model).ToOrder(model).Count();
            if (items.Count == 0 && model.Paging.Index > 1)
            {
                model.Paging.Index = 1;
                return db.ToQuery(model);
            }
            else
            {
                model.Paging.Total = count;
                if (model.Paging.Size.IsNumberNull()) model.Paging.Size = 20;
                model.Paging.Pages = Math.Ceiling((decimal)model.Paging.Total / model.Paging.Size).ToInt32();
                return new ResultApi
                {
                    Object = items,
                    ObjectExtra = model,
                };
            }
        }

        public static IQueryable<T> ToOrder<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();
            if (model.Orders.IsNullOrEmpty())
                model.Orders = new List<OrderData> { new OrderData { Name = "Id", Type = OrderType.Desc } };

            IQueryable<T> query = db;
            var propertyNames = typeof(T).GetProperties().Select(c => c.Name).ToList();
            foreach (var item in model.Orders.Where(c => !c.Name.IsStringNullOrEmpty()).Distinct())
            {
                if (propertyNames.Contains(item.Name))
                {
                    var name = item.Name;
                    var type = db.ElementType;
                    var parameter = Expression.Parameter(type, "");
                    var property = Expression.Property(parameter, name);
                    var lambda = Expression.Lambda(property, parameter);
                    var methodName = item.Type == OrderType.Asc ? "OrderBy" : "OrderByDescending";

                    Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName, new[] { type, property.Type }, db.Expression, Expression.Quote(lambda));
                    query = query.Provider.CreateQuery<T>(methodCallExpression);
                }
            }
            return query;
        }

        public static IQueryable<T> ToFilter<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();
            if (model.Filters.IsNullOrEmpty())
                model.Filters = new List<FilterData>
                {
                    new FilterData { Name = "IsActive", Value = true, Compare = CompareType.B_Equals },
                    new FilterData { Name = "IsDelete", Value = false, Compare = CompareType.B_Equals }
                };

            // Ignores
            if (!model.IgnoreIds.IsNullOrEmpty())
            {
                if (model.IgnoreIds.Count > 1 && !model.IgnoreIds.FirstOrDefault().IsNumberNull())
                    db = db.Where("!@0.Contains(Id)", model.IgnoreIds);
            }

            // Filter Searching
            if (!model.Search.IsStringNullOrEmpty())
            {
                var filter = new FilterData
                {
                    Value = model.Search,
                    Compare = CompareType.Search,
                    Name = string.Join(";", PROPERTIES),
                };
                db = FilterEx(db, filter);
            }

            // Filter
            var filters = model.Filters.Distinct().ToList();
            foreach (var filter in filters)
            {
                db = FilterEx(db, filter);
            }
            return db;
        }

        public static IQueryable<T> ToPaging<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();
            if (model.Paging == null)
                model.Paging = new PagingData { Index = 1, Size = 20 };
            if (model.Paging.Index.IsNumberNull())
                model.Paging.Index = 1;
            if (model.Paging.Size.IsNumberNull())
                model.Paging.Size = 20;

            int size = model.Paging.Size;
            int index = model.Paging.Index;
            var itemstoSkip = size * (index - 1);
            return db.Skip(itemstoSkip).Take(size);
        }

        public static IQueryable ToSelect<T>(this IQueryable<T> db, List<string> propertyNames)
        {
            return db.Select("new (" + string.Join(",", propertyNames) + ")", null);
        }


        public static async Task<ResultApi> ToQueryAsync<T>(this IQueryable<T> db, TableData model = null, int total = 0)
        {
            if (db == null)
            {
                return new ResultApi
                {
                    Object = null,
                    ObjectExtra = model,
                };
            }
            var items = await db.ToFilter(model).ToOrder(model).ToPaging(model).ToListAsync();
            if (items.Count == 0 && model.Paging.Index > 1)
            {
                model.Paging.Index = 1;
                return await db.ToQueryAsync(model);
            }
            else
            {
                var count = 0;
                if (total.IsNumberNull())
                {
                    if (model.Paging.Index <= 1)
                    {
                        if (items.Count > model.Paging.Size)
                            count = items.Count;
                    }
                    if (count.IsNumberNull()) count = await db.ToFilter(model).CountAsync();
                }
                else count = total;
                model.Paging.Total = count;
                model.Paging.Pages = Math.Ceiling((decimal)model.Paging.Total / model.Paging.Size).ToInt32();
                return new ResultApi
                {
                    Object = items,
                    ObjectExtra = model,
                };
            }
        }

        public static async Task<List<T>> ToQueryListAsync<T>(this IQueryable<T> db, TableData model = null)
        {
            if (db == null) return null;
            return await db.ToFilter(model).ToOrder(model).ToPaging(model).ToListAsync();
        }

        public static async Task<DataTable> ToDataTableAsync<T>(this IQueryable<T> db, TableData model = null)
        {
            var table = new DataTable();
            var items = await db.ToFilter(model).ToOrder(model).ToPaging(model).ToListAsync();
            using var reader = ObjectReader.Create(items);
            table.Load(reader);
            return table;
        }

        public static async Task<List<IDictionary<string, object>>> ToDictionaryAsync<T>(this IQueryable<T> db, TableData model = null)
        {
            var table = new DataTable();
            var items = await db.ToFilter(model).ToOrder(model).ToPaging(model).ToListAsync();
            if (!items.IsNullOrEmpty())
                return items.Select(c => c.ToDictionary()).ToList();
            return new List<IDictionary<string, object>>();
        }

        private static IQueryable<T> FilterEx<T>(IQueryable<T> db, FilterData filter)
        {
            var propertyItems = filter.Name.ToListString();
            if (propertyItems.IsNullOrEmpty()) return db;

            try
            {
                var propertyNames = propertyItems
                    .Where(c => typeof(T).GetProperty(c) != null)
                    .ToList() ?? new List<string>();
                if (filter.Compare == CompareType.Search)
                {
                    var whereQuery = string.Empty;
                    foreach (var property in propertyNames)
                    {
                        var valueIds = ToListInt(filter.Value.ToString());
                        var valueStrings = filter.Value.ToString().ToListString();
                        if (!whereQuery.IsStringNullOrEmpty()) whereQuery += " OR ";
                        whereQuery += property == "Id"
                            ? string.Join(" OR ", valueIds.Select(c => property + " = " + c))
                            : string.Join(" OR ", valueStrings.Select(c => property + ".Contains(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                    }
                    if (!whereQuery.IsStringNullOrEmpty()) db = db.Where(whereQuery);
                }
                else
                {
                    var parameter = Expression.Parameter(db.ElementType, "");
                    var properties = propertyNames.Select(c => Expression.Property(parameter, c)).ToList();
                    var property = propertyNames.FirstOrDefault();
                    if (!property.IsStringNullOrEmpty())
                    {
                        var compareType = CorrectCompareTypeEquals(filter.Compare, properties.First());
                        switch (compareType)
                        {
                            case CompareType.B_Equals:
                                {
                                    if (property == "IsActive" && filter.Value.ToString().ToBoolean() == true)
                                    {
                                        var values = filter.Value.ToString().ToListBoolean();
                                        var whereQuery = property + " == null OR " + property + ".Equals(true)";
                                        db = db.Where(whereQuery);
                                    }
                                    else if (property == "IsDelete" && filter.Value.ToString().ToBoolean() == false)
                                    {
                                        var values = filter.Value.ToString().ToListBoolean();
                                        var whereQuery = property + " == null OR " + property + ".Equals(false)";
                                        db = db.Where(whereQuery);
                                    }
                                    else
                                    {
                                        var values = filter.Value.ToString().ToListBoolean();
                                        var whereQuery = string.Join(" OR ", values.Select(c => property + ".Equals(" + c + ")"));
                                        db = db.Where(whereQuery);
                                    }
                                }
                                break;
                            case CompareType.B_NotEquals:
                                {
                                    var values = filter.Value.ToString().ToListBoolean();
                                    var whereQuery = string.Join(" AND ", values.Select(c => "!" + property + ".Equals(" + c + ")"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.N_Equals:
                                {
                                    var values = ToListInt(filter.Value.ToString(), false);
                                    var nullable = properties.First().Member.ToString().ContainsEx("nullable");
                                    var whereQuery = nullable && values.IsNullOrEmpty()
                                        ? "!" + property + ".HasValue"
                                        : string.Join(" OR ", values.Select(c => property + " == " + c));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.N_NotEquals:
                                {
                                    var values = ToListInt(filter.Value.ToString(), false);
                                    var whereQuery = string.Join(" AND ", values.Select(c => property + " != " + c));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.N_GreaterThan:
                                {
                                    var value = filter.Value.ToString().ToInt32();
                                    db = db.Where(property + " > @0", value);
                                }
                                break;
                            case CompareType.N_GreaterThanOrEqual:
                                {
                                    var value = filter.Value.ToString().ToInt32();
                                    db = db.Where(property + " >= @0", value);
                                }
                                break;
                            case CompareType.N_LessThan:
                                {
                                    var value = filter.Value2.IsNumberNull()
                                        ? filter.Value.ToString().ToInt32()
                                        : filter.Value2.ToString().ToInt32();
                                    db = db.Where(property + " < @0", value);
                                }
                                break;
                            case CompareType.N_LessThanOrEqual:
                                {
                                    var value = filter.Value2.IsNumberNull()
                                        ? filter.Value.ToString().ToInt32()
                                        : filter.Value2.ToString().ToInt32();
                                    db = db.Where(property + " <= @0", value);
                                }
                                break;
                            case CompareType.N_Between:
                                {
                                    var value = filter.Value.ToString().ToInt32();
                                    var value2 = filter.Value2.ToString().ToInt32();
                                    db = db.Where(property + " >= @0 and " + property + " <= @1", value, value2);
                                }
                                break;
                            case CompareType.N_NotBetween:
                                {
                                    var value = filter.Value.ToString().ToInt32();
                                    var value2 = filter.Value2.ToString().ToInt32();
                                    db = db.Where(property + " < @0 and " + property + " > @1", value, value2);
                                }
                                break;
                            case CompareType.S_Equals:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" OR ", values.Select(c => property + ".Equals(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_NotEquals:
                                {
                                    {
                                        var values = filter.Value.ToString().ToListString();
                                        var whereQuery = string.Join(" AND ", values.Select(c => "!" + property + ".Equals(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                        db = db.Where(whereQuery);
                                    }
                                }
                                break;
                            case CompareType.S_Contains:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" OR ", values.Select(c => property + ".Contains(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_NotContains:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" AND ", values.Select(c => "!" + property + ".Contains(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_StartWith:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" OR ", values.Select(c => property + ".StartsWith(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_NotStartWith:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" AND ", values.Select(c => "!" + property + ".StartsWith(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_EndWith:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" OR ", values.Select(c => property + ".EndsWith(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.S_NotEndWith:
                                {
                                    var values = filter.Value.ToString().ToListString();
                                    var whereQuery = string.Join(" AND ", values.Select(c => "!" + property + ".EndsWith(\"" + c + "\", StringComparison.OrdinalIgnoreCase)"));
                                    db = db.Where(whereQuery);
                                }
                                break;
                            case CompareType.D_Equals:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    var start = value; var end = value.AddDays(1).AddMilliseconds(-1);
                                    db = db.Where(property + " >= @0 and " + property + " <= @1", start, end);
                                }
                                break;
                            case CompareType.D_NotEquals:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    var start = value; var end = value.AddDays(1).AddMilliseconds(-1);
                                    db = db.Where(property + " < @0 and " + property + " > @1", start, end);
                                }
                                break;
                            case CompareType.D_GreaterThan:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " > @0", value);
                                }
                                break;
                            case CompareType.D_LessThan:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " < @0", value);
                                }
                                break;
                            case CompareType.D_GreaterThanOrEqual:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " >= @0", value);
                                }
                                break;
                            case CompareType.D_LessThanOrEqual:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " <= @0", value);
                                }
                                break;
                            case CompareType.D_Between:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    var value2 = filter.Value2.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " >= @0 and " + property + " <= @1", value, value2);
                                }
                                break;
                            case CompareType.D_NotBetween:
                                {
                                    var value = filter.Value.ToDateTime("dd/MM/yyyy");
                                    var value2 = filter.Value2.ToDateTime("dd/MM/yyyy");
                                    db = db.Where(property + " < @0 and " + property + " > @1", value, value2);
                                }
                                break;
                        }
                    }
                }
                return db;
            }
            catch
            {
                return db;
            }
        }
        private static List<int> ToListInt(string source, bool removeNullOrEmpty = true)
        {
            if (source.IsStringNullOrEmpty()) return new List<int>();
            var list = source.ReplaceNewLine().Split(new[] { "[", ",", ";", "]", "\"", " " }, StringSplitOptions.RemoveEmptyEntries).Select(c => c.ToInt32());
            return removeNullOrEmpty ? list.Where(c => !c.IsNumberNull()).ToList() : list.ToList();
        }
        private static CompareType CorrectCompareTypeEquals(CompareType type, Expression property)
        {
            if (type == CompareType.S_Equals)
            {
                if (property.Type.FullName.ContainsEx("int") || property.Type.FullName.ContainsEx(".Enums.") || property.Type.FullName.ContainsEx(".Enum."))
                {
                    return CompareType.N_Equals;
                }
                else if (property.Type.FullName.ContainsEx("bool"))
                {
                    return CompareType.B_Equals;
                }
                else if (property.Type.FullName.ContainsEx("float"))
                {
                    return CompareType.N_Equals;
                }
                else if (property.Type.FullName.ContainsEx("double"))
                {
                    return CompareType.N_Equals;
                }
                else if (property.Type.FullName.ContainsEx("decimal"))
                {
                    return CompareType.N_Equals;
                }
                else if (property.Type.FullName.ContainsEx("datetime"))
                {
                    return CompareType.D_Equals;
                }
            }
            return type;
        }
    }
}
