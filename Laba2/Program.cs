using System;
using System.Text;

namespace Laba2
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var host = "127.0.0.1";
      var port = int.Parse(Console.ReadLine());
      var client = new Client(host, port);

      while(!client.Ping());
      var inputData = client.GetInputData();

      var serializedInput = Encoding.UTF8.GetString(inputData);
      var input = serializedInput.Deserialize<Input>();
      var output = new Output(input);
      var serializedOutput = Encoding.UTF8.GetBytes(output.Serialize());

      client.WriteAnswer(serializedOutput);
    }
  }
}