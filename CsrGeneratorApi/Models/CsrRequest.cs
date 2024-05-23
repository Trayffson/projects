namespace CsrGeneratorApi
{
    public class CsrRequest
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string Locality { get; set; }
        public string Organization { get; set; }
        public string OrganizationalUnit { get; set; }
        public string CommonName { get; set; }
        public string EmailAddress { get; set; }
        public string FileName { get; set; }
        public string Directory { get; set; }
    }
}
