using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace URF.Core.Controller
{
    public static class QueryExtension
    {
        private static readonly List<string> PROPERTIES = new List<string> { "Id", "Code", "Name", "FullName", "Title", "Email", "Phone", "Subject", "UserName", "NameShort", "NameUnsigned", "FileName", "FilePath" };

        public static ResultApi ToQuery<T>(this IQueryable<T> db, TableData model = null)
        {
            var items = db.ToFilter(model).ToOrder(model).ToPaging(model).ToList();
            var count = db.ToFilter(model).ToOrder(model).Count();

            model.Paging.Total = count;
            model.Paging.Pages = count / model.Paging.Size;
            return new ResultApi
            {
                Object = items,
                ObjectExtra = model,
            };
        }

        public static IQueryable<T> ToOrder<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();
            if (model.Orders.IsNullOrEmpty())
                model.Orders = new List<OrderData> { new OrderData { Name = "Id", Type = OrderType.Desc } };

            IQueryable<T> query = db;
            foreach (var item in model.Orders.Distinct())
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
            return query;
        }

        public static IQueryable<T> ToFilter<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();

            // Filter Searching
            if (!model.Search.IsStringNullOrEmpty())
            {
                if (model.Filters.IsNullOrEmpty()) model.Filters = new List<FilterData>();
                foreach (var item in typeof(T).GetProperties()
                    .Where(c => PROPERTIES.Any(p => p.EqualsEx(c.Name))))
                {
                    model.Filters.Add(new FilterData
                    {
                        Name = item.Name,
                        Value = model.Search,
                        Compare = CompareType.S_Contains,
                    });
                }
            }

            // Filter
            foreach (var filter in model.Filters.Distinct())
            {
                var propertyNames = new string[] { filter.Name }
                    .Where(c => typeof(T).GetProperty(c) != null)
                    .ToList();
                if (propertyNames.IsNullOrEmpty()) continue;

                try
                {
                    MethodCallExpression whereCallExpression = null;
                    var parameter = Expression.Parameter(db.ElementType, "");
                    var properties = propertyNames.Select(c => Expression.Property(parameter, c)).ToList();

                    var compareType = filter.Compare;
                    var property = properties.First();
                    var value = filter.Value.ToString();
                    var value2 = filter.Value2.IsStringNullOrEmpty()
                        ? string.Empty
                        : filter.Value2.ToString();
                    var constantExpression = InitConstantExpression(property, value);
                    var constantExpression2 = InitConstantExpression(property, value2, true);

                    switch (compareType)
                    {
                        case CompareType.B_Equals:
                            {
                                BinaryExpression bodyExpression;
                                if (property.Member.Name.Equals("IsDelete") && value.ToBoolean() == false)
                                {
                                    var equalValue = Expression.Equal(property, constantExpression);
                                    var equalNull = Expression.Equal(property, InitConstantExpression(property, null));
                                    bodyExpression = Expression.Or(equalValue, equalNull);
                                }
                                else bodyExpression = Expression.Equal(property, constantExpression);

                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.B_NotEquals:
                            {
                                var bodyExpression = Expression.NotEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.F_Equals:
                        case CompareType.N_Equals:
                            {
                                Expression bodyExpression = null;
                                if (value.Contains(";") || value.Contains(","))
                                {
                                    foreach (var item in value.Split(new[] { ',', ';' }))
                                    {
                                        var methodItem = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                                        if (bodyExpression == null)
                                            bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                        else
                                            bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                    }
                                }
                                else
                                {
                                    bodyExpression = Expression.Equal(property, constantExpression);
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.F_NotEquals:
                        case CompareType.N_NotEquals:
                            {
                                var bodyExpression = Expression.NotEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_GreaterThan:
                            {
                                var bodyExpression = Expression.GreaterThan(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_GreaterThanOrEqual:
                            {
                                var bodyExpression = Expression.GreaterThanOrEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_LessThan:
                            {
                                var bodyExpression = Expression.LessThan(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_LessThanOrEqual:
                            {
                                var bodyExpression = Expression.LessThanOrEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_Between:
                            {
                                var greaterThan = !value.IsStringNullOrEmpty() ? constantExpression : null;
                                Expression expGreaterThan = greaterThan != null ? Expression.GreaterThanOrEqual(property, greaterThan) : null;

                                var lessThan = !value2.IsStringNullOrEmpty() ? constantExpression2 : null;
                                Expression expLessThan = lessThan != null ? Expression.LessThanOrEqual(property, lessThan) : null;

                                if (expGreaterThan == null && expLessThan == null) continue;
                                var bodyExpression = expGreaterThan != null
                                    ? (expLessThan == null ? expGreaterThan : Expression.And(expGreaterThan, expLessThan))
                                    : expLessThan;
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_NotBetween:
                            {
                                var greaterThan = !value.IsStringNullOrEmpty() ? constantExpression : null;
                                Expression expGreaterThan = greaterThan != null ? Expression.GreaterThanOrEqual(property, greaterThan) : null;

                                var lessThan = !value2.IsStringNullOrEmpty() ? constantExpression2 : null;
                                Expression expLessThan = lessThan != null ? Expression.LessThanOrEqual(property, lessThan) : null;

                                if (expGreaterThan == null && expLessThan == null) continue;
                                var bodyExpression = expGreaterThan != null
                                    ? (expLessThan == null ? expGreaterThan : Expression.And(expGreaterThan, expLessThan))
                                    : expLessThan;
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_Contains:
                        case CompareType.F_Contains:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(new[] { ',', ';' }))
                                {
                                    var methodItem = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.N_NotContains:
                        case CompareType.F_NotContains:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(new[] { ',', ';' }))
                                {
                                    var methodItem = typeof(int).GetMethod("Equals", new[] { typeof(int) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_Equals:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_NotEquals:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("Equals", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_Contains:
                        case CompareType.S_Contains:
                            {
                                Expression bodyExpression = null;
                                foreach (var itemProperty in properties)
                                {
                                    foreach (var itemValue in value.Split(new[] { ',', ';' }))
                                    {
                                        var methodItem = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                        if (bodyExpression == null)
                                            bodyExpression = Expression.Call(itemProperty, methodItem, InitConstantExpression(itemProperty, itemValue));
                                        else
                                            bodyExpression = Expression.Or(bodyExpression, Expression.Call(itemProperty, methodItem, InitConstantExpression(itemProperty, itemValue)));
                                    }
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_NotContains:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_StartWith:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_NotStartWith:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_EndWith:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.S_NotEndWith:
                            {
                                Expression bodyExpression = null;
                                foreach (var item in value.Split(';'))
                                {
                                    var methodItem = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
                                    if (bodyExpression == null)
                                        bodyExpression = Expression.Call(property, methodItem, InitConstantExpression(property, item));
                                    else
                                        bodyExpression = Expression.Or(bodyExpression, Expression.Call(property, methodItem, InitConstantExpression(property, item)));
                                }
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_Equals:
                            {
                                var bodyExpression = Expression.Equal(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_NotEquals:
                            {
                                var bodyExpression = Expression.NotEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_GreaterThan:
                            {
                                var bodyExpression = Expression.GreaterThan(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_GreaterThanOrEqual:
                            {
                                var bodyExpression = Expression.GreaterThanOrEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_LessThan:
                            {
                                var bodyExpression = Expression.LessThan(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_LessThanOrEqual:
                            {
                                var bodyExpression = Expression.LessThanOrEqual(property, constantExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_Between:
                            {
                                var greaterThan = !value.IsStringNullOrEmpty() ? constantExpression : null;
                                Expression expGreaterThan = greaterThan != null ? Expression.GreaterThanOrEqual(property, greaterThan) : null;

                                var lessThan = !value2.IsStringNullOrEmpty() ? constantExpression2 : null;
                                Expression expLessThan = lessThan != null ? Expression.LessThanOrEqual(property, lessThan) : null;

                                if (expGreaterThan == null && expLessThan == null) continue;
                                var bodyExpression = expGreaterThan != null
                                    ? (expLessThan == null ? expGreaterThan : Expression.And(expGreaterThan, expLessThan))
                                    : expLessThan;
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                        case CompareType.D_NotBetween:
                            {
                                var greaterThan = !value.IsStringNullOrEmpty() ? constantExpression : null;
                                Expression expGreaterThan = greaterThan != null ? Expression.GreaterThanOrEqual(property, greaterThan) : null;

                                var lessThan = !value2.IsStringNullOrEmpty() ? constantExpression2 : null;
                                Expression expLessThan = lessThan != null ? Expression.LessThanOrEqual(property, lessThan) : null;

                                if (expGreaterThan == null && expLessThan == null) continue;
                                var bodyExpression = expGreaterThan != null
                                    ? (expLessThan == null ? expGreaterThan : Expression.And(expGreaterThan, expLessThan))
                                    : expLessThan;
                                bodyExpression = Expression.Not(bodyExpression);
                                whereCallExpression = Expression.Call(typeof(Queryable), "Where", new[] { db.ElementType }, db.Expression, Expression.Lambda<Func<T, bool>>(bodyExpression, new[] { parameter }));
                            }
                            break;
                    }

                    if (whereCallExpression != null) db = db.Provider.CreateQuery<T>(whereCallExpression);
                }
                catch
                {
                    return db;
                }
            }
            return db;
        }

        public static IQueryable<T> ToPaging<T>(this IQueryable<T> db, TableData model = null)
        {
            if (model == null) model = new TableData();
            if (model.Paging == null) model.Paging = new PagingData();

            int size = model.Paging.Size;
            int index = model.Paging.Index;
            var itemstoSkip = size * (index - 1);
            return db.Skip(itemstoSkip).Take(size);
        }

        public static T Active<T>(this T model, DbContext db) where T : SqlExEntity
        {
            var entity = db.Set<T>().Find(model.Id);
            if (entity != null)
            {
                entity.IsActive = true;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return entity;
        }
        public static T Delete<T>(this T model, DbContext db) where T : SqlExEntity
        {
            var entity = db.Set<T>().Find(model.Id);
            if (entity != null)
            {
                entity.IsDelete = true;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return entity;
        }
        public static T DeActive<T>(this T model, DbContext db) where T : SqlExEntity
        {
            var entity = db.Set<T>().Find(model.Id);
            if (entity != null)
            {
                entity.IsActive = false;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return entity;
        }
        public static T UndoDelete<T>(this T model, DbContext db) where T : SqlExEntity
        {
            var entity = db.Set<T>().Find(model.Id);
            if (entity != null)
            {
                entity.IsDelete = false;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
            return entity;
        }

        private static Expression InitConstantExpression(Expression property, string value, bool value2 = false)
        {
            Expression constantExpression = null;
            if (property.Type.Name.ContainsEx("string"))
            {
                if (value.EqualsEx("null"))
                    constantExpression = Expression.Constant(null, typeof(string));
                else if (value.EqualsEx("empty"))
                    constantExpression = Expression.Constant(string.Empty, typeof(string));
                else
                    constantExpression = Expression.Constant(value, typeof(string));
            }
            else if (property.Type.FullName.ContainsEx("int"))
            {
                if (value.Contains(","))
                {
                    constantExpression = Expression.Constant(value, typeof(string));
                }
                else
                {
                    if (property.Type.Name.ContainsEx("nullable"))
                    {
                        constantExpression = value.EqualsEx("-1")
                            ? Expression.Constant(null, typeof(int?))
                            : Expression.Constant(value.ToInt32(), typeof(int?));
                    }
                    else constantExpression = Expression.Constant(value.ToInt32(), typeof(int));
                }
            }
            else if (property.Type.FullName.ContainsEx("bool"))
            {
                if (property.Type.Name.ContainsEx("nullable"))
                {
                    constantExpression = value == null || value.EqualsEx("-1")
                        ? Expression.Constant(null, typeof(bool?))
                        : Expression.Constant(value.ToBoolean(), typeof(bool?));
                }
                else constantExpression = Expression.Constant(value.ToBoolean(), typeof(bool));
            }
            else if (property.Type.FullName.ContainsEx("float"))
            {
                if (property.Type.Name.ContainsEx("nullable"))
                {
                    constantExpression = value.EqualsEx("-1")
                        ? Expression.Constant(null, typeof(float?))
                        : Expression.Constant(value.ToFloat(), typeof(float?));
                }
                else constantExpression = Expression.Constant(value.ToFloat(), typeof(float));
            }
            else if (property.Type.FullName.ContainsEx("double"))
            {
                if (property.Type.Name.ContainsEx("nullable"))
                {
                    constantExpression = value.EqualsEx("-1")
                        ? Expression.Constant(null, typeof(double?))
                        : Expression.Constant(value.ToDouble(), typeof(double?));
                }
                else constantExpression = Expression.Constant(value.ToDouble(), typeof(double));
            }
            else if (property.Type.FullName.ContainsEx("decimal"))
            {
                if (property.Type.Name.ContainsEx("nullable"))
                {
                    constantExpression = value.EqualsEx("-1")
                        ? Expression.Constant(null, typeof(decimal?))
                        : Expression.Constant(value.ToDecimal(), typeof(decimal?));
                }
                else constantExpression = Expression.Constant(value.ToDecimal(), typeof(decimal));
            }
            else if (property.Type.FullName.ContainsEx("datetime"))
            {
                if (property.Type.Name.ContainsEx("nullable"))
                {
                    constantExpression = value.IsStringNullOrEmpty()
                        ? Expression.Constant(null, typeof(DateTime?))
                        : value2
                            ? Expression.Constant(value.ToDateTime("dd/MM/yyyy").AddDays(1).AddSeconds(-1), typeof(DateTime?))
                            : Expression.Constant(value.ToDateTime("dd/MM/yyyy"), typeof(DateTime?));
                }
                else constantExpression = value2
                            ? Expression.Constant(value.ToDateTime("dd/MM/yyyy").AddDays(1).AddSeconds(-1), typeof(DateTime))
                            : Expression.Constant(value.ToDateTime("dd/MM/yyyy"), typeof(DateTime));
            }
            return constantExpression;
        }
    }
}
