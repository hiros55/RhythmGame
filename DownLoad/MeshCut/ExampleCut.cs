using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExampleCut: MonoBehaviour
{
    public Rigidbody rb1;
    public Rigidbody rb2;
    float time = 0.45f;
    
    public Material capMaterial;
    private GameController gameController;

    public LayerMask mask;

    private void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {

        RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit,0.8f,mask.value))
            {

                GameObject victim = hit.collider.gameObject;
                
                GameObject[] pieces = BLINDED_AM_ME.MeshCut.Cut(victim, transform.position, transform.right, capMaterial);



            if (!pieces[0].GetComponent<Rigidbody>())
            {
                rb1 = pieces[0].AddComponent<Rigidbody>();
                rb1.AddForce(Random.Range(-20f, 0f), Random.Range(5f, 18f), 0, ForceMode.Impulse);
                
            }
            

            if (!pieces[1].GetComponent<Rigidbody>())
            {
                rb2 = pieces[1].AddComponent<Rigidbody>();
                rb2.AddForce(Random.Range(0f, 20f), Random.Range(5f, 18f), 0, ForceMode.Impulse);
                
            }

                Destroy(pieces[0], time);
                Destroy(pieces[1], time);
            }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("cube"))
        {
            gameController.score += 100;
        }
    }

}
