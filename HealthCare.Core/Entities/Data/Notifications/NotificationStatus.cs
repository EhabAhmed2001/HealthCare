using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

public enum NotificationStatus
{
    [EnumMember(Value = "Pending")] Pending,

    [EnumMember(Value = "Approved")] Approved,

    [EnumMember(Value = "Rejected")] Rejected
}