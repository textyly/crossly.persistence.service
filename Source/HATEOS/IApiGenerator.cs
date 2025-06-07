namespace Persistence.HATEOS
{
    public interface IApiGenerator
    {
        Link GenerateLink(string id);
        Link[] GenerateLinks(string[] ids);
    }
}