using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Net.Infrastructure.Exceptions;

namespace Net.Infrastructure.Extensions
{
    public static class EntityFrameworkExtensions
    {

        public static dynamic GetEntityById(this DbContext context, string tableName, Guid id)
        {
            var entityType = context.Model.GetEntityTypes()
                .FirstOrDefault(x => x.Name.Right(tableName.Length) == tableName);

            if (entityType == null)
                throw new NotFoundException(
                    nameof(EntityType), 
                    new KeyValuePair<string, string>("TableName", tableName), 
                    new KeyValuePair<string, string>("Id", id.ToString()));

            var type = entityType.ClrType;

            return context.Find(type, id);
        }

        public static bool IsDisposed(this DbContext context)
        {
            var fieldInfo = typeof(DbContext).GetField("_disposed", BindingFlags.Instance | BindingFlags.NonPublic);
            var result = fieldInfo != null && (bool)fieldInfo.GetValue(context);

            return result;
        }

    }
}
