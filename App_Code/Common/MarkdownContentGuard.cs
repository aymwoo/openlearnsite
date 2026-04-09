namespace LearnSite.Common
{
    public static class MarkdownContentGuard
    {
        public static string NormalizeCodeFences(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return string.Empty;
            }

            return content.Replace("\\`\\`\\`", "```");
        }
    }
}
