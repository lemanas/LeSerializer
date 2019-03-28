using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace JsonSerializer
{
    class LeSerializer
    {
        private object _object;
        private readonly Type _type;
        private StringBuilder _stringBuilder;

        public LeSerializer(object obj)
        {
            _object = obj;
            _type = obj.GetType();
            _stringBuilder = new StringBuilder();
        }

        public string Serialize()
        {
            _object = CheckAndReplaceDatetime(_object);

            bool isSimple = IsSimple(_object.GetType());
            bool isCollection = IsCollection(_type);

            if (isSimple)
                SerializeSimple(ref _stringBuilder, _object);
            else if (isCollection)
                SerializeCollection(ref _stringBuilder, _object);
            else
                SerializeObject(ref _stringBuilder, _object);

            var result = CleanString(_stringBuilder.ToString());
            _stringBuilder.Clear();
            return result;
        }


        private void SerializeObject(ref StringBuilder existingBuilder, object obj)
        {
            existingBuilder.Append('{');

            var objectProperties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in objectProperties)
            {
                Type type = property.PropertyType;
                bool isSimple = IsSimple(type);
                if (isSimple)
                {
                    existingBuilder.Append(type == typeof(string)
                        ? $"\"{property.Name}\": \"{property.GetValue(obj)}\","
                        : $"\"{property.Name}\": {property.GetValue(obj)},");
                }
                else if (IsCollection(type))
                {
                    existingBuilder.Append($"\"{property.Name}\": ");
                    var value = property.GetValue(obj);
                    SerializeCollection(ref existingBuilder, value);
                    existingBuilder.Append(',');
                }
                else
                {
                    var value = property.GetValue(obj);
                    SerializeObject(ref existingBuilder, value);
                }
            }

            existingBuilder.Append('}');
        }

        private void SerializeCollection(ref StringBuilder existingBuilder, object obj)
        {
            existingBuilder.Append('[');

            var collection = (IList)obj;
            for (int i = 0; i < collection.Count; i++)
            {
                var itemType = collection[i].GetType();
                if (IsSimple(itemType))
                    existingBuilder.Append($"{collection[i]},");
                else
                {
                    SerializeObject(ref existingBuilder, collection[i]);
                    existingBuilder.Append(',');
                }
            }

            existingBuilder.Append(']');
        }

        private void SerializeSimple(ref StringBuilder existingBuilder, object obj)
        {
            existingBuilder.Append('{');

            if (obj is string)
                existingBuilder.Append($"\"{obj}\"");
            else
                existingBuilder.Append(obj);

            existingBuilder.Append('}');
        }

        private bool IsSimple(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(typeInfo.GetGenericArguments()[0]);
            }
            return typeInfo.IsPrimitive
                   || typeInfo.IsEnum
                   || type == typeof(string)
                   || type == typeof(decimal);
        }

        private bool IsCollection(Type type) => type.GetInterface(nameof(ICollection)) != null;

        private string CleanString(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == ',' && (input[i + 1] == '}' || input[i + 1] == ']'))
                    input = input.Remove(i, 1);
            }

            return input;
        }

        private object CheckAndReplaceDatetime(object obj)
        {
            if (obj is DateTime date)
                return $"{date.ToShortDateString()} {date.ToLongTimeString()}";

            return obj;
        }
    }
}
