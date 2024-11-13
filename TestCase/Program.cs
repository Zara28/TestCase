using RestSharp;
using TestCase;
using TestCase.Models;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using PIHelperSh.Configuration.Attributes;
using PIHelperSh.Configuration;

[TrackedType]
public class Program
{
    [Constant(BlockName = "AppConfig", ConstantName = "HOST")]
    private static string _host { get; set; } = string.Empty;

    [Constant(BlockName = "AppConfig", ConstantName = "KEYID")]
    private static string _keyId { get; set; } = string.Empty;

    [Constant(BlockName = "AppConfig", ConstantName = "SHAREDKEY")]
    private static string _sharedKey { get; set; } = string.Empty;

    public static void Main(string[] args)
    {
        IConfiguration config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.json")
           .Build().AddConstants();

        string url = $"https://{_host}/api/testassignments/pan";

        var protect = new Protected
        {
            alg = "HS256",
            kid = _keyId,
            signdate = DateTime.Now
        };

        string number = "4000001234567899";

        var payload = new Payload
        {
            CardInfo = new ()
            {
                Pan = number
            }
        };

        var jws = MakeJws(protect, payload, _sharedKey);

        RestClient restClient = new(url);

        var restRequest = new RestRequest(url, Method.Post);

        restRequest.AddBody(jws);

        var responce = restClient.ExecuteAsync<Answer>(restRequest).Result;

        if (!responce.IsSuccessful || responce.Data?.Error != null)
        {
            Console.WriteLine("Unsuccessfully");
        }
        else
        {
            Console.WriteLine("Successfully");
        }
    }

    private static string MakeJws(object header, object payload, string secretKey)
    {
        UTF8Encoding encoding = new UTF8Encoding();

        var headJson = JsonConvert.SerializeObject(header);
        var payloadJson = JsonConvert.SerializeObject(payload);

        var encodingHeader = encoding.GetBytes(headJson).Base64UrlEncode();
        var encodingPayload = payloadJson.Base64UrlEncode();

        var sign = new Signature
        {
            Base64Header = encodingHeader,
            Base64Payload = encodingPayload,
            SecretKey = secretKey
        };

        var signature = sign.GenerateSignature();

        return $"{encodingHeader}.{encodingPayload}.{signature}";
    }

}


