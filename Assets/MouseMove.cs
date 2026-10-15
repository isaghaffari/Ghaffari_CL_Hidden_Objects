using UnityEngine;
using UnityEngine.InputSystem;

public class MouseMove : MonoBehaviour
{

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        //this is to make the mouse move
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        if (mousePosition.x > minX && mousePosition.x < maxX)
            transform.position = new Vector3(mousePosition.x, transform.position.y, transform.position.z);
        if (mousePosition.y > minY && mousePosition.y < maxY)
            transform.position = new Vector3(transform.position.x, mousePosition.y, transform.position.z);
        
        // True only on the single frame the left button goes down
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            // Shoot a ray from the camera through the mouse and get the FIRST thing it hits (closest to the camera)
            RaycastHit2D hit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()));

            // Only continue if the ray hit something
            if (hit.collider != null)
            {
                if (hit.collider != null)
                {
                    HiddenObject hidden = hit.collider.GetComponent<HiddenObject>();

                    if (hidden != null)
                    {
                        hidden.OnClicked();
                    }
                    //otherwise, if what we clicked is the neon sign, make it flicker
                    else if (hit.collider.gameObject == GameManager.Instance.neonSign.gameObject)
                    {
                        GameManager.Instance.FlickerSign(); 
                        
                    }
                    //otherwise, if what we clicked is tagged as a decoy, play the wrong sound
                    else if (hit.collider.CompareTag("Decoy"))
                    {
                        GameManager.Instance.WrongObject();
                    }
                }
            }
        }
    }
    
}
