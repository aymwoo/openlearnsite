using System;

namespace LearnSite.Common
{
    public sealed class IframeUrlHelper
    {
        public static bool IsAllowed(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            string trimmed = url.Trim();
            string lowered = trimmed.ToLowerInvariant();
            if (lowered.StartsWith("javascript:") || lowered.StartsWith("data:") || lowered.StartsWith("vbscript:"))
            {
                return false;
            }

            Uri absoluteUri;
            if (Uri.TryCreate(trimmed, UriKind.Absolute, out absoluteUri))
            {
                return absoluteUri.Scheme == Uri.UriSchemeHttp || absoluteUri.Scheme == Uri.UriSchemeHttps;
            }

            if (trimmed.Contains(" "))
            {
                return false;
            }

            return trimmed.StartsWith("~/")
                || trimmed.StartsWith("/")
                || trimmed.StartsWith("./")
                || trimmed.StartsWith("../")
                || trimmed.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)
                || trimmed.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
                || trimmed.EndsWith(".htm", StringComparison.OrdinalIgnoreCase);
        }
    }
}
