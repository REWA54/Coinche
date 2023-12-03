using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class JoueurManager : MonoBehaviour
{
    public Joueur joueur = new Joueur();
    //[HideInInspector]
    public int IDJoueur;
    //[HideInInspector]
    public  MancheManager mancheManager;
    private List<GameObject> cartesDevantJoueurGO = new List<GameObject>();
    public int equipeID;
    [SerializeField] GameObject cartePrefab;
    [SerializeField] GameObject emplacementMain;
    [SerializeField] RectTransform pointPlacement;
    [SerializeField] TMP_Text UIValeurContrat;
    [SerializeField] TMP_Text UICouleurContrat;

    private void Awake()
    {
        joueur._init(IDJoueur, this);
    }

    private void Start()
    {
        AfficherContrat(0, 0, false);
    }
    public void InstancierCartesDevantJoueur()
    {
        // Position initiale devant le joueur
        Vector3 positionInitiale = transform.position;

        foreach (int carte in joueur.main)
        {

            // Instancie le prefab "Carte"
            GameObject nouvelleCarte = Instantiate(cartePrefab, Vector3.zero, Quaternion.identity);
            CarteManager CarteNouvelleCarte = nouvelleCarte.GetComponent<CarteManager>();
            CarteNouvelleCarte.InitCarte(carte, pointPlacement, joueur);
            nouvelleCarte.transform.SetParent(emplacementMain.transform);

            // Ajouter la carte aux listes
            cartesDevantJoueurGO.Add(nouvelleCarte);
            joueur.cartes.Add(CarteNouvelleCarte.carte);
        }
    }
    // Start is called before the first frame update
    public void PlacementCarte(Carte carte)
    {
        carte.aEteJouee = true;
        mancheManager.EnregistrerCarteJouee(carte);
    }
    public void AfficherContrat(int valeur, int couleur, bool afficher)
    {
        UIValeurContrat.gameObject.SetActive(afficher);
        UICouleurContrat.gameObject.SetActive(afficher);
        UIValeurContrat.text = valeur.ToString();
        UICouleurContrat.text = couleur.ToString();
    }
    public void ViderMain()
    {
        foreach (GameObject carte in cartesDevantJoueurGO)
        {
            Destroy(carte);
        }
        cartesDevantJoueurGO.Clear();
        //cartesEnMain.Clear();
        joueur.main.Clear();
    }

    public void RetirerCarteJouee(){
       for(int i=0;i< joueur.cartes.Count;i++){
            if  (joueur.cartes[i].aEteJouee){
                joueur.cartes[i].RetirerCarte();
                i--;
            }
       }
    }

    public void PermissionDeJouer()
    {
        // Par défaut on autorise aucune carte
        foreach (Carte carte_ in joueur.cartes)
        {
           carte_.Autoriser(false);
        }
        // Si c'est pas le tour du joueur, il ne peut pas jouer
        if (!joueur.autorisationJouer)
        {            
            return;
        }
        Debug.Log("Je suis le joueur " + joueur.IDJoueur + " et je suis autorisé à jouer, J'ai en main cartes :" + joueur.cartes.Count);

        // initialisation des variables
        Carte carteCouleurMaitre = mancheManager.carteMaitreCouleur;
        Carte carteAtoutMaitre = mancheManager.carteMaitreAtout;
        int couleurAtout = mancheManager.couleurContrat;
        int compteurCarteCouleurPossibles = 0;

        // Si personne n'a joué, on autorise toutes les cartes
        if (carteCouleurMaitre == null)
        {
            foreach (Carte carte_ in joueur.cartes)
            {
                carte_.Autoriser(true);
                compteurCarteCouleurPossibles++;
            }
            return;
        }
        

        // On peut jouer les cartes de la même couleur que la carte maitre de couleur
        foreach (Carte carte_ in joueur.cartes)
        {
            if (carte_.couleur == carteCouleurMaitre.couleur)
            {
                carte_.Autoriser(true);
                compteurCarteCouleurPossibles++;
            }
        }
        

        if (compteurCarteCouleurPossibles > 0)
        {
            return;
        }
        Debug.Log(compteurCarteCouleurPossibles);

        //On doit couper au dessus de l'atout maitre mais on peut pisser si c'est le collegue qui tient et qu'on a pas la couleur
        foreach (Carte carte_ in joueur.cartes)
        {
            if (carte_.couleur == couleurAtout && carteAtoutMaitre == null || joueur.equipe == mancheManager.Meneur().equipe)
            {
                carte_.Autoriser(true);
                compteurCarteCouleurPossibles++;
            }
            else if (carte_.couleur == couleurAtout && carteAtoutMaitre.valeur <= carte_.ValeurDansLaManche(couleurAtout))
            {
                carte_.Autoriser(true);
                compteurCarteCouleurPossibles++;
            }
        }
        Debug.Log("Je peux tout jouer");
        if (compteurCarteCouleurPossibles > 0) {return;}
        // On peut jouer toutes nos cartes si on rentre dans aucun des cas au dessus
        foreach (Carte carte_ in joueur.cartes)
        {
            carte_.Autoriser(true);
        }
        
    }
}

