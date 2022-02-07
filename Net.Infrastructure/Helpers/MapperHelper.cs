using System;
using System.Linq;
using System.Reflection;
using Net.Infrastructure.BaseTypes.Entities;
using Net.Infrastructure.BaseTypes.Models;

namespace Net.Infrastructure.Helpers
{
    public static class MapperHelper
    {
        public static bool IsNullable(this Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool IsNullOrEmpty(object value)
        {
            if (value is null)
                return true;

            var type = value.GetType();

            return type.Name switch
            {
                "Guid" => (Guid) value == Guid.Empty,
                "String" => string.IsNullOrEmpty(value.ToString()),
                "Decimal" => (Decimal) value == Decimal.Zero,
                "Double" => Math.Abs((Double) value) < Double.Epsilon,
                "Char" => (Char) value == (Char)0,
                
                _ => type.IsValueType && Equals(value, Activator.CreateInstance(type))
            };
        }

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Instance |
                                                  System.Reflection.BindingFlags.NonPublic |
                                                  System.Reflection.BindingFlags.Public;

        /// <summary>
        /// Итерируется по всем полям модели: 
        /// 1) Проверка не null ли значение поля 
        /// 2) Проверка не empty/zero ли значение у поля 
        /// 3) Проверка не равно ли значение поля модели текущему значению поля сущности 
        /// 4) Если проверки пройдены - меняем значение поля сущности 
        /// <b> !!!  Если ни одно поле не поменялось - возвращаем null !!! </b>
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="entity"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static TEntity UpdateFromModel<TEntity, TModel>(this TEntity entity, TModel model) 
            where TEntity : BaseEntity<Guid>
            where TModel : IViewModel
        {
            var changesCount = 0;

            var entityFields = entity.GetType().GetFields(BindingFlags);
            var modelFields = model.GetType().GetFields(BindingFlags);

            foreach (var modelField in modelFields)
            {
                var modelFieldValue = modelField.GetValue(model);
                // Если у модели поле равно null или дефолтное значение игнорируем его
                if(IsNullOrEmpty(modelFieldValue))
                    continue;

                // Находим entity с тем же именем и типом данных что у модели
                var entityField = entityFields.FirstOrDefault(x => 
                    x.Name == modelField.Name);

                // Если у сущности не найдено поле с именем модели игнорируем его
                if (entityField == null)
                    continue;

                // Если по ошибке типы дынных у entity и model отличаются - бомбим
                if (entityField.FieldType != modelField.FieldType)
                {
                    var underlyingTypeEntity = Nullable.GetUnderlyingType(entityField.FieldType);
                    var underlyingTypeModel = Nullable.GetUnderlyingType(modelField.FieldType);

                    // Если причина неравенства в том, что тип modelField является Nullable
                    if (underlyingTypeModel != null)
                    {
                        // Если типы дынных у entity и Nullabble<model> отличаются - бомбим
                        if (entityField.FieldType != underlyingTypeModel)
                        {
                            throw new InvalidCastException($"Cast error while mapping! Entity type is {entityField.FieldType}, but model type is {modelField.FieldType}!");
                        }
                    }
                    // Если причина неравенства в том, что тип entityField является Nullable
                    else if (underlyingTypeEntity != null)
                    {
                        // Если типы дынных у Nullable<entity> и model отличаются - бомбим
                        if (underlyingTypeEntity != modelField.FieldType)
                        {
                            throw new InvalidCastException($"Cast error while mapping! Entity type is {entityField.FieldType}, but model type is {modelField.FieldType}!");
                        }
                    }
                    // Если причина неравенства в том, что тип modelField является String, а entityType Enum
                    else if (entityField.FieldType.BaseType != null && 
                             entityField.FieldType.BaseType.Name == "Enum" && 
                             modelField.FieldType.Name == "String" &&
                             modelFieldValue != null)
                    {
                        modelFieldValue = Enum.Parse(entityField.FieldType, modelFieldValue.ToString()!);
                    }
                    else
                    {
                        throw new InvalidCastException($"Cast error while mapping! Entity type is {entityField.FieldType}, but model type is {modelField.FieldType}!");
                    }
                }

                var entityFieldValue = entityField.GetValue(entity);
                
                // Если поле пустое или значение entity и model отличаются то меняем значение
                if (IsNullOrEmpty(entityField) || entityFieldValue != modelFieldValue)
                {
                    entityField.SetValue(entity, modelFieldValue);
                    changesCount++;
                }
            }

            return changesCount > 0 ? entity : null;
        }
    }
}
