using System;

namespace Gambling.utilitaire;

public class Utilitaire
{
    private static Random random = new Random();
    static public int lancer_de(int nb)
    {
        return random.Next(1, nb);
    }

    static public int lancer_de_risque(int nb)
    {
        return random.Next(nb/2, nb + nb/3);
    }
}