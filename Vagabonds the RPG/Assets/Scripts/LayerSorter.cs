using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LayerSorter : MonoBehaviour
{
    private SortingGroup parentRenderer;

    private List<Obstacle> obstacles = new List<Obstacle>();

    private void Awake()
    {
        parentRenderer = transform.parent.GetComponent<SortingGroup>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle") {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeOut();

            if (obstacles.Count == 0 || o.MySpriteRenderer.sortingOrder - 1 < parentRenderer.sortingOrder) 
                parentRenderer.sortingOrder = o.MySpriteRenderer.sortingOrder - 1;

            obstacles.Add(o);
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Obstacle")
        {
            Obstacle o = collision.GetComponent<Obstacle>();
            o.FadeIn();
            obstacles.Remove(o);

            if (obstacles.Count == 0) 
                parentRenderer.sortingOrder = 200;
            else 
            {
                obstacles.Sort();
                parentRenderer.sortingOrder = obstacles[0].MySpriteRenderer.sortingOrder - 1;
            }
        }
    }
}
