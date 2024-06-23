// Copyright (c) InfinityFlow. All Rights Reserved.
// Licensed under the Apache 2.0. See LICENSE file in the solution root for full license information.

#pragma warning disable CA1812

namespace InfinityFlow.Temporal.Migrator;

using System.Text.Json;
using System.Text.Json.Serialization;

/// <summary>
/// Json Converter for Type.
/// </summary>
internal class TypeJsonConverter : JsonConverter<Type>
{
    private readonly IEnumerable<Type> _loadedTypes;

    /// <inheritdoc />
    public TypeJsonConverter()
    {
        _loadedTypes = GlobalReflector
            .GetRuntimeAssemblies()
            .SelectMany(assembly => assembly
                .GetTypes()
                .Where(t => GlobalReflector.Migration.IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false }));
    }

    /// <inheritdoc />
    public override Type? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.GetString() is { } value)
        {
            return LookupType(value);
        }

        return null;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, Type? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(writer);
        if (value is not null)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

    private Type? LookupType(string typeName) => _loadedTypes.FirstOrDefault(w => w.ToString() == typeName);
}
