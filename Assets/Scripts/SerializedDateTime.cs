using System;
using System.Globalization;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class SerializedDateTime : ISerializationCallbackReceiver, ISerializable, IComparable, IEquatable<SerializedDateTime>, IComparable<SerializedDateTime>
{
    public static readonly string DateFormat = "yyyy-MM-dd HH:mm:ss.fffffff";

    [SerializeField, HideInInspector] string valueData = string.Empty;

    public DateTime value = DateTime.MinValue;

    public SerializedDateTime()
    {
    }

    public SerializedDateTime(DateTime dateTime)
    {
        value = dateTime;
    }

    protected SerializedDateTime(SerializationInfo info, StreamingContext context)
    {
        if (info == null)
            return;

        try
        {
            value = info.GetDateTime(nameof(value));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    public static implicit operator DateTime(SerializedDateTime value)
    {
        return value.value;
    }

    public static implicit operator SerializedDateTime(DateTime value)
    {
        return new SerializedDateTime(value);
    }

    public int CompareTo(object obj)
    {
        return value.CompareTo(obj);
    }

    public int CompareTo(SerializedDateTime other)
    {
        return value.CompareTo(other.value);
    }

    #region Equals

    public static bool operator ==([CanBeNull] SerializedDateTime obj1, [CanBeNull] SerializedDateTime obj2)
    {
        if (ReferenceEquals(obj1, obj2))
            return true;
        if (ReferenceEquals(obj1, null) || ReferenceEquals(obj2, null))
            return false;
        return obj1.Equals(obj2);
    }

    public static bool operator !=([CanBeNull] SerializedDateTime obj1, [CanBeNull] SerializedDateTime obj2)
    {
        return !(obj1 == obj2);
    }

    public override bool Equals(object obj)
    {
        return obj != null && GetType() == obj.GetType() &&
               (ReferenceEquals(this, obj) || Equals(obj as SerializedDateTime));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
    public override int GetHashCode()
    {
        return value.GetHashCode();
    }

    public bool Equals(SerializedDateTime other)
    {
        if (ReferenceEquals(null, other))
            return false;
        if (ReferenceEquals(this, other))
            return true;
        return value.Equals(other.value);
    }

    #endregion

    #region ToString

    public override string ToString()
    {
        return value.ToString(DateFormat);
    }

    #endregion

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        try
        {
            info.AddValue(nameof(value), value);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        try
        {
            valueData = value.ToString(DateFormat, DateTimeFormatInfo.InvariantInfo);
        }
        catch (Exception ex)
        {
            Debug.LogError(ex);
        }
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        value = DateTime.TryParseExact(valueData, DateFormat, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None,
            out var dateTime)
            ? dateTime
            : DateTime.MinValue;
    }
}
