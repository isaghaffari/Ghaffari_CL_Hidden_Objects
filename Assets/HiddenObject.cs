using UnityEngine;

public class HiddenObject : MonoBehaviour
{
    //slot in the inspector for this object's matching icon in the on-screen list
    public GameObject listItem;
  
    //slot for this hidden object's instrument stem in the soundscape
    public AudioSource soundscapeLayer;

    //CHANGED: slot for this object's blue spotlight (starts switched off)
    public GameObject spotlight;
  
    public void OnClicked()
    {
        //this tells the GameManager that this object was found (adds score and plays sound)
        GameManager.Instance.ObjectFound();

        //this will turn on this object's instrument stem up to full volume. it has been playing silently in the background...
        //...until this happens
        soundscapeLayer.volume = 1f;
    
        //this will hide this object's icon in the on-screen list
        listItem.SetActive(false);
    
        //CHANGED: switch on this object's spotlight, like the band member stepping into the light
        spotlight.SetActive(true);

        //turn off the collider so it can't be clicked (and scored) again
        GetComponent<Collider2D>().enabled = false;
    }
}