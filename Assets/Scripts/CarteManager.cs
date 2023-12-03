using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Lean.Gui;
using System.Drawing;

public class CarteManager : MonoBehaviour
{
    public Carte carte = new Carte();

    public int couleur = 4;
    public int valeur = 0;
    [SerializeField] TMP_Text UIValeur;
    [SerializeField] Sprite[] couleursImages;
    [SerializeField] Image UICouleurImage;
    RectTransform pointPlacement;

    public void InitCarte(int nouvelID, RectTransform rectTransform, Joueur proprio)
    {
       
        carte.proprietaire = proprio;
        carte.manager = this;
        pointPlacement = rectTransform;
        carte._init(nouvelID);
        UpdateUI();
    }   

    // fonction appellée quand on swipe up la carte
    public void Jouercarte()
    {
        if (!carte.proprietaire.autorisationJouer)
        {
            return;
        }
        transform.position = pointPlacement.position;
        carte.proprietaire.manager.PlacementCarte(carte);
    }

    public void UpdateUI()
    {
        PeutEtreJouee();
        UIValeur.text = carte.ObtenirNomValeur();
        //UIValeur.color = color;
        //UIValeur.color = carte.ObtenirColor();
        //UICouleur.text = couleur;
        //UICouleur.color = color;
       // Debug.Log("Carte a charger : " + carte.ObtenirNomCouleur() + " " + carte.ObtenirNomValeur());
        UICouleurImage.sprite = couleursImages[carte.couleur];
    }
    void PeutEtreJouee()
    {
        if (carte.peutEtreJouee)
        {
            UICouleurImage.color = UnityEngine.Color.white;
        }
        else { UICouleurImage.color = UnityEngine.Color.grey; }
        GetComponent<LeanSwipe>().enabled = carte.peutEtreJouee;
    }
}

   

