using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NodeEditor.Model;

namespace NodeEditorDemo.Services;

internal class NodeSerializer : INodeSerializer
{
    private readonly JsonSerializerSettings _serializeSettings;
    private readonly JsonSerializerSettings _deserializeSettings;

    private class ListContractResolver : DefaultContractResolver
    {
        private readonly Type _listType;

        public ListContractResolver(Type listType)
        {
            _listType = listType;
        }

        public override JsonContract ResolveContract(Type type)
        {
            if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return base.ResolveContract(_listType.MakeGenericType(type.GenericTypeArguments[0]));
            }

            return base.ResolveContract(type);
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).Where(p => p.Writable).ToList();
        }
    }

    public NodeSerializer(Type listType)
    {
        _serializeSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            ContractResolver = new ListContractResolver(listType),
            NullValueHandling = NullValueHandling.Ignore,
        };

        _deserializeSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            ContractResolver = new ListContractResolver(listType),
            NullValueHandling = NullValueHandling.Ignore
        };
    }

    public string Serialize<T>(T value)
    {
        return JsonConvert.SerializeObject(value, _serializeSettings);
    }

    public T? Deserialize<T>(string text)
    {
        return JsonConvert.DeserializeObject<T>(text, _deserializeSettings);
    }

    public T? Load<T>(string path)
    {
        using var stream = File.OpenRead(path);
        using var streamReader = new StreamReader(stream, Encoding.UTF8);
        var text = streamReader.ReadToEnd();
        return Deserialize<T>(text);
    }

    public void Save<T>(string path, T value)
    {
        var text = Serialize(value);
        if (string.IsNullOrWhiteSpace(text)) return;
        using var stream = File.Create(path);
        using var streamWriter = new StreamWriter(stream, Encoding.UTF8);
        streamWriter.Write(text);
    }
}