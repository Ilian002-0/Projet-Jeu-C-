using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_Algo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Initialisation des variables :
            int type_jeu = 0;
            int duree_jeu = 0;
            int duree_tour = 0;

            Plateau plateau = new Plateau(1);
            //Création des joueurs
            Console.WriteLine("Joueur 1, entrez votre pseudo : ");
            Joueur joueur_1 = new Joueur(Convert.ToString(Console.ReadLine()));
            Console.Clear();
            Console.WriteLine("Joueur 2, entrez votre pseudo : ");
            Joueur joueur_2 = new Joueur(Convert.ToString(Console.ReadLine()));
            Console.Clear();


            //Création du jeu
                //Choix du type de plateau
            Console.WriteLine("Veuillez choisir le type de plateau :\n" +
                "1 : Générer aléatoirement le plateau\n" +
                "2 : Jouer sur des plateaux existants\n\n" +
                "Veuillez choisir un nombre entre 1 et 2 :");
            while (type_jeu!=1 && type_jeu != 2)
            {
                type_jeu = Convert.ToInt32(Console.ReadLine());
            }
            Console.Clear();

                //Choix des durées
            Console.WriteLine("Veuillez choisir la durée total du jeu en min (min : 1min | max : 5min) :");
            while (!Jeu.Verif_time(1, duree_jeu)) //Type 1 pour le temps total
            {
                duree_jeu = Convert.ToInt32(Console.ReadLine());
            }
            Console.Clear();
            Console.WriteLine("Veuillez choisir la durée de chaque tour en seconde (min : 5s | max : 60s) :");
            while (!Jeu.Verif_time(0, duree_tour)) //Type 0 pour le temps de chaque tour
            {
                duree_tour = Convert.ToInt32(Console.ReadLine());
            }
            Jeu jeu = new Jeu(joueur_1, joueur_2, plateau, duree_tour, duree_jeu);
            Console.Clear();
            Console.WriteLine("Veuillez patienter pendant la génération du jeu...");
            Dictionnaire dico = new Dictionnaire(); //tri du dictionnaire
            Console.Clear();

            //Fait en sorte d'éviter quelconque bug
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
            while (DateTime.Now< fin_jeu) //Joue au jeu durant la durée définie
            {
                jeu.Tour++;
                Joueur current_joueur = jeu.Joueur_tour(jeu.Tour);
                Console.Clear();
                Console.WriteLine($"{current_joueur.Nom} c'est à votre tour :");
                DateTime début_tour = DateTime.Now;
                while (DateTime.Now<début_tour + jeu.Durée_Tour)
                {
                    Console.WriteLine(plateau.toString());
                    Console.WriteLine("Saisissez un mot :");
                    string mot = Console.ReadLine().ToUpper();
                    if (!plateau.Recherche_Mot(mot))
                    {
                        Console.WriteLine($"Le mot n'est pas sur le plateau.");
                    }
                    else if (!dico.RechDichoRecursif(mot))
                    {
                        Console.WriteLine($"Le mot '{mot}' n'est pas dans le dictionnaire.");
                    }
                    else
                    {
                        plateau.Maj_Plateau();
                        Console.WriteLine("Le mot existe youhoouuuu");
                        current_joueur.Add_Mot(mot);
                    }
                }
            }
            //Fin du jeu 
            Console.Clear();
            Console.WriteLine("Le jeu est terminé !");
        }
    }
}