using System.Collections;
using UnityEngine;

public class PlataformaDrop : MonoBehaviour
{
    [SerializeField] private string plataformaLayer = "OneWayPlatform";

    private int playerLayer;

    private void Awake()
    {
        playerLayer = gameObject.layer;
    }

    public void Iniciar()
    {
        StartCoroutine(DesactivarColisionTemporal());
    }

    private IEnumerator DesactivarColisionTemporal()
    {
        int plataformaLayerIndex = LayerMask.NameToLayer(plataformaLayer);
        Physics2D.IgnoreLayerCollision(playerLayer, plataformaLayerIndex, true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(playerLayer, plataformaLayerIndex, false);
    }
}
