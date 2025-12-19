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
        private string chemin_dictionnaire = "Mots_Français.txt";
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
                    string[] lignes = File.ReadAllLines(this.chemin_dictionnaire); // Lit toutes les lignes du fichier

                foreach (string ligne in lignes)
                    {
                        string[] motsLigne = ligne.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); // Sépare les mots par espace

                    foreach (string mot in motsLigne)
                        {
                        this.motsDuDictionnaire.Add(mot.ToLower()); // Ajoute en minuscules 
                        }
                    }

                    this.nbMotsTotal = this.motsDuDictionnaire.Count;

                    // Tri de la liste complète si elle n'est pas vide
                    if (this.nbMotsTotal > 0)
                    {
                        this.motsDuDictionnaire = TriFusion(this.motsDuDictionnaire); // Tri fusion
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
        public List<string> TriFusion(List<string> liste)
        {
            if (liste.Count <= 1)
                return liste;

            int milieu = liste.Count / 2;
            List<string> gauche = TriFusion(liste.GetRange(0, milieu));
            List<string> droite = TriFusion(liste.GetRange(milieu, liste.Count - milieu));

            return Fusionner(gauche, droite);
        }
        private List<string> Fusionner(List<string> gauche, List<string> droite) // Fusionne deux listes triées en les triant
        {
            List<string> resultat = new List<string>();
            int i = 0, j = 0;

            while (i < gauche.Count && j < droite.Count)
            {
                if (string.Compare(gauche[i], droite[j]) <= 0)
                {
                    resultat.Add(gauche[i]);
                    i++;
                }
                else
                {
                    resultat.Add(droite[j]);
                    j++;
                }
            }
            while (i < gauche.Count)
            {
                resultat.Add(gauche[i]);
                i++;
            }
            while (j < droite.Count)
            {
                resultat.Add(droite[j]);
                j++;
            }
            return resultat;
        }
        public bool RechDichoRecursif(string mot)// Recherche dichotomique récursive
        {
            if (!this.verif || string.IsNullOrEmpty(mot))
                return false;

            string motNormalise = mot.ToLower();

            return RechDichoHelper(motNormalise, 0, this.nbMotsTotal - 1); // on utilise le helper pour pouvoir faciliter la récursivité et simplifier la commande de recherhche dans le main
        }
        private bool RechDichoHelper(string motCible, int debut, int fin)// Fonction récursive de recherche dichotomique
        {
            if (debut > fin)
            {
                return false; // Condition d'arrêt : le mot n'est pas trouvé
            }

            int milieu = (debut + fin) / 2;

            int comparaison = string.Compare(motCible, this.motsDuDictionnaire[milieu]);// Compare le mot cible avec le mot au milieu

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

            var motsGroupe = this.motsDuDictionnaire.GroupBy(m => m[0]).OrderBy(g => g.Key);// Groupe les mots par leur première lettre et les trie par lettre

            foreach (var groupe in motsGroupe)
            {
                resultat += $" - Lettre '{groupe.Key}' : {groupe.Count()} mots\n";
            }

            return resultat;
        }
        // Getters
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