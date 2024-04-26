using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[JsonConverter(typeof(StringEnumConverter))]
public enum NotificationStatus
{
    [EnumMember(Value = "Pending")]
    Pending,

    [EnumMember(Value = "Approved")]
    Approved,

    [EnumMember(Value = "Rejected")]
    Rejected
}