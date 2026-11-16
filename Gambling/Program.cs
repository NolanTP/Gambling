using Gambling.affichage;
using Gambling.jeu;

IAffichageBlackJackDe affichage = new AffichageBlackJackDeConsole();
BlackJackDe blackJack = new BlackJackDe(4, affichage);
blackJack.run();

IAffichageRoulette affichageR = new AffichageRouletteConsole();
Roulette roulette = new Roulette(2, affichageR);
roulette.run();