using HashidsNet;

namespace FileEventing.Service;

public static class EntityReference
{
    private const string Seed = "ae5a7b4b-cb71-40cd-a805-2803b849921d";

    private static Hashids _idCodec = new(Seed, minHashLength: 6);

    public static string Create(int realId) => _idCodec.Encode(realId);

    public static int Decode(string reference) => _idCodec.Decode(reference)[0];
}