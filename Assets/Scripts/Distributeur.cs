using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributeur : MonoBehaviour
{

    // Liste représentant le paquet de cartes
    private List<int> paquet;
    
    // Nombre de joueurs
    private int nombreJoueurs = 4;
    List<JoueurManager> joueursmanagers = new List<JoueurManager>();
    [SerializeField] MancheManager mancheManager;
    // Liste représentant les mains des joueurs
    private List<List<int>> mainsJoueurs;

    void Start()
    {
        foreach (JoueurManager jmanager in mancheManager.joueurs)
        {
            joueursmanagers.Add(jmanager);
        }
        // Appel de la fonction pour initialiser le paquet et les mains
        InitialiserPaquetEtMains();
        
        // Appel de la fonction pour distribuer les cartes
        DistribuerCartes();
    }


    void InitialiserPaquetEtMains()
    {
        paquet = new List<int>();
        mainsJoueurs = new List<List<int>>();

        // Remplissage du paquet avec les cartes de 1 à 52
        for (int i = 1; i <= 32; i++)
        {
            paquet.Add(i);
        }

        // Mélange du paquet de cartes de manière aléatoire
        paquet.Shuffle(); // Assurez-vous d'avoir une méthode d'extension Shuffle pour List<int>.

        // Initialisation des mains des joueurs
        for (int i = 0; i < nombreJoueurs; i++)
        {
            mainsJoueurs.Add(new List<int>());
        }
    }
    void TrierCartesDansMains()
    {
        // Trie chaque main de joueur individuellement
        for (int i = 0; i < mainsJoueurs.Count; i++)
        {
            mainsJoueurs[i].Sort();
        }
    }

    public void Redistribuer()
    {
        paquet.Clear();
        for (int i = 0; i < nombreJoueurs; i++)
        {
            mainsJoueurs[i].Clear();
        }
        foreach (JoueurManager joueur in joueursmanagers)
        {
            joueur.ViderMain();
        }
        InitialiserPaquetEtMains();
        DistribuerCartes();
    }

    void DistribuerCartes()
    {
        while (paquet.Count > 0)
        {
            // Distribution des cartes aux joueurs
            for (int i = 0; i < nombreJoueurs; i++)
            {
                if (paquet.Count > 0)
                {
                    int carte = paquet[0];
                    paquet.RemoveAt(0);
                    mainsJoueurs[i].Add(carte);
                }
            }
        }
        TrierCartesDansMains();

        // Affichage des mains des joueurs (peut être adapté en fonction de votre besoin)
        for (int i = 0; i < nombreJoueurs; i++)
        {
            joueursmanagers[i].joueur.main = mainsJoueurs[(int)i];
            joueursmanagers[i].InstancierCartesDevantJoueur();
            //Debug.Log("Joueur " + (i + 1) + " : " + string.Join(", ", mainsJoueurs[i]));
        }
    }
}

