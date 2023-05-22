namespace PipelinePoc.Api.DataAccess;

public interface IItemStore
{
    public Task<string> AddAsync(string item);
    public Task<IEnumerable<string>> GetAllAsync();
}
