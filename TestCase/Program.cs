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
        string host = "acstopay.online";
        string url = $"https://{host}/api/testassignments/pan";
        string KeyID = "47e8fde35b164e888a57b6ff27ec020f";
        string SharedKey = "ac/1LUdrbivclAeP67iDKX2gPTTNmP0DQdF+0LBcPE/3NWwUqm62u5g6u+GE8uev5w/VMowYXN8ZM+gWPdOuzg==";

        var protect = new Protected
        {
            alg = "HS256",
            kid = KeyID,
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

        var jws = MakeJws(protect, payload, SharedKey);

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


