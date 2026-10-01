using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Objet représentant une boule contrôlée par le joueur.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boule : MonoBehaviour
{
    [SerializeField, Tooltip("La cible pour le suvi de la caméra")]
    private Transform cibleCamera;

    [SerializeField, Tooltip("Force de déplacement de la boule.")]
    private float forceDeplacement;

    // Force appliquée à la boule pour le déplacement à chaque frame.
    private Vector3 forceAppliquee;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    /// <summary>
    /// Obtient la vélocité actuelle de la boule.
    /// </summary>
    public Vector3 Velocite => rigidbody.linearVelocity;

    private bool jeuCommencer;
    private bool accelerationCours;

    private void Start()
    {
        jeuCommencer = false; 
        accelerationCours=false;
        rigidbody = GetComponent<Rigidbody>();

        if (ControleurJeu.Instance == null)
            return;

        PlayerInput controles = ControleurJeu.Instance.Controles;
        controles.actions.FindAction("Commencer").performed += CommencerJeu;
        controles.actions.FindAction("Acceleration").performed += UtiliserAcceleration;

        controles.actions.FindAction("Diriger").performed += CommencerDirection;
        controles.actions.FindAction("Diriger").canceled += ArreterDirection;
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null)
            return;

        PlayerInput controles = ControleurJeu.Instance.Controles;

        if (controles == null) 
            return;
        controles.actions.FindAction("Diriger").performed -= CommencerDirection;
        controles.actions.FindAction("Diriger").canceled -= ArreterDirection;

        controles.actions.FindAction("Commencer").performed -= CommencerJeu;

    }

    private void Update()
    {
        if (cibleCamera != null)
        {
            cibleCamera.position = rigidbody.position;
        }
    }

    private void FixedUpdate()
    {
        Diriger();
    }
    private void CommencerJeu(InputAction.CallbackContext contexte)
    {
        rigidbody.useGravity = true;
        jeuCommencer = true;
        PlayerInput controles = ControleurJeu.Instance.Controles;
        controles.actions.FindAction("Commencer").Disable();
    }
    private void UtiliserAcceleration(InputAction.CallbackContext contexte)
    {
        if (!jeuCommencer) 
        {
            return;
        }     
        if (ControleurJeu.Instance.NombreCharges > 0)
        {
            ControleurJeu.Instance.DecrementerCharge();
            PlayerInput controles = ControleurJeu.Instance.Controles;
            controles.actions.FindAction("Acceleration").Disable();
            StartCoroutine(Charge());
        }
        
    }
    private IEnumerator Charge()
    {
        accelerationCours = true;
        while (accelerationCours)
        {
            forceAppliquee.z = 15; 
            yield return new WaitForSeconds(1);
            forceAppliquee.z = 0;
            PlayerInput controle = ControleurJeu.Instance.Controles;
            controle.actions.FindAction("Acceleration").Enable();
            accelerationCours = false;

        }
        
    }
    private void CommencerDirection(InputAction.CallbackContext contexte)
    {
        if (jeuCommencer)
        forceAppliquee += contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void ArreterDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee = Vector3.zero;
    }

    private void Diriger()
    {
        if(!Mathf.Approximately(forceAppliquee.sqrMagnitude, 0.0f))
        {
            rigidbody.AddForce(forceAppliquee, ForceMode.Force);
        }
    }
}
