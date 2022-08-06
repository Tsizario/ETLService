using ETLService.Models.InputModels;

namespace HT_1.Services.Extentions;

public static class AddressExtentions
{
    public static Address ToAddress(this string value)
    {
        // [city, street, room]
        var valueArray = value.Split(",").Select(x => x.Trim()).ToArray();

        var city = valueArray[0];
        // no need for now
        // var street = valueArray[1];
        // var room = valueArray[2];

        return new()
        {
            City = city,
            // no need for now
            // Street = street,
            // Room = room
        };
    }
}