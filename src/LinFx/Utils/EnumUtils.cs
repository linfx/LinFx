﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace System;

public static class EnumUtils
{
    public static IDictionary<Enum, string> ToDictionary(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        var dics = new Dictionary<Enum, string>();
        var enumValues = Enum.GetValues(type);

        foreach (Enum value in enumValues)
        {
            dics.Add(value, value.GetDisplayName());
        }

        return dics;
    }

    public static string GetDisplayName(this Enum value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value));

        var displayName = value.ToString();
        var fieldInfo = value.GetType().GetField(displayName);
        var attributes = (DisplayAttribute[])fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false);

        if (attributes?.Length > 0)
            displayName = attributes[0].Description;
        else
        {
            var desAttributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (desAttributes?.Length > 0)
                displayName = desAttributes[0].Description;
        }
        return displayName;
    }
}
