using System;
using System.Collections.Generic;
using System.Linq;
using Gambling.jeu;
using Gambling.models;

namespace Gambling.affichage;

public interface IAffichageRoulette
{
    void printInit();
    string setPseudo();
    void printRoulette(int[] numeros, Roulette.COLOR[] couleurs);
    int printChoiceAction(string pseudo, Dictionary<Joueur, List<(int gain, int[] numero)>> mises, Joueur player);
    int setMise(Joueur player, Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int action);
    void printInfoTurn(Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int turn);
    void printEndTurn(int[] gainsPlayers, List<Joueur> players, int numeroGagnant);
}

public class AffichageRouletteConsole() : IAffichageRoulette
{
    public void printInit()
    {
        Console.Clear();

        // Titre
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════════╗");
        Console.WriteLine("          ║                                          ║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("          ║             ROULETTE - REGLES            ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ║                                          ║");
        Console.WriteLine("          ╚══════════════════════════════════════════╝");

        Console.ResetColor();

        // Présentation
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("                         COMMENT JOUER ?");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  La roulette consiste à miser sur le résultat");
        Console.WriteLine("  d'une bille lancée sur une roue numérotée de 0 à 36.");
        Console.WriteLine();

        // Règles
        printRules("1", "Choisis le type de mise : numéro, cheval,");
        Console.WriteLine("     transversale, carré, sixain ou chance simple.");

        printRules("2", "Définis le montant de ta mise.");
        printRules("3", "La bille détermine le numéro gagnant.");
        printRules("4", "Si ton pari correspond au résultat, tu gagnes.");
        printRules("5", "Sinon, tu perds le montant misé.");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  Le zéro est vert, les numéros sont rouges ou noirs.");
        Console.ResetColor();

        // Création des joueurs
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════════╗");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("          ║           PREPARATION DE LA PARTIE       ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╚══════════════════════════════════════════╝");

        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  La présentation des règles est terminée.");
        Console.WriteLine("  Tu vas maintenant procéder à la création");
        Console.WriteLine("  des joueurs qui participeront à la partie.");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("  > Appuie sur Entrée pour continuer...");
        Console.ResetColor();

        Console.ReadLine();
    }

    private void printRules(string numero, string texte)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"  [{numero}] ");

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine(texte);

