
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    [SerializeField] private GameObject box;

    private bool _isOpened;
    
    // Start is called before the first frame update
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnCollisionEnter called");
        
        if (!_isOpened)
        {
            box.transform.position += new Vector3(4, 0, 0);
            _isOpened = true;
        }
    }
}
