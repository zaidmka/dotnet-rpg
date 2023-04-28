using System.Text.Json.Serialization;
namespace dotnet_rpg.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum RpgClass
    {
        Knight = 10,
        mage = 20,
        Healer=30,
        Cleric=40

    }
}