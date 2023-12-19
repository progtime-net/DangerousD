using DangerousD.GameCore;

// Force load assembly
var a = GameObject.debugTexture;
string path = Console.ReadLine();
while (path != "END")
{
    bool f = false;
    var type = Type.GetType($"DangerousD.GameCore.GameObjects.{path}, DangerousD");
    Console.Write(type is not null ? "OK" : "NF");
    path = Console.ReadLine();
}