        Console.ResetColor();
    }

    public string setPseudo()
    {
        string pseudo;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("          ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("               ROULETTE");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("               ║");

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("          ║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("          CRÉATION DU JOUEUR");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ║");

        Console.WriteLine("          ╚══════════════════════════════════════╝");

        Console.ResetColor();


        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Choisissez un pseudo pour la partie.");
        Console.WriteLine();

        do
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("  Pseudo > ");
            Console.ResetColor();

            pseudo = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(pseudo))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Le pseudo ne peut pas etre vide.");
                Console.ResetColor();
                Console.WriteLine();
            }

        } while (string.IsNullOrWhiteSpace(pseudo));

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  Joueur enregistre : {pseudo}");
        Console.ResetColor();

        Console.WriteLine();
        Console.Write("  Appuyez sur Entree pour continuer...");
        Console.ReadLine();

        return pseudo;
    }

    public void printRoulette(int[] rouletteNumber, Roulette.COLOR[] rouletteColor)
    {
        int[] ordre =
        {
            0, 32, 15, 19, 4, 21, 2, 25, 17, 34,
            6, 27, 13, 36, 11, 30, 8, 23, 10, 5,
            24, 16, 33, 1, 20, 14, 31, 9, 22, 18,
            29, 7, 28, 12, 35, 3, 26
        };

        int centreX = 32;
        int centreY = 13;
        int rayonX = 25;
        int rayonY = 10;

        Console.Clear();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("                    ROULETTE");
        Console.WriteLine();
        Console.SetCursorPosition(centreX, centreY - rayonY - 1);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("V");

        // Affichage des numéros en cercle
        for (int i = 0; i < ordre.Length; i++)
        {
            double angle = -Math.PI / 2 - 2 * Math.PI * i / ordre.Length;

            int x = centreX + (int)Math.Round(rayonX * Math.Cos(angle));
            int y = centreY + (int)Math.Round(rayonY * Math.Sin(angle));

            Console.SetCursorPosition(x - 1, y);
            Console.ForegroundColor = GetConsoleColor(rouletteColor[i]);
            Console.Write($"{rouletteNumber[i],2}");
        }

        Console.ResetColor();

        // Table de jeu
        int debutY = centreY + rayonY + 3;

        Console.SetCursorPosition(5, debutY);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("TABLE DE JEU");

        // Zéro
        Console.SetCursorPosition(5, debutY + 2);
        AfficherNumero(0, rouletteNumber, rouletteColor);

        // Grille de 1 à 36
        for (int ligne = 0; ligne < 3; ligne++)
        {
            Console.SetCursorPosition(10, debutY + 2 + ligne);

            for (int colonne = 0; colonne < 12; colonne++)
            {
                int numero = colonne * 3 + (3 - ligne);

                AfficherNumero(numero, rouletteNumber, rouletteColor);
                Console.Write(" ");
            }
        }

        Console.ResetColor();
        Console.SetCursorPosition(0, debutY + 7);
    }

    private void AfficherNumero(int numero, int[] rouletteNumber, Roulette.COLOR[] rouletteColor)
    {
        int index = Array.IndexOf(rouletteNumber, numero);

        if (index == -1 || index >= rouletteColor.Length)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($" {numero,2} ");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = GetConsoleColor(rouletteColor[index]);
        Console.Write($" {rouletteNumber[index],2} ");
        Console.ResetColor();
    }

    private ConsoleColor GetConsoleColor(Roulette.COLOR couleur)
    {
        return couleur switch
        {
            Roulette.COLOR.ROUGE => ConsoleColor.Red,
            Roulette.COLOR.NOIR  => ConsoleColor.DarkGray,
            Roulette.COLOR.VERT  => ConsoleColor.Green,
            _                    => ConsoleColor.White
        };
    }

    public int printChoiceAction(string pseudo, Dictionary<Joueur, List<(int gain, int[] numero)>> mises, Joueur player)
    {
        Console.Clear();

        // Titre
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════╗");
        Console.WriteLine("          ║                                      ║");
        Console.Write("          ║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("                ROULETTE");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("              ║");
        Console.WriteLine("          ║                                      ║");
        Console.Write("          ║");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("       C'est le moment de sortir ");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("     ║");
        Console.Write("          ║");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("           l'argent des poches");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("        ║");
        Console.WriteLine("          ║                                      ║");
        Console.WriteLine("          ╚══════════════════════════════════════╝");

        Console.ResetColor();

        // Joueur et ses mises actuelles
        Console.WriteLine();
        Console.Write("Joueur : ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(pseudo);
        Console.ResetColor();
        Console.Write("Mises : ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        foreach (var mise in mises[player])
        {
            printMises(mise.numero, mise.gain);
        }
        Console.ResetColor();

        // Question
        Console.WriteLine();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("                Comment souhaites-tu miser ?");
        Console.ResetColor();

        // Choix
        Console.WriteLine();

        printChoix("Numéro unique", 1);
        printChoix("À cheval", 2);
        printChoix("Transversale", 3);
        printChoix("Carré", 4);

        Console.WriteLine();
        Console.WriteLine();

        printChoix("Sixain", 5);
        printChoix("Chance simple", 6);
        printChoix("S'arreter", 7);

        Console.WriteLine();

        // Saisie
        Console.WriteLine();
        Console.WriteLine();


        // Vérif du input
        int choix;
        do
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("          > ");
            Console.ResetColor();

            if (!int.TryParse(Console.ReadLine(), out choix)
                || choix < 1
                || choix > 7)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Erreur : entre un nombre entre 1 et 7.");
                Console.ResetColor();
                choix = 0;
            }

        } while (choix < 1 || choix > 7);

        return choix;
    }

    private void printMises(int[] numeros, int mise)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write(" [");
        for(int i=0; i<numeros.Length; i++)
        {
            if (i == 0)
            {
                Console.Write($"{numeros[i]} ");
            }
            else
            {
                Console.Write($", {numeros[i]}");
            }
        }
        Console.Write("] : ");

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"{mise} pièces   ");

        Console.ResetColor();
    }

    private void printChoix(string texte, int numero)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"{texte} : ");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{numero}    ");

        Console.ResetColor();

    }

    public int setMise(Joueur player, Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int action)
    {
        // Initialiser la liste si nécessaire
        if (!mises.ContainsKey(player))
            mises[player] = new List<(int gain, int[] numero)>();

        Console.WriteLine();

        // Choix du type de mise
        string nomAction = action switch
        {
            1 => "Numéro unique",
            2 => "À cheval",
            3 => "Transversale",
            4 => "Carré",
            5 => "Sixain",
            6 => "Chance simple",
            _ => ""
        };

        // Nombre de numéros à sélectionner
        int nbNumeros = action switch
        {
            1 => 1,
            2 => 2,
            3 => 3,
            4 => 4,
            5 => 6,
            6 => 1,
            _ => 1
        };

        // Affichage des informations
        Console.WriteLine();
        Console.WriteLine($"Tu as choisi : {nomAction}");
        Console.WriteLine($"Tes pièces : {player.getCoins()}");

        // Saisie des numéros
        int[] numeros;

        while (true)
        {
            Console.WriteLine();
            Console.Write($"Sur quels numéros mises-tu ? ({nbNumeros} numéro(s) separe par des ',') : ");

            string saisie = Console.ReadLine();

            if (saisie == null)
                continue;

            string[] valeurs = saisie.Split(
                ',',
                StringSplitOptions.TrimEntries |
                StringSplitOptions.RemoveEmptyEntries);

            if (valeurs.Length != nbNumeros)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Entre exactement {nbNumeros} numéro(s).");
                Console.ResetColor();
                continue;
            }

            if (!valeurs.All(v =>
                    int.TryParse(v, out int n) && n >= 0 && n <= 36))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Les numéros doivent être compris entre 0 et 36.");
                Console.ResetColor();
                continue;
            }

            numeros = valeurs.Select(int.Parse).ToArray();

            if(!Roulette.isConsecutive(numeros, valeurs.Length))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Les numéros doivent être consécutifs et différents les uns des autres.");
                Console.ResetColor();
                continue;
            }

            break;
        }

        // Saisie du montant
        Console.WriteLine();

        int montant = LireEntier(
            "Combien mises-tu ? ",
            1,
            player.getCoins());

        // Enregistrer la mise
        mises[player].Add((montant, numeros));

        // Déduire les pièces misées
        player.setCoins(-montant);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Mise enregistrée : {montant} pièce(s).");
        Console.ResetColor();

        Console.WriteLine($"Pièces restantes : {player.getCoins()}");

        // Continuer ?
        if (player.getCoins() > 0)
        {
            Console.WriteLine();
            int choix = LireEntier(
                "Souhaites-tu continuer à miser ? (1 : oui, 2 : non) ",
                1,
                2);
            if(choix == 1)
            {
                return 1;
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.WriteLine("Tu n'as plus de pièces disponibles.");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.WriteLine("Fin des mises du joueur.");
        Console.WriteLine("Appuie sur Entrée pour continuer...");
        Console.ReadLine();

        return 0;
    }

    private int LireEntier(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int valeur)
                && valeur >= min
                && valeur <= max)
            {
                return valeur;
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  Entre un nombre entre {min} et {max}.");
            Console.ResetColor();
        }
    }

    public void printInfoTurn(Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int turn)
    {
        Console.Clear();

        // Titre
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════╗");
        Console.WriteLine("          ║                                      ║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("          ║              ROULETTE                ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ║                                      ║");
        Console.WriteLine("          ║            INFORMATIONS TOUR         ║");
        Console.WriteLine("          ║                                      ║");
        Console.WriteLine("          ╚══════════════════════════════════════╝");

        Console.ResetColor();

        // Numéro du tour
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"              TOUR N° {turn}");
        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("  ═══════════════ MISES DES JOUEURS ═══════════════");
        Console.WriteLine();

        if (mises.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  Aucune mise enregistrée.");
            Console.ResetColor();
        }
        else
        {
            foreach (var (joueur, listeMises) in mises)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"  Joueur : {joueur.getPseudo()}");
                Console.ResetColor();

                if (listeMises.Count == 0)
                {
                    Console.WriteLine("    Aucune mise.");
                }
                else
                {
                    foreach (var mise in listeMises)
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("    Numéros : ");

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(string.Join(", ", mise.numero));

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write("    Montant misé : ");

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"{mise.gain} pièce(s)");

                        Console.ResetColor();
                        Console.WriteLine();
                    }
                }
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("  ════════════════════════════════════════════════");
        Console.ResetColor();

        Console.WriteLine();
        Console.Write("  Appuie sur Entrée pour lancer le tour...");
        Console.ReadLine();
    }

    public void printEndTurn(int[] gainsPlayers, List<Joueur> players, int numeroGagnant)
    {
        Console.WriteLine();
        Console.WriteLine();

        // Titre
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ╔══════════════════════════════════════╗");
        Console.WriteLine("          ║                                      ║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("          ║                ROULETTE              ║");
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.WriteLine("          ║                                      ║");
        Console.WriteLine("          ║              FIN DU TOUR             ║");
        Console.WriteLine("          ║                                      ║");
        Console.WriteLine("          ╚══════════════════════════════════════╝");

        Console.ResetColor();

        // Message
        
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"              NUMERO GAGNANT N° {numeroGagnant}");
        Console.ResetColor();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("              Résultats du tour");
        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("          ┌──────────────────────────────────┐");

        // Affichage des gains de chaque joueur
        for (int i = 0; i < players.Count; i++)
        {
            Console.Write("          │ ");

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write($"{players[i]}");

            Console.ResetColor();
            Console.Write(" : ");

            if (gainsPlayers[i] > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"+{gainsPlayers[i]}");
            }
            else if (gainsPlayers[i] < 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(gainsPlayers[i]);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("0");
            }

            Console.ResetColor();
            Console.WriteLine(" pièces");

        }

        Console.WriteLine("          └──────────────────────────────────┘");

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Write("          > ");
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write("Appuie sur Entrée pour continuer...");
        Console.ResetColor();

        Console.ReadLine();
    }
}