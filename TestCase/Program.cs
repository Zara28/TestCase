using RestSharp;
using TestCase;
using TestCase.Models;
using RestSharp.Serializers.NewtonsoftJson;
using System;
using System.Text;
using Newtonsoft.Json;

public class Program
{
    public static void Main(string[] args)
    {
        string host = "";
        string url = $"";
        string KeyID = "";
        string SharedKey = "";

        Protected protect = new Protected()
        {
            alg = "HS256",
            kid = KeyID,
            signdate = DateTime.Now.Date,
        };

        string number = "1";

        //Payload payload = new Payload()
        //{
        //    CardInfo = new CardInfo()
        //    {
        //        Pan = number
        //    }
        //};
        var payload = new
        {
            EchoMessage = "TestMessage"
        };

        var jws = MakeJws(protect, payload, SharedKey);

        RestClient restClient = new(url, configureSerialization: s => s.UseNewtonsoftJson());

        var restRequest = new RestRequest(url, Method.Post);

        restRequest.AddBody(jws);

        var responce = restClient.ExecuteAsync(restRequest).Result;

        Console.WriteLine(responce.Content);
        if (!responce.IsSuccessful)
        {
            Console.WriteLine(responce.Content);
        }
    }

    private static string MakeJws(object header, object payload, string secretKey)
    {
        UTF8Encoding encoding = new UTF8Encoding();

        var headJson = JsonConvert.SerializeObject(header);
        var payloadJson = JsonConvert.SerializeObject(payload);

        var sign = new Signature
        {
            Base64Header = encoding.GetString(encoding.GetBytes(headJson)).Base64UrlEncode(),
            Base64Payload = payloadJson.Base64UrlEncode(),
            SecretKey = secretKey
        };

        var signature = sign.GenerateSignature();

        return $"{headJson.Base64UrlEncode()}.{payloadJson.Base64UrlEncode()}.{signature}";
    }

}

