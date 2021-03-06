﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class FloatUnityEvent : UnityEvent<float> { }
public class Table : MonoBehaviour
{
    private Vector3 sphereCastPos;
    public Vector3 direction;
    public float radius;
    public bool occupied;
    public GameObject customer;
    public GameObject dish;
    public TextMeshProUGUI OrderText;
    public AudioClip complete;
    public AudioSource audioSource;

    public FloatUnityEvent OnScoreEvent = new FloatUnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        radius = .90f;
        sphereCastPos = this.transform.position;
        direction = transform.TransformDirection(new Vector3(0, 1f, 0));
        occupied = false;
        Debug.DrawRay(sphereCastPos, direction);
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckDish();
    }

    public void SetCustomer(GameObject customer)
    {
        this.customer = customer;
    }

    void CheckDish()
    {
        int layerMask = 1 << 8;

        if (Physics.SphereCast(transform.position-new Vector3(0,2,0), radius, direction, out RaycastHit hit, Mathf.Infinity,layerMask))
        {
            if (hit.collider.CompareTag("Dish") && Input.GetKeyDown(KeyCode.E) && dish == null && customer != null )
            {
                if (hit.collider.GetComponent<Dish>().ingredients[0] == customer.GetComponent<Customer>().food)
                {
                    dish = hit.collider.gameObject;
                    hit.collider.gameObject.transform.parent.gameObject.GetComponent<Player>().item = null;
                    dish.transform.SetParent(transform);
                    dish.transform.localPosition = new Vector3(-1, dish.GetComponent<Renderer>().bounds.size.y * 3.5f, 0);
                    dish.GetComponent<Rigidbody>().detectCollisions = false;
                    ComputeScore(dish);
                    audioSource.clip = complete;
                    audioSource.Play();
                }
                else
                {
                    Player.instance.AlertText.text = "Wrong Dish!";
                }
            }
        }
    }

    void ComputeScore(GameObject dish)
    {
        float multiplier = customer.GetComponent<Customer>().totaltime/ (customer.GetComponent<Customer>().totaltime-customer.GetComponent<Customer>().timeleft);
        int spiceDifference = Mathf.Abs(dish.GetComponent<Dish>().Spiciness - customer.GetComponent<Customer>().spiceLevel);
        int baseScore = 0;

        foreach (var food in dish.GetComponent<Dish>().ingredients)
        {
            if (food =="Steak" || food=="Curry" || food == "Soup")
            {
                baseScore = (food == customer.GetComponent<Customer>().food) ? 100 : -200;
                break;
            }
        }
        

        float totalScore = baseScore * multiplier - spiceDifference;
        OnScoreEvent.Invoke(totalScore);
    }
}
