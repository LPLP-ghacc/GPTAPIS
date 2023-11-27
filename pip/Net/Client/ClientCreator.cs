﻿using System;
using System.Net;
using System.Net.Http;

namespace GPTAPIS
{
    public static class ClientCreator
    {
        /// <summary>
        /// address format: 0.0.0.0
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        /// <param name="credential"></param>
        /// <returns></returns>
        public static HttpClient GetCredentialClient(string address, string port, NetworkCredential credential)
        {
            var proxy = new WebProxy
            {
                Address = new Uri($"http://{address}:{port}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                Credentials = credential
            };

            var httpClientHandler = new HttpClientHandler()
            {
                Proxy = proxy,
            };

            httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var httpClient = new HttpClient(httpClientHandler, true);

            return httpClient;
        }
    }
}
