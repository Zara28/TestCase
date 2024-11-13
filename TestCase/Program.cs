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

        Chilkat.JsonObject jwsProtHdr = new Chilkat.JsonObject();
        jwsProtHdr.AppendString("alg", "HS256");
        jwsProtHdr.AppendString("kid", _keyId);
        jwsProtHdr.AppendString("signdate", DateTime.Now.ToString());
        jwsProtHdr.AppendString("cty", "application/json");

        Chilkat.Jws jws = new Chilkat.Jws();

        int signatureIndex = 0;
        jws.SetMacKey(signatureIndex, _sharedKey, "base64url");

        jws.SetProtectedHeader(signatureIndex, jwsProtHdr);

        bool bIncludeBom = false;

        var payload = new Payload
        {
            CardInfo = new CardInfo
            {
                Pan = "4000001234567899"
            }
        };

        jws.SetPayload(JsonConvert.SerializeObject(payload), "utf-8", bIncludeBom);

        string jwsCompact = jws.CreateJws();

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
}


