using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLineParser;
using CommandLineParser.Arguments;
using System.Net;
using System.Net.Http;

namespace prtgHttpCustomResponseCode
{
    class Program
    {
        //static async Task<HttpResponseMessage> RunAsync(string url, int responseCode)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        // New code:
        //        HttpResponseMessage response = await client.GetAsync(url);
        //        return response;
        //    }

        //}

        private class Result
        {
            public int ReturnCode { get; set; } = 1;
            public string Message { get; set; } = "Unknown";
            public int ReturnValue { get; set; } = -1;

        }

        private static async Task<Result> RunAsync(string[] args)
        {
            var result = new Result();

            var parser = new CommandLineParser.CommandLineParser();
            var url = new ValueArgument<string>('u', "url", "Url");
            var responseCode = new ValueArgument<int>('r', "responseCode", "Expected Response Code");
            url.Optional = false;
            responseCode.Optional = false;
            parser.Arguments.Add(url);
            parser.Arguments.Add(responseCode);

            try
            {
                parser.ParseCommandLine(args);

                using (var client = new HttpClient())
                {
                    // New code:
                    HttpResponseMessage response = await client.GetAsync(url.Value);
                    
                    result.ReturnValue = (int)response.StatusCode;
                    if ((int)response.StatusCode == responseCode.Value)
                    {
                        result.ReturnValue = (int)response.StatusCode;
                        result.Message = "Success";
                        result.ReturnCode = 0;
                    }
                    else
                    {
                        result.ReturnValue = (int)response.StatusCode;
                        result.Message = response.StatusCode.ToString();
                        result.ReturnCode = 1;
                    }
                }

            }

            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.ReturnValue = 0;
                result.ReturnCode = 1;
            }
            return result;
        }


        static int Main(string[] args)
        {
            Result result = RunAsync(args).Result;
            Console.WriteLine(result.ReturnValue + ":" + result.Message);
            
            #if DEBUG
            {
                Console.WriteLine("Return:" + result.ReturnCode);
                Console.ReadLine();
            }
            #endif

            return result.ReturnCode;
        }
    }
}
