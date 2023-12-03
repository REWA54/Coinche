using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VerificateurAnnonces : MonoBehaviour
{

    private List<Carte> cartesEnMain = new List<Carte>();
    public MancheManager mancheManager;
    public int ObtenirMeilleureAnnonce(List<Carte> cartesAVerifier)
    {
        Debug.Log("Annonces pour le joueur " + cartesEnMain[0].proprietaire);
        cartesEnMain = cartesAVerifier;
        if (cartesEnMain.Count < 5)
        {
            Debug.Log("Pas assez de cartes pour les annonces.");
            return 0;
        }

        int meilleureAnnonce = 0;

        // Vérifier Le Carré
        int annonceCarre = ObtenirValeurAnnonceCarre();
        if (annonceCarre > meilleureAnnonce)
            meilleureAnnonce = annonceCarre;

        // Vérifier Le Cent
        int annonceCent = ObtenirValeurAnnonceCent();
        if (annonceCent > meilleureAnnonce)
            meilleureAnnonce = annonceCent;

        // Vérifier Le Cinquante
        int annonceCinquante = ObtenirValeurAnnonceCinquante();
        if (annonceCinquante > meilleureAnnonce)
            meilleureAnnonce = annonceCinquante;

        // Vérifier La Tierce
        int annonceTierce = ObtenirValeurAnnonceTierce();
        if (annonceTierce > meilleureAnnonce)
            meilleureAnnonce = annonceTierce;

        // Vérifier La Belote
        int annonceBelote = ObtenirValeurAnnonceBelote();
            meilleureAnnonce += annonceBelote;

        foreach (Carte carte in cartesEnMain){
            cartesEnMain.Remove(carte);
        }

        return meilleureAnnonce;
    }

    int ObtenirValeurAnnonceCarre()
    {
        for (int couleur = 0; couleur < 4; couleur++)
        {
            foreach (Carte carte in cartesEnMain)
            {
                if (cartesEnMain.Count(c => c.valeur == carte.valeur && c.couleur != carte.couleur && c.couleur == couleur) == 3)
                {
                    Debug.Log("Annonce de Carré avec des " + carte.ObtenirNomValeur() + " de couleur " + carte.ObtenirNomCouleur() );
                    
                    int valeurAnnonce = carte.ValeurDansLaManche(0); // Utiliser la couleur d'atout 0 pour cette vérification
                    
                    switch (carte.valeur)
                    {
                        case 11: // Valet
                            return 200 + valeurAnnonce;
                        case 9: // Neuf
                            return 150 + valeurAnnonce;
                        case 14: // As
                        case 10: // Dix
                        case 13: // Roi
                        case 12: // Dame
                            return 100 + valeurAnnonce;
                        default:
                            Debug.LogError("Erreur : Valeur de Carré non reconnue.");
                            return 0;
                    }
                }
            }
        }
        return 0;
    }

    int ObtenirValeurAnnonceCent()
    {
        for (int couleur = 0; couleur < 4; couleur++)
        {
            for (int i = 7; i <= 10; i++)
            {
                if (cartesEnMain.Exists(c => c.valeur == i && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 1 && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 2 && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 3 && c.couleur == couleur)&&
                    cartesEnMain.Exists(c => c.valeur == i + 4 && c.couleur == couleur))
                {
                    Debug.Log("Annonce de Cinquante de couleur " + cartesEnMain[0].ObtenirNomCouleur());
                    return 100;
                }
            }
        }
        return 0;
    }

    int ObtenirValeurAnnonceCinquante()
    {
        for (int couleur = 0; couleur < 4; couleur++)
        {
            for (int i = 7; i <= 11; i++)
            {
                if (cartesEnMain.Exists(c => c.valeur == i && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 1 && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 2 && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 3 && c.couleur == couleur))
                {
                    Debug.Log("Annonce de Cinquante de couleur " + cartesEnMain[0].ObtenirNomCouleur() + " !");
                    return 50;
                }
            }
        }
        return 0;
    }

    
    int ObtenirValeurAnnonceTierce()
    {
        for (int couleur = 0; couleur < 4; couleur++)
        {
            for (int i = 7; i <= 12; i++)
            {
                if (cartesEnMain.Exists(c => c.valeur == i && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 1 && c.couleur == couleur) &&
                    cartesEnMain.Exists(c => c.valeur == i + 2 && c.couleur == couleur))
                {
                    Debug.Log("Annonce de Tierce de couleur " + cartesEnMain[0].ObtenirNomCouleur() + " !");
                    return 20;
                }
            }
        }
        return 0;
    }

    int ObtenirValeurAnnonceBelote()
    {

            if (cartesEnMain.Exists(c => c.valeur == 12 && c.couleur == mancheManager.couleurContrat) && 
                cartesEnMain.Exists(c => c.valeur == 13 && c.couleur == mancheManager.couleurContrat))
            {
                Debug.Log("Annonce de Belote de couleur " + cartesEnMain[0].ObtenirNomCouleur() + " !");
                return 20;
            }
        
        return 0;
    }
}

