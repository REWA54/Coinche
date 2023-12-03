using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joueur
{
    public int IDJoueur;
    public string nom;
    public List<int> main;
    public int points;
    public Equipe equipe;
    public JoueurManager manager;
    public bool autorisationJouer = false;
    public List<Carte> cartes = new List<Carte>();

    public void _init(int newID, JoueurManager mana)
    {
        IDJoueur = newID;
        manager = mana;
    }

    public void AutorisationJouer(bool autorisation)
    {
        autorisationJouer = autorisation;
        manager.PermissionDeJouer();
    }
}
