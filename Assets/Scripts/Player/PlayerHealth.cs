﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour {
    public int startHealth = 100;
    public int currHealth;
    public int currMaxHealth;

    public Slider healthSlider;
    public Text healthText;


    void Awake() {
        currHealth = startHealth;
        currMaxHealth = startHealth;
        setHealthSlider();
    }

    void Update() {

    }

    void setHealthSlider() {
        healthSlider.value = currHealth;
        healthText.text = currHealth + "/" + currMaxHealth;
    }
}