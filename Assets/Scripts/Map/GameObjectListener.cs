using System.Collections.Generic;
using UnityEngine;

public class GameObjectListener : MonoBehaviour
{
    [SerializeField]
    private Renderer[] renderersObject = new Renderer[] { };

    private void Awake()
    {
        renderersObject = GameObject.FindObjectsOfType<Renderer>();
    }


    public Renderer[] GetAllRenderers()
    {
        if (renderersObject == null)
            throw new System.Exception("renderersObject == null");
        return renderersObject;
    }
}
