using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class MancheManager : MonoBehaviour
{
    int valeurContrat;
    public int couleurContrat;

    public VerificateurAnnonces verificateurAnnonces;
    public List<JoueurManager> joueurs = new List<JoueurManager>();
    [SerializeField] Image tourUI;
    [SerializeField] Contrat contrat;
    List<Carte> CartesSurTable = new List<Carte>();
    List<Equipe> equipes = new List<Equipe>();
    Joueur joueurQuiDoitJouer;
    public Carte carteMaitreCouleur;
    public Carte carteMaitreAtout;

    private void Awake()
    {
        verificateurAnnonces = GetComponent<VerificateurAnnonces>();
        verificateurAnnonces.mancheManager = this;

        int i = 0;        
        foreach (JoueurManager joueur in joueurs)
        {
            joueur.mancheManager = this;
            joueur.IDJoueur = i;
            joueur.joueur._init(i,joueur);
            i++;
        }
        LancerNouvelleManche();
        
    }
    void Start(){
        
        CreationEquipe(1);
        CreationEquipe(2);
        //contrat.premierJoueurChoisi = ChoixJoueurHasard();
    }

    public void LancerNouvelleManche()
    {
        carteMaitreAtout = null;
        carteMaitreCouleur = null;

        if (joueurQuiDoitJouer == null)
        {
            joueurQuiDoitJouer = ChoixJoueurHasard();
        }
        /* else
        {
            joueurQuiDoitJouer = ProchainJoueur(contrat.premierJoueurChoisi);
        } */
        contrat.premierJoueurChoisi = joueurQuiDoitJouer;
        TournerFlecheVers(joueurQuiDoitJouer);
    }

    void TournerFlecheVers(Joueur joueur)
    {
        tourUI.transform.rotation = Quaternion.Euler(0, 0, -90 * joueur.IDJoueur);
    } 

    public void CreationEquipe(int equipeID){
        Equipe nouvelleEquipe = new Equipe();
        nouvelleEquipe._init(equipeID);
        switch(equipeID)
          {
            case 1 :
            nouvelleEquipe.AjouterJoueur(joueurs[0].joueur);
            nouvelleEquipe.AjouterJoueur(joueurs[2].joueur);
            break;
            case 2 :
            nouvelleEquipe.AjouterJoueur(joueurs[1].joueur);
            nouvelleEquipe.AjouterJoueur(joueurs[3].joueur);
            break;
          }

        equipes.Add(nouvelleEquipe);
        
    }

    void CalculerLesAnnonces(){
        foreach (JoueurManager joueurM in joueurs){
            joueurM.joueur.equipe.pointsAnnonces += verificateurAnnonces.ObtenirMeilleureAnnonce(joueurM.joueur.cartes);
        }
    }


    public void ChangerParieur()
    {
        ChangerLeJoueurQuiDoitJouer();
        contrat.joueurDemande = joueurQuiDoitJouer;
        contrat.UpdateUI();
    }

    public void ChangerLeJoueurQuiDoitJouer()
    {
        joueurQuiDoitJouer = ProchainJoueur(joueurQuiDoitJouer);
        TournerFlecheVers(joueurQuiDoitJouer); 
    }    

    public void EnregistrerCarteJouee(Carte carte)
    {
        if (carteMaitreCouleur == null || 
        carte.couleur == carteMaitreCouleur.couleur && 
        carte.ValeurDansLaManche(couleurContrat) > carteMaitreCouleur.ValeurDansLaManche(couleurContrat))
        {
            Debug.Log("nouveau meneur de la couleur");
            carteMaitreCouleur = carte;
        }

        if (carte.couleur == couleurContrat && carteMaitreAtout == null || 
        carte.couleur == couleurContrat && 
        carte.ValeurDansLaManche(couleurContrat) > carteMaitreAtout.ValeurDansLaManche(couleurContrat))
        {
            Debug.Log("nouveau meneur à l'atout");
            carteMaitreAtout = carte;
        }
        
        CartesSurTable.Add(carte);
        if (CartesSurTable.Count == 4)
        {
            DesignerGagnantManche();
            foreach(Carte carte_ in CartesSurTable)
            {
                CartesSurTable.Remove(carte_);
            }
        }

        Debug.Log("Carte jouée : " + carte.ObtenirNomValeur() + " " + carte.ObtenirNomCouleur() + " Valeur dans la manche : " + carte.ValeurDansLaManche(couleurContrat) );

        ChangerLeJoueurQuiDoitJouer();
        AutorisationJoueur(joueurQuiDoitJouer);               
    }
    void DesignerGagnantManche()
    {
        Debug.Log("Manche terminée, le gagnant de la manche est le joueur " + Meneur().IDJoueur);
        joueurQuiDoitJouer = Meneur();
        foreach (JoueurManager joueurM in joueurs)
        {
            joueurM.RetirerCarteJouee();
        }        
        LancerNouvelleManche();
        AutorisationJoueur(joueurQuiDoitJouer);
    }

    public Joueur Meneur()
    {
        if (carteMaitreAtout != null)
        {
            Debug.Log("Le Meneur à l'atout est : " + carteMaitreAtout.proprietaire.IDJoueur);
            return carteMaitreAtout.proprietaire;
        }
        else if (carteMaitreCouleur != null)
        {
            Debug.Log("Le Meneur à la couleur est : " + carteMaitreCouleur.proprietaire.IDJoueur);
            return carteMaitreCouleur.proprietaire;
        }
        return joueurQuiDoitJouer;
    }

    

    Joueur ChoixJoueurHasard()
    {
        int choisi = Mathf.RoundToInt(Random.Range(0, 3));
        joueurQuiDoitJouer= joueurs[choisi].joueur;
        contrat.premierJoueurChoisi = joueurQuiDoitJouer;
        return joueurs[choisi].joueur;
    }

    public void FigerLeContrat(int valeur, int couleur, Joueur parieur)
    {
        Debug.Log("Contrat établi, valeur : " + valeur + " couleur : " + couleur + " pour le joueur " + parieur.IDJoueur);
        valeurContrat = valeur;
        couleurContrat= couleur;
        
        joueurs[parieur.IDJoueur].AfficherContrat(valeur, couleur, true);
        contrat.ActiverUI(false);
        joueurQuiDoitJouer = contrat.premierJoueurChoisi;
        AutorisationJoueur(contrat.premierJoueurChoisi);
        CalculerLesAnnonces();
        foreach (Equipe equipe in equipes){
            equipe.aPrisLeContrat = false;
            Debug.Log("L'équipe " + equipe.IDEquipe + " a " + equipe.pointsAnnonces + " points d'annonce");
        }
        parieur.equipe.aPrisLeContrat =true;
    }

    void AutorisationJoueur(Joueur joueur)
    {
        TournerFlecheVers(joueur);
        foreach (var j in joueurs)
        {
            j.joueur.AutorisationJouer(false);
        }
        joueurs[joueur.IDJoueur].joueur.AutorisationJouer(true);
    }

    public Joueur ProchainJoueur(Joueur joueur)
    {
        int nextPlayerID = (joueur.IDJoueur + 1) % joueurs.Count;
        return joueurs[nextPlayerID].joueur;
    }
}
