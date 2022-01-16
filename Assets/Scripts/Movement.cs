using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Movement : MonoBehaviour
{
    Dictionary<string, KeyCode> Inputs = new Dictionary<string, KeyCode>();
    [SerializeField]
    Vector3 shieldSize;
    public Image ultimateImg,AImg,EImg, HpImg, profileImg, healthRed, headRed;
    public TMPro.TMP_Text ultimateTxt, HpTxt;
    public int hp, bhp, maxhp, ultimateCharge,squad, ultimateChargeSpeed;
    public bool dead, delay, invincible, sendBack;
    Vector3 velocity, ETP;
    float rotationX;
    public bool canMove;
    public float lookSpeed, lookXLimit;
    public GameObject playerCamera, projectilePrefab, origin, body, canvas, tentacleEffect1, tentacleEffect2, personalCanvas,groundDetect, shieldImg, hitImg, nameText;
    public float bSpeed, speed, runSpeed,bRunSpeed;
    private float gravity = -5.81f;
    public bool isGrounded = false, aSpell,eSpell;
    public Animator animBow, animChar;
    PhotonView pv;
    private bool rea;
    public Color spellcolor;
    public Sprite profile, AspellDmg, AspellHeal, Aback;
    public string Projectile = "Arrow1";
    public int Armor;
    GameObject settings;
    float conteur = 0, conteur2 = 0;
    public AudioSource audioSource;
    public AudioClip killSoundEffect;

    public enum Spell
    {
        GhostRun,
        GhostBlade,
        SoulHarvest,
        BlessedArrow,
        BackToThePast,
        SeismicEscape,
        LastHope,
        HeavenGift,
        Angelization,
        AscentToHell,
        LowBlow,
        Demonization,
        SoulShield,
        BloodPact,
        WelcomeToHell
    }
    public Spell spell1;
    public Spell spell2;
    public Spell spellUltimate;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.isMine)
        {
            StartCoroutine(DelayStart());
            groundDetect.SetActive(true);
            personalCanvas.SetActive(true);
            gameObject.layer = 2;
            gameObject.transform.name = "local";
            GameObject.Find("Manager").GetComponent<Manager>().player = gameObject;
            Hashtable hash = new Hashtable();
            hash.Add("Death", 0);
            hash.Add("Kill", 0);
            hash.Add("Dead", false);
            PhotonNetwork.player.SetCustomProperties(hash);
            body.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            profileImg.sprite = profile;
            nameText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.isMine && Inputs != null)
        {
            Vector3 move = new Vector3();
            if (hitImg.transform.localScale.x > 0 && conteur < Time.time)
            {
                conteur = Time.time + 0.0001f;
                hitImg.transform.localScale = new Vector2(hitImg.transform.localScale.x - 0.1f, hitImg.transform.localScale.y - 0.1f);
            }
            if (headRed.transform.localScale.x > 0 && conteur2 < Time.time)
            {
                conteur2 = Time.time + 0.0001f;
                headRed.transform.localScale = new Vector2(headRed.transform.localScale.x - 0.1f, headRed.transform.localScale.y - 0.1f);
            }
            if (hp < bhp / 4)
            {
                healthRed.color = new Color(255, 0, 0, (float)Mathf.Cos(Time.time * 2));
            }
            else
            {
                healthRed.color = new Color(255, 0, 0, 0);
            }
            if(hp > maxhp)
            {
                hp = maxhp;
            }
            if (delay)
            {
                StartCoroutine(UltimateChargeDelay());
            }
            ultimateTxt.text = ultimateCharge + "%";
            ultimateImg.fillAmount = (float)ultimateCharge / 100;
            HpTxt.text = hp.ToString();
            HpImg.fillAmount = (float)hp / maxhp;
            shieldImg.SetActive(invincible);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSettings();
            }
            if (Input.GetKey(Inputs["Run"]))
            {
                speed = runSpeed;
                animBow.SetBool("run", true);
                animChar.SetBool("run", true);
            }
            else
            {
                speed = bSpeed;
                animBow.SetBool("run", false);
                animChar.SetBool("run", false);
            }
            if (Input.GetKey(Inputs["Jump"]) && canMove)
            {
                if (isGrounded)
                {
                    velocity.y = Mathf.Sqrt(1.5f * -2f * gravity);
                    //anim.SetBool("jump", true);
                }
            }
            velocity.y += -5.81f * Time.deltaTime;
            GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
            if (canMove)
            {
                if (Input.GetKey(Inputs["Forward"]))
                {
                    animBow.SetBool("walk", true);
                    animChar.SetBool("walk", true);
                }
                else
                {
                    animBow.SetBool("walk", false);
                    animChar.SetBool("walk", false);
                }
                
                
                float x = 0;
                if (Input.GetKey(Inputs["Forward"]))
                {
                    x = 1;
                }
                else if (Input.GetKey(Inputs["Backward"]))
                {
                    x = -1;
                }

                float z = 0;
                if (Input.GetKey(Inputs["Left"]))
                {
                    z = -1;
                }
                else if (Input.GetKey(Inputs["Right"]))
                {
                    z = 1;
                }
                
                move = transform.right * z + transform.forward * x;
                move.Normalize();
                rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
                rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
                playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
                transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
                move = move.normalized;
                
                if (Input.GetButton("Fire1"))
                {
                    animBow.SetBool("attack", true);
                    animChar.SetBool("attack", true);
                }
            }
            GetComponent<CharacterController>().Move(move * speed * Time.deltaTime);
            if (Input.GetKey(Inputs["Spell1"]) && aSpell)
            {
                aSpell = false;
                AImg.color = Color.gray;
                switch (spell1)
                {
                    case Spell.GhostRun:
                        runSpeed += 2.5f;
                        StartCoroutine(GhostDelay(4));
                        StartCoroutine(BoolDelay(0, 15f));
                        break;
                    case Spell.BlessedArrow:
                        Projectile = "Arrow2";
                        StartCoroutine(BoolDelay(0, 18f));
                        break;
                    case Spell.LastHope:
                        foreach (GameObject pla in GameObject.FindGameObjectsWithTag("Player"))
                        {
                            pla.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(pla.GetComponent<PhotonView>().viewID), -100, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false,0);
                        }
                        StartCoroutine(BoolDelay(0, 17f));
                        break;
                    case Spell.AscentToHell:
                        runSpeed += 7f;
                        StartCoroutine(GhostDelay(3));
                        StartCoroutine(BoolDelay(0, 8f));
                        break;
                    case Spell.SoulShield:
                        StartCoroutine(BoolDelay(0, 12f));
                        Armor += 45;
                        StartCoroutine(ArmorCD(-45, 4));
                        break;
                }
            }
            if (Input.GetKey(Inputs["Spell2"]) && eSpell)
            {
                eSpell = false;
                EImg.color = Color.gray;
                switch (spell2)
                {
                    case Spell.GhostBlade:
                        GetComponentInChildren<Fire>().Damage = GetComponentInChildren<Fire>().BDamage + 150;
                        StartCoroutine(BoolDelay(1, 25f));
                        tentacleEffect1.SetActive(true);
                        tentacleEffect2.SetActive(true);
                        break;
                    case Spell.BackToThePast:
                        velocity.y = Mathf.Sqrt(-20f * gravity);
                        GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
                        StartCoroutine(BoolDelay(1, 25f));
                        break;
                    case Spell.HeavenGift:
                        PhotonNetwork.Instantiate("MagePotion", transform.position, Quaternion.identity, 0);
                        StartCoroutine(BoolDelay(1, 9f));
                        break;
                    case Spell.LowBlow:
                        GetComponentInChildren<Fire>().percentageHp = 45;
                        GetComponentInChildren<Fire>().Damage = 30;
                        StartCoroutine(BoolDelay(1, 25f));
                        tentacleEffect1.SetActive(true);
                        break;
                    case Spell.BloodPact:
                        sendBack = true;
                        StartCoroutine(BoolDelay(1, 20f));
                        StartCoroutine(BoolDelay(2, 5f));
                        break;
                }
            }
            if (Input.GetKey(Inputs["Ultimate"]) && ultimateCharge >= 100)
            {
                switch (spellUltimate)
                {
                    case Spell.SoulHarvest:
                        GameObject go = PhotonNetwork.Instantiate("SoulHarvest", transform.position, Quaternion.identity, 0);
                        go.GetComponent<FollowPlayer>().player = gameObject;
                        StartCoroutine(UltimateDestroy(go,10));
                        ultimateCharge = 0;
                        break;
                    case Spell.SeismicEscape:
                        if (isGrounded)
                        {
                            GameObject goA = PhotonNetwork.Instantiate("Inferno", transform.position, Quaternion.identity, 0);
                            velocity.y = Mathf.Sqrt(-20f * gravity);
                            GetComponent<CharacterController>().Move(velocity * Time.deltaTime);
                            StartCoroutine(UltimateDestroy(goA, 1));
                            ultimateCharge = 0;
                        }
                        break;
                    case Spell.Angelization:
                        RaycastHit hit;
                        Ray ray;
                        Vector2 SCP = new Vector2(Screen.width / 2, Screen.height / 2);
                        ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(SCP);
                        if (Physics.Raycast(ray, out hit, 50))
                        {
                            if (hit.transform.tag == "Player" && hit.transform.gameObject.name != "local" && hit.transform.gameObject.layer == 0)
                            {
                                if ((int)hit.transform.GetComponent<PhotonView>().owner.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"])
                                {
                                    print(hit.transform.GetComponent<PhotonView>().owner.CustomProperties["Squad"]);
                                    hit.transform.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(hit.transform.GetComponent<PhotonView>().viewID), -10, PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, true, false,0);
                                    hit.transform.GetComponent<PhotonView>().RPC("SummonShield", PhotonPlayer.Find(hit.transform.GetComponent<PhotonView>().viewID));
                                    ultimateCharge = 0;
                                }
                            }
                        }
                        break;
                    case Spell.Demonization:
                        maxhp = bhp * 2;
                        hp += bhp;
                        StartCoroutine(Heathreset(12f));
                        StartCoroutine(Heal(3,3,0,50));
                        ultimateCharge = 0;
                        break;
                    case Spell.WelcomeToHell:
                        GameObject go2 = PhotonNetwork.Instantiate("Inferno", transform.position, Quaternion.identity, 0);
                        StartCoroutine(UltimateDestroy(go2, 1));
                        ultimateCharge = 0;
                        break;
                }
            }
            if (hp <= 0 && !rea)
            {
                rea = true;
                print("mort");
                Hashtable hash = new Hashtable();
                dead = true;
                hash.Add("Dead", true);
                PhotonNetwork.player.SetCustomProperties(hash);
                animChar.SetBool("dead", true);
                canMove = false;
                GetComponent<CharacterController>().enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                playerCamera.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                StartCoroutine(ResDelay());
            }
        }
    }
    public IEnumerator ResDelay()
    {
        yield return new WaitForSeconds(3f);
        GameObject.Find("Manager").GetComponent<Manager>().Inst((int)PhotonNetwork.player.CustomProperties["Squad"] - 1);
    }
    public IEnumerator GhostDelay(float cd)
    {
        yield return new WaitForSeconds(cd);
        runSpeed = bRunSpeed;
    }
    public IEnumerator Heathreset(float cd)
    {
        yield return new WaitForSeconds(cd);
        maxhp = bhp;
    }
    public IEnumerator Heal(float cd,int count,int value,int missingHealth)
    {
        yield return new WaitForSeconds(cd);
        hp += value;
        hp += (int)Mathf.Clamp((maxhp - hp) * (float)(missingHealth / 100f), 0, maxhp);
        if (count > 0)
        {
            StartCoroutine(Heal(cd, count - 1, value,missingHealth));
        }
    }
    public IEnumerator BoolDelay(int spellID, float time)
    {
        yield return new WaitForSeconds(time);
        switch (spellID)
        {
            case 0:
                AImg.color = spellcolor;
                aSpell = true;
                break;
            case 1:
                eSpell = true;
                EImg.color = spellcolor;
                break;
            case 2:
                sendBack = false;
                break;
        }
    }
    public void respawn()
    {
        rea = false;
        GetComponent<CharacterController>().enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        isGrounded = true;
        hp = maxhp;
        dead = false;
        Hashtable hash = new Hashtable();
        hash.Add("Dead", false);
        PhotonNetwork.player.SetCustomProperties(hash);
        playerCamera.SetActive(true);
        animChar.SetBool("dead", false);
        canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void EndFire()
    {
        animChar.SetBool("attack", false);
    }
    [PunRPC]
    public void res(int dammage, int ID, int Team, int ArmorModif = 0, bool INV = false, bool Perma = false, int ActualPvDmg = 0)
    {
        if ((Team != (int)PhotonNetwork.player.CustomProperties["Squad"] && dammage > 0) || (Team == (int)PhotonNetwork.player.CustomProperties["Squad"] && dammage < 0) || (Team != (int)PhotonNetwork.player.CustomProperties["Squad"] && Perma) || (Team == (int)PhotonNetwork.player.CustomProperties["Squad"] && INV))
        {
            hp -= (int)Mathf.Clamp(hp * (float)(ActualPvDmg / 100f),0,maxhp);

            if (sendBack)
            {
                foreach (GameObject pla in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if(pla.GetComponent<PhotonView>().ownerId == ID)
                    {
                        pla.GetComponent<PhotonView>().RPC("res", PhotonPlayer.Find(ID), (int)(dammage - dammage * (float)(55f / 100f)), PhotonNetwork.player.ID, (int)PhotonNetwork.player.CustomProperties["Squad"], 0, false, false, 0);
                    }
                }
            }
            if (ArmorModif != 0)
            {
                Armor += ArmorModif;
                StartCoroutine(ArmorCD(-ArmorModif, 5));
            }
            if (INV)
            {
                invincible = INV;
                StartCoroutine(InvincibleCD(6));
            }
            if(Perma)
            {
                hp = 0;
            }
            if ((int)Mathf.Clamp(hp - (float)(dammage - dammage * (float)(Armor / 100f)), 0, maxhp) > 0 && hp > 0 && !invincible && dammage > 0)
            {
                hp = (int)Mathf.Clamp(hp - (float)(dammage - dammage * (float)(Armor/100f)),0,maxhp);
                animChar.SetBool("hit", true);
            }
            else if(dammage < 0 && hp > 0)
            {
                hp = (int)Mathf.Clamp(hp - dammage, 0, maxhp);
            }
            else if (!dead && !invincible)
            {
                hp = (int)Mathf.Clamp(hp - (float)(dammage - dammage * (float)(Armor / 100f)), 0, maxhp);
                pv.RPC("AddPoint", PhotonPlayer.Find(ID));
                int killScore = (int)PhotonNetwork.player.CustomProperties["Death"];
                killScore++;
                Hashtable hash = new Hashtable();
                hash.Add("Death", killScore);
                PhotonNetwork.player.SetCustomProperties(hash);
                print(PhotonNetwork.player.CustomProperties["Death"]);
            }
        }
    }
    [PunRPC]
    public void AddPoint()
    {
        Kill();
        int killScore = (int)PhotonNetwork.player.CustomProperties["Kill"];
        killScore++;
        Hashtable hash = new Hashtable();
        hash.Add("Kill", killScore);
        PhotonNetwork.player.SetCustomProperties(hash);
        print(PhotonNetwork.player.CustomProperties["Kill"]);
    }
    [PunRPC]
    public void SummonShield()
    {
        GameObject goS = PhotonNetwork.Instantiate("Shield", transform.position, Quaternion.identity, 0);
        goS.transform.localScale = shieldSize;
        goS.GetComponent<FollowPlayer>().player = gameObject;
        StartCoroutine(UltimateDestroy(goS, 6));
    }
    public void Hit()
    {
        animChar.SetBool("hit", false);
    }
    public IEnumerator UltimateChargeDelay()
    {
        delay = false;
        yield return new WaitForSeconds(ultimateChargeSpeed);
        delay = true;
        ultimateCharge = Mathf.Clamp(ultimateCharge + 1, 0, 100);
    }
    public IEnumerator UltimateDestroy(GameObject go, int time)
    {
        yield return new WaitForSeconds(time);
        PhotonNetwork.Destroy(go);
    }
    public IEnumerator ArmorCD(int armor, int time)
    {
        yield return new WaitForSeconds(time);
        Armor += armor;
    }
    public IEnumerator InvincibleCD(int time)
    {
        yield return new WaitForSeconds(time);
        invincible = false;
    }
    public void UpdateDic()
    {
        Inputs = settings.GetComponent<KILLER.Setting>().InputCollection;
    }
    public void CloseSettings()
    {
        if (settings.active == true)
        {
            settings.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            canMove = true;
        }
        else
        {
            settings.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canMove = false;
        }
    }
    public void HitMark()
    {
        hitImg.transform.localScale = new Vector2(hitImg.transform.localScale.x + 3, hitImg.transform.localScale.y + 3);
    }
    public void Kill()
    {
        headRed.transform.localScale = new Vector2(headRed.transform.localScale.x + 3, headRed.transform.localScale.y + 3);
        audioSource.PlayOneShot(killSoundEffect);
    }
    public void SensSet()
    {
        lookSpeed = PlayerPrefs.GetFloat("Sens");
    }
    public void SetColor(bool ally, string name)
    {
        if (ally) { nameText.GetComponentInChildren<TMPro.TMP_Text>().color = Color.blue; }
        else { nameText.GetComponentInChildren<TMPro.TMP_Text>().color = Color.red; }
        nameText.GetComponentInChildren<TMPro.TMP_Text>().text = name;
    }
    public IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1f);
        settings = GameObject.Find("PlayFabManager").GetComponent<KILLER.PlayFabManager>().settingsWindow;
        settings.GetComponent<KILLER.Setting>().myevent.AddListener(UpdateDic);
        settings.GetComponent<KILLER.Setting>().myCloseEvent.AddListener(CloseSettings);
        settings.GetComponent<KILLER.Setting>().mySensEvent.AddListener(SensSet);
        Inputs = settings.GetComponent<KILLER.Setting>().InputCollection;
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
        {
            player.GetComponent<Movement>().SetColor((int)player.GetComponent<PhotonView>().owner.CustomProperties["Squad"] == (int)PhotonNetwork.player.CustomProperties["Squad"], player.GetComponent<PhotonView>().owner.NickName);
        }
    }
}
