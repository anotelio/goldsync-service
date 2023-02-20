using System.Text.Json;

namespace GoldSync.Service.Contracts.Dtos.Requests;

public class BackgroundSettingsRequest
{
    public int? ProcessEnabled { get; set; }

    public double? PeriodInSeconds { get; set; }

    public string Serialized()
    {
        return JsonSerializer.Serialize(this);
    }
}
