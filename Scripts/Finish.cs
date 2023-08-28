using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinishLine : MonoBehaviour
{
    static string ExtractNumericPart(string input)
    {
        string numericPart = "";

        foreach (char c in input)
        {
            if (char.IsDigit(c))
            {
                numericPart += c;
            }
        }

        return numericPart;
    }

    public GameObject player;
    public Text coinsText;
    public Text bigScoreText;
    public GameObject car;

    private bool playerReachedFinishLine = false;

    private void Start()
    {
        bigScoreText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !playerReachedFinishLine)
        {
            playerReachedFinishLine = true;

            string numericPart = ExtractNumericPart(coinsText.text);
            int coins = int.Parse(numericPart);

            bigScoreText.text = "Score: " + coins.ToString() + "\n Hold forward key to Restart";
            coinsText.gameObject.SetActive(false);
            bigScoreText.gameObject.SetActive(true);

            StartCoroutine(DeactivatePlayerMeshWithDelay(0.05f));
        }
    }

    private IEnumerator DeactivatePlayerMeshWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        MeshRenderer playerMeshRenderer = player.GetComponent<MeshRenderer>();

        if (playerMeshRenderer != null)
        {
            car.gameObject.SetActive(false);
            playerMeshRenderer.enabled = false;
        }
    }
}
