using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Contrat : MonoBehaviour
{
    private const int DefaultValue = 80;
    private const int CapotValue = 250;
    private const int GeneraleValue = 500;
    private const int DefaultColor = 4;

    private int ancienneValeur;
    private int ancienneCouleur;
    private int valeurChoisie = DefaultValue;
    public Joueur joueurQuiChoisi;

    [SerializeField] private TMP_Text valeurContrat;
    [SerializeField] private TMP_Text UIJoueur;
    [SerializeField] private Image[] outlineCouleurs;
    [SerializeField] private MancheManager mancheManager;
    [SerializeField] private Distributeur distributeur;
    [SerializeField] GameObject UIPanel;

    public int couleurChoisie = DefaultColor;
    public Joueur premierJoueurChoisi;
    private Joueur joueurMeneur ;
    public Joueur joueurDemande;

    private void Start()
    {
        //SetValeurDeBase();
       UpdateUI();
    }

    public void AugmenterContrat()
    {
        switch (valeurChoisie)
        {
            case 160:
                valeurChoisie = CapotValue;
                break;
            case CapotValue:
                valeurChoisie = GeneraleValue;
                break;
            case GeneraleValue:
                break;
            default:
                valeurChoisie += 10;
                break;
        }
        UpdateUI();
    }

    public void BaisserContrat()
    {
        if (valeurChoisie == ancienneValeur)
        {
            UpdateUI();
            return;
        }

        switch (valeurChoisie)
        {
            case DefaultValue:
                break;
            case CapotValue:
                valeurChoisie = 160;
                break;
            case GeneraleValue:
                valeurChoisie = CapotValue;
                break;
            default:
                valeurChoisie -= 10;
                break;
        }
        UpdateUI();
    }

    public void ChoixCouleur(int couleur)
    {
        couleurChoisie = couleur;
        UpdateUI();
    }

    public void ValiderChoix()
    {
        // Le joueur valide son choix sans le figer et passe au joueur suivant 
        ancienneCouleur = couleurChoisie;
        ancienneValeur = valeurChoisie;
        joueurMeneur = joueurDemande;
        mancheManager.ChangerParieur();
    }

    public void PasserChoix()
    {
        if (mancheManager.ProchainJoueur(joueurDemande) == premierJoueurChoisi && ancienneValeur == 0)
        {
            distributeur.Redistribuer();
            Debug.Log("redémarrer la partie");
            return;
        }

        if (joueurMeneur == mancheManager.ProchainJoueur(joueurDemande))
        {
            Debug.Log("Je fige le contrat");
            mancheManager.FigerLeContrat(valeurChoisie, couleurChoisie, joueurMeneur);
            return;
        }

        mancheManager.ChangerParieur();
    }

    public void ActiverUI(bool value)
    {
        UIPanel.SetActive(value);
    }

    public void UpdateUI()
    {
        if (joueurDemande == null)
        {
            joueurDemande = premierJoueurChoisi;
        }
        UIJoueur.text = joueurDemande.IDJoueur.ToString();

        switch (valeurChoisie)
        {
            case CapotValue:
                valeurContrat.text = "Capot";
                break;
            case GeneraleValue:
                valeurContrat.text = "Générale";
                break;
            default:
                valeurContrat.text = valeurChoisie.ToString();
                break;
        }

        foreach (var item in outlineCouleurs)
        {
            item.gameObject.SetActive(false);
        }

        if (couleurChoisie != DefaultColor)
        {
            outlineCouleurs[couleurChoisie].gameObject.SetActive(true);
        }
    }
}