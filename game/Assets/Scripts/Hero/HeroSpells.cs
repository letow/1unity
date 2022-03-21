using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSpells : MonoBehaviour
{
    HeroController heroController;
    HeroInventory heroInventory;
    Animator anim;

    public GameObject spellBook;
    public List<SpellDrag> spellDrags;
    public List<Spell> spells = new List<Spell>();

    public GameObject cell;
    public Transform cellParent;

    public GameObject ArcaneMissileObj;
    public string pathPrefab;
    public Transform lHand;
    public Quaternion castRotation;
    public Vector3 target;
    public Transform targetInteract;
    public bool castReady = false;


    public void SpellBookActive()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (spellBook.activeSelf)
            {
                SpellBookDisable();
            }
            else
            {
                SpellBookEnable();
            }
        }
    }

    public void SpellBookEnable()
    {
        spellBook.SetActive(true);

        foreach (SpellDrag spellDrag in spellDrags)
            Destroy(spellDrag.gameObject);
        spellDrags.Clear();

        for (int i = 0; i < spells.Count; i++)
        {
            GameObject newCell = Instantiate(cell);
            newCell.transform.SetParent(cellParent, false);
            spellDrags.Add(newCell.GetComponent<SpellDrag>());

            spellDrags[i].spell = spells[i];
            spellDrags[i].defaultSprite = spells[i].textureSprite;
            spellDrags[i].ownerSpell = "mySpell";
            spellDrags[i].heroSpells = this;
        }

    }

    public void SpellBookDisable()
    {
        foreach (SpellDrag spellDrag in spellDrags)
            Destroy(spellDrag.gameObject);
        spellDrags.Clear();

        spellBook.SetActive(false);
    }

    public void Cast(Spell spell)
    {
        print("К♂ass♂туем: " + spell.name);
        castReady = true;
    }
   
    // преколы ############
    void Start()
    {
        GameObject Scroll = GameObject.Find("Magic");
        Spell missile = Scroll.GetComponent<Spell>();
        //spells.Add(missile);
        anim = GetComponent<Animator>();
        heroInventory = GetComponent<HeroInventory>();
    }

    void Update()
    {
        SpellBookActive();
        if(spells.Count != 0)
        {
            heroInventory.inventoryMain.scroll = true;
        }
    }
    public void FinalCast()
    {
        SpellBookDisable();
        print("Вызван файнал каст");
        GameObject newMissile = Instantiate(ArcaneMissileObj, lHand.position, castRotation);
        castReady = false;
        anim.SetBool("Casting", false);
    }

}
