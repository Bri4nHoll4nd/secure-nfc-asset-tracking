using System.Net.Http.Json;

const string apiUrl =
    "http://localhost:5201/api/1.0/V1Scans";

using HttpClient httpClient = new();

bool running = true;

while (running)
{
    Console.Clear();

    Console.WriteLine("Virtual NFC Reader");
    Console.WriteLine();
    Console.WriteLine("1. Scan NFC tag");
    Console.WriteLine("0. Exit");
    Console.WriteLine();

    Console.Write("> ");

    string? input = Console.ReadLine();

    switch (input)
    {
        case "1":
            await ScanTag();
            break;

        case "0":
            running = false;
            break;
    }
}

async Task ScanTag()
{
    Console.Clear();

    Console.Write("Enter NFC UID: ");

    string? uid = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(uid))
    {
        Console.WriteLine("UID cannot be empty.");
        WaitForInput();
        return;
    }

    var request = new
    {
        tagUid = uid,
        source = "Simulator"
    };

    try
    {
        HttpResponseMessage response =
            await httpClient.PostAsJsonAsync(
                apiUrl,
                request);

        string body =
            await response.Content.ReadAsStringAsync();

        Console.WriteLine();
        Console.WriteLine(
            $"Status: {(int)response.StatusCode} " +
            $"{response.StatusCode}");

        Console.WriteLine(body);
    }
    catch (Exception ex)
    {
        Console.WriteLine();
        Console.WriteLine("Failed to contact API:");
        Console.WriteLine(ex.Message);
    }

    WaitForInput();
}

void WaitForInput()
{
    Console.WriteLine();
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}