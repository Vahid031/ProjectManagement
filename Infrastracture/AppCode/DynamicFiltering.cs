using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Infrastracture.AppCode
{
    public static class DynamicFiltering
    {

        static List<Type> predefinedTypes = new List<Type>() {
                typeof(Object),
                typeof(Boolean),
                typeof(Char),
                typeof(String),
                typeof(SByte),
                typeof(Byte),
                typeof(Int16),
                typeof(UInt16),
                typeof(Int32),
                typeof(UInt32),
                typeof(Int64),
                typeof(UInt64),
                typeof(Single),
                typeof(Double),
                typeof(Decimal),
                typeof(DateTime),
                typeof(TimeSpan),
                typeof(Guid),
                typeof(Math),
                typeof(Convert)
            };

        
        public static string GetFilter<TEntity>(TEntity entity, ref Paging pg)
        {
            string Expression = "{0}.Contains(@{1})";

            foreach (var item in entity.GetType().GetProperties())
            {
                object itemValue = item.GetValue(entity, null);

                if (itemValue != null && itemValue.ToString() != "" && !(item.Name == "Id" && Convert.ToInt64(itemValue) == 0))
                {
                    if (predefinedTypes.Any(i => i.Name == item.PropertyType.Name))
                    {
                        if (item.PropertyType.Name.ToUpper() != "STRING")
                            Expression = "{0} = @{1}";
                        else
                            Expression = "{0}.Contains(@{1})";

                        if (pg._filter == "")
                        {
                            pg._filter = string.Format(Expression, item.Name, pg._values.Count());
                        }
                        else
                        {
                            pg._filter += string.Format(" and " + Expression, item.Name, pg._values.Count());
                        }
                        pg._values.Add(itemValue);
                    }
                    else
                    {
                        foreach (PropertyInfo childItem in item.PropertyType.GetProperties())
                        {
                            object childItemValue = childItem.GetValue(item.GetValue(entity, null), null);

                            if (childItemValue != null && childItemValue.ToString() != "" && !(childItem.Name == "Id" && Convert.ToInt64(childItemValue) == 0))
                            {
                                if (childItem.PropertyType.Name.ToUpper() != "STRING")
                                    Expression = "{0} = @{1}";
                                else
                                    Expression = "{0}.Contains(@{1})";

                                if (pg._filter == "")
                                {
                                    pg._filter = string.Format(Expression, item.Name + "." + childItem.Name, pg._values.Count());
                                }
                                else
                                {
                                    pg._filter += string.Format(" and " + Expression, item.Name + "." + childItem.Name, pg._values.Count());
                                }

                                pg._values.Add(childItemValue);
                            }
                        }
                    }
                }
            }

            return pg._filter;
        }

        public static string GetSqlFilterDynamic<TEntity>(TEntity entity, string className)
        {
            string filter = " ";
            string and = "";

            foreach (var item in entity.GetType().GetProperties())
            {
                object itemValue = item.GetValue(entity, null);

                if (itemValue == null || item.Name == "Id")
                    continue;

                if (item.PropertyType == typeof(Nullable<DateTime>))
                    continue;

                if (itemValue != null && item.Name == "Id" && Convert.ToInt64(itemValue) == 0)
                    continue;

                if (item.PropertyType != typeof(string))
                {
                    filter += and;
                    filter += className + "." + item.Name + " = '" + itemValue + "'";
                    and = " AND ";
                    continue;
                }

                if (item.PropertyType == typeof(string))
                {
                    filter += and;
                    filter += className + "." + item.Name + " LIKE N'%" + itemValue + "%'";
                    and = " AND ";
                }
            }

            return filter;
        }
    }
}
