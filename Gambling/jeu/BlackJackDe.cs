using System;
using System.Collections.Generic;
using System.Linq;
using Gambling.models;
using Gambling.utilitaire;
using Gambling.affichage;

namespace Gambling.jeu;

public class BlackJackDe
{
    public enum STATE {EN_JEU, ELIMINE, DERNIERE_CHANCE, ARRETE};
    private IAffichageBlackJackDe affichage;


    private List<Joueur> players = new List<Joueur>();
    private int nbPlayers;
    private int[] classement;

    private int[] scoresPlayer;
    private STATE[] actifPlayer;
    private int[] bonusPlayer;


    private const int OBJECTIF_SCORE = 21;
    private int objectifScore;

    ////////////////////////
    //    Constructeur    //
    ////////////////////////
    public BlackJackDe(int nbPlayers, int objectifScore, IAffichageBlackJackDe affichage)
    {
        this.affichage = affichage;

        this.nbPlayers = nbPlayers;
        scoresPlayer = new int[nbPlayers];
        actifPlayer = new STATE[nbPlayers];
        bonusPlayer = new int[nbPlayers];
        Array.Fill(bonusPlayer, 0);
        classement = Enumerable.Range(0, nbPlayers).ToArray();
        Array.Fill(actifPlayer, STATE.EN_JEU);
        initJoueur();
        this.objectifScore = objectifScore;
    }

    public BlackJackDe(int nbPlayers, IAffichageBlackJackDe affichage)
    {
        this.affichage = affichage;
        
        this.nbPlayers = nbPlayers;
        scoresPlayer = new int[nbPlayers];
        actifPlayer = new STATE[nbPlayers];
        bonusPlayer = new int[nbPlayers];
        Array.Fill(bonusPlayer, 0);
        classement = Enumerable.Range(0, nbPlayers).ToArray();
        Array.Fill(actifPlayer, STATE.EN_JEU);
        initJoueur();
        objectifScore = OBJECTIF_SCORE;
    }

    ////////////////////////
    //       Getter       //
    ////////////////////////
    public IReadOnlyList<Joueur> getPlayers()
    {
        return players;
    }

    public int getNbPlayers()
    {
        return nbPlayers;
    }

    public int[] getClassement()
    {
        return classement.ToArray();
    }

    public int[] getScoresPlayers()
    {
        return scoresPlayer.ToArray();
    }

    public int[] getBonusPlayer()
    {
        return bonusPlayer.ToArray();
    }

    public STATE[] getActifPlayer()
    {
        return actifPlayer.ToArray();
    }

    public int getObjectif()
    {
        return objectifScore;
    }


    ////////////////////////
    //       Joueur       //
    ////////////////////////
    private void initJoueur()
    {
        affichage.printInit();
        string pseudo;
        for(int i=0; i<nbPlayers; i++)
        {
            pseudo = affichage.setPseudo();
            if (string.IsNullOrEmpty(pseudo))
            {
                players.Add(new Joueur());
            }
            else
            {
                players.Add(new Joueur(pseudo));
            }
            scoresPlayer[i] = 0;
        }

        affichage.usingBonus(nbPlayers, players, bonusPlayer);
    }

    public int nextPlayer(int id)
    {
        for(int i=1; i<=nbPlayers; i++)
        {
            int pos = (id + i) % nbPlayers;
            if(actifPlayer[pos] == STATE.EN_JEU || actifPlayer[pos] == STATE.DERNIERE_CHANCE) {return pos;}
        }
        return -1;
    }

    
    ////////////////////////
    //       Statut       //
    ////////////////////////
    public bool isEliminated(int idPlayer)
    {
        if(scoresPlayer[idPlayer] > objectifScore)
        {
            if(bonusPlayer[idPlayer] >= 0)
                actifPlayer[idPlayer] = STATE.ELIMINE;
            else
                actifPlayer[idPlayer] = STATE.DERNIERE_CHANCE;
        }
        else
        {
            if(actifPlayer[idPlayer] != STATE.ARRETE)
                actifPlayer[idPlayer] = STATE.EN_JEU;
        }
        
        return scoresPlayer[idPlayer] > objectifScore;
    }

    public void stop(int idPlayer)
    {
        if(actifPlayer[idPlayer] == STATE.ELIMINE) return;
        actifPlayer[idPlayer] = STATE.ARRETE;
    }

    public bool onePlayerActif()
    {
        for(int i=0; i<nbPlayers; i++)
        {
            if(actifPlayer[i] == STATE.EN_JEU || actifPlayer[i] == STATE.DERNIERE_CHANCE) {return true;}
        }
        return false;
    }

    
    ////////////////////////
    //        Trie        //
    ////////////////////////
    public void trierClassement()
    {
        for (int i = 0; i < nbPlayers - 1; i++)
        {
            for (int j = i + 1; j < nbPlayers; j++)
            {
                int scoreI = scoresPlayer[classement[i]];
                int scoreJ = scoresPlayer[classement[j]];

                bool depasseI = scoreI > objectifScore;
                bool depasseJ = scoreJ > objectifScore;

                if ((depasseI && !depasseJ) ||
                    (depasseI == depasseJ &&
                    ((!depasseI && scoreJ > scoreI) ||
                    (depasseI && scoreJ < scoreI))))
                {
                    int temp = classement[i];
                    classement[i] = classement[j];
                    classement[j] = temp;
                }
            }
        }
    }


    ////////////////////////
    //      Game Loop     //
    ////////////////////////
    public void run()
    {
        int playersID = 0;
        int de = 0;
        int turn = 0;

        // Demande a chaque joueur
        while(onePlayerActif())
        {
            affichage.printInfoTour(turn, players[playersID].getPseudo(), scoresPlayer[playersID], objectifScore);
            affichage.printChoiceAction();
            
            int choice = affichage.chooseAction(actifPlayer, playersID);
            
            switch (choice)
            {
                case 1:
                    // Lancer le dé
                    affichage.printAnimationLancer();
                    de = Utilitaire.lancer_de(20);
                    affichage.printRoll(de);
                    break;

                case 2:
                    // Lancer risque
                    affichage.printAnimationLancer();
                    de = Utilitaire.lancer_de_risque(20);
                    affichage.printRoll(de);
                    break;

                case 3:
                    // Utiliser bonus
                    affichage.printUseBonus();
                    scoresPlayer[playersID] += bonusPlayer[playersID];
                    bonusPlayer[playersID] = 0;
                    break;

                case 4:
                    // Terminer la partie
                    affichage.printStop();
                    stop(playersID);
                    break;

                default:
                    // Gérer l'erreur
                    break;
            }

            scoresPlayer[playersID] += de;
            isEliminated(playersID);
            de = 0;

            trierClassement();
            affichage.printClassement(actifPlayer, nbPlayers, classement, players, scoresPlayer);

            if(choice != 3 || actifPlayer[playersID] == STATE.ELIMINE)
            {
                turn++;
                playersID = nextPlayer(playersID);
            }
        }
        affichage.printRecapPartie(objectifScore, nbPlayers, classement,players, scoresPlayer);
    }
}