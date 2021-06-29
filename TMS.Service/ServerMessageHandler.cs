
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;


using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TMS.Common.Security;

namespace TMS.Service
{
    public class ServerMessageHandler
    {
        private readonly RequestDelegate _next;
        
        public ServerMessageHandler(RequestDelegate next)
        {
            _next = next;
          
        }


        public async Task Invoke(HttpContext httpContext)
        {

            if (httpContext.Request.Path.ToString().Contains("login")|| httpContext.Request.Path.ToString().Contains("weatherforecast"))
            {
                await _next.Invoke(httpContext);
            }
            else
            {
                if (httpContext.Request.Headers.ContainsKey(TMS.Common.Constants.Authorization))
                {
                    if (!string.IsNullOrEmpty(httpContext.Request.Headers[TMS.Common.Constants.Authorization].ToString()))
                    {
                        string token = httpContext.Request.Headers[TMS.Common.Constants.Authorization].ToString().Replace(Common.Constants.Authentication_Schema, "");

                        try
                        {
                           var tokenObj = new JwtSecurityTokenHandler().ReadToken(token);
                        }
                        catch (Exception ex)
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            httpContext.Response.ContentType = "application/json";
                            await httpContext.Response.WriteAsync(ex.Message);
                            return;
                        }
                        if (!string.IsNullOrEmpty(token))
                        {
                            await _next.Invoke(httpContext);
                        }
                        else
                        {
                            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            httpContext.Response.ContentType = "application/json";
                            await httpContext.Response.WriteAsync(Common.Constants.AuthenticationProblem);
                            return;
                        }
                    }
                    else
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        httpContext.Response.ContentType = "application/json";
                        await httpContext.Response.WriteAsync(Common.Constants.AuthenticationProblem);
                        return;
                    }
                }
                else
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(Common.Constants.AuthenticationProblem);
                    return;
                }

                return;
            }
        }



        private void DecryptRequest(HttpRequestMessage request)
        {
            byte[] requestByte = request.Content.ReadAsByteArrayAsync().Result;
            //byte[] decryptedRequestByte = EncryptionHelper.RijndaelEncryption().Decrypt(requestByte, decryptionKey);

            HttpContent decryptedContent = new StreamContent(new System.IO.MemoryStream(requestByte));
            decryptedContent.Headers.ContentType = request.Content.Headers.ContentType;

            request.Content = decryptedContent;

        }
        private void EncryptResponse(HttpResponseMessage response)
        {
            if (response.Content != null)
            {
                byte[] responseByte = response.Content.ReadAsByteArrayAsync().Result;
                HttpContent encryptedContent = new StreamContent(new System.IO.MemoryStream(responseByte));
                encryptedContent.Headers.ContentType = response.Content.Headers.ContentType;
                response.Content = encryptedContent;
            }
        }



    }
}