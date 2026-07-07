using System.Net.Http.Json;

bool running = true;

while (running)
{
    Console.Title = "Virtual NFC Reader";
    Console.WriteLine("Welcome, What would you like to do?");
    Console.WriteLine("");
    Console.WriteLine("1. Scan Operator");
    Console.WriteLine("2. Scan Asset");
    Console.WriteLine("3. Create Operator");
    Console.WriteLine("4. Create Asset");
    Console.WriteLine("0. Exit");

    var input = Console.ReadLine();

    switch (input) 
    {
        case "1":
            {
                await scanOperator();
                break;
            }

        case "2":
            {
                await scanAsset();
                break;
            }

        case "3":
            {
                await scanOperator();
                break;
            }

        case "4":
            {
                await scanAsset();
                break;
            }

        case "0":
            {
                Console.WriteLine("Exiting...");
                running = false;
                break;
            }

        default:
            {
                Console.WriteLine("Wrong/no number entered");
                break;
            }
    }
}

async Task scanOperator()
{
    Console.Clear();

    Console.WriteLine("Enter Operator UID:");
    var operatorUid = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(operatorUid))
    {
        Console.WriteLine("No UID entered.");
        return;
    }

    Console.WriteLine("Enter Operator ID:");
    var operatorId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(operatorId))
    {
        Console.WriteLine("No ID entered.");
        return;
    }

    Console.WriteLine("Enter Operator Type:");
    var operatorType = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(operatorType))
    {
        Console.WriteLine("No Type entered.");
        return;
    }

    Console.WriteLine("Enter Operator Version:");
    var operatorVersion = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(operatorVersion))
    {
        Console.WriteLine("No Version entered.");
        return;
    }

    Console.WriteLine("Enter Operator Signature:");
    var operatorSignature = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(operatorSignature))
    {
        Console.WriteLine("No Signature entered.");
        return;
    }

    await SendScan(operatorUid, operatorId, operatorType, operatorVersion, operatorSignature);

    Console.WriteLine($"Operator UID scanned: {operatorUid}");

    Console.WriteLine("");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

async Task scanAsset()
{
    Console.Clear();

    Console.WriteLine("Enter Asset UID:");
    var assetUid = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(assetUid))
    {
        Console.WriteLine("No UID entered.");
        return;
    }

    Console.WriteLine("Enter Asset ID:");
    var assetId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(assetId))
    {
        Console.WriteLine("No ID entered.");
        return;
    }

    Console.WriteLine("Enter Asset Type:");
    var assetType = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(assetType))
    {
        Console.WriteLine("No Type entered.");
        return;
    }

    Console.WriteLine("Enter Asset Version:");
    var assetVersion = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(assetVersion))
    {
        Console.WriteLine("No Version entered.");
        return;
    }

    Console.WriteLine("Enter Asset Signature:");
    var assetSignature = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(assetSignature))
    {
        Console.WriteLine("No Signature entered.");
        return;
    }

    await SendScan(assetUid, assetId, assetType, assetVersion, assetSignature);

    Console.WriteLine($"Asset UID scanned: {assetUid}");

    Console.WriteLine("");
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}

async Task SendScan(string tagUid, string tagId, string tagType, string tagVersion, string tagSignature)
{
    if (string.IsNullOrWhiteSpace(tagUid))
    {
        Console.WriteLine("No UID entered.");
        return;
    }


    var scanRequest = new
    {
        tagUid = tagUid,
        tagId = tagId,
        tagType = tagType,
        tagVersion = tagVersion,
        tagSignature = tagSignature
    };

    using HttpClient httpClient = new HttpClient();

    try
    {
        var response = await httpClient.PostAsJsonAsync(
            "http://localhost:5201/api/1.0/V1Scans",
            scanRequest
        );

        Console.WriteLine($"Status: {response.StatusCode}");

        string responseText = await response.Content.ReadAsStringAsync();

        Console.WriteLine("Response body: ");
        Console.WriteLine(string.IsNullOrWhiteSpace(responseText) ? "<empty>" : responseText);
    }
    catch (Exception e)
    {
        Console.WriteLine("Request failed:");
        Console.WriteLine($"{e.Message}");
    }
}


