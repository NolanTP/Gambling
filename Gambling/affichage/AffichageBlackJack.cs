using System;
using System.Collections.Generic;
using System.Threading;
using Gambling.models;
using Gambling.jeu;

namespace Gambling.affichage;

public interface IAffichageBlackJackDe
{
    void usingBonus(int nbPlayers, List<Joueur> players, int[]bonusPlayer);
    int chooseAction(BlackJackDe.STATE[] actifPlayer, int playersID);
    void printRoll(int roll);
    void printUseBonus();
    void printStop();
    string setPseudo();
    void setBonus(int nbPlayers, List<Joueur>players, int[] bonusPlayer);
    void printRecapPartie(int objectifScore, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer);
    void printInit();
    void printInfoTour(int numeroTour, string pseudo, int score, int objectif);
    void printClassement(BlackJackDe.STATE[] actifPlayer, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer);
    void printChoiceAction();
    void printAnimationLancer();
}

public class AffichageBlackJackDeConsole() : IAffichageBlackJackDe
{
    public void usingBonus(int nbPlayers, List<Joueur> players, int[]bonusPlayer)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Souhaitez-vous utilisez des bonus ? (1 pour accepter)");
        Console.Write("  > ");
        string res = Console.ReadLine();
        if(res == "1"){setBonus(nbPlayers, players, bonusPlayer);}
    }

    public int chooseAction(BlackJackDe.STATE[] actifPlayer, int playersID)
    {
        Console.Write("> ");
            int choice;
            bool choiceIsCorrect = true;
            do
            {
                Console.Write("Votre choix (1-4) : ");
                string saisie = Console.ReadLine();

                if (!int.TryParse(saisie, out choice))
                {
                    choice = 0;
                }

                if (choice < 1 || choice > 4)
                {
                    Console.WriteLine("Choix invalide. Entrez un nombre entre 1 et 4.");
                    choiceIsCorrect = true;
                }
                else if(choice == 1 || choice == 2)
                {
                    choiceIsCorrect = actifPlayer[playersID] == BlackJackDe.STATE.DERNIERE_CHANCE;
                }
                else
                {
                    choiceIsCorrect = false;
                }

            } while (choiceIsCorrect);
            return choice;
    }

    public void printRoll(int roll)
    {
        Console.WriteLine($"\rLe résultat obtenu est {roll}");
    }

    public void printUseBonus()
    {
        Console.WriteLine("Vous avez utilise votre bonus !");
    }

    public void printStop()
    {
        Console.WriteLine("\rVous avez decide de vous arreter");
    }

    public string setPseudo()
    {
        string pseudo;

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("             BLACKJACK");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("                ║");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("          CRÉATION DU JOUEUR");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("          ║");

        Console.WriteLine("╚══════════════════════════════════════╝");

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

    public void setBonus(int nbPlayers, List<Joueur>players, int[] bonusPlayer)
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("             BLACKJACK");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("                ║");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("          ATTRIBUTION DES BONUS");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("        ║");

        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  CONFIGURATION DES JOUEURS");
        Console.ResetColor();

        Console.WriteLine();
        Console.WriteLine("  Définissez les bonus de chaque joueur.");
        Console.WriteLine("  Ces bonus pourront être utilisés pendant la partie.");
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("═════════════════════════════════════════");
        Console.ResetColor();

        for (int i = 0; i < nbPlayers; i++)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"  Joueur {i + 1} ");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"- {players[i].getPseudo()}");
            Console.ResetColor();

            int bonus = 1;

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("  Nombre de bonus : ");
                Console.ResetColor();

                string saisie = Console.ReadLine();

                if (!int.TryParse(saisie, out bonus) || bonus >= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("  Veuillez entrer un nombre positif ou nul.");
                    Console.ResetColor();
                }
            }
            while (bonus >= 0);

            bonusPlayer[i] = bonus;

            Console.WriteLine();
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  Les bonus ont été configurés !");
        Console.ResetColor();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Appuyez sur ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("ENTREE");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" pour continuer...");
        Console.ResetColor();
    }

    public void printRecapPartie(int objectifScore, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.WriteLine("╔══════════════════════════════════════╗");
        Console.WriteLine("║           FIN DE LA PARTIE           ║");
        Console.WriteLine("║           RÉCAPITULATIF              ║");
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();
        Console.WriteLine();

        Console.WriteLine($"  Objectif : {objectifScore} points");
        Console.WriteLine();
        Console.WriteLine($"  {"Rang",-8} {"Joueur",-18} {"Score",-8} {"Résultat",-12}");
        Console.WriteLine("  " + new string('-', 48));

        for (int i = 0; i < nbPlayers; i++)
        {
            int indiceJoueur = classement[i];
            string pseudo = players[indiceJoueur].getPseudo();
            int score = scoresPlayer[indiceJoueur];

            string resultat = score > objectifScore
                ? "Dépassé"
                : score == objectifScore
                    ? "Objectif atteint"
                    : "En dessous";

            if (i == 0)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (score > objectifScore)
                Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine(
                $"  {i + 1,-8} {pseudo,-18} {score,-8} {resultat,-12}");

            Console.ResetColor();
        }

        Console.WriteLine();
        Console.WriteLine("  " + new string('-', 48));
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"  Vainqueur : {players[classement[0]].getPseudo()}");
        Console.ResetColor();

        Console.WriteLine();
        Console.Write("  Appuyez sur Entrée pour quitter...");
        Console.ReadLine();
    }

    public void printInit()
    {
        Console.Clear();

        // Titre principal
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("║");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("             BLACK");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("JACK");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("                ║");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("║");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("             JEU DE ");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("DÉS");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("               ║");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();


        Console.ResetColor();
        Console.WriteLine();

        // Accueil
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  BIENVENUE DANS LA PARTIE !");
        Console.ResetColor();

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  Le hasard decide du resultat.");
        Console.ResetColor();

        Console.ResetColor();
        Console.WriteLine("  A vous de choisir comment prendre vos risques !");

        Console.WriteLine();

        // Séparateur
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("═════════════════════════════════════════");
        Console.ResetColor();

        Console.WriteLine();

        // Étape actuelle
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  ETAPE 1");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  INITIALISATION DES JOUEURS");
        Console.ResetColor();

        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("  La partie va commencer.");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("  Les joueurs doivent maintenant etre crees.");
        Console.ResetColor();

        Console.WriteLine();

        // Invitation à continuer
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("  Appuyez sur ");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("ENTREE");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" pour continuer...");
        Console.ResetColor();

        Console.ReadLine();

    }

    public void printInfoTour(int numeroTour, string pseudo, int score, int objectif)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;

        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("             INFORMATIONS             ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("║");

        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();


        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  Tour       : {numeroTour}");
        Console.WriteLine($"  Joueur     : {pseudo}");
        Console.WriteLine($"  Score      : {score}");
        Console.WriteLine($"  Objectif   : {objectif}");
        Console.WriteLine();
        Console.WriteLine("═════════════════════════════════════════");
        Console.WriteLine();
    }

    public void printClassement(BlackJackDe.STATE[] actifPlayer, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;

        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("              CLASSEMENT              ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("║");

        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();


        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine($"  {"Rang",-6} {"Joueur",-18} {"Score",-8} {"Etat",-10}");
        Console.WriteLine("  " + new string('-', 44));

        for (int j = 0; j < nbPlayers; j++)
        {
            int i = classement[j];
            string rang = (j + 1).ToString();
            string pseudo = players[i].getPseudo();
            string score = scoresPlayer[i].ToString();
            string etat = actifPlayer[i] == BlackJackDe.STATE.ELIMINE
                                        ? "ELIMINE"
                                        : actifPlayer[i] == BlackJackDe.STATE.ARRETE
                                            ? "ARRETE"
                                            : actifPlayer[i] == BlackJackDe.STATE.DERNIERE_CHANCE
                                                ? "DERNIERE CHANCE"
                                                : "EN JEU";

            if (etat == "ELIMINE")
                Console.ForegroundColor = ConsoleColor.Red;
            else if(actifPlayer[i] == BlackJackDe.STATE.ARRETE)
                Console.ForegroundColor = ConsoleColor.Gray;
            else if(actifPlayer[i] == BlackJackDe.STATE.DERNIERE_CHANCE)
                Console.ForegroundColor = ConsoleColor.DarkRed;
            else if (j == 0)
                Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($"  {rang,-6} {pseudo,-18} {score,-8} {etat,-10}");
            Console.ResetColor();
        }

        Console.WriteLine();
        Console.Write("  Appuyez sur Entree pour revenir...");
        Console.ReadLine();
    }

    public void printChoiceAction()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("╔══════════════════════════════════════╗");

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write("║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("             À TON TOUR !");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("             ║");

        Console.WriteLine("╚══════════════════════════════════════╝");

        Console.ResetColor();


        Console.ResetColor();
        Console.WriteLine();
        Console.WriteLine("  Choisissez votre action.");
        Console.WriteLine();
        Console.WriteLine("  [1] Lancer le de");
        Console.WriteLine("  [2] Effectuer un lancer risque");
        Console.WriteLine("  [3] Utiliser un bonus");
        Console.WriteLine("  [4] S'arreter");
        Console.WriteLine();
        Console.WriteLine("═════════════════════════════════════════");
        Console.WriteLine();
    }

   public void printAnimationLancer()
    {
        Console.WriteLine("\r");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  LANCEMENT EN COURS");
        Console.ResetColor();
        Console.WriteLine();

        for (int i = 0; i < 15; i++)
        {
            Console.Write($"\r  Le de roule... {Random.Shared.Next(1, 21),2}   ");
            Thread.Sleep(100);
        }
    }
}