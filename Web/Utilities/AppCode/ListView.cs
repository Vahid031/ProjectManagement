using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Web.Utilities.AppCode
{
    public static class ListView
    {
        public static IQueryable<TEntity> OrderByGrid<TEntity>(this IQueryable<TEntity> source, string orderByValues) where TEntity : class
        {
            IQueryable<TEntity> returnValue = null;

            string orderPair = orderByValues.Trim().Split(',')[0];
            string command = orderPair.ToUpper().Contains("DESC") ? "OrderByDescending" : "OrderBy";

            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            string propertyName = (orderPair.Split(' ')[0]).Trim();

            System.Reflection.PropertyInfo property;
            MemberExpression propertyAccess;

            if (propertyName.Contains('.'))
            {
                String[] childProperties = propertyName.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = property.PropertyType;
                    if (!t.IsGenericType)
                    {
                        property = t.GetProperty(childProperties[i]);
                    }
                    else
                    {
                        property = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                    }

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                }
            }
            else
            {
                property = type.GetProperty(propertyName);
                propertyAccess = Expression.MakeMemberAccess(parameter, property);
            }

            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var resultExpression = Expression.Call(typeof(Queryable), command, new Type[] { type, property.PropertyType },

            source.Expression, Expression.Quote(orderByExpression));

            returnValue = source.Provider.CreateQuery<TEntity>(resultExpression);

            if (orderByValues.Trim().Split(',').Count() > 1)
            {
                // remove first item
                string newSearchForWords = orderByValues.ToString().Remove(0, orderByValues.ToString().IndexOf(',') + 1);
                return null;// source.OrderBy(newSearchForWords);
            }

            return returnValue;
        }

        public static Expression<Func<TEntity, bool>> GetSearchExpressionn<TEntity>(TEntity entity)
        {
            Expression exp = GetExpression<TEntity>("Member.Id", "1");

            foreach (var item in entity.GetType().GetProperties())
            {
                item.GetValue(entity);
            }

            //foreach (var item in entity.GetType().GetProperties())
            //{
            //    if (item.GetValue(entity) != null && item.GetValue(entity).ToString().Trim() != "" && item.GetValue(entity).ToString() != "0")
            //    {
            //        if (exp == null)
            //        {
            //            exp = GetExpression(item.Name, item.GetValue(entity));
            //        }
            //        else
            //        {
            //            exp = Expression.And(exp, GetExpression(item.Name, item.GetValue(entity)));
            //        }
            //    }
            //}
            //
            //if (exp != null)
            //    return Expression.Lambda<Func<TEntity, bool>>(exp, param);
            //
            return null;
        }


        public static Expression GetExpression<TEntity>(string name, object value)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "t");
            var type = typeof(TEntity);
            System.Reflection.PropertyInfo property;
            MemberExpression propertyAccess;

            if (name.Contains('.'))
            {
                String[] childProperties = name.Split('.');
                property = typeof(TEntity).GetProperty(childProperties[0]);
                propertyAccess = Expression.MakeMemberAccess(param, property);

                for (int i = 1; i < childProperties.Length; i++)
                {
                    Type t = property.PropertyType;
                    if (!t.IsGenericType)
                    {
                        property = t.GetProperty(childProperties[i]);
                    }
                    else
                    {
                        property = t.GetGenericArguments().First().GetProperty(childProperties[i]);
                    }

                    propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
                    MemberExpression member2 = propertyAccess;
                }
            }
            else
            {
                property = type.GetProperty(name);
                propertyAccess = Expression.MakeMemberAccess(param, property);
            }

            

            //MemberExpression member = Expression.Property(param, name);
            ConstantExpression constantDefault = Expression.Constant(value);

            if (propertyAccess.Type == typeof(string))
            {
                return Expression.Call(propertyAccess, typeof(string).GetMethod("Contains"), constantDefault);
            }
            else
            {
                if (propertyAccess.Type.IsGenericType && propertyAccess.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Expression constant = Expression.Constant(Convert.ChangeType(value, propertyAccess.Type.GetGenericArguments()[0]));
                    Expression typeConstant = Expression.Convert(constant, propertyAccess.Type);
                    return Expression.Equal(propertyAccess, typeConstant);
                }
                else
                {
                    return Expression.Equal(propertyAccess, constantDefault);
                }
            }
        }
    }
}