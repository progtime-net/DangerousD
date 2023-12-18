using DangerousD.GameCore;

// Force load assembly
var a = GameObject.debugTexture;

string path = Console.ReadLine();
while (path != "END")
{
    bool f = false;
    var type = Type.GetType($"DangerousD.GameCore.GameObjects.{path}, DangerousD");
    if (type is not null)
        Console.WriteLine("OK");
    else
        Console.WriteLine("NF");
    path = Console.ReadLine();
}