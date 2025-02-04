public record ProjectRegistry(IEnumerable<ProjectItem> Projects);

public record ProjectItem(string ProjectId, int Version = 0);

public record Settings(string AccountName, string Container);
