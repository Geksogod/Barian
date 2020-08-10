using System.Collections;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField]
    private Transform mainCharacters;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    [Range(0, 10)]
    private float cameraMovingSpeed;
    [SerializeField]
    [Range(0, 3)]
    private float cameraMovingYCoefficient;


    private void Awake()
    {
        if (!gameObject.TryGetComponent(out mainCamera))
            mainCamera = Camera.main;
    }

    private void FixedUpdate()
    {
        Moving(GetNextCameraPositon());
        if (Input.GetKeyDown(KeyCode.R))
            StartCoroutine(CameraShake(4f));
    }

    private Vector3 GetNextCameraPositon()
    {
        Vector3 nextCameraPositiom = Vector3.zero;
        if (mainCharacters.transform.position.x <= 0)
            return mainCamera.transform.position;
        nextCameraPositiom = new Vector3(mainCharacters.transform.position.x, 0 + mainCharacters.transform.position.y * cameraMovingYCoefficient, -10);
        if (nextCameraPositiom.y < 0)
            nextCameraPositiom.y = 0;
        else if (nextCameraPositiom.y > 1.4f)
            nextCameraPositiom.y = 1.4f;
        return nextCameraPositiom;
    }

    private void Moving(Vector3 cameraFinishPosition)
    {
        mainCamera.transform.position = Vector3.Lerp(transform.position, cameraFinishPosition, Time.deltaTime * cameraMovingSpeed);
    }

    private IEnumerator CameraShake(float time)
    {
        float timer = 0;
        while (timer <= time / 2)
        {
            gameObject.transform.Rotate(0, 0, 0.001f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }
        timer = 0;
        while (timer <= time / 2)
        {
            gameObject.transform.Rotate(0, 0, -1 * 0.01f);
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.001f);
        }
        yield break;
    }
}
