using Gambling.affichage;
using Gambling.jeu;
using Gambling.models;

namespace GamblingTests.Tests;

public class FauxAffichage : IAffichageBlackJackDe
{
    public void usingBonus(int nbPlayers, List<Joueur> players, int[] bonusPlayer) {}
    public int chooseAction(BlackJackDe.STATE[] actifPlayer, int playersID) {return 1;}
    public void printRoll(int roll){}
    public void printUseBonus(){}
    public void printStop(){}
    public string setPseudo(){return "PSEUDO_TEST";}
    public void setBonus(int nbPlayers, List<Joueur>players, int[] bonusPlayer){}
    public void printRecapPartie(int objectifScore, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer){}
    public void printInit(){}
    public void printInfoTour(int numeroTour, string pseudo, int score, int objectif){}
    public void printClassement(BlackJackDe.STATE[] actifPlayer, int nbPlayers, int[] classement, List<Joueur> players, int[] scoresPlayer){}
    public void printChoiceAction(){}
    public void printAnimationLancer(){}
}

// Probleme pour tester : comment contourner le fait que les scores sont gérés exclusivement en interne...

public class BlackJackTests
{
    ////////////////////////
    //    Constructeur    //
    ////////////////////////
    [Fact]
    public void Constructeur_Avec_Objectif()
    {
        IAffichageBlackJackDe aff = new FauxAffichage();
        BlackJackDe jeu = new BlackJackDe(2, 30, aff);

        // Attendu
        BlackJackDe.STATE[] actif = {BlackJackDe.STATE.EN_JEU,BlackJackDe.STATE.EN_JEU};
        int[] bonus = {0, 0};
        int[] scores = {0, 0};
        int[] classement = {0, 1};
        IReadOnlyList<Joueur> p = jeu.getPlayers();

        // Tests que tous les paramètres sont bons
        Assert.Equal(2, jeu.getNbPlayers());
        Assert.Equal(actif, jeu.getActifPlayer());
        Assert.Equal(bonus, jeu.getBonusPlayer());
        Assert.Equal(scores, jeu.getScoresPlayers());
        Assert.Equal(classement, jeu.getClassement());
        Assert.Equal(30, jeu.getObjectif());
        for(int i=0; i<2; i++)
        {
            Assert.Equal("PSEUDO_TEST", p[1].getPseudo());
        }
    }

    [Fact]
    public void Constructeur_Sans_Objectif()
    {
        IAffichageBlackJackDe aff = new FauxAffichage();
        BlackJackDe jeu = new BlackJackDe(2, aff);

        // Attendu
        BlackJackDe.STATE[] actif = {BlackJackDe.STATE.EN_JEU, BlackJackDe.STATE.EN_JEU};
        int[] bonus = {0, 0};
        int[] scores = {0, 0};
        int[] classement = {0, 1};
        IReadOnlyList<Joueur> p = jeu.getPlayers();

        // Tests que tous les paramètres sont bons
        Assert.Equal(2, jeu.getNbPlayers());
        Assert.Equal(actif, jeu.getActifPlayer());
        Assert.Equal(bonus, jeu.getBonusPlayer());
        Assert.Equal(scores, jeu.getScoresPlayers());
        Assert.Equal(classement, jeu.getClassement());
        Assert.Equal(21, jeu.getObjectif());
        for(int i=0; i<2; i++)
        {
            Assert.Equal("PSEUDO_TEST", p[1].getPseudo());
        }
    }


    ////////////////////////
    //       Statut       //
    ////////////////////////
    [Fact]
    public void Stop()
    {
        IAffichageBlackJackDe aff = new FauxAffichage();
        BlackJackDe jeu = new BlackJackDe(2, aff);

        BlackJackDe.STATE[] p = jeu.getActifPlayer();
        Assert.Equal(BlackJackDe.STATE.EN_JEU, p[0]);
        Assert.Equal(BlackJackDe.STATE.EN_JEU, p[1]);
        jeu.stop(0);
        p = jeu.getActifPlayer();
        Assert.Equal(BlackJackDe.STATE.ARRETE, p[0]);
        Assert.Equal(BlackJackDe.STATE.EN_JEU, p[1]);
    }

    [Fact]
    public void One_Player_Actif()
    {
        IAffichageBlackJackDe aff = new FauxAffichage();
        BlackJackDe jeu = new BlackJackDe(2, aff);

        Assert.True(jeu.onePlayerActif());
        jeu.stop(0);
        Assert.True(jeu.onePlayerActif());
        jeu.stop(1);
        Assert.False(jeu.onePlayerActif());
    }


    ////////////////////////
    //       Joueur       //
    ////////////////////////
    [Fact]
    public void Next_Player()
    {
        IAffichageBlackJackDe aff = new FauxAffichage();
        BlackJackDe jeu = new BlackJackDe(3, aff);
        
        Assert.Equal(1, jeu.nextPlayer(0));
        jeu.stop(1);
        Assert.Equal(2, jeu.nextPlayer(0));
        jeu.stop(2);
        Assert.Equal(0, jeu.nextPlayer(0));
        jeu.stop(0);
        Assert.Equal(-1, jeu.nextPlayer(0));
    }


    ////////////////////////
    //        Trie        //
    ////////////////////////
}
