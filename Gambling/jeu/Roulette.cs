using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gambling.affichage;
using Gambling.models;

namespace Gambling.jeu;


public class Roulette
{
    public enum COLOR {NOIR, ROUGE, VERT};
    public enum STATE {EN_JEU, ARRETE};
    
    private IAffichageRoulette affichage;

    private List<Joueur> players =  new List<Joueur>();
    private int nbPlayers;
    private STATE[] actifPlayers;
    private int[] gainsPlayers;
    private Dictionary<Joueur, List<(int gain, int[] numero)>> mises = new();



    /*
    Numéro plein : 35
    A Cheval : 17
    Transversale : 11
    Carré : 8
    Sixain : 5
    Chance simpl(rouge/noir, pair/impair) = 1
    */
    private int[] tabMultiplicateurGain = {35, 17, 11, 8, 5, 1};

    private COLOR[] rouletteColor = new COLOR[37];
    private int[] rouletteNumber = new int[37];

    ////////////////////////
    //    Constructeur    //
    ////////////////////////
    public Roulette(int nbPlayers, IAffichageRoulette affichage)
    {
        this.affichage = affichage;
        affichage.printInit();

        this.nbPlayers = nbPlayers;
        gainsPlayers = new int[nbPlayers];
        actifPlayers = new STATE[nbPlayers];
        Array.Fill(actifPlayers, STATE.EN_JEU);
        Array.Fill(gainsPlayers, 0);

        initPlayers();
        initRoulette();
    }

    ////////////////////////
    //    Getter/Setter   //
    ////////////////////////
    public int getNbPlayers()
    {
        return nbPlayers;
    }

    public IReadOnlyList<Joueur> getPlayers()
    {
        return players;
    }
    
    public STATE[] getActifPlayers()
    {
        return actifPlayers.ToArray();
    }

    public int[] getGainsPlayers()
    {
        return gainsPlayers.ToArray();
    }
    
    public COLOR[] getRouletteColor()
    {
        return rouletteColor.ToArray();
    }

    public int[] getRouletteNumber()
    {
        return rouletteNumber.ToArray();
    }
    

    ////////////////////////
    //       Joueur       //
    ////////////////////////
    private void initPlayers()
    {
        for(int i=0; i<nbPlayers; i++)
        {
            players.Add(new Joueur(affichage.setPseudo()));
        }

        foreach (Joueur player in players)
        {
            mises[player] = new List<(int gain, int[] numero)>();
        }
    }

    public bool onePlayerActif()
    {
        for(int i=0; i<nbPlayers; i++)
        {
            if(actifPlayers[i] == STATE.EN_JEU) {return true;}
        }
        return false;
    }

    public void stop(int idPLayer)
    {
        actifPlayers[idPLayer] = STATE.ARRETE;
    }

    public void isEliminated(int idPlayer)
    {
        if(gainsPlayers[idPlayer] <= 0) {stop(idPlayer);}
    }

    public int nextPlayer(int id)
    {
        for(int i=1; i<=nbPlayers; i++)
        {
            int pos = (id + i) % nbPlayers;
            if(actifPlayers[pos] == STATE.EN_JEU) {return pos;}
        }
        return -1;
    }

    ////////////////////////
    //        Roue        //
    ////////////////////////
    private void initRoulette()
    {
        for(int i=0; i<37; i++)
        {
            if(i == 0){rouletteColor[i] = COLOR.VERT;}
            else {rouletteColor[i] = (COLOR)(i%2);}
            rouletteNumber[i] = i;
        }
    }

    public void spinOneTurn()
    { 
        for(int i=0; i<rouletteNumber.Length-1; i++)
        {
            int temp = rouletteNumber[i+1];
            rouletteNumber[i+1] = rouletteNumber[i];
            rouletteNumber[i] = temp;

            COLOR tmp = rouletteColor[i+1];
            rouletteColor[i+1] = rouletteColor[i];
            rouletteColor[i] = tmp;
        }
    }

    public void spinRoulette()
    {
        Random random = new Random();
        for (int i = 0; i < random.Next(20, 50); i++)
        {
            affichage.printRoulette(rouletteNumber, rouletteColor);
            spinOneTurn();
            affichage.printRoulette(rouletteNumber, rouletteColor);
            Thread.Sleep(100);
        }
        for (int i = 0; i < random.Next(10, 20); i++)
        {
            spinOneTurn();
            affichage.printRoulette(rouletteNumber, rouletteColor);
            Thread.Sleep(300);
        }
    }

    ////////////////////////
    //    Verification    //
    ////////////////////////
    public void verifGain(int numero)
    {
        if(numero == 0)
        {
            return;
        }

        for(int i=0; i<nbPlayers; i++)
        {
            foreach (var mise in mises[players[i]])
            {
                if(mise.numero.Contains(numero))
                    gainsPlayers[i] += mise.gain + (mise.gain * tabMultiplicateurGain[mise.numero.Length-1]);
                else
                    gainsPlayers[i] -= mise.gain;
                    isEliminated(i);
            }
        }
    }

    public static bool isConsecutive(int[] numero, int taille)
    {
        int tmp = numero[0]; // Car les nombres a saisir sont forcement positif
        for(int i=tmp; i<taille; i++)
        {
            if(numero[i] != tmp+1)
            {
                return false;
            }
            tmp = numero[i];
        }
        return true;
    }
    
    ////////////////////////
    //       Action       //
    ////////////////////////
    public int choiceAction(int idPLayer)
    {
        return affichage.printChoiceAction(players[idPLayer].getPseudo(), mises, players[idPLayer]);
    }

    public void miser(int idPlayer)
    {
        int continuer = 1;
        while(continuer == 1){
            int action = choiceAction(idPlayer);
            if(action == 7)
            {
                stop(idPlayer);
                break;
            }
            continuer = affichage.setMise(players[idPlayer], mises, action);
        }
    }

    public void remiseZeroMise()
    {
        for(int i=0; i<nbPlayers; i++)
        {
            foreach (var mise in mises.Values)
            {
                mise.Clear();
            }
        }
    }



    ////////////////////////
    //     Game Loop      //
    ////////////////////////
    public void run()
    {
        //Afficher le tour actuel
        //Afficher les choix : tourner, s'arreter
        //Jouer le tour
        //Afficher le classement
        int turn = 0;
        int playerTurn = 0;
        while (onePlayerActif())
        {
            // Chaque joueur enregistre les mises qu'ils jouent
            for(int i=0; i<nbPlayers; i++)
            {
                if(actifPlayers[i] == STATE.EN_JEU)
                {
                    // Choisir les mises
                    miser(i);
                }
            }
            affichage.printInfoTurn(mises, turn);
            // La roulette tourne !!!
            spinRoulette();
            // Verif des gains pour chacun des joueurs
            verifGain(rouletteNumber[0]);
            // Remise à Zero des mises et affichage des gains de chacun
            remiseZeroMise();
            affichage.printEndTurn(gainsPlayers, players, rouletteNumber[0]);
            playerTurn = nextPlayer(playerTurn);
            turn++;
        }
    }
}