using System.Text.Json;

namespace PipelinePoc.Api.DataAccess;

public class ItemStore : IItemStore
{
    private readonly string _filePath;
    private readonly JsonSerializerOptions _serializerOptions;

    public ItemStore(string filePath)
    {
        _filePath = filePath;
        _serializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
    }

    public async Task<string> AddAsync(string item)
    {
        var data = await GetDataAsync();
        if(data.Contains(item))
        {
            throw new InvalidOperationException($"Item {item} exists");
        }

        data.Add(item);
        await SaveDataAsync(data);

        return item;
    }

    public async Task<IEnumerable<string>> GetAllAsync()
    {
        return await GetDataAsync();
    }

    private async Task<List<string>> GetDataAsync()
    {
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<string>>(json)!; 
    }

    private async Task SaveDataAsync(List<string> data)
    {
        var json = JsonSerializer.Serialize(data, _serializerOptions);
        await File.WriteAllTextAsync(_filePath, json);
    }
}
