using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipe
{
    public int IDEquipe;
    public List<Joueur> joueurs =new List<Joueur>();
    public int pointsAnnonces; 
    public bool aPrisLeContrat;

    public void _init(int IDEquipe)
    {
        this.IDEquipe = IDEquipe;
    }

    public void AjouterJoueur(Joueur joueur)
    {
        joueurs.Add(joueur);
        joueur.equipe = this;
    }

}
