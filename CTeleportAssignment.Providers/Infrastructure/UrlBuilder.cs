using System.Text;

namespace CTeleportAssignment.Providers.Infrastructure
{
    public class UrlBuilder
    {
        private readonly StringBuilder _urlBuilder;
        public UrlBuilder(string url)
        {
            _urlBuilder = new StringBuilder(url);
        }
        public UrlBuilder AddSegment(string segment)
        {
            _urlBuilder.Append("/");
            _urlBuilder.Append(segment);
            return this;
        }
        public string Build()
        {
            return _urlBuilder.ToString();
        }
    }
}
