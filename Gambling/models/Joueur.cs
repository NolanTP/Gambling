using System;
using Gambling.utilitaire;

namespace Gambling.models;

public class Joueur
{
    private string pseudo;
    private int coins = 20;

    // Constructeurs
    public Joueur(string pseudo)
    {
       this.pseudo = pseudo; 
    }

    public Joueur()
    {
        pseudo = $"Player {Utilitaire.lancer_de(100)}";
    }


    // Getter et setter
    public string getPseudo()
    {
        return pseudo;
    }

    public int getCoins()
    {
        return coins;
    }

    public void setCoins(int nbCoins)
    {
        coins += nbCoins;
    }

    // Static methode
    public static string setPseudo()
    {
        Console.WriteLine("Quel est ton pseudo ? ()");
        string pseudo = Console.ReadLine();

        return pseudo;
    }
}