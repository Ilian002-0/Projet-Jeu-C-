using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_Algo
{
    internal class Program
    {
        static void Menu()
        {
            while (true)
            {
                int type_jeu_1 = 0;
                AfficherEcranAccueil();
                Console.WriteLine("Que voulez-vous faire :\n" +
                        "1 : Jouer à un nouveau jeu\n" +
                        "2 : Tester les fonctions\n" +
                        "3 : Quitter\n" +
                        "Veuillez choisir un nombre entre 1 et 3 :");
                while (type_jeu_1 != 1 && type_jeu_1 != 2 && type_jeu_1 != 3)
                {
                    string a = Console.ReadLine();
                    int.TryParse(a, out type_jeu_1);
                }
                Console.Clear();
                if (type_jeu_1 == 1)
                {
                    Game();
                }
                else if (type_jeu_1 == 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Merci d'avoir joué aux Mots Glissés ! À bientôt !\n\n");
                    break;
                }
                else
                {
                    TestsUnitaires();
                }
            }
        }
        static void Game()
        {
            bool jeu_en_cours = true;

            while (jeu_en_cours)
            {
                //Initialisation des variables :
                int type_jeu = 0;
                int duree_jeu = 0;
                int duree_tour = 0;
                int taille_plateau = 0;

                //Création des joueurs
                Console.WriteLine("Joueur 1, entrez votre pseudo : ");
                Joueur joueur_1 = new Joueur(Convert.ToString(Console.ReadLine()));
                Console.Clear();
                Console.WriteLine("Joueur 2, entrez votre pseudo : ");
                Joueur joueur_2 = new Joueur(Convert.ToString(Console.ReadLine()));
                Console.Clear();

                //Création du jeu
                Console.WriteLine("Veuillez choisir le type de plateau :\n" +
                    "1 : Générer aléatoirement le plateau\n" +
                    "2 : Jouer sur des plateaux existants\n" +
                    "3 : Arrêter le jeu\n\n" +
                    "Veuillez choisir un nombre entre 1 et 3 :");
                while (type_jeu != 1 && type_jeu != 2 && type_jeu != 3)
                {
                    string a = Console.ReadLine();
                    int.TryParse(a, out type_jeu);
                }
                Console.Clear();

                if (type_jeu == 1)
                {
                    Console.WriteLine("Veuillez choisir la taille du plateau (3 à 15) :");
                    while (taille_plateau < 3 || taille_plateau > 15)
                    {
                        string a = Console.ReadLine();
                        int.TryParse(a, out taille_plateau);
                    }
                    Console.Clear();
                }
                else if (type_jeu == 3)
                    break;

                Plateau plateau = new Plateau(type_jeu, taille_plateau);
                if (!plateau.Verif)
                {
                    Console.WriteLine(plateau.Error_Message);
                    return;
                }

                //Choix des durées
                Console.WriteLine("Veuillez choisir la durée total du jeu en min (min : 1min | max : 5min) :");
                while (!Jeu.Verif_time(1, duree_jeu))
                {
                    string a = Console.ReadLine();
                    int.TryParse(a, out duree_jeu);
                }
                Console.Clear();
                Console.WriteLine("Veuillez choisir la durée de chaque tour en seconde (min : 5s | max : 60s) :");
                while (!Jeu.Verif_time(0, duree_tour))
                {
                    string a = Console.ReadLine();
                    int.TryParse(a, out duree_tour);
                }
                Jeu jeu = new Jeu(joueur_1, joueur_2, plateau, duree_tour, duree_jeu);
                Console.Clear();
                Console.WriteLine("Veuillez patienter pendant la génération du jeu...");
                Dictionnaire dico = new Dictionnaire();
                Console.Clear();

                if (!plateau.Verif)
                {
                    Console.WriteLine("Problème avec le fichier ou la création de la matrice");
                    Console.WriteLine(plateau.Error_Message);
                    Console.ReadKey();
                    return;
                }

                if (!dico.Verif)
                {
                    Console.WriteLine("Problème avec le fichier ou la création du dictionnaire");
                    Console.WriteLine(dico.Error_Message);
                    Console.ReadKey();
                    return;
                }

                //Début du jeu
                DateTime début_jeu = DateTime.Now;
                DateTime fin_jeu = début_jeu + jeu.Durée_Jeu;

                // Variable pour arrêter tout le jeu si plateau vide
                bool partieTerminee = false;

                while (DateTime.Now < fin_jeu && !partieTerminee)
                {
                    jeu.Tour++;
                    Joueur current_joueur = jeu.Joueur_tour(jeu.Tour);
                    Console.Clear();
                    DateTime début_tour = DateTime.Now;
                    if (début_tour + jeu.Durée_Tour > fin_jeu)
                    {
                        début_tour = fin_jeu - jeu.Durée_Tour;
                    }

                    while (DateTime.Now < début_tour + jeu.Durée_Tour && !partieTerminee)
                    {
                        Console.WriteLine($"{current_joueur.Nom} c'est à votre tour :");
                        Console.WriteLine(plateau.toString());
                        Console.WriteLine(current_joueur.Message);
                        Console.WriteLine("Saisissez un mot :");
                        string mot = Console.ReadLine().ToUpper();

                        if (current_joueur.Contient(mot))
                        {
                            current_joueur.Message += $"Vous avez déjà trouvé ce mot.\n";
                            Console.Clear();
                        }
                        else if (!dico.RechDichoRecursif(mot))
                        {
                            current_joueur.Message += $"Le mot '{mot}' n'est pas dans le dictionnaire.\n";
                            Console.Clear();
                        }
                        else if (!plateau.Recherche_Mot(mot))
                        {
                            current_joueur.Message += $"Le mot '{mot}' n'est pas sur le plateau.\n";
                            Console.Clear();
                        }
                        else
                        {
                            current_joueur.Add_Mot(mot);
                            int score = plateau.Calcul_Score_Mot(mot);
                            current_joueur.Add_Score(score);
                            current_joueur.Message += $"'{mot}' existe youhou : +{score} points\n";
                            Console.Clear();

                            if (plateau.Est_Vide())
                            {
                                Console.Clear();
                                Console.WriteLine(plateau.toString());
                                Console.WriteLine("\n\nLE PLATEAU EST VIDE ! FIN DE LA PARTIE !");
                                System.Threading.Thread.Sleep(3000);
                                partieTerminee = true;
                                break;
                            }
                        }
                    }
                }

                //Fin du jeu 
                Console.Clear();
                Console.WriteLine("Le jeu est terminé !");
                int count = 0;
                while (count < 4)
                {
                    Console.Clear();
                    Console.Write("Décompte des points");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    System.Threading.Thread.Sleep(200);
                    Console.Write(".");
                    System.Threading.Thread.Sleep(200);
                    count++;
                }
                Console.Clear();
                Console.WriteLine(joueur_1.toString());
                Console.WriteLine();
                Console.WriteLine(joueur_2.toString());

                string choix_jeu = "0";
                Console.WriteLine("Souhaitez-vous rejouer ? (1: Oui, 2: Non)");
                while (choix_jeu != "1" && choix_jeu != "2")
                {
                    choix_jeu = Console.ReadLine();
                }
                if (choix_jeu == "2")
                {
                    jeu_en_cours = false;
                    break;
                }
                Console.Clear();
            }
        }
        static void AfficherEcranAccueil()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine(@"
███╗   ███╗ ██████╗ ████████╗███████╗     ██████╗ ██╗     ██╗███████╗███████╗███████╗███████╗
████╗ ████║██╔═══██╗╚══██╔══╝██╔════╝    ██╔════╝ ██║     ██║██╔════╝██╔════╝██╔════╝██╔════╝
██╔████╔██║██║   ██║   ██║   ███████╗    ██║  ███╗██║     ██║███████╗███████╗█████╗  ███████╗
██║╚██╔╝██║██║   ██║   ██║   ╚════██║    ██║   ██║██║     ██║╚════██║╚════██║██╔══╝  ╚════██║
██║ ╚═╝ ██║╚██████╔╝   ██║   ███████║    ╚██████╔╝███████╗██║███████║███████║███████╗███████║
╚═╝     ╚═╝ ╚═════╝    ╚═╝   ╚══════╝     ╚═════╝ ╚══════╝╚═╝╚══════╝╚══════╝╚══════╝╚══════╝
                                                                                             
    ");

            Console.ResetColor();
            Console.WriteLine("\n\tBienvenue dans le jeu des Mots Glissés !");
            Console.WriteLine("\tProjet Algorithmique & Programmation - 2025");
            Console.WriteLine("\n\t---------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\tAppuyez sur une touche pour commencer...");
            Console.ResetColor();

            Console.ReadKey();
            Console.Clear();
        }
        static void TestsUnitaires()
        {
            Console.WriteLine("=== Lancement des Tests Unitaires ===\n");
            int testsReussis = 0;
            int testsTotal = 0;

            // TEST 1 : Vérification du temps
            testsTotal++;
            Console.Write("Test 1 : Jeu.Verif_time (Tour 30s)... ");
            bool resTime1 = Jeu.Verif_time(0, 30);
            bool resTime2 = Jeu.Verif_time(0, 2);
            if (resTime1 && !resTime2)
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("OK"); testsReussis++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("ECHEC");
            }
            Console.ResetColor();

            // TEST 2 : Vérification du nom
            testsTotal++;
            Console.Write("Test 2 : Joueur.Verif_nom... ");
            Joueur jTest = new Joueur("Testeur");
            if (jTest.Verif_nom("Toto") && !jTest.Verif_nom(""))
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("OK"); testsReussis++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("ECHEC");
            }
            Console.ResetColor();

            // TEST 3 : Gestion des mots
            testsTotal++;
            Console.Write("Test 3 : Joueur.Add_Mot & Contient... ");
            jTest.Add_Mot("TEST");
            if (jTest.Contient("TEST") && !jTest.Contient("RATÉ"))
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("OK"); testsReussis++;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("ECHEC");
            }
            Console.ResetColor();

            // TEST 4 : Calcul du score
            testsTotal++;
            Console.Write("Test 4 : Plateau.Calcul_Score_Mot (Mot 'LE')... ");
            Plateau pTest = new Plateau(1, 8);
            if (pTest.Verif)
            {
                int score = pTest.Calcul_Score_Mot("LE");
                if (score > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"OK (Score: {score})"); testsReussis++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"ECHEC (Score: {score})");
                }
            }
            else
            {
                Console.WriteLine("IGNORÉ (Problème fichier Lettres.txt)");
            }
            Console.ResetColor();

            // TEST 5 : Recherche Dictionnaire
            testsTotal++;
            Console.Write("Test 5 : Dictionnaire.RechDichoRecursif... ");
            Dictionnaire dTest = new Dictionnaire();
            if (dTest.Verif)
            {
                bool motExiste = dTest.RechDichoRecursif("le");
                bool motInconnu = dTest.RechDichoRecursif("xyzxyz");
                if (motExiste && !motInconnu)
                {
                    Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine("OK"); testsReussis++;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine("ECHEC");
                }
            }
            else
            {
                Console.WriteLine("IGNORÉ (Fichier Mots_Français.txt introuvable)");
            }
            Console.ResetColor();

            Console.WriteLine($"\nRésultat final : {testsReussis}/{testsTotal} tests réussis.");
            Console.WriteLine("Appuyez sur une touche pour revenir au menu...");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            Menu();
        }
    }
}