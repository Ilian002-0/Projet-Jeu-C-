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
            Plateau plateau = new Plateau(1);
            Console.WriteLine("Génération et trie du dictionnaire...");
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

            Console.WriteLine(plateau.toString());
            Console.WriteLine("Saisissez un mot");
            string mot = Console.ReadLine().ToUpper();


            if (!plateau.Recherche_Mot(mot))
            {
                 Console.WriteLine($"Le mot '{mot}' est français, mais n'est pas sur le plateau.");
            }
            else if (!dico.RechDichoRecursif(mot))
            {
                Console.WriteLine($"Le mot '{mot}' n'est pas dans le dictionnaire.");
            }
            else
            {
                plateau.Maj_Plateau();
                Console.WriteLine("Le mot existe youhoouuuu");
            }


                Console.ReadKey();
        }
    }
}