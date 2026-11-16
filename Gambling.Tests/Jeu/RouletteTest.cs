using Gambling.affichage;
using Gambling.jeu;
using Gambling.models;

namespace GamblingTests.Tests;

/*
A faire:
-Tester la protection des donnees (ne pas pouvoir modifier autrement que par les 
fonctions donnees)
*/

public class FauxAffichageRoulette : IAffichageRoulette
{
    public string setPseudo() {return "TEST";}
    public void setGains(int nbPlayers, int[] gainsPlayer) {
        for(int i=0; i<nbPlayers; i++)
        {
            gainsPlayer[i] = i-1;
        }
    }
    public void printRoulette(int[] numeros, Roulette.COLOR[] couleurs) {}
    public void printInit(){}
    public int printChoiceAction(string pseudo){return -1;}
    public void setMise(Joueur player, Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int action)
    {}
    public void printInfoTurn(Dictionary<Joueur, List<(int gain, int[] numero)>> mises, int turn)
    {
        
    }
    public void printEndTurn(int[] gainsPlayers, List<Joueur>  players){}
}


public class RouletteTest
{
    ////////////////////////
    //    Constructeur    //
    ////////////////////////
    [Fact]
    public void Constructeur()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(2, aff);

        IReadOnlyList<Joueur> p = roulette.getPlayers();
        Roulette.STATE[] a = {Roulette.STATE.EN_JEU, Roulette.STATE.EN_JEU};
        int[] gains = {0, 0};
        int[] rouletteNumber = {0, 1, 2, 3, 4, 5,
                                6, 7, 8, 9, 10, 11,
                                12, 13, 14, 15, 16, 17,
                                18, 19, 20, 21, 22, 23,
                                24, 25, 26, 27, 28, 29,
                                30, 31, 32, 33, 34, 35,
                                36};
        Roulette.COLOR[] rouletteColor = 
                    {Roulette.COLOR.VERT, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, 
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, 
                    Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, 
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR,Roulette.COLOR.ROUGE, 
                    Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, 
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE,
                    Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR,
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, 
                    Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, 
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, 
                    Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR,
                    Roulette.COLOR.ROUGE, Roulette.COLOR.NOIR, Roulette.COLOR.ROUGE, 
                    Roulette.COLOR.NOIR};


        Assert.Equal(2, roulette.getNbPlayers());
        Assert.Equal(a, roulette.getActifPlayers());
        Assert.Equal(gains, roulette.getGainsPlayers());
        Assert.Equal(rouletteNumber, roulette.getRouletteNumber());
        Assert.Equal(rouletteColor, roulette.getRouletteColor());
        for(int i=0; i<2; i++)
        {
            Assert.Equal("TEST", p[1].getPseudo());
        }
    }

    ////////////////////////
    //       Joueur       //
    ////////////////////////
    [Fact]
    public void stop1()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(2, aff);
        
        Roulette.STATE[] p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.EN_JEU, p[0]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[1]);
        roulette.stop(0);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[1]);
        roulette.stop(1);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.ARRETE, p[1]);
    }

    [Fact]
    public void stop2()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(2, aff);
        
        Roulette.STATE[] p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.EN_JEU, p[0]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[1]);
        roulette.stop(1);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.EN_JEU, p[0]);
        Assert.Equal(Roulette.STATE.ARRETE, p[1]);
        roulette.stop(0);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.ARRETE, p[1]);
    }

    [Fact]
    public void onePlayerActif()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(2, aff);

        Assert.True(roulette.onePlayerActif());
        roulette.stop(0);
        Assert.True(roulette.onePlayerActif());
        roulette.stop(1);
        Assert.False(roulette.onePlayerActif());
    }

    [Fact]
    public void isEliminated()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(3, aff);

        Roulette.STATE[] p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.EN_JEU, p[0]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[1]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[2]);
        roulette.isEliminated(0);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[1]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[2]);
        roulette.isEliminated(1);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.ARRETE, p[1]);
        Assert.Equal(Roulette.STATE.EN_JEU, p[2]);
        roulette.isEliminated(2);
        p = roulette.getActifPlayers();
        Assert.Equal(Roulette.STATE.ARRETE, p[0]);
        Assert.Equal(Roulette.STATE.ARRETE, p[1]);
        Assert.Equal(Roulette.STATE.ARRETE, p[2]);
    }

    ////////////////////////
    //        Roue        //
    ////////////////////////
    [Fact]
    public void spinOneTurn()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(3, aff);

        int[] rouletteNumber = {1, 2, 3, 4, 5, 6,
                        7, 8, 9, 10, 11, 12,
                        13, 14, 15, 16, 17, 18,
                        19, 20, 21, 22, 23, 24,
                        25, 26, 27, 28, 29, 30,
                        31, 32, 33, 34, 35, 36, 
                        0};


        roulette.spinOneTurn();
        Assert.Equal(rouletteNumber, roulette.getRouletteNumber());
    }
    
    ////////////////////////
    //       Action       //
    ////////////////////////
    
    ////////////////////////
    //    Verification    //
    ////////////////////////
    [Fact]
    public void isConsecutive()
    {
        FauxAffichageRoulette aff = new FauxAffichageRoulette();
        Roulette roulette = new Roulette(3, aff);

        int[] numbers = {1,2,3};

        Assert.True(Roulette.isConsecutive(numbers, 3));

        int[] numbers2 = {1,5,3};
        Assert.False(Roulette.isConsecutive(numbers2, 3));

        
        int[] numbers3 = {1,3};
        Assert.False(Roulette.isConsecutive(numbers3, 2));

        int[] numbers4 = {20,21,22};

        Assert.True(Roulette.isConsecutive(numbers4, 3));
    }
}