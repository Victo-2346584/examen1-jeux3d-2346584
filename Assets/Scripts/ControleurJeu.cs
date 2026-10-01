using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// Contrôleur de jeu qui gère l'état global du jeu et fournit un accès centralisé aux contrôles et aux autres systèmes.
/// </summary>
public class ControleurJeu : MonoBehaviour
{
    /// <summary>
    /// Instance singleton du contrôleur de jeu.
    /// </summary>
    public static ControleurJeu Instance { get; private set; }

    [field: SerializeField, Tooltip("Référence aux contrôles du joueur.")]
    public PlayerInput Controles { get; private set; }

    public int NombreCharges { get; private set; }
    [SerializeField, Tooltip("Tableau des images de charges")]
    private Image[] charges;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        NombreCharges = 0;
        for (int i = 0; i < charges.Length; i++)
        {
            charges[i].gameObject.SetActive(false);
        }
    }
    public void IncrementerCharge()
    {
        if (NombreCharges < 3)
        {
            NombreCharges += 1;
            charges[NombreCharges-1].gameObject.SetActive(true);
        }
    }
    public void DecrementerCharge()
    {
        if (NombreCharges >0 )
        {
            NombreCharges -= 1;
            charges[NombreCharges].gameObject.SetActive(false);
        }
    }
}
