using shortid;
using shortid.Configuration;
namespace Robobobot.Server.Services;

public class IdGenerator : IIdGenerator
{
    public string Generate()
    {
        var options = new GenerationOptions
        {
            UseNumbers = true,
            UseSpecialCharacters = false
        };
        
        return ShortId.Generate(options);
    }
}
public interface IIdGenerator
{
    string Generate();
}