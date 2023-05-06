using System.Text.Json.Serialization;
namespace dotnet_rpg.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum RpgClass
    {
        Knight = 1,
        mage = 2,
        Healer = 3,
        Cleric = 4

    }
}