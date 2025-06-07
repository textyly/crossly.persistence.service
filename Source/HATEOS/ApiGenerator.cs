namespace Persistence.HATEOS
{
    public class ApiGenerator(LinkGenerator linkGenerator) : IApiGenerator
    {
        public Link GenerateLink(string id)
        {
            string? getById = linkGenerator.GetPathByName("GetPatternById", new { id });
            string? replace = linkGenerator.GetPathByName("ReplacePattern", new { id });
            string? rename = linkGenerator.GetPathByName("RenamePattern", new { id });
            string? delete = linkGenerator.GetPathByName("DeletePattern", new { id });

            return getById is null || replace is null || rename is null || delete is null
                ? throw new Exception($"cannot be generated link for id: ${id}.")
                : new(getById, replace, rename, delete);
        }

        public Link[] GenerateLinks(string[] ids)
        {
            List<Link> links = [];

            foreach (string id in ids)
            {
                Link link = GenerateLink(id);
                links.Add(link);
            }

            return [.. links];
        }
    }

    public record Link(string GetById, string Replace, string Rename, string Delete);
}