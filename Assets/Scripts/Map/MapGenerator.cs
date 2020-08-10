using System.Linq.Expressions;
using Unity.Mathematics;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private Camera currentCamera;
    [SerializeField]
    private Transform mainCharacters;
    [SerializeField]
    private GameObject testBox;
    [SerializeField]
    private GameObject backGround;
    [SerializeField]
    private GameObject prevBackGround;

    private Renderer[] renderersObject = new Renderer[] { };

    private void Start()
    {
        currentCamera = currentCamera == null ? Camera.main : currentCamera;
        renderersObject = GameObject.FindObjectOfType<GameObjectListener>().GetAllRenderers();
    }
    private void LateUpdate()
    {
        foreach (Renderer renderer in renderersObject)
            HideObject(renderer.gameObject, !IsTargetVisible(currentCamera, renderer));
        GenerateBackGround(backGround);
    }


    public void SetCurrentCamera(Camera newCamera)
    {
        currentCamera = newCamera;
    }

    private bool IsTargetVisible(Camera c, Renderer go)
    {
        if (go == null)
            return false;
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
        return GeometryUtility.TestPlanesAABB(planes, go.bounds);
    }

    private void HideObject(GameObject gameObject, bool isHide = true)
    {
        if(gameObject.layer != 11)
        gameObject.SetActive(!isHide);
    }

    private void GenerateBackGround(GameObject newbackGround)
    {
        if (mainCharacters == null)
            return;
        if (backGround.transform.position.x - mainCharacters.position.x < 0)
        {
            prevBackGround = backGround;
            if (backGround.transform.position.x - mainCharacters.position.x < 0)
                InitializeNewBackGround(new Vector3(prevBackGround.transform.position.x + 17.4f, prevBackGround.transform.position.y, prevBackGround.transform.position.z));
        }
    }

    private void InitializeNewBackGround(Vector3 newBackGroundPosition)
    {
        if (prevBackGround == null)
            return;
        backGround = new GameObject("New Background");
        backGround.transform.position = newBackGroundPosition;
        backGround.transform.localScale = prevBackGround.transform.localScale;
        SpriteRenderer newSpriteRenderer = backGround.AddComponent<SpriteRenderer>();
        newSpriteRenderer.sprite = prevBackGround.GetComponent<SpriteRenderer>().sprite;
        newSpriteRenderer.sortingOrder = -100;
    }
}
