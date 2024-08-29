using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization;

[Serializable]
public class SerializedDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField, HideInInspector] List<TKey> keyData = new List<TKey>();

    [SerializeField, HideInInspector] List<TValue> valueData = new List<TValue>();

    public SerializedDictionary()
    {
    }

    public SerializedDictionary(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Clear();

        for (var i = 0; i < keyData.Count && i < valueData.Count; i++)
        {
            this[keyData[i]] = valueData[i];
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        keyData.Clear();
        valueData.Clear();

        foreach (var item in this)
        {
            keyData.Add(item.Key);
            valueData.Add(item.Value);
        }
    }
}

