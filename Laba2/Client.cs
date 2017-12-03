using System.IO;
using System.Net;
using System.Text;

namespace Laba2
{
  public class Client
  {
    HttpStatusCode status;
    private string host;
    private int port;

    public bool Ping()
    {
      this.SendHttpRequest("GET", "Ping", out status);
      return (status == HttpStatusCode.OK) ? true : false;
    }

    public byte[] GetInputData()
    {
      return this.SendHttpRequest("GET", "GetInputData", out status);
    }

    public void WriteAnswer(byte[] requestBody)
    {
      this.SendHttpRequest("POST", "WriteAnswer", out status, requestBody);
    }

    private byte[] SendHttpRequest(string httpMethod, string method, out HttpStatusCode status, byte[] requestBody = null)
    {
      try
      {
        var webRequest = (HttpWebRequest)WebRequest.Create($"http://{this.host}:{this.port}/{method}");
        webRequest.Method = httpMethod;
        webRequest.Timeout = 1000;

        if (requestBody != null && requestBody.Length > 0)
        {
          using (var sw = new StreamWriter(webRequest.GetRequestStream()))
          {
            sw.Write(Encoding.UTF8.GetString(requestBody));
          }
        }

        var response = (HttpWebResponse)webRequest.GetResponse();
        status = response.StatusCode;

        using (var sr = new StreamReader(response.GetResponseStream()))
        {
          var responseString = Encoding.UTF8.GetBytes(sr.ReadToEnd());
          return responseString;
        }
      }
      catch
      {
        status = HttpStatusCode.InternalServerError;
        return new byte[0];
      }
    }

    public Client(string host, int port)
    {
      this.host = host;
      this.port = port;
    }
  }
}