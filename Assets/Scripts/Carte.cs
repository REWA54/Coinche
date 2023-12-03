using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carte
{
    public int carteID;
    public int couleur;
    public int valeur;
    public Joueur proprietaire;
    public bool peutEtreJouee;
    public CarteManager manager;
    public bool aEteJouee= false;

    public void _init(int ID)
    {
        carteID = ID;
        // V�rifier que le num�ro de la carte est dans la plage valide
        if (ID < 1 || ID > 32)
        {
            Debug.LogError("Numéro de carte invalide. Assurez-vous que le num�ro est entre 1 et 32. L'ID de cette carte est " + ID);
        }

        // D�terminer la couleur et la valeur de la carte en fonction de son num�ro
        int valeurCarte = (ID - 1) % 8 + 7;
        int couleurCarte = (ID - 1) / 8;

        couleur = couleurCarte;
        valeur = valeurCarte;

        manager.couleur = couleur;
        manager.valeur = valeur;

    }

    public void RetirerCarte()
    {
        if(aEteJouee)
        {
        proprietaire.cartes.Remove(this);
        manager.gameObject.SetActive(false);            
        }           
    }


    public int ValeurDansLaManche(int couleurAtout)
    {
        int[] ordreSansAtout = {7,8,9,11,12,13,10,14};
        int[] ordreAvecAtout = {7,8,12,13,10,14,9,11};

        int[] ordreActuel = couleurAtout == couleur ? ordreAvecAtout : ordreSansAtout;

        for (int i = 0; i < ordreActuel.Length; i++)
        {
            if (valeur == ordreActuel[i])
            {
                return i + 1; 
            }
        }
        Debug.LogError("Erreur : La valeur de base de la carte n'est pas valide.");
        return -1;
    }

    public string ObtenirNomComplet(){
        return ObtenirNomValeur() + " de " + ObtenirNomCouleur();
    
    }

    public string ObtenirNomCouleur()
    {
        switch (couleur)
        {
            case 0:

                return "Pique";
            case 1:

                return "Coeur";
            case 2:

                return "Trèfle";
            case 3:
                return "Carreau";
            default:
                return "Inconnu";
        }
    }

    public string ObtenirNomValeur()
    {
        switch (valeur)
        {
            case 14:
                return "A";
            case 11:
                return "V";
            case 12:
                return "D";
            case 13:
                return "R";
            default:
                return valeur.ToString();
        }
    }

    public void Autoriser(bool autorisation)
    {
        peutEtreJouee= autorisation;
        manager.UpdateUI();
    }
        
    }

