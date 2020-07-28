using UnityEngine;

public class HeroInfoUIController : MonoBehaviour
{
    [SerializeField]
    private Character character;
    [SerializeField]
    private GameObject heltUI;

    private void Start()
    {
        SetHelthLifeInfo(1f);
    }
    private void SetHelthLifeInfo(float normalizeHelthValue)
    {
        heltUI.transform.localScale = new Vector3(normalizeHelthValue, 1, 1);
    }
}
