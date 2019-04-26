﻿using UnityEngine;
using System.Collections;
using static SpellDB;

public class SpellManager : MonoBehaviour {
    
    SpellDB.Spell Spell;
    ArrayList currentCast;
    private bool combo = false;
    private bool canCast = false;
    private float timer;
    private string combination = "";
    

    public GameObject player;
    public float castingTime = 10f;
    public float speed = 100f;
    public float second = 1f;

    private PlayerMana playerMana;
    private PlayerHealth playerHealth;

    // Use this for initialization
    void Start() {
        currentCast = new ArrayList();
        playerMana = player.GetComponent<PlayerMana>();
        playerHealth = player.GetComponent<PlayerHealth>();
        GM.mgr_spells = this;
    }

    public void resetCombination(){
        combination = "";
    }
    public PlayerMana getPlayerMana() {
        return playerMana;
    }
    public PlayerHealth getPlayerHealth() {
        return playerHealth;
    }

    private bool isCoroutineExecuting = false;
    // Update is called once per frame
    void Update() {
        IEnumerator ExecuteAfterTime(float time)
        {
            if (isCoroutineExecuting)
                yield break;
        
            isCoroutineExecuting = true;
        
            yield return new WaitForSeconds(time);
        
            // Code to execute after the delay
            combo = false;
            combination = "";
        
            isCoroutineExecuting = false;
        }

        if(Input.GetKeyDown("p") || Input.GetKeyDown("o") || Input.GetKeyDown("i"))
        {
            if(!combo){
                combo = true;
                 StartCoroutine(ExecuteAfterTime(2f));
            }
        }


        if (Input.GetKeyDown("l")) {
            //playerMana.setManaSlider(castSpell(Elements.Dark, playerMana.getMana()));
            GM.mgr_element.DisplayElement(Elements.Dark);
        }

        if (Input.GetKeyDown("k")) {
           // playerMana.setManaSlider(castSpell(Elements.Light, playerMana.getMana()));
            GM.mgr_element.DisplayElement(Elements.Light);
        }

        if (combo) {
            if (Input.GetKeyDown("p")) {
                combination += "F";
                //Debug.Log(combination);
                playerMana.setManaSlider(castSpell(Elements.Fire, playerMana.getMana()));
                GM.mgr_combats.detectCombo(combination);
                
                //GM.mgr_element.DisplayElement(Elements.Light);
            }
            if (Input.GetKeyDown("o")) {
                combination += "W";
                //Debug.Log(combination);
                playerMana.setManaSlider(castSpell(Elements.Water, playerMana.getMana()));
                GM.mgr_combats.detectCombo(combination);
            }
            if (Input.GetKeyDown("i")) {
                combination += "E";
                //Debug.Log(combination);
                playerMana.setManaSlider(castSpell(Elements.Earth, playerMana.getMana()));
                GM.mgr_combats.detectCombo(combination);
            }
        }


    }

    public void lightEffect(){
        playerHealth.addHealth(30);
    }

    public float calculateDamage(string spellCombo){
        // if light/dark calculate that as well
        // or just calculate normal
        float dmg = 0;
        if(GM.db_spells.getSpellDB().ContainsKey(spellCombo)){
            // perform visual effects and collisions
            Spell = GM.db_spells.getSpellDB()[spellCombo];
            dmg = Spell.attackDmg;
        }

        return dmg;
    }

    public int castSpell(Elements e, int currStat) { // DEALS WITH DRAINING OF STATS
        int cost = GM.mgr_element.getElementCost(e);

        if (e == Elements.Dark) {
            if (currStat <= playerHealth.getMaxHP()) { // Gets the mana.
                currStat -= cost;

                if (currStat < 0) {
                    currStat = 0;
                }
            }
        } else if (e == Elements.Light) { // Gets the 
            if (currStat > 0) {
                currStat -= cost;
                if (currStat < 0) {
                    currStat = 0;
                }
            } else {
                GM.mgr_element.DisplayElement(e);
            }
        } else {
            if (currStat >= cost) {
                currStat -= cost;
                GM.mgr_element.DisplayElement(e);

            }
        }

        return currStat;
    }
}
