using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GroupDocs.Conversion.Cloud.Sdk.Api;
using Aspose.BarCode.Cloud.Sdk.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GroupDocs.Conversion.Cloud.Sdk.Model.Requests;
using Microsoft.Extensions.Configuration;

namespace sample_barcode.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Sample : ControllerBase
    {
        internal const string AsposeClientHeaderName = "x-aspose-client";
        internal const string AsposeClientVersionHeaderName = "x-aspose-client-version";


        private readonly ILogger<Sample> _logger;
        private readonly IConfiguration _configuration;
        public Sample(ILogger<Sample> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var filename = "testbarcode.png";
            var version = GetType().Assembly.GetName().Version;
            
            var barcodeConfig = new Aspose.BarCode.Cloud.Sdk.Api.Configuration()
            {
                AppKey = _configuration.GetValue<string>("Settings:AsposeCloudAppKey"),
                AppSid = _configuration.GetValue<string>("Settings:AsposeCloudAppSid")
            };
            barcodeConfig.DefaultHeaders.Add(AsposeClientHeaderName, "test proxy app");
            barcodeConfig.DefaultHeaders.Add(AsposeClientVersionHeaderName, $"{version.Major}.{version.Minor}");

            IBarcodeApi barcodeApi = new Aspose.BarCode.Cloud.Sdk.Api.BarcodeApi(barcodeConfig);
            var groupdocsConfig = new GroupDocs.Conversion.Cloud.Sdk.Client.Configuration(
                _configuration.GetValue<string>("Settings:GroupdocsCloudAppSid"), _configuration.GetValue<string>("Settings:GroupdocsCloudAppKey"));
            var infoApi = new InfoApi(groupdocsConfig);

            var barcodeResponse = barcodeApi.PutBarcodeGenerateFile(new Aspose.BarCode.Cloud.Sdk.Model.Requests.PutBarcodeGenerateFileRequest(
                        text: "TEST", type: "qr", format: "PNG", textLocation: "None", name: filename, storage: null, folder: null));

            var supportedFileFormatsResponse = infoApi.GetSupportedConversionTypes(new GetSupportedConversionTypesRequest(format: System.IO.Path.GetExtension(filename)?.Trim('.')));
            return new JsonResult(new
            {
                filename = filename,
                width = barcodeResponse.ImageWidth,
                height = barcodeResponse.ImageHeight,
                canBeConvertedTo = supportedFileFormatsResponse != null && supportedFileFormatsResponse.Any() ? supportedFileFormatsResponse[0].TargetFormats : null
            });
        }
    }
}
