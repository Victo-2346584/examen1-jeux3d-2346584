using Unity.VisualScripting;
using UnityEngine;

public class Acceleration : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boule"))
        {
            ControleurJeu.Instance.IncrementerCharge();
            Destroy(gameObject);
        }
    }
}
