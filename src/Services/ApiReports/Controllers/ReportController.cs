using ApiReports.Helpers;
using ApiReports.Models;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiReports.Controllers
{
    public class ReportController : ApiController
    {
        [Route("api/Report/Download")]
        [HttpGet]
        public HttpResponseMessage Download(ParamsRequest parameters)
        {
            GenericResponse _response = new GenericResponse();
            try
            {
                byte[] report = null;
                using (LocalReport lr = new LocalReport())
                {
                    string format = string.Empty;
                    string deviceInfo = string.Empty;
                    string fileName = string.Empty;
                    foreach (var item in parameters.rdlcProps)
                    {
                        if (item.Key == "ReportName")
                        {
                            lr.ReportPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Reports"), item.Value);
                        }
                        if (item.Key == "format")
                        {
                            format = item.Value;
                        }
                        if (item.Key == "fileName")
                        {
                            fileName = item.Value;
                        }
                    }
                    foreach (var item in parameters.dataSources)
                    {
                        lr.DataSources.Add(new ReportDataSource(item.Key, DataExtensions.JsonToDataTable(JsonConvert.SerializeObject(item.Value))));
                    }
                    if (parameters.rdlcParams != null)
                    {
                        int index = 0;
                        ReportParameter[] rp = new ReportParameter[parameters.rdlcParams.Keys.Count];
                        foreach (var item in parameters.rdlcParams)
                        {
                            rp[index++] = new ReportParameter(item.Key, item.Value);
                        }
                        lr.SetParameters(rp);
                    }
                    report = lr.Render(format,
                                       deviceInfo,
                                       PageCountMode.Actual,
                                       out string mimeType,
                                       out string encoding,
                                       out string fileNameExtension,
                                       out string[] streams,
                                       out Warning[] warnings);
                    _response.message = "Report Created";
                    _response.mimeType = mimeType;
                    _response.fileName = fileName + "." + fileNameExtension;
                    _response.payload = report;

                    //Uncomment to preview report in postman

                    //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);

                    ////Set the Response Content.
                    //response.Content = new ByteArrayContent(_response.payload);

                    ////Set the Response Content Length.
                    //response.Content.Headers.ContentLength = _response.payload.LongLength;

                    ////Set the Content Disposition Header Value and FileName. 
                    //response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    //response.Content.Headers.ContentDisposition.FileName = _response.fileName;

                    ////Set the File Content Type. 
                    //response.Content.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
                    //return response;

                    //End Preview

                    return this.Request.CreateResponse(HttpStatusCode.OK, _response);
                }
            }
            catch (Exception ex)
            {
                _response.message = ex.Message;
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException);
            }
        }
    }
}
