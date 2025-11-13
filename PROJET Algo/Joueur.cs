using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_Algo
{
    internal class Joueur
    {
        private string nom_joueur = "";
        private List<string> mots_trouvés = null;
        private int score = 0;

        public Joueur(string nom_joueur)
        {
            this.nom_joueur = char.ToUpper(nom_joueur[0])+nom_joueur.Substring(1);
            this.mots_trouvés = null;
            this.score = 0;
        }
        public bool Verif_nom(string nom) //Verification si le nom est correcte
        {
            if (string.IsNullOrEmpty(nom))
                return false;
            return true;
        }
        public void Add_Mot(string mot)
        {
            this.Add_Mot(mot);
        }
        public void Add_Score(int val)
        {
            this.score += val;
        }
        public bool Contient(string mot)
        {
            for(int i = 0; i<this.mots_trouvés.Count; i++)
            {
                if(mot== mots_trouvés[i])
                    return true;
            }
            return false;
        }
        public string toString()
        {
            string liste_mot = "";
            for (int i = 0; i < this.mots_trouvés.Count; i++)
            {
                liste_mot += this.mots_trouvés[i];
                if(i!= this.mots_trouvés.Count-1)
                    liste_mot+=", ";
            }
            return $"Nom : {this.nom_joueur}\nMots trouvés : {liste_mot}\nScore : {this.score}";
        }
    }
}
