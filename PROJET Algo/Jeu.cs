using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PROJET_Algo
{
    internal class Jeu
    {
        private Joueur joueur1=null;
        private Joueur joueur2=null;
        private Plateau current_plateau=null;
        private TimeSpan duree_tour = TimeSpan.Zero;
        private TimeSpan duree_jeu = TimeSpan.Zero;
        private int tour = 0;

        public Jeu(Joueur joueur1, Joueur joueur2, Plateau current_plateau, int duree_tour, int duree_jeu)
        {
            this.joueur1 = joueur1;
            this.joueur2 = joueur2;
            this.duree_tour = new TimeSpan(0,0,duree_tour);
            this.duree_jeu = new TimeSpan(0,0, duree_jeu);
        }
        public static bool Verif_time(int type, int duree) //cas 0 : duree_tour(max1min min 5sec)| case 1 : duree jeu(min1min max5min)
        {
            switch (type)
            {
                case 0:
                    {
                        if (duree < 5 || duree > 60)
                            return false;
                        break;
                    }
                case 1:
                    {
                        if (duree*60 < 60 || duree*60 > 350)
                            return false;
                        break;
                    }
                default: return false;
            }
            return true;
        }
        public Joueur Joueur_tour(int tour)
        {
            if(tour%2==0) //Tour paire c'est au joueur 2
                return this.joueur2 ;
            else //Tour impaire c'est au joueur 1
                return this.joueur1;
        }
        public int Tour
        {
            get { return this.tour; }
            set { this.tour = value; }
        }
        public TimeSpan Durée_Tour
        {
            get { return this.duree_tour; }
        }
        public TimeSpan Durée_Jeu
        {
            get { return this.duree_jeu; }
        }
    }
}
