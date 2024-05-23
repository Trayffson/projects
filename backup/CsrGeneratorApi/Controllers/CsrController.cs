using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;

namespace CsrGeneratorApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CsrController : ControllerBase
    {
        private readonly ILogger<CsrController> _logger;

        public CsrController(ILogger<CsrController> logger)
        {
            _logger = logger;
        }

        [HttpPost("ssl")]
        public IActionResult CreateCsr([FromBody] CsrRequest request)
        {
            _logger.LogInformation("Received request to create CSR for CommonName: {CommonName}", request.CommonName);

            try
            {
                var csrFilePath = Path.Combine(request.Directory, $"{request.FileName}.csr");
                var pemFilePath = Path.Combine(request.Directory, $"{request.FileName}.pem");

                GenerateCsr(request, csrFilePath, pemFilePath);

                _logger.LogInformation("CSR generated successfully for CommonName: {CommonName}", request.CommonName);
                return Ok(new { Message = "CSR generated successfully", CsrFilePath = csrFilePath, PemFilePath = pemFilePath });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating CSR for CommonName: {CommonName}", request.CommonName);
                return StatusCode(500, "An error occurred while generating the CSR.");
            }
        }

        private void GenerateCsr(CsrRequest request, string csrFilePath, string pemFilePath)
        {
            var processInfo = new ProcessStartInfo("openssl", $"req -new -newkey rsa:2048 -nodes -keyout {pemFilePath} -out {csrFilePath} -subj \"/C={request.Country}/ST={request.State}/L={request.Locality}/O={request.Organization}/OU={request.OrganizationalUnit}/CN={request.CommonName}/emailAddress={request.EmailAddress}\"")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var process = Process.Start(processInfo);
            process.WaitForExit();
        }
    }
}
