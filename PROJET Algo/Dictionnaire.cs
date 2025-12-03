using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PROJET_Algo
{
    internal class Dictionnaire
    {
        static string user = "Ilian"; //Augustin sinon
        private string chemin_dictionnaire = user == "Ilian" ? @"C:\Users\stwx2\Documents\ESILV\Année 2\Informatique\C#\Projet\MotsFrancais.txt" : @"C:\Users\nicol\OneDrive - DVHE\Documents\ESILV\A2\Algorithme\Projet Algo\MotsFrancais (1).txt";

        private List<string> motsDuDictionnaire;
        private bool verif = false;
        private string error_message = "Dictionnaire non initialisé";
        private string langue = "français";
        private int nbMotsTotal = 0;

        public Dictionnaire()
        {
            this.motsDuDictionnaire = new List<string>();

            if (!File.Exists(this.chemin_dictionnaire))
            {
                this.verif = false;
                this.error_message = "Fichier dictionnaire introuvable : " + this.chemin_dictionnaire;
                return;
            }
            else
                this.verif = true;

            try
            {
                if (!this.verif) return;
                    string[] lignes = File.ReadAllLines(this.chemin_dictionnaire);

                    foreach (string ligne in lignes)
                    {
                        string[] motsLigne = ligne.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string mot in motsLigne)
                        {
                        this.motsDuDictionnaire.Add(mot.ToLower()); // Ajoute en minuscules 
                        }
                    }

                    this.nbMotsTotal = this.motsDuDictionnaire.Count;

                    // Tri de la liste complète si elle n'est pas vide
                    if (this.nbMotsTotal > 0)
                    {
                        Tri_Rapide(this.motsDuDictionnaire, 0, this.nbMotsTotal - 1);
                        this.verif = true;
                    }
                    else
                    {
                        this.verif = false;
                        this.error_message = "Le dictionnaire est vide ou n'a pas pu être lu.";
                    }
            }
            catch
            {
                this.verif = false;
                this.error_message = "Erreur technique lors de la lecture du dictionnaire: ";
            }
        }
        public void Tri_Rapide(List<string> liste, int debut, int fin)
        {
            if (debut < fin)
            {
                int pivotIndex = Partition(liste, debut, fin);
                Tri_Rapide(liste, debut, pivotIndex - 1);
                Tri_Rapide(liste, pivotIndex + 1, fin);
            }
        }
        private int Partition(List<string> liste, int debut, int fin)
        {
            string pivot = liste[fin];
            int i = (debut - 1);

            for (int j = debut; j < fin; j++)
            {
                // string.Compare pour trier des chaînes alphabétiquement
                if (string.Compare(liste[j], pivot) <= 0)
                {
                    i++;
                    string temp = liste[i];
                    liste[i] = liste[j];
                    liste[j] = temp;
                }
            }

            string tempPivot = liste[i + 1];
            liste[i + 1] = liste[fin];
            liste[fin] = tempPivot;

            return i + 1;
        }
        public bool RechDichoRecursif(string mot)
        {
            if (!this.verif || string.IsNullOrEmpty(mot))
                return false;

            string motNormalise = mot.ToLower();

            return RechDichoHelper(motNormalise, 0, this.nbMotsTotal - 1); // on utilise le helper pour pouvoir faciliter la récursivité et simplifier la commande de recherhche dans le main
        }
        private bool RechDichoHelper(string motCible, int debut, int fin)
        {
            if (debut > fin)
            {
                return false; // Condition d'arrêt : le mot n'est pas trouvé
            }

            int milieu = (debut + fin) / 2;

            int comparaison = string.Compare(motCible, this.motsDuDictionnaire[milieu]);

            if (comparaison == 0)
            {
                return true; // Mot trouvé
            }
            else if (comparaison < 0)
            {
                return RechDichoHelper(motCible, debut, milieu - 1); // Cherche à gauche
            }
            else
            {
                return RechDichoHelper(motCible, milieu + 1, fin); // Cherche à droite
            }
        }
        public string toString()
        {
            if (!this.verif)
            {
                return "Erreur Dictionnaire: " + this.error_message;
            }

            string resultat = "";
            resultat += $"Dictionnaire (langue: {this.langue})\n";
            resultat += $"Nombre total de mots: {this.nbMotsTotal}\n";
            resultat += "------------------------------------\n";

            // Utilise LINQ pour grouper les mots par leur première lettre
            var motsGroupe = this.motsDuDictionnaire
                .GroupBy(m => m[0])
                .OrderBy(g => g.Key);

            foreach (var groupe in motsGroupe)
            {
                resultat += $" - Lettre '{groupe.Key}' : {groupe.Count()} mots\n";
            }

            return resultat;
        }
        public bool Verif
        {
            get { return this.verif; }
        }
        public string Error_Message
        {
            get { return this.error_message; }
        }
    }
}