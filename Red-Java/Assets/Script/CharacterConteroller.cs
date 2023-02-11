using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterConteroller : MonoBehaviour
{
    public Sprite[] WaitAnim;                       //bayat games, 2D plartform
    public Sprite[] jumpAnim;
    public Sprite[] runAnim;

    SpriteRenderer SpriteRenderer;
    [SerializeField] int countForWait;
    [SerializeField] int countForRun;

    float horzontal = 0;
    float WaitAnimTime = 0;
    float runAnimTime = 0;

    [SerializeField] int speed;
    [SerializeField] int jumpPower;

    bool jumpOnces = true;
    Rigidbody2D fizik;

    Vector3 vector;


    void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        fizik = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
    }

    void FixedUpdate()
    {
        Animation();
        CharacterC();
    }

    public void Animation()
    {
        if (jumpOnces == true) // zýplarken saða sola gitmemesi için
        {
            if (horzontal == 0)//idle
            {
                WaitAnimTime += Time.deltaTime;
                if (WaitAnimTime >= 0.05f) //0,05
                {
                    SpriteRenderer.sprite = WaitAnim[countForWait++];
                    if (countForWait == WaitAnim.Length) // uzunluða eþit olduðunda sýfýrla. ___Unity___
                    {
                        countForWait = 0;
                    }
                    WaitAnimTime = 0;
                }
            }
            else if (horzontal > 0)//ileri hareket
            {
                Run();
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horzontal < 0)//geri hareket
            {
                Run(); // koþu componenti
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            //Debug.Log(fizik.velocity.y); // zýplama + - log.

            if (fizik.velocity.y > 0)
            {
                SpriteRenderer.sprite = jumpAnim[0];
            }
            else
            {
                SpriteRenderer.sprite = jumpAnim[1];
            }

            ZýplaSagSol();
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpOnces == true)
            {
                fizik.AddForce(new Vector2(0, jumpPower)); // mass 5, jumpPower: 1750
                jumpOnces = false;
            }
        }
    }

    public void ZýplaSagSol()
    {
        if (horzontal > 0) //zýplarken sað sol için
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horzontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Run() // koþu iþlerinden sorumlu arkadaþ
    {
        runAnimTime += Time.deltaTime;
        if (runAnimTime >= 0.01f)
        {
            SpriteRenderer.sprite = runAnim[countForRun++]; // anim sayaç
            if (countForRun == runAnim.Length) // uzunluða eþit olduðunda sýfýrla.
            {
                countForRun = 0;
            }
            runAnimTime = 0;
        }
    }

    public void CharacterC()
    {
        // sað sol
        horzontal = Input.GetAxisRaw("Horizontal");
        vector = new Vector3(horzontal * speed, fizik.velocity.y, 0); // y yerçekimi;
        fizik.velocity = vector;
    } // zemini otomatik algýlar. Zýplama true yapar

    private void OnCollisionEnter2D(Collision2D collision) // zemine temas varsa tekrar zýpla true olur
    {
        jumpOnces = true;
    }
}
