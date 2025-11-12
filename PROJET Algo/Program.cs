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
            if (!plateau.Verif) 
            { 
                Console.WriteLine("Problème avec le fichier ou la création de la matrice"); 
                Console.WriteLine(plateau.Error_Message); 
            }

            Console.WriteLine(plateau.toString());
            Console.WriteLine("Saisissez un mot");
            string mot = Console.ReadLine().ToUpper();
            if(plateau.Recherche_Mot(mot))
            {
                plateau.Maj_Plateau();
                Console.WriteLine("Le mot existe youhouuu !!!");
            }
            else
                Console.WriteLine("Le mot n'existe pas");
        }
    }
}
