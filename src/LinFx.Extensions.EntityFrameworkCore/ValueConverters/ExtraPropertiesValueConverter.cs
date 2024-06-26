﻿using LinFx.Extensions.EntityFrameworkCore.ObjectExtending;
using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace LinFx.Extensions.EntityFrameworkCore.ValueConverters;

public class ExtraPropertiesValueConverter : ValueConverter<ExtraPropertyDictionary, string>
{
    public ExtraPropertiesValueConverter(Type entityType)
        : base(
            d => SerializeObject(d, entityType),
            s => DeserializeObject(s, entityType))
    { }

    private static string SerializeObject(ExtraPropertyDictionary extraProperties, Type entityType)
    {
        var copyDictionary = new Dictionary<string, object>(extraProperties);

        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    if (property.IsMappedToFieldForEf())
                    {
                        copyDictionary.Remove(property.Name);
                    }
                }
            }
        }

        return JsonSerializer.Serialize(copyDictionary);
    }

    private static ExtraPropertyDictionary DeserializeObject(string extraPropertiesAsJson, Type entityType)
    {
        if (string.IsNullOrEmpty(extraPropertiesAsJson) || extraPropertiesAsJson == "{}")
            return new ExtraPropertyDictionary();

        var deserializeOptions = new JsonSerializerOptions();
        //deserializeOptions.Converters.Add(new ObjectToInferredTypesConverter());
        var dictionary = JsonSerializer.Deserialize<ExtraPropertyDictionary>(extraPropertiesAsJson, deserializeOptions) ??
                         new ExtraPropertyDictionary();

        if (entityType != null)
        {
            var objectExtension = ObjectExtensionManager.Instance.GetOrNull(entityType);
            if (objectExtension != null)
            {
                foreach (var property in objectExtension.GetProperties())
                {
                    dictionary[property.Name] = GetNormalizedValue(dictionary, property);
                }
            }
        }

        return dictionary;
    }

    private static object GetNormalizedValue(
        Dictionary<string, object> dictionary,
        ObjectExtensionPropertyInfo property)
    {
        var value = dictionary.GetOrDefault(property.Name);
        if (value == null)
        {
            return property.GetDefaultValue();
        }

        try
        {
            if (property.Type.IsEnum)
                return Enum.Parse(property.Type, value.ToString(), true);

            //return Convert.ChangeType(value, property.Type);
            return value;
        }
        catch
        {
            return value;
        }
    }
}
