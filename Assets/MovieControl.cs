using UnityEngine;
using System.Collections;

public class MovieControl : MonoBehaviour
{

    public MovieTexture movie;
    public AudioClip movieSound;

    void Start()
    {
        Play();
    }

    public void Play()
    {
        if (movie != null)
        {
            MovieTexture temp = (MovieTexture)gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture;

            if (temp != null)
            {
                temp.Stop();
            }

            gameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = movie;
            movie.Play();
            if (movieSound != null)
            {
                GetComponent<AudioSource>().clip = movieSound;
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void Stop()
    {
        movie.Stop();
    }

}
