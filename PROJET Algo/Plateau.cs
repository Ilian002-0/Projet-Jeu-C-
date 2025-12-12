using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace PROJET_Algo
{
    internal class Plateau
    {
        private string[,] tableau;
        private string tab_fait = "";
        private bool verif = true; //Permettra de vérifier si le fichier a bien été trouvé
        private string error_message = "Pas d'appel pour la création de matrice";
        private List<int[]> liste_coord_lettre = new List<int[]>();
        List<string> CreerListe_Lettres() //Créer une liste remplie des lettres en fonction de leur nb d'apparition (pour la méthode aléatoire)
        {
            List<string> Liste_Lettres = new List<string>();
            string[,] mat_fichier = ToRead("Lettres.txt");
            if (mat_fichier == null)
            {
                this.verif = false;
                this.error_message = "Fichier lettres inexistant ou mauvais chemin d'accès";
                return null;
            }

            int nbLignes = mat_fichier.GetLength(0); // nombre de lignes 

            for (int i = 0; i < nbLignes; i++)
            {
                string lettre = mat_fichier[i, 0]; //la lettre
                int nbApparition = Convert.ToInt32(mat_fichier[i, 1]);// nombre d'apparitions max possible de la lettre

                for (int j = 0; j < nbApparition; j++) //Ajouter la lettre autant de fois que le nombre d'apparitions
                    Liste_Lettres.Add(lettre);
            }
            return Liste_Lettres;
        }
        public string[,] ToRead(string chemin) //Lire le fichier et renvoyer une matrice
        {
            string[,] matrice_lettre = null;
            if (File.Exists(chemin))
            {
                string[] lignes = File.ReadAllLines(chemin);
                int nbLignes = lignes.Length;
                int nbColonnes = lignes[0].Split(',').Length; //Il faut absolument que toutes les lignes aies le même nombre d'éléments
                int ligne_mat = 0;
                matrice_lettre = new string[nbLignes, nbColonnes];

                foreach (string ligne in lignes)
                {
                    string[] valeurs = ligne.Split(','); //Crée un mini tableau avec les données de chaque ligne
                    for (int i = 0; i < nbColonnes; i++)
                        matrice_lettre[ligne_mat, i] = valeurs[i].ToUpper(); //On rempli la matrice avec les données en majuscule
                    ligne_mat++;
                }
                this.verif = true;
            }
            else
            { this.verif = false; this.error_message = "Fichier inexistant ou mauvais chemin d'accès"; return null; }
            return matrice_lettre;
        }
        public void ToFile(string nomfile)
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\stwx2\Downloads\" + nomfile + ".txt"))
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        writer.Write(tableau[i, j]);
                        if (j < 7) writer.Write(",");
                    }
                    writer.WriteLine();
                }
            }
        }
        public Plateau(int type, int taille=0)
        {
            if (type == 1) // type 1 : façon aléatoire
            {
                int taille_verif = taille == 0 ? 8 : taille;
                tableau = new string[taille_verif, taille_verif];
                Random rand = new Random();
                List<string> Liste_Lettres = CreerListe_Lettres();
                if (Liste_Lettres == null)
                    return;
                int longueur = Liste_Lettres.Count;
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        int index = rand.Next(0, longueur );
                        tableau[i, j] = Liste_Lettres.ElementAt(index); // Remplir chaque case avec un nombre aléatoire entre 0 et la ongueur de la liste
                        Liste_Lettres.RemoveAt(index); // Supprimer la lettre pour garder les bonnes probabilités
                        longueur--;
                    }
                }
            }
            if (type == 2) //type 2 : chercher un tableau déjà fait
            {
                this.tableau = ToRead(tab_fait);
            }
        } //Créer le plateau de jeu avec les 2 types différents
        public string toString() //Renvoie une chaîne de caractères qui décrit le plateau
        {
            string res = "";
            Console.ForegroundColor = ConsoleColor.Cyan; // Optionnel : met la grille en couleur
            res += "    ╔═══╦═══╦═══╦═══╦═══╦═══╦═══╦═══╗\n";

            for (int i = 0; i < 8; i++)
            {
                res += "    ║"; // Décalage pour centrer un peu
                for (int j = 0; j < 8; j++)
                {
                    // On récupère la lettre, ou un espace si c'est null/vide
                    string lettre = string.IsNullOrEmpty(this.tableau[i, j]) ? " " : this.tableau[i, j];

                    // Affichage de la lettre centrée
                    res += $" {lettre} ║";
                }
                res += "\n"; // Retour à la ligne

                // Si ce n'est pas la dernière ligne, on dessine le séparateur intermédiaire
                if (i < 7)
                    res += "    ╠═══╬═══╬═══╬═══╬═══╬═══╬═══╬═══╣\n";
            }

            res += "    ╚═══╩═══╩═══╩═══╩═══╩═══╩═══╩═══╝\n";
            Console.ResetColor(); // On remet la couleur par défaut
            return res;
        }
        public bool Recherche_Mot(string mot)
        {
            liste_coord_lettre.Clear();
            int ligneBase = 7;

            for (int i = 0; i < 8; i++)
            {
                if (this.tableau[ligneBase, i] == mot[0].ToString())
                {
                    if (RechercheRecursive(mot, ligneBase, i, 0, new bool[8, 8]))
                        return true;
                }
            }
            return false;
        }
        private bool RechercheRecursive(string mot, int ligne, int col, int index, bool[,] visite)
        {
            if (index == mot.Length) return true;

            if (ligne < 0 || ligne >= 8 || col < 0 || col >= 8) return false;
            if (visite[ligne, col]) return false;

            if (this.tableau[ligne, col] != mot[index].ToString()) return false;

            visite[ligne, col] = true;
            liste_coord_lettre.Add(new int[] { ligne, col });

            if (RechercheRecursive(mot, ligne, col - 1, index + 1, visite)) return true; // Recherche à gauche
            if (RechercheRecursive(mot, ligne, col + 1, index + 1, visite)) return true; // à droite
            if (RechercheRecursive(mot, ligne - 1, col, index + 1, visite)) return true; //en haut
            if (RechercheRecursive(mot, ligne - 1, col - 1, index + 1, visite)) return true; //en diagonale haut-gauche
            if (RechercheRecursive(mot, ligne - 1, col + 1, index + 1, visite)) return true; //en diagonale haut-droite

            visite[ligne, col] = false;  //Si le chemin ne fonctionne pas on revient en arrière pour en trouver un autre
            liste_coord_lettre.RemoveAt(liste_coord_lettre.Count - 1);

            return false;
        }
        // Les get pour avoir les infos
        public bool Verif
        {
            get { return this.verif; }
        }
        public string Error_Message
        {
            get { return this.error_message; }
        }
        public void Maj_Plateau()//Met le plateau à jour en faisant une animation, attention à bien gérer car il y a un Console.WriteLine()
        {
            for (int i = 0; i < liste_coord_lettre.Count; i++)
            {
                int[] coord = liste_coord_lettre[i];
                int ligne = coord[0];
                int colonne = coord[1];
                tableau[ligne, colonne] = "*";
                Console.Clear();
                Console.WriteLine(this.toString());
                System.Threading.Thread.Sleep(200);
            }
            for (int i = liste_coord_lettre.Count-1; i >= 0; i--)
            {
                int[] coord = liste_coord_lettre[i];
                Recursive_Descente_lettre(coord);
            }

            this.liste_coord_lettre.Clear();
        }
        private void Recursive_Descente_lettre(int[] coord)
        {
            int ligne = coord[0]; 
            int colonne = coord[1];
            if (ligne == 0)
                return;
            if (tableau[ligne - 1, colonne] == " " || tableau[ligne - 1, colonne] == null) //Si on tombe sur une case vide ou null on arrête
                return;
            tableau[ligne, colonne] = tableau[ligne - 1, colonne]; //On fait descendre la lettre à la ligne d'en dessous
            tableau[ligne - 1, colonne] = " "; // on écrase la lettre du dessus (qui a été mise en bas) par un espace
            Console.Clear();
            Console.WriteLine(this.toString());
            System.Threading.Thread.Sleep(100);
            int[] newCoord = new int[] { ligne - 1, colonne };
            Recursive_Descente_lettre(newCoord);
        }
        public int Calcul_Score_Mot(string mot)
        {
            int score = 0;
            string[,] matrice_score = ToRead(chemin_matrice);
            for(int i = 0; i<mot.Length; i++)
            {
                for (int j = 0; j < matrice_score.GetLength(0); j++)
                {
                    if (mot[i].ToString() == matrice_score[j, 0])
                    {
                        score += Convert.ToInt32(matrice_score[i, 2]);
                        break;
                    }
                }
            }
            return score;
        }
    }
}

