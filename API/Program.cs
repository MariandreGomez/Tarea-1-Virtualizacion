using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Paso 1: Generar un número utilizando la API randomnumberapi.com
        string apiUrl = "http://www.randomnumberapi.com/api/v1.0/random?min=100&max=1000&count=1";
        int randomNumber = await GetRandomNumber(apiUrl);

        // Paso 2: Crear y escribir en el archivo de texto
        string fileName = $"{randomNumber}.txt";
        string fileContent = "1184519\n1000119";

        System.IO.File.WriteAllText(fileName, fileContent);

        // Paso 3: Subir el archivo a Google Drive
        await UploadToGoogleDrive(fileName);

        Console.WriteLine("Proceso completado.");
    }

    static async Task<int> GetRandomNumber(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            string response = await client.GetStringAsync(apiUrl);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(response)[0];
        }
    }

    static async Task UploadToGoogleDrive(string fileName)
    {
        string[] scopes = { DriveService.Scope.DriveFile };

        var credential = GoogleCredential.FromFile("C:\\Users\\maria\\Desktop\\API\\sec.json")
            .CreateScoped(scopes);

        var driveService = new DriveService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = "YourAppName",
        });

        var fileMetadata = new Google.Apis.Drive.v3.Data.File()
        {
            Name = Path.GetFileName(fileName),
            Parents = new List<string> { "1c87jtEK0ry_1bwcFXAYRmluqXFiP7xtf" },
        };

        FilesResource.CreateMediaUpload request;
        using (var stream = new FileStream(fileName, FileMode.Open))
        {
            request = driveService.Files.Create(fileMetadata, stream, "text/plain");
            request.Fields = "id";
            await request.UploadAsync();
        }

        var file = request.ResponseBody;
        Console.WriteLine($"Archivo '{file.Name}' subido a Google Drive con el ID: {file.Id}");
    }
}




